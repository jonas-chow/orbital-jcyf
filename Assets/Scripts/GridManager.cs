using System;
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
            grid = new GameObject[length, height];
            traps = new Trap[length, height];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = null;
                    traps[i, j] = null;
                }
            }
        }
    }

    public int length = 16;
    public int height = 16;
    private GameObject[,] grid;
    // Future development: could have multiple trap-like objects
    public Trap[,] traps;

    // returns false if there is something in that space and you can't insert it
    public bool InsertObject(GameObject obj, int x, int y)
    {
        if (!IsValidCoords(x, y)) {
            return false;
        }
        if (grid[x, y] == null) {
            grid[x, y] = obj;
            if (traps[x, y] != null)
            {
                CharacterMovement cm = obj.GetComponent<CharacterMovement>();
                if (cm != null) {
                    traps[x, y].Trigger(cm);
                }
            }
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
            InsertObject(RemoveObject(prevX, prevY), newX, newY);
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
        if (obj != null) {
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

    public int DistanceFromChar(CharacterMovement self, CharacterMovement target)
    {
        return Math.Abs(self.GetX() - target.GetX()) + 
            Math.Abs(self.GetY() - target.GetY());
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

    public bool MoveToAndInsert(GameObject obj, int x, int y)
    {
        if (IsValidCoords(x, y) && grid[x, y] == null) {
            obj.transform.position = GetCoords(x, y);
            InsertObject(obj, x, y);
            return true;
        } else {
            return false;
        }
    }

    public bool InsertTrap(Trap trap, int x, int y)
    {
        if (IsValidCoords(x, y) && traps[x, y] == null) {
            trap.transform.position = GetCoords(x, y);
            traps[x, y] = trap;
            return true;
        } else {
            return false;
        }
    }

    public void RemoveTrap(int x, int y)
    {
        traps[x, y] = null;
    }

    public Vector3 GetCoords(int x, int y)
    {
        return transform.position + new Vector3(x, y, 0);
    }

    public int GetX(float xCoord)
    {
        return (int)(xCoord - transform.position.x);
    }

    public int GetY(float yCoord)
    {
        return (int)(yCoord - transform.position.y);
    }
}
