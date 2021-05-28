using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static GridManager grid;
    private static ActionQueue queue;
    public bool isControllable;
    public int meleeDamage = 10;

    private bool isActive = false;
    public HealthBar hp;
    public SelectionAura selection;
    public string faceDirection = "up";

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
            if (Input.GetButtonDown("MeleeAttack")) {
                queue.EnqueueAction(new MeleeAttack(this, meleeDamage));
            }
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
