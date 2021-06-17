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

public class SummonerMovement : CharacterMovement
{
    public GameObject familiarPrefab;
    public bool hasFamiliar = false;
    public FamiliarMovement familiarMovement = null;

    private class Attack1 : LinearAttack
    {
        public SummonerMovement self;

        public Attack1(SummonerMovement cm)
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

    private class Attack2 : RangedTargetAttack
    {
        public SummonerMovement self;

        public Attack2(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.cooldown = 5;
            this.damage = 20;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget();
            if (target == null) {
                // currently existing familiar explodes
                if (self.familiarMovement != null) {
                    self.familiarMovement.Die();
                }

                GameObject familiar = GameObject.Instantiate(self.familiarPrefab, Vector3.zero, Quaternion.identity);
                GridManager.Instance.MoveToAndInsert(familiar, posX, posY);
                self.familiarMovement = familiar.GetComponent<FamiliarMovement>();
                self.familiarMovement.init(false);
            } else if (target.isEnemy) {
                target.TakeDamage(self.GetAttack(), damage);
            } else {
                // refund cooldown if whiffed
                self.attack2Turn = -999;
            }
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
                // currently existing familiar explodes
                if (self.familiarMovement != null) {
                    self.familiarMovement.Die();
                }

                GameObject familiar = GameObject.Instantiate(self.familiarPrefab, Vector3.zero, Quaternion.identity);
                GridManager.Instance.MoveToAndInsert(familiar, posX, posY);
                self.familiarMovement = familiar.GetComponent<FamiliarMovement>();
                self.familiarMovement.init(true);
            } else if (!target.isEnemy) {
                target.TakeDamage(self.GetAttack(), damage);
            } 
        }
    }

    private class Attack3 : SelfAttack
    {
        public SummonerMovement self;

        public Attack3(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
        }

        public override void Execute()
        {
            SendEvent();
            Vector3 selfPos = self.transform.position;
            self.transform.position = self.familiarMovement.transform.position;
            self.familiarMovement.transform.position = selfPos;
            GridManager.Instance.RemoveObject(self.GetX(), self.GetY());
            GridManager.Instance.RemoveObject(self.familiarMovement.GetX(), self.familiarMovement.GetY());
            GridManager.Instance.InsertObject(self.gameObject, self.GetX(), self.GetY());
            GridManager.Instance.InsertObject(
                self.familiarMovement.gameObject, 
                self.familiarMovement.GetX(), 
                self.familiarMovement.GetY());
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 3, null);
        }

        public override void EventExecute(object[] extraData)
        {
            Vector3 selfPos = self.transform.position;
            self.transform.position = self.familiarMovement.transform.position;
            self.familiarMovement.transform.position = selfPos;
            GridManager.Instance.RemoveObject(self.GetX(), self.GetY());
            GridManager.Instance.RemoveObject(self.familiarMovement.GetX(), self.familiarMovement.GetY());
            GridManager.Instance.InsertObject(self.gameObject, self.GetX(), self.GetY());
            GridManager.Instance.InsertObject(
                self.familiarMovement.gameObject, 
                self.familiarMovement.GetX(), 
                self.familiarMovement.GetY());
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
