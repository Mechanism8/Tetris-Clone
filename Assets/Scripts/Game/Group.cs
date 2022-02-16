using UnityEngine;

public class Group : MonoBehaviour
{
    private bool _canFall;
    private float _stepTime;

    [SerializeField] private StepDelayValueScriptableObject _stepDelayValueScriptableObject;
    [SerializeField] private GameObject _pivot;

    private bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Pivot") continue;
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
            if (child.tag == "Pivot") continue;
            Vector3 v = Playfield.RoundVector(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    private void Start()
    {
        if (!IsValidGridPos())
        {
            Debug.Log("Game Over");
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePiece(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePiece(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotatePiece();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePiece(Vector3.down);
        }
        else if (Input.GetButtonDown("Jump"))
        {
            HardDrop();
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

    private void MovePiece(Vector3 direction)
    {
        transform.Translate(direction, Space.World);
        if (direction == Vector3.down)
        {
            CheckVerticalMovement(direction);
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

    private void CheckVerticalMovement(Vector3 direction)
    {
        if (IsValidGridPos())
        {
            UpdateGrid();
            _canFall = true;
        }
        else
        {
            RevertMovement(direction);
            _canFall = false;

            Playfield.DeleteFullRows();

            GameManager.Instance.SpawnNextPiece();

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
            MovePiece(Vector3.down);
        }
    }
}
