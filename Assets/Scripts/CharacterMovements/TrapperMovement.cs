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
    public static int _hp = 100;
    public static int _attack = 15;
    public static int _defense = 15;

    public GameObject trapPrefab;
    private List<Trap> traps = new List<Trap>();
    public ParticleSystem arrowEffect;

    public class Attack1 : LinearAttack
    {
        public TrapperMovement self;

        public Attack1(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 5;
            this.damage = 20;
            this.cooldown = 2;
            this.type = "attack";
        }

        public override void Execute()
        {
            SendEvent();
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
            ParticleSystem arrowEffect = ParticleSystem.Instantiate(self.arrowEffect, 
                GridManager.Instance.GetCoords(self.GetX(), self.GetY()), Quaternion.identity);
            self.RotateProjectileEffect(self.faceDirection, arrowEffect);
            AudioManager.Instance.Play("ArrowMiss");
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage to the first target in front of you.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack2 : SelfAttack
    {
        public TrapperMovement self;

        public Attack2(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 0;
            this.cooldown = 10;
            this.damage = 10;
            this.type = "other";
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
                self.ResetCD(2);
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

        public override string GetDescription()
        {
            return $"Sets a trap at your position.\n" +
            $"Traps do {damage} damage when an enemy steps on them.\n" +
            $"Traps also disable the enemy and grants you vision of them for 3 turns.\n\n" +
            $"Cooldown: {cooldown}";
        }
    }

    public class Attack3 : SelfAttack
    {
        public TrapperMovement self;

        public Attack3(TrapperMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 0;
            this.damage = 30;
            this.cooldown = 20;
            this.type = "attack";
        }

        public override void Execute()
        {
            SendEvent();
            self.traps.ForEach(trap => trap.Explode(damage));
            self.traps = new List<Trap>();
            //AudioManager.Instance.Play("TrapExplosion");
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
            //AudioManager.Instance.Play("TrapExplosion");
        }

        public override string GetDescription()
        {
            return $"All traps explode, dealing {damage} damage in an area of effect.\n\n" +
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

    public void RemoveTrap(Trap trap)
    {
        traps.Remove(trap);
    }
}
