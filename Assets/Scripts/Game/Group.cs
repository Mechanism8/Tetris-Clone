using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Group : MonoBehaviour
{
    private bool _canFall;
    private float _stepTime;
    private float _timer = 0f;
    private float _moveDelay = .1f;
    private int _spawnCount = 0;

    [SerializeField] private StepDelayValueScriptableObject _stepDelayValueScriptableObject;
    [SerializeField] private GameObject _pivot;

    private bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Pivot")) continue;
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

    private void UpdateGrid()
    {
        for (int y = 0; y < Playfield.height; y++)
        {
            for (int x = 0; x < Playfield.width; x++)
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
            if (child.CompareTag("Pivot")) continue;
            Vector3 v = Playfield.RoundVector(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    private void Start()
    {
        if (!IsValidGridPos())
        {
            Destroy(gameObject);
            GameManager.Instance.EndGame();
        }
        else
        {
            UpdateGrid();
        }


    }

    private void Update()
    {
        _timer += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow) && _timer > _moveDelay)
        {
            MovePiece(Vector3.left);
            _timer = 0;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && _timer > _moveDelay)
        {
            MovePiece(Vector3.right);
            _timer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotatePiece();
        }
        else if (Input.GetKey(KeyCode.DownArrow) && _timer > _moveDelay)
        {
            MovePiece(Vector3.down, isManual: true);
            _timer = 0;
        }
        else if (Input.GetButtonDown("Hard Drop"))
        {
            HardDrop();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            GameManager.Instance.TogglePause();
        }

        if (Time.time >= _stepTime)
        {
            AutoFall();
        }


    }

    private void RotatePiece()
    {
        transform.RotateAround(_pivot.transform.position, new Vector3(0, 0, 1), 90);

        if (IsValidGridPos())
        {
            UpdateGrid();
        }
        else
        {
            transform.RotateAround(_pivot.transform.position, new Vector3(0, 0, 1), -90);

        }
    }

    private void MovePiece(Vector3 direction, bool isManual = false)
    {
        transform.Translate(direction, Space.World);
        if (direction == Vector3.down)
        {
            CheckVerticalMovement(direction, isManual);
        }
        else
        {
            CheckHorizontalMovement(direction);
        }

    }

    private void CheckHorizontalMovement(Vector3 direction)
    {
        if (IsValidGridPos())
        {
            UpdateGrid();
        }
        else
        {
            RevertMovement(direction);
        }
    }

    private void RevertMovement(Vector3 direction)
    {
        transform.Translate(direction * -1, Space.World);
    }

    private void CheckVerticalMovement(Vector3 direction, bool isManual)
    {
        if (IsValidGridPos())
        {
            UpdateGrid();
            _canFall = true;
            if (isManual)
            {
                GameManager.Instance.IncreaseScore(1);
            }
        }
        else
        {
            RevertMovement(direction);
            _canFall = false;           
            Playfield.DeleteFullRows();

            enabled = false;            
            if (_spawnCount == 0) GameManager.Instance.SpawnNextPiece();
            _spawnCount++;

        }
    }

    private void AutoFall()
    {
       _stepTime = Time.time + _stepDelayValueScriptableObject.stepDelay;
        MovePiece(Vector3.down);
    }


    private void HardDrop()
    {
        while (_canFall)
        {
            MovePiece(Vector3.down, isManual: true);
        }
    }
}