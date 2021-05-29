using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static GridManager grid;
    private static ActionQueue queue;
    public bool isControllable;

    private bool isActive = false;
    public HealthBar hp;
    public SelectionAura selection;
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

    [Header("Attack 2")]
    public AttackTypes attack2Type;
    public int attack2Damage;
    public int attack2Range;

    [Header("Attack 3")]
    public AttackTypes attack3Type;
    public int attack3Damage;
    public int attack3Range;

    [Header("Attack 4")]
    public AttackTypes attack4Type;
    public int attack4Damage;
    public int attack4Range;

    private int attackNum;

    private Attack attack;

    // Start is called before the first frame update
    void Start()
    {
        hp.SetVisible(isActive);
        if (isControllable) {
            selection.SetSelect(isActive);
        }

        if (grid == null) {
            grid = GameObject.FindObjectOfType<GridManager>();
            grid.init();
        }

        if (queue == null) {
            queue = gameObject.GetComponentInParent<ActionQueue>();
        }

        grid.InsertObject(gameObject, getX(), getY());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (!aiming) {
                if (Input.GetButtonDown("Horizontal")) {
                    if (Input.GetAxis("Horizontal") > 0) {
                        if (Input.GetButton("Stay Still")) {
                            queue.EnqueueAction(new FaceRight(this));
                        } else {
                            queue.EnqueueAction(new MoveRight(this));
                        }
                    } else {
                        if (Input.GetButton("Stay Still")) {
                            queue.EnqueueAction(new FaceLeft(this));
                        } else {
                            queue.EnqueueAction(new MoveLeft(this));
                        }
                    }
                }
                if (Input.GetButtonDown("Vertical")) {
                    if (Input.GetAxis("Vertical") > 0) {
                        if (Input.GetButton("Stay Still")) {
                            queue.EnqueueAction(new FaceUp(this));
                        } else {
                            queue.EnqueueAction(new MoveUp(this));
                        }
                    } else {
                        if (Input.GetButton("Stay Still")) {
                            queue.EnqueueAction(new FaceDown(this));
                        } else {
                            queue.EnqueueAction(new MoveDown(this));
                        }
                    }
                }
                if (Input.GetButtonDown("Attack1")) {
                    AimingMode(1);
                }
                if (Input.GetButtonDown("Attack2")) {
                    AimingMode(2);
                }
                if (Input.GetButtonDown("Attack3")) {
                    AimingMode(3);
                }
                if (Input.GetButtonDown("Attack4")) {
                    AimingMode(4);
                }
            } else {
                // aiming mode
                if ((attackNum == 1 && Input.GetButtonUp("Attack1")) ||
                    (attackNum == 2 && Input.GetButtonUp("Attack2")) ||
                    (attackNum == 3 && Input.GetButtonUp("Attack3")) ||
                    (attackNum == 4 && Input.GetButtonUp("Attack4")) ) {
                    aiming = false;
                    queue.EnqueueAction(attack);
                    queue.Enable();
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
        queue.Disable();
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
            case 4:
                attack = NewAttack(attack4Type, attack4Damage, attack4Range);
                break;
        }
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
        selection.SetSelect(true);
    }

    public void Deactivate()
    {
        isActive = false;
        hp.SetVisible(false);
        selection.SetSelect(false);

        // in case turn ends and player was in the middle of aiming
        aiming = false;
        queue.Enable();
    }

    // For Game Manager to call, to ensure that this initial state happens regardless of order of start() being called
    public void init()
    {
        isActive = true;

        hp.SetVisible(true);
        selection.SetSelect(true);
    }
    private int getX()
    {
        return (int) transform.localPosition.x;
    }

    private int getY()
    {
        return (int) transform.localPosition.y;
    }

    public void TakeDamage(int damage)
    {
        if (hp.TakeDamage(damage)) {
            grid.RemoveObject(getX(), getY());
            GameObject.Destroy(gameObject);
        }
    }
}
