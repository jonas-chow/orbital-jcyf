using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject enemyColor;
    public GameObject damageText;
    public bool isEnemy;
    public GameObject fog;
    public int charID;
    private bool isActive = false;
    public HealthBar hp;
    public GameObject selection;
    public string faceDirection = "up";

    private bool aiming = false;
    public enum AttackTypes
    {
        Melee = 1,
        Ranged = 2,
        MeleeAOE = 3,
        RangedAOE = 4
    }

    [Header("Attack 1")]
    public AttackTypes attack1Type;
    public int attack1Damage;
    public int attack1Range;
    public int attack1Cooldown;
    private int attack1Turn = -999;

    [Header("Attack 2")]
    public AttackTypes attack2Type;
    public int attack2Damage;
    public int attack2Range;
    public int attack2Cooldown;
    private int attack2Turn = -999;

    [Header("Attack 3")]
    public AttackTypes attack3Type;
    public int attack3Damage;
    public int attack3Range;
    public int attack3Cooldown;
    private int attack3Turn = -999;

    private int attackNum;

    private Attack attack;

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
                }
                if (Input.GetButtonDown("Vertical")) {
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
                }
                if (Input.GetButtonDown("Attack1") && 
                    GameManager.Instance.actionCount - attack1Turn > attack1Cooldown) {
                    AimingMode(1);
                }
                if (Input.GetButtonDown("Attack2") && 
                    GameManager.Instance.actionCount - attack2Turn > attack2Cooldown) {
                    AimingMode(2);
                }
                if (Input.GetButtonDown("Attack3") && 
                    GameManager.Instance.actionCount - attack3Turn > attack3Cooldown) {
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
                }

                if (Input.GetButtonDown("Horizontal")) {
                    if (Input.GetAxis("Horizontal") > 0) {
                        attack.AimRight();
                    } else {
                        attack.AimLeft();
                    }
                }
                if (Input.GetButtonDown("Vertical")) {
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
        attackNum = attackNumber;
        switch (attackNumber)
        {
            case 1:
                attack = NewAttack(attack1Type, attack1Damage, attack1Range);
                break;
            case 2:
                attack = NewAttack(attack2Type, attack2Damage, attack2Range);
                break;
            case 3:
                attack = NewAttack(attack3Type, attack3Damage, attack3Range);
                break;
        }
    }

    private void DisableAiming()
    {
        aiming = false;
        Attack.ClearIndicators();
        Attack.ClearLimits();
    }

    private Attack NewAttack(AttackTypes type, int damage, int range)
    {
        switch (type)
        {
            case AttackTypes.Melee:
                return new MeleeAttack(this, damage);
            case AttackTypes.Ranged:
                return new RangedAttack(this, damage, range);
            case AttackTypes.MeleeAOE:
                return new MeleeAOEAttack(this, damage);
            case AttackTypes.RangedAOE:
                return new RangedAOEAttack(this, damage, range);
            default:
                return null;
        }
    }

    public void Activate()
    {
        isActive = true;
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

    private int getX()
    {
        return GridManager.Instance.GetX(transform.position.x);
    }

    private int getY()
    {
        return GridManager.Instance.GetY(transform.position.y);
    }

    public void TakeDamage(int damage)
    {
        GameObject
            .Instantiate(damageText, this.transform.position + Vector3.up * 0.8f, Quaternion.identity)
            .GetComponent<DamageText>()
            .SetText(damage.ToString());

        // if died
        if (hp.TakeDamage(damage, isEnemy ? -1 : charID)) {
            GridManager.Instance.RemoveObject(getX(), getY());
            GameObject.Destroy(gameObject);
            if (isEnemy) {
                GameManager.Instance.RemoveEnemy();
            } else {
                GameManager.Instance.RemoveFriendly(this);
            }
        }
    }

    public void Move(string direction)
    {
        switch (direction)
        {
            case "up":
                if (GridManager.Instance.MoveObject(getX(), getY(), getX(), getY() + 1)) {
                    transform.position += Vector3.up;
                }
                Face("up");
                break;
            case "down":
                if (GridManager.Instance.MoveObject(getX(), getY(), getX(), getY() - 1)) {
                    transform.position += Vector3.down;
                }
                Face("down");
                break;
            case "left":
                if (GridManager.Instance.MoveObject(getX(), getY(), getX() - 1, getY())) {
                    transform.position += Vector3.left;
                }
                Face("left");
                break;
            case "right":
                if (GridManager.Instance.MoveObject(getX(), getY(), getX() + 1, getY())) {
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
                transform.up = Vector3.up;
                faceDirection = "up";
                hp.transform.up = Vector3.up;
                hp.transform.localPosition = new Vector3(0, 0.55f, 0);
                break;
            case "down":
                transform.up = Vector3.down;
                faceDirection = "down";
                hp.transform.localPosition = new Vector3(0, -0.55f, 0);
                break;
            case "left":
                transform.up = Vector3.left;
                faceDirection = "left";
                hp.transform.up = Vector3.up;
                hp.transform.localPosition = new Vector3(0.55f, 0, 0);
                break;
            case "right":
                transform.up = Vector3.right;
                faceDirection = "right";
                hp.transform.localPosition = new Vector3(-0.55f, 0, 0);
                break;
        }

        hp.transform.up = Vector3.up;
    }

    public void SetEnemy(bool isEnemy)
    {
        this.isEnemy = isEnemy;
        enemyColor.SetActive(isEnemy);
        fog.SetActive(!isEnemy);
    }
}
