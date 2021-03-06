using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : CharacterMovement
{
    public static int _hp = 100;
    public static int _attack = 10;
    public static int _defense = 20;

    public ParticleSystem slashEffect;

    public class Attack1 : LinearAttack
    {
        private TankMovement self;

        public Attack1(TankMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 10;
            this.cooldown = 2;
            this.type = "attack";
            this.name = "attack1";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack1(self);
        }

        // basic melee attack with low damage
        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
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
            this.direction = (string)extraData[0];
            Execute();
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage to the target in front of you.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack2 : SelfAttack
    {
        private TankMovement self;

        public Attack2(TankMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.damage = 30;
            this.cooldown = 15;
            this.type = "heal";
            this.name = "attack2";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack2(self);
        }

        // self heal
        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            self.Heal(damage);
            AudioManager.Instance.Play("Heal");
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
            return $"Heal yourself for {damage} HP.\n\nCooldown: {cooldown}";
        }
    }

    public class Attack3 : MeleeAOEAttack
    {
        private TankMovement self;
        private int debuffStrength;

        public Attack3(TankMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 0;
            this.damage = 0;
            this.cooldown = 10;
            this.debuffStrength = 10;
            this.type = "buff";
            this.name = "attack3";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack3(self);
        }

        // aoe invincible, self def debuff
        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            List<CharacterMovement> allies = FindTargets().FindAll(cm => !cm.IsEnemyOf(self));
            allies.ForEach(cm => {
                if (cm.Equals(self)) {
                    cm.AddBuff(new DefenseBuff(-debuffStrength, 2));
                } else {
                    cm.AddBuff(new InvincibleBuff(2));
                }
            });
            AudioManager.Instance.Play("Taunt");
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
            return $"All allies around you become invincible until the start of your next turn.\n" +
            $"You lose {debuffStrength} defense until the start of your next turn\n\n" +
            $"Cooldown: {cooldown}";
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
