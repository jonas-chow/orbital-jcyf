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

public class WizardMovement : CharacterMovement
{
    private class Attack1 : LinearAttack
    {
        public WizardMovement self;

        public Attack1(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 25;
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
        public WizardMovement self;

        public Attack2(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 25;
            this.cooldown = 3;
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> enemies = FindTargets().FindAll(cm => cm.isEnemy);
            enemies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
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
            List<CharacterMovement> allies = FindEventTargets(offsetX, offsetY)
                .FindAll(cm => !cm.isEnemy);
            allies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            FaceTargetDirection(offsetX, offsetY);
        }
    }

    private class Attack3 : SelfAttack
    {
        public WizardMovement self;

        public Attack3(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 20;
        }

        public override void Execute()
        {
            SendEvent();
            self.attack1Turn = -999;
            self.attack2Turn = -999;
        }

        public override void SendEvent()
        {
            object[] extraData = null; // change this
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            self.attack1Turn = -999;
            self.attack2Turn = -999;
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
