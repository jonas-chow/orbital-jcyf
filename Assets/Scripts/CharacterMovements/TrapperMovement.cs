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

public class TrapperMovement : CharacterMovement
{
    public GameObject trapPrefab;
    private List<Trap> traps = new List<Trap>();

    private class Attack1 : LinearAttack
    {
        public TrapperMovement self;

        public Attack1(TrapperMovement cm)
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
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                target.TakeDamage(self.GetAttack(), damage);
                AudioManager.Instance.Play("ArrowHit");
            }
            AudioManager.Instance.Play("ArrowMiss");
        }

        public override void SendEvent()
        {
            object[] extraData = new object[] {direction};
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
    }

    private class Attack2 : SelfAttack
    {
        public TrapperMovement self;

        public Attack2(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 0;
            this.cooldown = 10;
            this.damage = 10;
        }

        public override void Execute()
        {
            SendEvent();
            if (GridManager.Instance.traps[GetX(), GetY()] == null) {
                GameObject trapObj = GameObject.Instantiate(self.trapPrefab, Vector3.zero, Quaternion.identity);
                Trap trap = trapObj.GetComponent<Trap>();
                self.traps.Add(trap);
                trap.Init(self, damage);
                AudioManager.Instance.Play("Trap");
            } else {
                self.attack2Turn = -999;
            }
        }

        public override void SendEvent()
        {
            object[] extraData = null; 
            EventHandler.Instance.SendAttackEvent(self.charID, 2, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            if (GridManager.Instance.traps[GetX(), GetY()] == null) {
                GameObject trapObj = GameObject.Instantiate(self.trapPrefab, Vector3.zero, Quaternion.identity);
                Trap trap = trapObj.GetComponent<Trap>();
                self.traps.Add(trap);
                trap.Init(self, damage);
                AudioManager.Instance.Play("Trap");
            }
        }
    }

    private class Attack3 : SelfAttack
    {
        public TrapperMovement self;

        public Attack3(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 0;
            this.damage = 30;
            this.cooldown = 20;
        }

        public override void Execute()
        {
            SendEvent();
            self.traps.ForEach(trap => trap.Explode(damage));
            self.traps = new List<Trap>();
            AudioManager.Instance.Play("TrapExplosion");
        }

        public override void SendEvent()
        {
            object[] extraData = null;
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            self.traps.ForEach(trap => trap.Explode(damage));
            self.traps = new List<Trap>();
            AudioManager.Instance.Play("TrapExplosion");
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

    public void RemoveTrap(Trap trap)
    {
        traps.Remove(trap);
    }
}
