using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSpawner : MonoBehaviour
{
    public GameObject rangeIndicator;
    public GameObject rangeLimit;

    public GameObject[] LinearIndicator(CharacterMovement character, int range, string direction)
    {
        Vector3 dir = Vector3.zero;
        switch (direction)
        {
            case "up":
                dir = Vector3.up;
                break;
            case "down":
                dir = Vector3.down;
                break;
            case "left":
                dir = Vector3.left;
                break;
            case "right":
                dir = Vector3.right;
                break;
        }
        GameObject[] objects = new GameObject[range];
        for (int i = 1; i <= range; i++) {
            objects[i - 1] = GameObject.Instantiate(
                rangeIndicator, 
                character.transform.position + dir * i, 
                Quaternion.identity
            );
        }
        
        return objects;
    }

    public GameObject[] AOEIndicator(CharacterMovement character, int offsetX, int offsetY)
    {
        Vector3 pos = character.transform.position + new Vector3(offsetX, offsetY, 0);
        GameObject[] objects = new GameObject[9];
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                objects[(i + 1) * 3 + j + 1] = GameObject.Instantiate(
                    rangeIndicator,
                    pos + new Vector3(j, i, 0),
                    Quaternion.identity
                );
            }
        }

        return objects;
    }

    public GameObject[] RangeLimit(CharacterMovement character, int range)
    {
        Vector3 pos = character.transform.position + new Vector3(0, range, 0);
        Vector3 posChange = Vector3.zero;
        GameObject[] objects = new GameObject[range * 4];

        for (int i = 0; i < 4; i++) {
            switch (i)
            {
                case 0:
                    posChange = new Vector3(1, -1, 0);
                    break;
                case 1:
                    posChange = new Vector3(-1, -1, 0);
                    break;
                case 2:
                    posChange = new Vector3(-1, 1, 0);
                    break;
                case 3:
                    posChange = new Vector3(1, 1, 0);
                    break;
            }
            for (int j = 0; j < range; j++) {
                objects[i * range + j] = GameObject.Instantiate(rangeLimit, pos, Quaternion.identity);
                pos += posChange;
            }
        }

        return objects;
    }

}