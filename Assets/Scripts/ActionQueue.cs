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
    private int objCount = 0;
    private Queue<GameObject> objQueue = new Queue<GameObject>();

    [SerializeField]
    private GameObject Transition, FaceDown, FaceUp, FaceLeft, FaceRight;
    [SerializeField]
    private GameObject MoveDown, MoveUp, MoveLeft, MoveRight;
    [SerializeField]
    private GameObject MeleeAttack;

    private Vector3 GetNextPosition()
    {
        return transform.position + new Vector3(objCount % 4, -objCount / 4, 0) * 2;
    }

    // This is terrible practice but until I figure another way out...
    private GameObject GetPrefab(Action action)
    {
        switch (action.name)
        {
            case "FaceDown":
                return FaceDown;
            case "FaceUp":
                return FaceUp;
            case "FaceLeft":
                return FaceLeft;
            case "FaceRight":
                return FaceRight;
            case "MoveDown":
                return MoveDown;
            case "MoveUp":
                return MoveUp;
            case "MoveLeft":
                return MoveLeft;
            case "MoveRight":
                return MoveRight;
            case "MeleeAttack":
                return MeleeAttack;
            default:
                return MeleeAttack;
        }
    }

    public void EnqueueAction(Action action)
    {
        actions.Enqueue(action);
        GameManager.Instance.ActionAdded();
        Vector3 pos = GetNextPosition();
        // the key
        objQueue.Enqueue(GameObject.Instantiate(GetPrefab(action), pos, Quaternion.identity));
        // the arrow
        objQueue.Enqueue(GameObject.Instantiate(Transition, pos + Vector3.right, Quaternion.identity));
        objCount++;
    }

    public void ExecuteNext()
    {
        if (isEnabled) {
            Action next = actions.Dequeue();
            // only execute the action if the char didnt manage to die
            if (next.character.isAlive) {
                next.Execute();
            }
            // the key
            GameObject.Destroy(objQueue.Dequeue());
            // the arrow
            GameObject.Destroy(objQueue.Dequeue());
        }
    }

    public void ResetQueue()
    {
        objCount = 0;
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

