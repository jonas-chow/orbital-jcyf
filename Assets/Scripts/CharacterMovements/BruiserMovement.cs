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

public class BruiserMovement : CharacterMovement
{
    public static int _hp = 100;
    public static int _attack = 15;
    public static int _defense = 15;

    public ParticleSystem slashEffect;
    public ParticleSystem aoeSlashEffect;
    public ParticleSystem undyingEffect;

    public class Attack1 : LinearAttack
    {
        private BruiserMovement self;

        public Attack1(BruiserMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 15;
            this.cooldown = 2;
            this.type = "attack";
            this.name = "attack1";
        }

        // basic melee attack with med damage
        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                ParticleSystem slashEffect = ParticleSystem.Instantiate(self.slashEffect, 
                    GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                AudioManager.Instance.Play("MeleeHit");  
            }
            AudioManager.Instance.Play("MeleeMiss");  
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {InvertDirection(direction)};
            EventHandler.Instance.SendAttackEvent(self.charID, 1, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            string dir = (string)extraData[0];
            CharacterMovement target = FindTarget(dir);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                ParticleSystem slashEffect = ParticleSystem.Instantiate(self.slashEffect, 
                    GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                AudioManager.Instance.Play("MeleeHit");  
            }
            AudioManager.Instance.Play("MeleeMiss");  
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage to the target in front of you.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack2 : MeleeAOEAttack
    {
        private BruiserMovement self;

        public Attack2(BruiserMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 25;
            this.cooldown = 5;
            this.type = "attack";
            this.name = "attack2";
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> enemies = FindTargets().FindAll(cm => cm.IsEnemyOf(self));
            enemies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            ParticleSystem aoeSlashEffect = ParticleSystem.Instantiate(self.aoeSlashEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);

            // use defense as the attack so that always take fixed damage
            self.TakeDamage(self.GetDefense(), damage);
            AudioManager.Instance.Play("BruiserAOE");  
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 2, null);
        }

        public override void EventExecute(object[] extraData)
        {
            List<CharacterMovement> allies = FindTargets().FindAll(cm => cm.IsEnemyOf(self));
            allies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            ParticleSystem aoeSlashEffect = ParticleSystem.Instantiate(self.aoeSlashEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);

            self.TakeDamage(self.GetDefense(), 20);
            AudioManager.Instance.Play("BruiserAOE");  
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage to all enemies around you.\n" +
            $"You take {damage} damage as recoil.\n\nCooldown: {cooldown}";
        }
    }

    public class Attack3 : SelfAttack
    {
        private BruiserMovement self;

        public Attack3(BruiserMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 40;
            this.type = "buff";
            this.name = "attack3";
        }

        public override void Execute()
        {
            SendEvent();
            self.AddBuff(new InvincibleBuff(2));
            ParticleSystem undyingEffect = ParticleSystem.Instantiate(self.undyingEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
            undyingEffect.transform.parent = self.transform; 
            AudioManager.Instance.Play("BruiserInvincible");  
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 3, null);
        }

        public override void EventExecute(object[] extraData)
        {
            self.AddBuff(new InvincibleBuff(2));
            ParticleSystem undyingEffect = ParticleSystem.Instantiate(self.undyingEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
            undyingEffect.transform.parent = self.transform; 
            AudioManager.Instance.Play("BruiserInvincible");  
        }

        public override string GetDescription()
        {
            return $"Become invincible until the start of your next turn.\n" +
            $"You cannot take any damage when invincible.\n\nCooldown: {cooldown}";
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
}
