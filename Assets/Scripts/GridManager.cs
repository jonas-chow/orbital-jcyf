using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public static GridManager Instance { get { return instance; } }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    [SerializeField]
    private int length = 16, height = 16;
    private GameObject[,] grid;

    public void init()
    {
        grid = new GameObject[length, height];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = null;
            }
        }
    }

    // returns false if there is something in that space and you can't insert it
    public bool InsertObject(GameObject obj, int x, int y)
    {
        if (!IsValidCoords(x, y)) {
            return false;
        }
        if (grid[x, y] == null) {
            grid[x, y] = obj;
            return true;
        } else {
            return false;
        }
    }

    // returns the object that is removed, or null if nothing is removed
    public GameObject RemoveObject(int x, int y)
    {
        if (!IsValidCoords(x, y)) {
            return null;
        }
        GameObject obj = grid[x, y];
        grid[x, y] = null;
        return obj;
    }

    // moves the object from one position to another
    public bool MoveObject(int prevX, int prevY, int newX, int newY)
    {
        if (!IsValidCoords(newX, newY)) {
            return false;
        }
        if (grid[newX, newY] == null) {
            grid[newX, newY] = RemoveObject(prevX, prevY);
            return true;
        } else {
            return false;
        }
    }

    // get object if there is an object at position, returns null if empty
    public GameObject GetObject(int x, int y)
    {
        if (!IsValidCoords(x, y)) {
            return null;
        }
        GameObject obj = grid[x, y];
        return obj;
    }

    public CharacterMovement GetCharacter(int x, int y)
    {
        GameObject obj = GetObject(x, y);
        CharacterMovement cm = null;
        if (obj != null && obj.tag == "Character") {
            cm = obj.GetComponent<CharacterMovement>();
        }
        return cm;
    }

    // finds the first character in a line from a given point, excluding the point
    public CharacterMovement GetFirstCharacterInLine(int x, int y, int range, string direction)
    {
        CharacterMovement cm = null;
        int currentX = x;
        int currentY = y;
        int i = 0;
        while (cm == null && i < range)
        {
            switch (direction)
            {
                case "up":
                    currentY++;
                    break;
                case "down":
                    currentY--;
                    break;
                case "left":
                    currentX--;
                    break;
                case "right":
                    currentX++;
                    break;
            }

            cm = GetCharacter(currentX, currentY);
            i++;
        }
        return cm;
    }

    public List<CharacterMovement> GetAllCharactersInAOE(int x, int y)
    {
        List<CharacterMovement> cms = new List<CharacterMovement>();
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                CharacterMovement cm = GetCharacter(x + i, y + j);
                if (cm != null) {
                    cms.Add(cm);
                }
            }
        }
        return cms;
    }

    public bool IsValidCoords(int x, int y)
    {
        return x >= 0 && x < length && y >= 0 && y < height;
    }
}
