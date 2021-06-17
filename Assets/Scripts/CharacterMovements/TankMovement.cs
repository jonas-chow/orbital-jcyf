using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : CharacterMovement
{
    private class Attack1 : LinearAttack
    {
        public TankMovement self;

        public Attack1(TankMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 10;
            this.cooldown = 2;
        }

        // basic melee attack with low damage
        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget();
            if (target != null && target.isEnemy) {
                target.TakeDamage(self.GetAttack(), damage);
            }
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {InvertDirection(direction)};
            EventHandler.Instance.SendAttackEvent(self.charID, 1, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            string dir = (string)extraData[0];
            CharacterMovement target = FindEventTarget(dir);
            if (target != null && !target.isEnemy) {
                target.TakeDamage(self.GetAttack(), damage);
            }
        }
    }

    private class Attack2 : SelfAttack
    {
        public TankMovement self;

        public Attack2(TankMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.damage = 30;
            this.cooldown = 10;
        }

        // self heal
        public override void Execute()
        {
            SendEvent();
            self.Heal(damage);
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 2, null);
        }

        public override void EventExecute(object[] extraData)
        {
            self.Heal(damage);
        }
    }

    private class Attack3 : MeleeAOEAttack
    {
        public TankMovement self;

        public Attack3(TankMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 0;
            this.damage = 0;
            this.cooldown = 10;
        }

        // aoe invincible, self def debuff
        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> allies = FindTargets().FindAll(cm => !cm.isEnemy);
            allies.ForEach(cm => {
                if (cm.Equals(self)) {
                    cm.defBuff -= 5;
                } else {
                    cm.invincible = true;
                }
            });
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 3, null);
        }

        public override void EventExecute(object[] extraData)
        {
            List<CharacterMovement> enemies = FindEventTargets().FindAll(cm => cm.isEnemy);
            enemies.ForEach(cm => {
                if (cm.Equals(self)) {
                    cm.defBuff -= 5;
                } else {
                    cm.invincible = true;
                }
            });
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
}
