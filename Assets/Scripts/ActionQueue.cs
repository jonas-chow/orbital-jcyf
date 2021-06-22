using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    private static ActionQueue instance;
    public static ActionQueue Instance { get { return instance; } }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private Queue<Action> actions = new Queue<Action>();
    private bool isEnabled = true;

    public void EnqueueAction(Action action)
    {
        actions.Enqueue(action);
        GameManager.Instance.ActionAdded();
    }

    public void ExecuteNext()
    {
        if (isEnabled) {
            Action next = actions.Dequeue();
            // only execute the action if the char didnt manage to die
            if (next.character.isAlive && !next.character.disabled) {
                next.Execute();
            }
        }
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

