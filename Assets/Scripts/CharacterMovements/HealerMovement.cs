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
    public static int _hp = 100;
    public static int _attack = 5;
    public static int _defense = 15;

    public class Attack1 : LinearAttack
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
        public HealerMovement self;

        public Attack2(HealerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 40;
            this.cooldown = 15;
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> allies = FindTargets(offsetX, offsetY)
                .FindAll(cm => !cm.IsEnemyOf(self));
            allies.ForEach(cm => {
                cm.Heal(damage);
            });
            AudioManager.Instance.Play("Heal");
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
            List<CharacterMovement> enemies = FindTargets(offsetX, offsetY)
                .FindAll(cm => !cm.IsEnemyOf(self));
            enemies.ForEach(cm => {
                cm.Heal(damage);
            });
            AudioManager.Instance.Play("Heal");
        }

        public override string GetDescription()
        {
            return $@"
            Heals all allies for {damage} HP in an area of effect.

            Cooldown: {cooldown}
            Range: {range}";
        }
    }

    public class Attack3 : RangedAOEAttack
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
            List<CharacterMovement> allies = FindTargets(offsetX, offsetY)
                .FindAll(cm => !cm.IsEnemyOf(self));
            allies.ForEach(cm => {
                cm.AddBuff(new AttackBuff(damage, 2));
                cm.AddBuff(new DefenseBuff(damage, 2));
            });
            AudioManager.Instance.Play("AOEbuff");
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
            List<CharacterMovement> allies = FindTargets(offsetX, offsetY)
                .FindAll(cm => !cm.IsEnemyOf(self));
            allies.ForEach(cm => {
                cm.AddBuff(new AttackBuff(damage, 2));
                cm.AddBuff(new DefenseBuff(damage, 2));
            });
            AudioManager.Instance.Play("AOEbuff");
        }

        public override string GetDescription()
        {
            return $@"
            Buffs all allies in an area of effect until the start of next turn.

            Affected allies have {damage} higher attack and defense.

            Cooldown: {cooldown}
            Range: {range}";
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
