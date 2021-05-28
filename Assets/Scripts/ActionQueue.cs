using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    private Queue<Action> actions = new Queue<Action>();
    // private bool animationPhase = false;
    private GameManager gameManager;
    private ActionSpawner actionSpawner;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        actionSpawner = gameObject.GetComponentInChildren<ActionSpawner>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnqueueAction(Action action)
    {
        actions.Enqueue(action);
        actionSpawner.AddAction(action);
    }

    public void ExecuteNext()
    {
        actions.Dequeue().Execute();
        actionSpawner.RemoveAction();
    }

    public bool hasActions()
    {
        return actions.Count > 0;
    }
}
