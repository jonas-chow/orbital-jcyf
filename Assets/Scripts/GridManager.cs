using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
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
        if (x < 0 || x >= length || y < 0 || y >= height) {
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
        if (x < 0 || x >= length || y < 0 || y >= height) {
            return null;
        }
        GameObject obj = grid[x, y];
        grid[x, y] = null;
        return obj;
    }

    // moves the object from one position to another
    public bool MoveObject(int prevX, int prevY, int newX, int newY)
    {
        if (newX < 0 || newX >= length || newY < 0 || newY >= height) {
            return false;
        }
        if (grid[newX, newY] == null) {
            grid[newX, newY] = RemoveObject(prevX, prevY);
            return true;
        } else {
            return false;
        }
    }
}
