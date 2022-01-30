using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Group : MonoBehaviour
{
    private bool _canFall;
    private float _stepTime;

    [SerializeField] private float _stepDelay;
    [SerializeField] private StepDelayValueScriptableObject stepDelayValue;

    public static event Action BottomReached;

    bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector3 v = Playfield.RoundVector(child.position);

            if (!Playfield.InsideBorder(v))
            {
                return false;
            }

            if (Playfield.grid[(int)v.x, (int)v.y] != null
                && Playfield.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    void UpdateGrid()
    {
        for (int y = 0; y < Playfield.height; ++y)
        {
            for (int x = 0; x < Playfield.width; ++x)
            {
                if (Playfield.grid[x, y] != null)
                {
                    if (Playfield.grid[x, y].parent == transform)
                    {
                        Playfield.grid[x, y] = null;
                    }
                }

            }
        }

        foreach (Transform child in transform)
        {
            Vector3 v = Playfield.RoundVector(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    private void Start()
    {
        _stepDelay = stepDelayValue.stepDelay;
        _stepTime = Time.time + _stepDelay;
        if (!IsValidGridPos())
        {
            Debug.Log("Game Over");
            Destroy(gameObject);
        }


    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePieceDown();
        }
        else if (Input.GetButtonDown("Jump"))
        {
            QuickFall();
        }

        if (Time.time >= _stepTime)
        {
            AutoFall();
        }


    }

    private void MovePieceDown()
    {
        transform.position += new Vector3(0, -1, 0);

        if (IsValidGridPos())
        {
            UpdateGrid();
            _canFall = true;
        }
        else
        {
            transform.position += new Vector3(0, 1, 0);

            _canFall = false;

            Playfield.DeleteFullRows();

            BottomReached?.Invoke();

            enabled = false;
        };
    }


    private void AutoFall()
    {
        _stepTime = Time.time + _stepDelay;
        MovePieceDown();
    }


    private void QuickFall()
    {
        while (_canFall)
        {
            MovePieceDown();
        }
    }


    private void MovePieceHorizontal()
    {

    }

}
