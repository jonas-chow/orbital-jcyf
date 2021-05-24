using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private bool isActive = false;
    private HealthBar hp;
    private SelectionAura selection;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponentInChildren<HealthBar>();
        hp.SetVisible(false);

        selection = GetComponentInChildren<SelectionAura>();
        selection.SetSelect(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (Input.GetButtonDown("Horizontal")) {
                if (Input.GetAxis("Horizontal") > 0) {
                    transform.position += Vector3.right;
                } else {
                    transform.position += Vector3.left;
                }
            }
            if (Input.GetButtonDown("Vertical")) {
                if (Input.GetAxis("Vertical") > 0) {
                    transform.position += Vector3.up;
                } else {
                    transform.position += Vector3.down;
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

    public void init()
    {
        hp = GetComponentInChildren<HealthBar>();
        hp.SetVisible(false);

        selection = GetComponentInChildren<SelectionAura>();
        selection.SetSelect(false);
    }
}
