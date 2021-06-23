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
    public static int _hp = 100;
    public static int _attack = 15;
    public static int _defense = 15;

    public GameObject familiarPrefab;
    private FamiliarMovement familiarMovement = null;

    public class Attack1 : LinearAttack
    {
        public SummonerMovement self;

        public Attack1(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 15;
            this.cooldown = 2;
        }

        // basic melee attack
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

    public class Attack2 : RangedTargetAttack
    {
        public SummonerMovement self;

        public Attack2(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.cooldown = 5;
            this.damage = 30;
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget(offsetX, offsetY);
            if (target == null) {
                // currently existing familiar explodes
                if (self.familiarMovement != null) {
                    self.familiarMovement.Die();
                    AudioManager.Instance.Play("TrapExplosion");
                }

                GameObject familiar = GameObject.Instantiate(self.familiarPrefab, Vector3.zero, Quaternion.identity);
                GridManager.Instance.MoveToAndInsert(familiar, GetX() + offsetX, GetY() + offsetY);
                self.familiarMovement = familiar.GetComponent<FamiliarMovement>();
                self.familiarMovement.Init(self);
                AudioManager.Instance.Play("Summon");
            } else if (target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("FamiliarAttack");
            } else {
                // refund cooldown if whiffed
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
            int offsetX = (int)extraData[0];
            int offsetY = (int)extraData[1];
            CharacterMovement target = FindTarget(offsetX, offsetY);
            if (target == null) {
                // currently existing familiar explodes
                if (self.familiarMovement != null) {
                    self.familiarMovement.Die();
                    AudioManager.Instance.Play("TrapExplosion");
                }

                GameObject familiar = GameObject.Instantiate(self.familiarPrefab, Vector3.zero, Quaternion.identity);
                GridManager.Instance.MoveToAndInsert(familiar, GetX() + offsetX, GetY() + offsetY);
                self.familiarMovement = familiar.GetComponent<FamiliarMovement>();
                self.familiarMovement.Init(self);
                AudioManager.Instance.Play("Summon");
            } else if (target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("FamiliarAttack");
            } 
        }

        public override string GetDescription()
        {
            return $@"
            Summon a controllable familiar at the target position which explodes on death, dealing {damage} damage to all enemies around it.

            If cast on an enemy, will instead deal {damage} to the target.

            Cooldown: {cooldown}
            Range: {range}";
        }
    }

    public class Attack3 : SelfAttack
    {
        public SummonerMovement self;

        public Attack3(SummonerMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 5;
        }

        public override void Execute()
        {
            if (self.familiarMovement != null) {
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
                AudioManager.Instance.Play("Swap");
            } else {
                // refund cooldown if whiffed
                self.ResetCD(3);
            }
        }

        public override void SendEvent()
        {
            EventHandler.Instance.SendAttackEvent(self.charID, 3, null);
        }

        public override void EventExecute(object[] extraData)
        {
            if (self.familiarMovement != null) {
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
                AudioManager.Instance.Play("Swap");
            }
        }

        public override string GetDescription()
        {
            return $@"
            Swap places with the familiar.

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

    public override void Die()
    {
        // familiar also dies when summoner dies
        if (familiarMovement != null) {
            familiarMovement.Die();
        }
        base.Die();
    }

    public void FamiliarDied()
    {
        familiarMovement = null;
    }
}
