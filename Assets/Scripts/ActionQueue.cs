using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    private Queue<Action> actions = new Queue<Action>();
    // private bool animationPhase = false;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnqueueAction(Action action)
    {
        actions.Enqueue(action);
        // queueRenderer.render(action)
    }

    public void ExecuteAll()
    {
        // animationPhase = true;
        while (actions.Count > 0) {
            actions.Dequeue().Execute();
        }
    }
}
