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
    public static int _hp = 100;
    public static int _attack = 15;
    public static int _defense = 15;

    public GameObject ward;
    public ParticleSystem arrowEffect;
    public ParticleSystem arrowImpactEffect;

    public class Attack1 : LinearAttack
    {
        private ScoutMovement self;

        public Attack1(ScoutMovement cm)
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
                AudioManager.Instance.Play("ArrowHit");
            }
            ParticleSystem arrowEffect = ParticleSystem.Instantiate(self.arrowEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
            self.RotateProjectileEffect(self.faceDirection, arrowEffect);
            AudioManager.Instance.Play("ArrowMiss");
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

    public class Attack2 : RangedTargetAttack
    {
        private ScoutMovement self;

        public Attack2(ScoutMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = globalRange;
            this.cooldown = 10;
            this.damage = 10;
            this.type = "other";
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
            CharacterMovement target = FindTarget(offsetX, offsetY);
            if (target == null) {
                GameObject ward = GameObject.Instantiate(self.ward, Vector3.zero, Quaternion.identity);
                ward.GetComponent<WardMovement>().Init(self);
                GridManager.Instance.MoveToAndInsert(ward, GetX() + offsetX, GetY() + offsetY);
                AudioManager.Instance.Play("Ward");
            } else if (target.IsEnemyOf(self)) {
                target.AddBuff(new VisibleDebuff(2));
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("Ward");
            } else {
                // refund cooldown if it whiffed
                self.ResetCD(2);
            }
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
            return $"Places a ward at the target position that grants vision.\n" +
            $"If cast on an enemy, will deal {damage} to the target instead.\n" +
            $"Will also grant vision of the target until the start of the next turn.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack3 : RangedAOEAttack
    {
        private ScoutMovement self;

        public Attack3(ScoutMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = globalRange;
            this.damage = 20;
            this.cooldown = 10;
            this.type = "attack";
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
            List<CharacterMovement> enemies = FindTargets(offsetX, offsetY)
                .FindAll(cm => cm.IsEnemyOf(self));
            enemies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
                cm.AddBuff(new VisibleDebuff(4));
            });
            ParticleSystem arrowImpactEffect = ParticleSystem.Instantiate(self.arrowImpactEffect, 
                GridManager.Instance.GetCoords(self.GetX() + offsetX, self.GetY() + offsetY), Quaternion.identity);
            AudioManager.Instance.Play("Scout3");
        }

        public override void SendEvent()
        {
            object[] extraData = InvertOffsets();
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            this.offsetX = (int)extraData[0];
            this.offsetY = (int)extraData[1];
            Execute();
        }

        public override string GetDescription()
        {
            return $"Deals {damage} to all enemies in an area of effect.\n" +
            $"Also grants vision of every enemy hit for 4 turns.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
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
