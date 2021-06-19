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

public class HealerMovement : CharacterMovement
{
    private class Attack1 : LinearAttack
    {
        public HealerMovement self;

        public Attack1(HealerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 15;
            this.cooldown = 2;
        }

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

    private class Attack2 : RangedAOEAttack
    {
        public HealerMovement self;

        public Attack2(HealerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 40;
            this.cooldown = 5;
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> allies = FindTargets().FindAll(cm => !cm.isEnemy);
            allies.ForEach(cm => {
                cm.Heal(damage);
            });
        }

        public override void SendEvent()
        {
            object[] extraData = InvertOffsets();
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            int offsetX = (int)extraData[0];
            int offsetY = (int)extraData[1];
            List<CharacterMovement> enemies = FindEventTargets(offsetX, offsetY)
                .FindAll(cm => cm.isEnemy);
            enemies.ForEach(cm => {
                cm.Heal(damage);
            });
            FaceTargetDirection(offsetX, offsetY);
        }
    }

    private class Attack3 : RangedAOEAttack
    {
        public HealerMovement self;

        public Attack3(HealerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.cooldown = 5;
            this.damage = 10; // dk how strong numbers are for atk/def buffs prob need adjustments
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> allies = FindTargets().FindAll(cm => !cm.isEnemy);
            allies.ForEach(cm => {
                cm.AddBuff(new AttackBuff(damage, 3));
                cm.AddBuff(new DefenseBuff(damage, 3));
            });
        }

        public override void SendEvent()
        {
            object[] extraData = InvertOffsets();
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            int offsetX = (int)extraData[0];
            int offsetY = (int)extraData[1];
            List<CharacterMovement> enemies = FindEventTargets(offsetX, offsetY)
                .FindAll(cm => cm.isEnemy);
            enemies.ForEach(cm => {
                cm.AddBuff(new AttackBuff(damage, 3));
                cm.AddBuff(new DefenseBuff(damage, 3));
            });
            FaceTargetDirection(offsetX, offsetY);
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
