using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpawner : MonoBehaviour
{
    private int count = 0;
    private Queue<GameObject> queue = new Queue<GameObject>();

    [SerializeField]
    private GameObject Transition, FaceDown, FaceUp, FaceLeft, FaceRight;
    [SerializeField]
    private GameObject MoveDown, MoveUp, MoveLeft, MoveRight;
    [SerializeField]
    private GameObject MeleeAttack;


    public void AddAction(Action action)
    {
        Vector3 pos = GetNextPosition();
        // the key
        queue.Enqueue(GameObject.Instantiate(GetPrefab(action), pos, Quaternion.identity));
        // the arrow
        queue.Enqueue(GameObject.Instantiate(Transition, pos + Vector3.right, Quaternion.identity));
        count++;
    }

    public void RemoveAction()
    {
        // the key
        GameObject.Destroy(queue.Dequeue());
        // the arrow
        GameObject.Destroy(queue.Dequeue());
    }

    public void ResetCount()
    {
        count = 0;
    }

    private Vector3 GetNextPosition()
    {
        return transform.position + new Vector3(count % 4, -count / 4, 0) * 2;
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
                return null;
        }
    }
}
