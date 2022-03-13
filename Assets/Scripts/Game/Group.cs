using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Group : MonoBehaviour
{
    private bool _canFall;
    private float _stepTime;
    private float _timer = 0f;
    private float _moveDelay = .1f;

    [SerializeField] private StepDelayValueScriptableObject _stepDelayValueScriptableObject;
    [SerializeField] private GameObject _pivot;


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
            if (CompareTag("Pivot")) continue;
            Vector3 v = Playfield.RoundVector(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    private void Start()
    {
        if (!Playfield.IsValidGridPos(transform))
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

        if (Playfield.IsValidGridPos(transform))
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
        if (Playfield.IsValidGridPos(transform))
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
        if (Playfield.IsValidGridPos(transform))
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

            GameManager.Instance.SpawnNextPiece();
            Playfield.DeleteFullRows();

            enabled = false;
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
