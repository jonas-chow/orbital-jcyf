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

    public ParticleSystem magicAttackEffect;
    public ParticleSystem electricAttackEffect;

    public class Attack1 : LinearAttack
    {
        private WizardMovement self;

        public Attack1(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 15;
            this.cooldown = 2;
            this.type = "attack";
            this.name = "attack1";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack1(self);
        }

        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
            }
            ParticleSystem magicAttackEffect = ParticleSystem.Instantiate(self.magicAttackEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
            self.RotateProjectileEffect(self.faceDirection, magicAttackEffect);
            AudioManager.Instance.Play("MagicAttack");
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
            return $"Deals {damage} damage to the first target in front of you.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack2 : RangedAOEAttack
    {
        private WizardMovement self;

        public Attack2(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 20;
            this.cooldown = 10;
            this.type = "attack";
            this.name = "attack2";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack2(self);
        }

        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            List<CharacterMovement> enemies = FindTargets(offsetX, offsetY)
                .FindAll(cm => cm.IsEnemyOf(self));
            enemies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            ParticleSystem electricAttackEffect = ParticleSystem.Instantiate(self.electricAttackEffect, 
                GridManager.Instance.GetCoords(self.GetX() + offsetX, self.GetY() + offsetY), Quaternion.identity);
            AudioManager.Instance.Play("AOEMagicAttack");
        }

        public override void SendEvent()
        {
            object[] extraData = InvertOffsets();
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            this.offsetX = (int)extraData[0];
            this.offsetY = (int)extraData[1];
            Execute();
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage in an area of effect.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack3 : SelfAttack
    {
        private WizardMovement self;

        public Attack3(WizardMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 50;
            this.type = "other";
            this.name = "attack3";
            this.charID = cm ? cm.charID : -1;
        }

        public override Attack Copy()
        {
            return new Attack3(self);
        }

        public override void Execute()
        {
            if (!self.isEnemy) {
                SendEvent();
            }
            self.ResetCD(1);
            self.ResetCD(2);
            AudioManager.Instance.Play("ResetCD");
        }

        public override void SendEvent()
        {
            object[] extraData = null; // change this
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            Execute();
        }

        public override string GetDescription()
        {
            return $"End the cooldown of your other skills.\n\n" +
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
