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

public class ScoutMovement : CharacterMovement
{
    public GameObject ward;

    private class Attack1 : LinearAttack
    {
        public ScoutMovement self;

        public Attack1(ScoutMovement cm)
        {
            this.sourceChar = cm;
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

    private class Attack2 : RangedTargetAttack
    {
        public ScoutMovement self;
        

        public Attack2(ScoutMovement cm)
        {
            this.sourceChar = cm;
            this.self = cm;
            this.range = 5;
            this.cooldown = 0; // change this
            this.damage = 19;
        }

        public override void Execute()
        {
            SendEvent();
            if (FindTarget() == null) {
                GameObject.Instantiate(self.ward,
                    new Vector3(posX, posY, 0),
                    Quaternion.identity);
            } else {
                FindTarget().TakeDamage(self.GetAttack(), damage);
            }
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {offsetX, offsetY};
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            if (FindEventTarget((int)extraData[0], (int)extraData[1]) == null) {
                GameObject.Instantiate(self.ward,
                    new Vector3((int)extraData[0], (int)extraData[1], 0),
                    Quaternion.identity);
            } else {
                FindEventTarget((int)extraData[0], (int)extraData[1]).TakeDamage(self.GetAttack(), damage);
            }
        }
    }

    private class Attack3 : RangedAOEAttack
    {
        public ScoutMovement self;

        public Attack3(ScoutMovement cm)
        {
            this.sourceChar = cm;
            this.self = cm;
            this.range = 30;
            this.damage = 15;
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
            object[] extraData = new object[] {offsetX, offsetY};
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            List<CharacterMovement> allies = FindEventTargets((int)extraData[0], (int)extraData[1])
                .FindAll(cm => !cm.isEnemy);
            allies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
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
