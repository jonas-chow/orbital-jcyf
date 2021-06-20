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
        public int backstabBonus;

        public Attack1(AssassinMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 20;
            this.backstabBonus = 10;
            this.cooldown = 2;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget();
            if (target != null && target.IsEnemyOf(self)) {
                if (target.faceDirection == self.faceDirection) {
                    target.TakeDamage(self.GetAttack(), damage + backstabBonus);
                } else {
                    target.TakeDamage(self.GetAttack(), damage);
                }
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
            CharacterMovement target = FindEventTarget(dir);
            if (target != null && target.IsEnemyOf(self)) {
                if (target.faceDirection == self.faceDirection) {
                    target.TakeDamage(self.GetAttack(), damage + backstabBonus);
                } else {
                    target.TakeDamage(self.GetAttack(), damage);
                }
                AudioManager.Instance.Play("MeleeHit");   
            }
            AudioManager.Instance.Play("MeleeMiss");
        }
    }

    private class Attack2 : LinearAttack
    {
        public AssassinMovement self;

        public Attack2(AssassinMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 10;
            this.cooldown = 10;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget();
            if (target != null && target.IsEnemyOf(self)) {
                target.AddBuff(new PoisonDebuff(3));
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("Poison");
            }
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {InvertDirection(direction)};
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            string dir = (string)extraData[0];
            CharacterMovement target = FindEventTarget(dir);
            if (target != null && target.IsEnemyOf(self)) {
                target.AddBuff(new PoisonDebuff(3));
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("Poison");
            }
        }
    }

    private class Attack3 : SelfAttack
    {
        public AssassinMovement self;

        public Attack3(AssassinMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 20;
        }

        public override void Execute()
        {
            SendEvent();
            self.AddBuff(new StealthBuff(2));
            AudioManager.Instance.Play("Stealth");
        }

        public override void SendEvent()
        {
            object[] extraData = null;
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            self.AddBuff(new StealthBuff(2));
            AudioManager.Instance.Play("Stealth");
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
