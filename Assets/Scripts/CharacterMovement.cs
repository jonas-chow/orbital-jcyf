using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static GridManager grid;
    private static ActionQueue queue;

    private bool isActive = false;
    public HealthBar hp;
    private SelectionAura selection;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponentInChildren<HealthBar>();
        hp.SetVisible(isActive);

        selection = GetComponentInChildren<SelectionAura>();
        selection.SetSelect(isActive);

        if (grid == null) {
            grid = GameObject.FindObjectOfType<GridManager>();
            grid.init();
        }

        if (queue == null) {
            queue = gameObject.GetComponentInParent<ActionQueue>();
        }

        grid.InsertObject(gameObject, (int) transform.localPosition.x, (int) transform.localPosition.y);
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

        hp = GetComponentInChildren<HealthBar>();
        hp.SetVisible(true);

        selection = GetComponentInChildren<SelectionAura>();
        selection.SetSelect(true);
    }
}
