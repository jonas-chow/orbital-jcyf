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

public class AssassinMovement : CharacterMovement
{
    private class Attack1 : LinearAttack
    {
        public AssassinMovement self;

        public Attack1(AssassinMovement cm)
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
            EventHandler.Instance.SendAttackEvent(self.charID, 1, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            // ...
        }
    }

    private class Attack2 : SelfAttack
    {
        public AssassinMovement self;

        public Attack2(AssassinMovement cm)
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
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            // ...
        }
    }

    private class Attack3 : MeleeAOEAttack
    {
        public AssassinMovement self;

        public Attack3(AssassinMovement cm)
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
                attack = attack2;
                attack2 = new Attack2(this);
                break;
        }

        attack.InitialiseAim();
    }
}
