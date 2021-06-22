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
    public static int _hp = 100;
    public static int _attack = 20;
    public static int _defense = 10;

    public class Attack1 : LinearAttack
    {
        public WizardMovement self;

        public Attack1(WizardMovement cm)
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
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
            }
            AudioManager.Instance.Play("MagicAttack");
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
            }
            AudioManager.Instance.Play("MagicAttack");
        }

        public override string GetDescription()
        {
            return $@"
            Deals {damage} damage to the first target in front of you. 

            Cooldown: {cooldown}
            Range: {range}";
        }
    }

    public class Attack2 : RangedAOEAttack
    {
        public WizardMovement self;

        public Attack2(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 20;
            this.cooldown = 10;
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> enemies = FindTargets(offsetX, offsetY)
                .FindAll(cm => cm.IsEnemyOf(self));
            enemies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            AudioManager.Instance.Play("AOEMagicAttack");
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
            List<CharacterMovement> allies = FindTargets(offsetX, offsetY)
                .FindAll(cm => cm.IsEnemyOf(self));
            allies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            FaceTargetDirection(offsetX, offsetY);
            AudioManager.Instance.Play("AOEMagicAttack");
        }

        public override string GetDescription()
        {
            return $@"
            Deals {damage} damage in an area of effect.

            Cooldown: {cooldown}
            Range: {range}";
        }
    }

    public class Attack3 : SelfAttack
    {
        public WizardMovement self;

        public Attack3(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 50;
        }

        public override void Execute()
        {
            SendEvent();
            self.attack1Turn = -999;
            self.attack2Turn = -999;
            CharacterMenu.Instance.ResetCD(2, 0);
            CharacterMenu.Instance.ResetCD(2, 1);
            AudioManager.Instance.Play("ResetCD");
        }

        public override void SendEvent()
        {
            object[] extraData = null; // change this
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            AudioManager.Instance.Play("ResetCD");
        }

        public override string GetDescription()
        {
            return $@"
            End the cooldown of your other skills.

            Cooldown: {cooldown}";
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
