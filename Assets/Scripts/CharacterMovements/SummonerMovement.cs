using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO:
1) Change what each attack inherits from
1a) Add to the constructors/add more fields if needed
2) Change damage/range/cooldown as you want
2) Implement execute, sendEvent, eventExecute
*/

public class SummonerMovement : CharacterMovement
{
    public static int _hp = 100;
    public static int _attack = 15;
    public static int _defense = 15;

    public ParticleSystem magicAttackEffect;
    public ParticleSystem familiarAttackEffect;
    public ParticleSystem summonEffect;
    public ParticleSystem swapEffect;
    public GameObject familiarPrefab;
    private FamiliarMovement familiarMovement = null;

    public class Attack1 : LinearAttack
    {
        private SummonerMovement self;

        public Attack1(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 15;
            this.cooldown = 2;
            this.type = "attack";
            this.name = "attack1";
            this.charID = cm.charID;
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
            }
            ParticleSystem magicAttackEffect = ParticleSystem.Instantiate(self.magicAttackEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
            self.RotateProjectileEffect(self.faceDirection, magicAttackEffect);
            AudioManager.Instance.Play("MagicAttack");
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
            return $"Deals {damage} damage to the first target in front of you.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack2 : RangedTargetAttack
    {
        private SummonerMovement self;

        public Attack2(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.cooldown = 5;
            this.damage = 30;
            this.type = "other";
            this.name = "attack2";
            this.charID = cm.charID;
        }

        public override Attack Copy()
        {
            return new Attack2(self);
        }

        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            CharacterMovement target = FindTarget(offsetX, offsetY);
            if (target == null) {
                // currently existing familiar explodes
                if (self.familiarMovement != null) {
                    self.familiarMovement.Die();
                    AudioManager.Instance.Play("TrapExplosion");
                }

                GameObject familiar = GameObject.Instantiate(self.familiarPrefab, Vector3.zero, Quaternion.identity);
                GridManager.Instance.MoveToAndInsert(familiar, GetX() + offsetX, GetY() + offsetY);
                self.familiarMovement = familiar.GetComponent<FamiliarMovement>();
                self.familiarMovement.Init(self);
                ParticleSystem summonEffect = ParticleSystem.Instantiate(self.summonEffect, 
                    GridManager.Instance.GetCoords(GetX() + offsetX, GetY() + offsetY), Quaternion.identity);
                AudioManager.Instance.Play("Summon");
            } else if (target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                ParticleSystem familiarAttackEffect = ParticleSystem.Instantiate(self.familiarAttackEffect, 
                    GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                AudioManager.Instance.Play("FamiliarAttack");
            } else {
                // refund cooldown if whiffed
                self.ResetCD(2);
            }
        }

        public override void SendEvent()
        {
            object[] extraData = InvertOffsets();
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            this.offsetX = (int)extraData[0];
            this.offsetY = (int)extraData[1];
            Execute();
        }

        public override string GetDescription()
        {
            return $"Summon a controllable familiar at the target position which explodes on death, " +
            $"dealing {damage} damage to all enemies around it.\n" +
            $"If cast on an enemy, will instead deal {damage} to the target.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack3 : SelfAttack
    {
        private SummonerMovement self;

        public Attack3(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 5;
            this.type = "other";
            this.name = "attack3";
            this.charID = cm.charID;
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
            if (self.familiarMovement != null) {
                Vector3 selfPos = self.transform.position;
                self.transform.position = self.familiarMovement.transform.position;
                self.familiarMovement.transform.position = selfPos;
                GridManager.Instance.RemoveObject(self.GetX(), self.GetY());
                GridManager.Instance.RemoveObject(self.familiarMovement.GetX(), self.familiarMovement.GetY());
                GridManager.Instance.InsertObject(self.gameObject, self.GetX(), self.GetY());
                GridManager.Instance.InsertObject(
                    self.familiarMovement.gameObject, 
                    self.familiarMovement.GetX(), 
                    self.familiarMovement.GetY());
                ParticleSystem swapEffect1 = ParticleSystem.Instantiate(self.swapEffect, 
                    GridManager.Instance.GetCoords(self.familiarMovement.GetX(), self.familiarMovement.GetY()), Quaternion.identity);
                ParticleSystem swapEffect2 = ParticleSystem.Instantiate(self.swapEffect, 
                    GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
                swapEffect1.transform.parent = self.familiarMovement.transform;
                swapEffect2.transform.parent = self.transform;
                AudioManager.Instance.Play("Swap");
            } else {
                // refund cooldown if whiffed
                self.ResetCD(3);
            }
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
            return $"Swap places with the familiar.\n\nCooldown: {cooldown}";
        }
    }

    void Awake()
    {
        attack1 = new Attack1(this);
        attack2 = new Attack2(this);
        attack3 = new Attack3(this);
        hp.maxHp = _hp;
        atk = _attack;
        def = _defense;
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
        // familiar also dies when summoner dies
        if (familiarMovement != null) {
            familiarMovement.Die();
        }
        base.Die();
    }

    public void FamiliarDied()
    {
        familiarMovement = null;
    }
}
