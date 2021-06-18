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
    private class Attack1 : LinearAttack
    {
        public BruiserMovement self;

        public Attack1(BruiserMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 20;
            this.cooldown = 2;
        }

        // basic melee attack with med damage
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

    private class Attack2 : MeleeAOEAttack
    {
        public BruiserMovement self;

        public Attack2(BruiserMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 30;
            this.cooldown = 3;
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> enemies = FindTargets().FindAll(cm => cm.isEnemy);
            enemies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });

            // use defense as the attack so that always take 20 fixed damage
            self.TakeDamage(self.GetDefense(), 20);
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 2, null);
        }

        public override void EventExecute(object[] extraData)
        {
            List<CharacterMovement> allies = FindEventTargets().FindAll(cm => !cm.isEnemy);
            allies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            self.TakeDamage(self.GetDefense(), 20);
        }
    }

    private class Attack3 : SelfAttack
    {
        public BruiserMovement self;

        public Attack3(BruiserMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 10;
        }

        public override void Execute()
        {
            SendEvent();
            self.AddBuff(new InvincibleBuff(2));
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 3, null);
        }

        public override void EventExecute(object[] extraData)
        {
            self.AddBuff(new InvincibleBuff(2));
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
