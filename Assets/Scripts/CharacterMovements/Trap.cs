using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO:
1) Change what each attack inherits from
1a) Add to the constructors/add more fields if needed
2) Change damage/range/cooldown as you want in the constructors
2) Implement execute, sendEvent, eventExecute
*/

public class Trap : MonoBehaviour
{
    public GameObject sprites;
    private int damage;
    TrapperMovement trapper;
    private int x;
    private int y;

    public void Init(TrapperMovement trapper, int damage)
    {
        this.trapper = trapper;
        sprites.SetActive(!trapper.isEnemy);
        this.damage = damage;
        this.x = trapper.GetX();
        this.y = trapper.GetY();
        GridManager.Instance.InsertTrap(this, x, y);
    }

    public void Trigger(CharacterMovement character)
    {
        if (character.IsEnemyOf(trapper)) {
            character.TakeDamage(trapper.GetAttack(), damage);
            character.AddBuff(new DisabledDebuff(2));
            character.AddBuff(new VisibleDebuff(2));
            GridManager.Instance.RemoveTrap(x, y);
            GameObject.Destroy(this.gameObject);
            trapper.RemoveTrap(this);
        }
    }

    public void Explode(int damage)
    {
        GridManager.Instance.RemoveTrap(x, y);
        List<CharacterMovement> enemies = GridManager.Instance.GetAllCharactersInAOE(x, y)
            .FindAll(cm => cm.IsEnemyOf(trapper));

        enemies.ForEach(cm => cm.TakeDamage(trapper.GetAttack(), damage));
        GameObject.Destroy(this.gameObject);
    }
}
