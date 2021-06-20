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
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 15;
            this.cooldown = 2;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget();
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
            CharacterMovement target = FindEventTarget(dir);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("ArrowHit");
            }
            AudioManager.Instance.Play("ArrowMiss");
        }
    }

    private class Attack2 : RangedTargetAttack
    {
        public ScoutMovement self;

        public Attack2(ScoutMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = globalRange;
            this.cooldown = 10; // change this
            this.damage = 5;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget();
            if (target == null) {
                GameObject ward = GameObject.Instantiate(self.ward, Vector3.zero, Quaternion.identity);
                ward.GetComponent<WardMovement>().init(false);
                GridManager.Instance.MoveToAndInsert(ward, posX, posY);
                AudioManager.Instance.Play("Ward");
            } else if (target.IsEnemyOf(self)) {
                target.AddBuff(new VisibleDebuff(2));
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("Ward");
            } else {
                // refund cooldown if it whiffed
                self.attack2Turn = -999;
            }
            FaceTargetDirection(offsetX, offsetY);
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
            CharacterMovement target = FindEventTarget(offsetX, offsetY);
            if (target == null) {
                GameObject ward = GameObject.Instantiate(self.ward, Vector3.zero, Quaternion.identity);
                ward.GetComponent<WardMovement>().init(true);
                GridManager.Instance.MoveToAndInsert(ward, posX, posY);
                AudioManager.Instance.Play("Ward");
            } else if (target.IsEnemyOf(self)) {
                target.AddBuff(new VisibleDebuff(2));
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("Ward");
            }
            FaceTargetDirection(offsetX, offsetY);
        }
    }

    private class Attack3 : RangedAOEAttack
    {
        public ScoutMovement self;

        public Attack3(ScoutMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = globalRange;
            this.damage = 15;
            this.cooldown = 3;
        }

        public override void Execute()
        {
            SendEvent();
            List<CharacterMovement> enemies = FindTargets().FindAll(cm => cm.IsEnemyOf(self));
            enemies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            AudioManager.Instance.Play("Scout3");
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
            List<CharacterMovement> allies = FindEventTargets(offsetX, offsetY)
                .FindAll(cm => cm.IsEnemyOf(self));
            allies.ForEach(cm => {
                cm.TakeDamage(self.GetAttack(), damage);
            });
            FaceTargetDirection(offsetX, offsetY);
            AudioManager.Instance.Play("Scout3");
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
