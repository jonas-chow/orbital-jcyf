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

public class TrapperMovement : CharacterMovement
{
    private List<Trap> traps = new List<Trap>();

    private class Attack1 : LinearAttack
    {
        public TrapperMovement self;

        public Attack1(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 20;
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
            object[] extraData = new object[] {direction};
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

    private class Attack2 : LinearAttack
    {
        public TrapperMovement self;

        public Attack2(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.cooldown = 10;
            this.damage = 10;
        }

        public override void Execute()
        {
            SendEvent();
            // ...
        }

        public override void SendEvent()
        {
            object[] extraData = null; // change this
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            // ...
        }
    }

    private class Attack3 : MeleeAOEAttack
    {
        public TrapperMovement self;

        public Attack3(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
        }

        public override void Execute()
        {
            SendEvent();
            // ...
        }

        public override void SendEvent()
        {
            object[] extraData = null; // change this
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            // ...
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
