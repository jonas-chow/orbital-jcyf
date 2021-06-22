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

public class HunterMovement : CharacterMovement
{
    public static int _hp = 100;
    public static int _attack = 20;
    public static int _defense = 10;

    public class Attack1 : LinearAttack
    {
        public HunterMovement self;

        public Attack1(HunterMovement cm)
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
                AudioManager.Instance.Play("ArrowHit");
            }
            AudioManager.Instance.Play("ArrowMiss");
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
                AudioManager.Instance.Play("ArrowHit");
            }
            AudioManager.Instance.Play("ArrowMiss");
        }

        public override string GetDescription()
        {
            return $@"
            Deals {damage} damage to the first target in front of you. 

            Cooldown: {cooldown}
            Range: {range}";
        }
    }

    public class Attack2 : LinearAttack
    {
        public HunterMovement self;
        public int distanceScaling;

        public Attack2(HunterMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = globalRange;
            this.damage = 10;
            this.distanceScaling = 3;
            this.cooldown = 10;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                int distance = GridManager.Instance.DistanceFromChar(self, target);
                int dmg = damage + distance * distanceScaling;
                target.TakeDamage(self.GetAttack(), dmg);
                AudioManager.Instance.Play("ArrowHit");
            }
            AudioManager.Instance.Play("ArrowMiss");
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {InvertDirection(direction)};
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            string dir = (string)extraData[0];
            CharacterMovement target = FindTarget(dir);
            if (target != null && target.IsEnemyOf(self)) {
                int distance = GridManager.Instance.DistanceFromChar(self, target);
                int dmg = damage + distance * distanceScaling;
                target.TakeDamage(self.GetAttack(), dmg);
                AudioManager.Instance.Play("ArrowHit");
            }
            AudioManager.Instance.Play("ArrowMiss");
        }

        public override string GetDescription()
        {
            return $@"
            Deals {damage} damage to the first target in front of you. 

            Deals an additional {distanceScaling} damage for each tile between you and the target.

            Cooldown: {cooldown}
            Range: {range}";
        }
    }

    public class Attack3 : LinearAttack
    {
        public HunterMovement self;
        public int knockback;

        public Attack3(HunterMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 15;
            this.knockback = 2;
            this.cooldown = 5;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);

                if (target.isAlive) {
                    string targetDir = target.faceDirection;
                    for (int i = 0; i < knockback; i++) {
                        target.Move(self.faceDirection);
                    }
                    target.Face(targetDir);
                }
                AudioManager.Instance.Play("Knockback");
            }
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {InvertDirection(direction)};
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            string dir = (string)extraData[0];
            CharacterMovement target = FindTarget(dir);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);

                if (target.isAlive) {
                    string targetDir = target.faceDirection;
                    for (int i = 0; i < knockback; i++) {
                        target.Move(self.faceDirection);
                    }
                    target.Face(targetDir);
                }
                AudioManager.Instance.Play("Knockback");
            }
        }

        public override string GetDescription()
        {
            return $@"
            Deals {damage} damage to the target in front of you.

            Also knocks the target back {knockback} tiles.

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
