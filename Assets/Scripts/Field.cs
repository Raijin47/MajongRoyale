using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private Cell _prefab;
    [SerializeField] private Sprite[] _sprites;

    private Cell[,] _cell;
    private Vector2Int _firstCell;
    private Vector2Int _secondCell;

    private readonly List<int> FreeID = new();
    private readonly Vector2Int[] Direction =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };
    private readonly Vector2 SelectedSize = new(1.3f, 1.3f);

    private int _currentHeight;
    private int _currentWidth;
    private bool _isGameActive;

    public Vector2Int FirstCell
    {
        get => _firstCell;
        set
        {
            _cell[_firstCell.x, _firstCell.y].Selected(Vector2.one);
            IsSelected = true;
            _firstCell = value;
            _cell[_firstCell.x, _firstCell.y].Selected(SelectedSize);
        }
    }
    public Vector2Int SecondCell
    {
        get => _secondCell;
        set
        {
            if (_cell[value.x, value.y].ID == _cell[_firstCell.x, _firstCell.y].ID)
            {
                IsSelected = false;
                _secondCell = value;
                StartCoroutine(FindPath());
            }
            else FirstCell = value;
        }
    }
    public bool IsSelected { get; set; }

    public void Init()
    {
        GameController.OnStartGame += StartGame;
    }

    private void InitializeField()
    {
        DeleteField();

        _currentHeight = Data.Instance.Level.Height;
        _currentWidth = Data.Instance.Level.Width;


        _cell = new Cell[_currentWidth, _currentHeight];
        for (int y = 0; y < _currentHeight; y++)
        {
            for (int x = 0; x < _currentWidth; x++)
            {
                var newSlot = Instantiate(_prefab, _content);
                newSlot.X = x;
                newSlot.Y = y;
                newSlot.Field = this;
                _cell[x, y] = newSlot;
            }
        }
    }

    public void Shuffle()
    {
        for (int y = 0; y < _currentHeight; y++)
        {
            for (int x = 0; x < _currentWidth; x++)
            {
                int randomX = Random.Range(0, _currentWidth);
                int randomY = Random.Range(0, _currentHeight);

                int oldID = _cell[x, y].ID;
                int newID = _cell[randomX, randomY].ID;

                _cell[randomX, randomY].ID = oldID;
                _cell[randomX, randomY].Sprite = _sprites[oldID];

                _cell[x, y].ID = newID;
                _cell[x, y].Sprite = _sprites[newID];
            }
        }
    }

    private void StartGame()
    {
        _isGameActive = true;
        InitializeField();
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        Canvas.ForceUpdateCanvases();

        yield return null;

        for (int i = 0; i < _currentHeight * _currentWidth / 2; i++)
        {
            int r = Random.Range(1, _sprites.Length);
            FreeID.Add(r);
            FreeID.Add(r);
        }

        for (int y = 0; y < _currentHeight; y++)
        {
            for (int x = 0; x < _currentWidth; x++)
            {
                int r = Random.Range(0, FreeID.Count);
                _cell[x, y].ID = FreeID[r];
                _cell[x, y].Sprite = _sprites[FreeID[r]];
                FreeID.RemoveAt(r);
            }
        }
    }

    private void DeleteField()
    {
        if (_cell == null) return;

        for (int y = 0; y < _currentHeight; y++)
            for (int x = 0; x < _currentWidth; x++)
                Destroy(_cell[x, y].gameObject);

        _cell = null;
    }

    private IEnumerator FindPath()
    {
        Vector2Int cell;
        Vector2Int cell2;
        Vector2Int cell3;
        Vector2Int direction2;
        Vector2Int direction3;

        for (int i = 0; i < Direction.Length; i++)
        {
            for (cell = _firstCell + Direction[i]; IsNotBorder(cell); cell += Direction[i])
            {
                direction2 = GetDirection(cell, Direction[i]);

                for (cell2 = cell; IsNotBorder(cell2); cell2 += direction2)
                {
                    direction3 = GetDirection(cell2, direction2);

                    for (cell3 = cell2; IsNotBorder(cell3); cell3 += direction3)
                    {
                        if (IsCompliant(cell3))
                        {
                            Cell first = _cell[_firstCell.x, _firstCell.y];
                            Cell second = _cell[_secondCell.x, _secondCell.y];

                            int id = first.ID;
                            Vector3[] poss = new Vector3[] { GetPos(cell), GetPos(cell2), GetPos(cell3), GetPos(_secondCell) };
                            ClearSelection();

                            first.Sprite = _sprites[0];
                            second.Sprite = _sprites[0];
                            yield return MoveCell.MoveToPositions(first.transform.GetChild(0), second.transform.GetChild(0), poss, _sprites[id]);

                            CheckWin();
                            yield break;
                        }
                        if (!IsClearPath(cell3)) break;
                        yield return null;
                    }
                    if (!IsClearPath(cell2)) break;
                }
                if (!IsClearPath(cell)) break;
            }
        }

        FirstCell = _secondCell;
    }

    public void FindAndPrintCollapsibleCells()
    {
        print("FindAndPrintCollapsibleCells");
        for (int y1 = 0; y1 < _currentHeight; y1++)
        {
            for (int x1 = 0; x1 < _currentWidth; x1++)
            {
                Vector2Int firstCell = new Vector2Int(x1, y1);
                if (_cell[x1, y1].ID == 0) continue;

                for (int y2 = 0; y2 < _currentHeight; y2++)
                {
                    for (int x2 = 0; x2 < _currentWidth; x2++)
                    {
                        Vector2Int secondCell = new Vector2Int(x2, y2);
                        if (firstCell == secondCell || _cell[x2, y2].ID == 0) continue;

                        if (_cell[firstCell.x, firstCell.y].ID != _cell[secondCell.x,secondCell.y].ID) continue;

                        _firstCell = firstCell;
                        _secondCell = secondCell;

                        if (CanCollapseCells())
                        {
                            _cell[firstCell.x, firstCell.y].transform.GetChild(0).localScale = Vector3.one * 0.7f;
                            _cell[secondCell.x, secondCell.y].transform.GetChild(0).localScale = Vector3.one * 0.7f;
                            //Anim(firstCell, secondCell);
                            return;
                        }
                    }
                }
            }
        }

        print("not found");
    }

    public bool CanCollapseCells()
    {
        Vector2Int cell;
        Vector2Int cell2;
        Vector2Int cell3;
        Vector2Int direction2;
        Vector2Int direction3;

        for (int i = 0; i < Direction.Length; i++)
        {
            for (cell = _firstCell + Direction[i]; IsNotBorder(cell); cell += Direction[i])
            {
                direction2 = GetDirection(cell, Direction[i]);

                for (cell2 = cell; IsNotBorder(cell2); cell2 += direction2)
                {
                    direction3 = GetDirection(cell2, direction2);

                    for (cell3 = cell2; IsNotBorder(cell3); cell3 += direction3)
                    {
                        if (IsCompliant(cell3))
                        {
                            return true;
                        }
                        if (!IsClearPath(cell3)) break;
                    }
                    if (!IsClearPath(cell2)) break;
                }
                if (!IsClearPath(cell)) break;
            }
        }
        return false;
    }

    private Vector3 GetPos(Vector2Int cell)
    {
        if (IsNotBorder(cell))
        {
            return _cell[cell.x, cell.y].transform.position;
        }

        return Vector3.zero;
    }

    private Vector2Int GetDirection(Vector2Int cell, Vector2Int dir)
    {
        if (dir == Vector2.up | dir == Vector2.down)
            return cell.x > _secondCell.x ? Vector2Int.left : Vector2Int.right;
        else
            return cell.y > _secondCell.y ? Vector2Int.down : Vector2Int.up;
    }

    private bool IsNotBorder(Vector2Int cell)
    {
        return cell.y < _currentHeight && cell.y >= 0 && cell.x < _currentWidth && cell.x >= 0;
    }

    private bool IsClearPath(Vector2Int cell)
    {
        return _cell[cell.x, cell.y].ID == 0;
    }

    private bool IsCompliant(Vector2Int cell)
    {
        return _cell[cell.x, cell.y].ID == _cell[_firstCell.x, _firstCell.y].ID && cell == _secondCell;
    }

    private void ClearSelection()
    {
        var first = _cell[_firstCell.x, _firstCell.y];
        var second = _cell[_secondCell.x, _secondCell.y];

        first.ID = 0;
        second.ID = 0;

        GameController.AddScore?.Invoke();
    }

    private void CheckWin()
    {
        if (!_isGameActive) return;

        if (IsComplated())
        {
            _isGameActive = false;
            GameController.OnWinGame?.Invoke();
        }
    }

    private bool IsComplated()
    {
        for (int y = 0; y < _currentHeight; y++)
            for (int x = 0; x < _currentWidth; x++)
                if (_cell[x, y].ID != 0)
                    return false;
        return true;
    }
}