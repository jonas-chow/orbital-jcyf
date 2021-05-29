using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    private Queue<Action> actions = new Queue<Action>();
    // private bool animationPhase = false;
    private GameManager gameManager;
    private ActionSpawner actionSpawner;
    private bool isEnabled = true;

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
        if (isEnabled) {
            actions.Dequeue().Execute();
            actionSpawner.RemoveAction();
        }
    }

    public void ResetQueue()
    {
        actionSpawner.ResetCount();
    }

    public bool hasActions()
    {
        return actions.Count > 0;
    }

    public void Disable()
    {
        this.isEnabled = false;
    }

    public void Enable()
    {
        this.isEnabled = true;
    }
}

