using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO:
1) Change what each attack inherits from
1a) Add to the constructors/add more fields if needed
2) Change damage/range/cooldown as you want in the constructors
2) Implement execute, sendEvent, eventExecute
*/

public class FamiliarMovement : CharacterMovement
{
    SummonerMovement summoner;
    public ParticleSystem familiarAttackEffect;
    public ParticleSystem explosionEffect;
    public ParticleSystem swapEffect;

    public class Attack1 : LinearAttack
    {
        private FamiliarMovement self;

        public Attack1(FamiliarMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 20;
            this.cooldown = 2;
            this.type = "attack";
            this.name = "attack1";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack1(self);
        }

        // basic melee attack
        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                ParticleSystem familiarAttackEffect = ParticleSystem.Instantiate(self.familiarAttackEffect, 
                    GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                AudioManager.Instance.Play("FamiliarAttack");
            }
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {InvertDirection(direction)};
            EventHandler.Instance.SendAttackEvent(self.charID, 1, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            this.direction = (string)extraData[0];
            Execute();
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage to the target in front of you. \n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack2 : MeleeAOEAttack
    {
        private FamiliarMovement self;

        public Attack2(FamiliarMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 30;
            this.cooldown = 99;
            this.type = "other";
            this.name = "attack2";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack2(self);
        }

        // explode and die
        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            if (!self.isEnemy) {
                CharacterMenu.Instance.SetHealth(self.charID, 0f);
            }
            self.Die();
            self.hp.SetVisible(false);
            AudioManager.Instance.Play("TrapExplosion");
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 2, null);
        }

        public override void EventExecute(object[] extraData)
        {
            Execute();
        }

        public override string GetDescription()
        {
            return $"Explode and deal {damage} to all enemies in an area of effect.\n\n" +
            $"Cooldown: {cooldown}";
        }
    }

    public class Attack3 : SelfAttack
    {
        private FamiliarMovement self;

        public Attack3(FamiliarMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 5;
            this.type = "other";
            this.name = "attack3";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack3(self);
        }

        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            Vector3 selfPos = self.transform.position;
            self.transform.position = self.summoner.transform.position;
            self.summoner.transform.position = selfPos;
            GridManager.Instance.RemoveObject(self.GetX(), self.GetY());
            GridManager.Instance.RemoveObject(self.summoner.GetX(), self.summoner.GetY());
            GridManager.Instance.InsertObject(self.gameObject, self.GetX(), self.GetY());
            GridManager.Instance.InsertObject(
                self.summoner.gameObject, 
                self.summoner.GetX(), 
                self.summoner.GetY());
            ParticleSystem swapEffect1 = ParticleSystem.Instantiate(self.swapEffect, 
                GridManager.Instance.GetCoords(self.summoner.GetX(), self.summoner.GetY()), Quaternion.identity);
            ParticleSystem swapEffect2 = ParticleSystem.Instantiate(self.swapEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
            swapEffect1.transform.parent = self.summoner.transform;
            swapEffect2.transform.parent = self.transform;
            AudioManager.Instance.Play("Swap");
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 3, null);
        }

        public override void EventExecute(object[] extraData)
        {
            Execute();
        }

        public override string GetDescription()
        {
            return $"Swap positions with the summoner.\n\nCooldown: {cooldown}";
        }
    }

    void Awake()
    {
        attack1 = new Attack1(this);
        attack2 = new Attack2(this);
        attack3 = new Attack3(this);
    }

    public override void SetupAttack(int attackNumber)
    {
        attackNum = attackNumber;
        switch (attackNumber)
        {
            case 1:
                attack = attack1;
                attack1 = new Attack1(this);
                break;
            case 2:
                attack = attack2;
                attack2 = new Attack2(this);
                break;
            case 3:
                attack = attack3;
                attack3 = new Attack3(this);
                break;
        }

        attack.InitialiseAim();
    }

    public override void Die()
    {
        // find all characters that are in your opposing team
        List<CharacterMovement> enemies = GridManager.Instance
            .GetAllCharactersInAOE(GetX(), GetY())
            .FindAll(cm => cm.IsEnemyOf(this));
        // explode
        enemies.ForEach(cm => cm.TakeDamage(GetAttack(), attack2.GetDamage()));
        summoner.FamiliarDied();
        base.Die();
        ParticleSystem explosionEffect = ParticleSystem.Instantiate(this.explosionEffect, 
            GridManager.Instance.GetCoords(GetX(), GetY()), Quaternion.identity);
    }

    public void Init(SummonerMovement summoner)
    {
        SetEnemy(summoner.isEnemy);
        if ((summoner.isEnemy && !GameManager.Instance.reversedPov) || (GameManager.Instance.reversedPov && !summoner.isEnemy)) {
            this.Face("down");
        }
        this.summoner = summoner;
        // familiar inherits summoner's attack at time of summon
        this.atk = summoner.atk;
        GameManager.Instance.InsertChar(this);
    }
}
