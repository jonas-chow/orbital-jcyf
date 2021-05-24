using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionAura : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelect(bool select)
    {
        GetComponent<SpriteRenderer>().enabled = select;
    }
}
