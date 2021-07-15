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

public class AssassinMovement : CharacterMovement
{
    public static int _hp = 100;
    public static int _attack = 20;
    public static int _defense = 10;

    public ParticleSystem smokeEffect;
    public ParticleSystem slashEffect;
    public ParticleSystem heavySlashEffect;
    public ParticleSystem poisonEffect;

    public class Attack1 : LinearAttack
    {
        public AssassinMovement self;
        public int backstabBonus;

        public Attack1(AssassinMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 10;
            this.backstabBonus = 10;
            this.cooldown = 2;
            this.type = "attack";
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                if (target.faceDirection == self.faceDirection) {
                    target.TakeDamage(self.GetAttack(), damage + backstabBonus);
                    ParticleSystem heavySlashEffect = ParticleSystem.Instantiate(self.heavySlashEffect, 
                        GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                } else {
                    target.TakeDamage(self.GetAttack(), damage);
                    ParticleSystem slashEffect = ParticleSystem.Instantiate(self.slashEffect, 
                        GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                }
                AudioManager.Instance.Play("MeleeHit");            
            }
            AudioManager.Instance.Play("MeleeMiss");
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
                if (target.faceDirection == self.faceDirection) {
                    target.TakeDamage(self.GetAttack(), damage + backstabBonus);
                    ParticleSystem heavySlashEffect = ParticleSystem.Instantiate(self.heavySlashEffect, 
                        GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                } else {
                    target.TakeDamage(self.GetAttack(), damage);
                    ParticleSystem slashEffect = ParticleSystem.Instantiate(self.slashEffect, 
                        GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                }
                AudioManager.Instance.Play("MeleeHit");   
            }
            AudioManager.Instance.Play("MeleeMiss");
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage to the target in front of you.\n" +
            $"Does an extra {backstabBonus} damage if you attack the target from the back.\n\n" + 
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack2 : LinearAttack
    {
        public AssassinMovement self;

        public Attack2(AssassinMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.range = 1;
            this.damage = 10;
            this.cooldown = 10;
            this.type = "attack";
        }

        public override void Execute()
        {
            SendEvent();
            CharacterMovement target = FindTarget(direction);
            if (target != null && target.IsEnemyOf(self)) {
                target.AddBuff(new PoisonDebuff(3));
                target.TakeDamage(self.GetAttack(), damage);
                ParticleSystem poisonEffect = ParticleSystem.Instantiate(self.poisonEffect, 
                    GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                AudioManager.Instance.Play("Poison");
            }
            AudioManager.Instance.Play("MeleeMiss");
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
                target.AddBuff(new PoisonDebuff(3));
                target.TakeDamage(self.GetAttack(), damage);
                ParticleSystem poisonEffect = ParticleSystem.Instantiate(self.poisonEffect, 
                    GridManager.Instance.GetCoords(target.GetX(), target.GetY()), Quaternion.identity);
                AudioManager.Instance.Play("Poison");
            }
            AudioManager.Instance.Play("MeleeMiss");
        }

        public override string GetDescription()
        {
            return $"Deals {damage} damage to the target in front of you.\n" +
            $"Also poisons the target, dealing 10 damage at the end of each turn.\n\n" +
            $"Cooldown: {cooldown}\nRange: {range}";
        }
    }

    public class Attack3 : SelfAttack
    {
        public AssassinMovement self;

        public Attack3(AssassinMovement cm)
        {
            this.character = cm;
            this.self = cm;
            this.cooldown = 40;
            this.type = "buff";
        }

        public override void Execute()
        {
            SendEvent();
            self.AddBuff(new StealthBuff(2));
            ParticleSystem smokeEffect = ParticleSystem.Instantiate(self.smokeEffect, 
                GridManager.Instance.GetCoords(GetX(), GetY()), Quaternion.identity);
            AudioManager.Instance.Play("Stealth");
        }

        public override void SendEvent()
        {
            object[] extraData = null;
            EventHandler.Instance.SendAttackEvent(self.charID, 3, extraData);
        }

        public override void EventExecute(object[] extraData)
        {
            self.AddBuff(new StealthBuff(2));
            ParticleSystem smokeEffect = ParticleSystem.Instantiate(self.smokeEffect, 
                GridManager.Instance.GetCoords(GetX(), GetY()), Quaternion.identity);
            AudioManager.Instance.Play("Stealth");
        }

        public override string GetDescription()
        {
            return $"Go into stealth until the start of your next turn.\n" +
            $"Enemies cannot see you when you are stealthed, but you will be revealed if " +
            $"you take damage.\n\nCooldown: {cooldown}";
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
