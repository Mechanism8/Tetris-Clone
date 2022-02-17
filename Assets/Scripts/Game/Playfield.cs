using System;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];
    public static event Action RowCleared;

    public static Vector3 RoundVector(Vector3 vector)
    {
        return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), 0);
    }

    public static bool InsideBorder(Vector3 position)
    {
        return ((int)position.x >= 0 
            && (int)position.x < width 
            && (int)position.y >= 0
            && (int)position.y < height);
    }

    public static void DeleteRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < height; i++)
        {
            DecreaseRow(i);
        }
    }

    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    public static void DeleteFullRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsRowFull(y))
            {                
                DeleteRow(y);
                DecreaseRowsAbove(y+1);
                y--;
                RowCleared?.Invoke();
            }
        }
    }

}
