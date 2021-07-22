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
    // stores all the executed actions to be used for replays
    public Queue<Action> actionCache = new Queue<Action>();
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
            if (next.charID == -1 || (next.GetCharacter().isAlive && !next.GetCharacter().disabled)) {
                next.Execute();
                actionCache.Enqueue(next);
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

    public class TurnEndAction : Action
    {
        public override void Execute() {
            System.Array.ForEach(GameManager.Instance.friendly, cm => cm.TurnPass());
            System.Array.ForEach(GameManager.Instance.enemies, cm => cm.TurnPass());
        }
        public override void SendEvent() {}

        public TurnEndAction()
        {
            this.name = "turnEnd";
            this.charID = -1;
        }
    }

    public void TurnEnd()
    {
        actionCache.Enqueue(new TurnEndAction());
    }

    public void RecordEnemyEvent(Action action)
    {
        actionCache.Enqueue(action);
    }
}

