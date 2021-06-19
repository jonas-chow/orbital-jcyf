using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
    public GameObject stealthedSprite;
    public GameObject friendlySprite;
    public GameObject enemySprite;
    public GameObject selection;
    // so you can rotate the sprites without rotating the hp
    public GameObject overallSprites;

    public GameObject damageText;
    public bool isEnemy;
    public GameObject fog;

    // 0 for warrior, 1 for ranged, 2 for mage
    public int charID;
    private bool isActive = false;
    public HealthBar hp;
    public string faceDirection = "up";

    public int atk;
    public int def;
    public bool invincible = false;
    public int atkBuff = 0;
    public int defBuff = 0;
    public bool disabled;
    public bool stealthed;
    public bool poisoned;
    public bool isAlive = true;

    private bool aiming = false;

    // currently aimed attack
    protected int attackNum;
    protected Attack attack;

    public Attack attack1;
    public Attack attack2;
    public Attack attack3;

    public int attack1Turn = -999;
    public int attack2Turn = -999;
    public int attack3Turn = -999;

    public List<Buff> buffs = new List<Buff>();

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (!aiming) {
                if (Input.GetButtonDown("Horizontal")) {
                    if (Input.GetAxis("Horizontal") > 0) {
                        if (Input.GetButton("Stay Still")) {
                            ActionQueue.Instance.EnqueueAction(new FaceRight(this));
                        } else {
                            ActionQueue.Instance.EnqueueAction(new MoveRight(this));
                        }
                    } else {
                        if (Input.GetButton("Stay Still")) {
                            ActionQueue.Instance.EnqueueAction(new FaceLeft(this));
                        } else {
                            ActionQueue.Instance.EnqueueAction(new MoveLeft(this));
                        }
                    }
                } else if (Input.GetButtonDown("Vertical")) {
                    if (Input.GetAxis("Vertical") > 0) {
                        if (Input.GetButton("Stay Still")) {
                            ActionQueue.Instance.EnqueueAction(new FaceUp(this));
                        } else {
                            ActionQueue.Instance.EnqueueAction(new MoveUp(this));
                        }
                    } else {
                        if (Input.GetButton("Stay Still")) {
                            ActionQueue.Instance.EnqueueAction(new FaceDown(this));
                        } else {
                            ActionQueue.Instance.EnqueueAction(new MoveDown(this));
                        }
                    }
                } else if (Input.GetButtonDown("Attack1") && 
                    GameManager.Instance.actionCount - attack1Turn > attack1.cooldown) {
                    AimingMode(1);
                } else if (Input.GetButtonDown("Attack2") && 
                    GameManager.Instance.actionCount - attack2Turn > attack2.cooldown) {
                    AimingMode(2);
                } else if (Input.GetButtonDown("Attack3") && 
                    GameManager.Instance.actionCount - attack3Turn > attack3.cooldown) {
                    AimingMode(3);
                }
            } else {
                // aiming mode
                if ((attackNum == 1 && Input.GetButtonUp("Attack1")) ||
                    (attackNum == 2 && Input.GetButtonUp("Attack2")) ||
                    (attackNum == 3 && Input.GetButtonUp("Attack3")) ) {
                    switch (attackNum) {
                        case 1:
                            attack1Turn = GameManager.Instance.actionCount;
                            break;
                        case 2:
                            attack2Turn = GameManager.Instance.actionCount;
                            break;
                        case 3:
                            attack3Turn = GameManager.Instance.actionCount;
                            break;
                    }

                    DisableAiming();
                    ActionQueue.Instance.EnqueueAction(attack);
                    CharacterMenu.Instance.UseSkill(charID, attackNum - 1);
                    ActionQueue.Instance.Enable();
                } else if (Input.GetButtonDown("Horizontal")) {
                    if (Input.GetAxis("Horizontal") > 0) {
                        attack.AimRight();
                    } else {
                        attack.AimLeft();
                    }
                } else if (Input.GetButtonDown("Vertical")) {
                    if (Input.GetAxis("Vertical") > 0) {
                        attack.AimUp();
                    } else {
                        attack.AimDown();
                    }
                }
            }
        }
    }

    private void AimingMode(int attackNumber)
    {
        aiming = true;
        ActionQueue.Instance.Disable();
        SetupAttack(attackNumber);
    }

    public abstract void SetupAttack(int attackNumber);

    private void DisableAiming()
    {
        aiming = false;
        Attack.ClearIndicators();
        Attack.ClearLimits();
    }

    public void Activate()
    {
        if (!disabled) {
            isActive = true;
        }
        hp.SetVisible(true);
        selection.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        hp.SetVisible(false);
        selection.SetActive(false);

        // in case turn ends and player was in the middle of aiming
        DisableAiming();
        ActionQueue.Instance.Enable();
    }

    public void Disable()
    {
        disabled = true;
        isActive = false;
    }

    public int GetX()
    {
        return GridManager.Instance.GetX(transform.position.x);
    }

    public int GetY()
    {
        return GridManager.Instance.GetY(transform.position.y);
    }

    public virtual void TakeDamage(float enemyAtk, int damage)
    {
        // enemy attack is a float so that the division happens as a float
        damage = (int)((enemyAtk / GetDefense()) * damage);
        if (invincible) {
            damage = 0;
        }

        GameObject
            .Instantiate(damageText, this.transform.position + Vector3.up * 0.8f, Quaternion.identity)
            .GetComponent<DamageText>()
            .SetText(damage.ToString());

        if (stealthed) {
            stealthed = false;
            Buff stealthBuff = buffs.Find(buff => typeof(StealthBuff).IsInstanceOfType(buff));
            stealthBuff.Remove();
            buffs.Remove(stealthBuff);
        }

        // if died
        if (hp.TakeDamage(damage, isEnemy ? -1 : charID)) {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        GameObject
            .Instantiate(damageText, this.transform.position + Vector3.up * 0.8f, Quaternion.identity)
            .GetComponent<DamageText>()
            .SetText(healAmount.ToString());

        hp.TakeDamage(-healAmount, isEnemy ? -1 : charID);
    }

    public virtual void Die()
    {
        GridManager.Instance.RemoveObject(GetX(), GetY());
        GameObject.Destroy(gameObject);
        isAlive = false;
        if (isEnemy) {
            GameManager.Instance.RemoveEnemy();
        } else {
            GameManager.Instance.RemoveFriendly(this);
        }
    }

    public void Move(string direction)
    {
        switch (direction)
        {
            case "up":
                if (GridManager.Instance.MoveObject(GetX(), GetY(), GetX(), GetY() + 1)) {
                    transform.position += Vector3.up;
                }
                Face("up");
                break;
            case "down":
                if (GridManager.Instance.MoveObject(GetX(), GetY(), GetX(), GetY() - 1)) {
                    transform.position += Vector3.down;
                }
                Face("down");
                break;
            case "left":
                if (GridManager.Instance.MoveObject(GetX(), GetY(), GetX() - 1, GetY())) {
                    transform.position += Vector3.left;
                }
                Face("left");
                break;
            case "right":
                if (GridManager.Instance.MoveObject(GetX(), GetY(), GetX() + 1, GetY())) {
                    transform.position += Vector3.right;
                }
                Face("right");
                break;
        }
    }

    public void Face(string direction)
    {
        switch (direction)
        {
            case "up":
                overallSprites.transform.up = Vector3.up;
                faceDirection = "up";
                break;
            case "down":
                overallSprites.transform.up = Vector3.down;
                faceDirection = "down";
                break;
            case "left":
                overallSprites.transform.up = Vector3.left;
                faceDirection = "left";
                break;
            case "right":
                overallSprites.transform.up = Vector3.right;
                faceDirection = "right";
                break;
        }
    }

    public void SetEnemy(bool isEnemy)
    {
        this.isEnemy = isEnemy;
        fog.SetActive(!isEnemy);
        enemySprite.SetActive(isEnemy);
        friendlySprite.SetActive(!isEnemy);
    }

    public void EventAttack(int attackId, object[] extraData)
    {
        switch (attackId)
        {
            case 1:
                attack1.EventExecute(extraData);
                break;
            case 2:
                attack2.EventExecute(extraData);
                break;
            case 3:
                attack3.EventExecute(extraData);
                break;
        }
    }

    public int GetAttack()
    {
        int attack = atk + atkBuff;
        if (atk < 1) {
            return 1;
        } else {
            return attack;
        }
    }

    public int GetDefense()
    {
        int defense = def + defBuff;
        if (defense < 1) {
            return 1;
        } else {
            return defense;
        }
    }

    public void TurnPass()
    {
        buffs.RemoveAll(buff => buff.TurnPass());
    }

    public void AddBuff(Buff buff)
    {
        buff.Add(this);
        buffs.Add(buff);
    }

    public bool IsEnemyOf(CharacterMovement other)
    {
        return this.isEnemy != other.isEnemy;
    }
}
