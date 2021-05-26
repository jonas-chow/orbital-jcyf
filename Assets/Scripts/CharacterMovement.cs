using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private static GridManager grid;
    private static GameManager gm;

    private bool isActive = false;
    private HealthBar hp;
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
        }
        if (gm == null) {
            gm = gameObject.GetComponentInParent<GameManager>();
        }

        grid.InsertObject(gameObject, getX(), getY());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (Input.GetButtonDown("Horizontal")) {
                if (Input.GetAxis("Horizontal") > 0) {
                    if (!Input.GetButton("Stay Still") && grid.MoveObject(getX(), getY(), getX() + 1, getY())) {
                        transform.position += Vector3.right;
                    }
                    // face right
                    transform.up = Vector3.right;
                    // hp bar stays on top
                    hp.transform.up = Vector3.up;
                    hp.transform.localPosition = new Vector3(-0.55f, 0, 0);
                } else {
                    if (!Input.GetButton("Stay Still") && grid.MoveObject(getX(), getY(), getX() - 1, getY())) {
                        transform.position += Vector3.left;
                    }
                    // face left
                    transform.up = Vector3.left;
                    // hp bar stays on top
                    hp.transform.up = Vector3.up;
                    hp.transform.localPosition = new Vector3(0.55f, 0, 0);
                }
            }
            if (Input.GetButtonDown("Vertical")) {
                if (Input.GetAxis("Vertical") > 0) {
                    if (!Input.GetButton("Stay Still") && grid.MoveObject(getX(), getY(), getX(), getY() + 1)) {
                        transform.position += Vector3.up;
                    }
                    // face up
                    transform.up = Vector3.up;
                    // hp bar stays on top
                    hp.transform.up = Vector3.up;
                    hp.transform.localPosition = new Vector3(0, 0.55f, 0);
                } else {
                    if (!Input.GetButton("Stay Still") && grid.MoveObject(getX(), getY(), getX(), getY() - 1)) {
                        transform.position += Vector3.down;
                    }
                    // face down
                    transform.up = Vector3.down;
                    // hp bar stays on top
                    hp.transform.up = Vector3.up;
                    hp.transform.localPosition = new Vector3(0, -0.55f, 0);
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

    private int getX()
    {
        return (int) transform.localPosition.x;
    }

    // unity Y axis is 
    private int getY()
    {
        return (int) transform.localPosition.y;
    }
}
