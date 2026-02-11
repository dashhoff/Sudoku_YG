using UnityEngine;

public class GameController : MonoBehaviour
{
    private int[,] _current;
    private int[,] _solution;
    private Vector2Int _selected;
    private bool _playing;
    private int _selectedNumber;

    private void OnEnable()
    {
        EventBus.OnCellSelected += SelectCell;
        EventBus.OnNumberSelected += SelectNumber;
    }

    private void OnDisable()
    {
        EventBus.OnCellSelected -= SelectCell;
        EventBus.OnNumberSelected -= SelectNumber;
    }

    public void InitBoard(int[,] puzzle, int[,] solution)
    {
        _solution = solution;
        _current = (int[,])puzzle.Clone();
        _playing = true;
        EventBus.InvokeGameStarted();
    }

    private void SelectNumber(int number)
    {
        _selectedNumber = number;
        EventBus.InvokeHighlightNumber(number);
    }

    private void SelectCell(Vector2Int coord)
    {
        if (!_playing) return;
        if (_selectedNumber == 0) return;

        _selected = coord;
        EnterNumber(_selectedNumber);
    }

    private void EnterNumber(int number)
    {
        _current[_selected.x,_selected.y] = number;
        EventBus.InvokeCellValueChanged(_selected, number);
        EventBus.InvokeBoardUpdated();

        if (CheckWin())
        {
            EventBus.InvokeWin();
            EndGame();
        }
    }

    private bool CheckWin()
    {
        if (_current == null || _solution == null) return false;

        for(int y=0;y<9;y++)
        for(int x=0;x<9;x++)
            if(_current[x,y]!=_solution[x,y])
                return false;

        return true;
    }

    public void EndGame()
    {
        _playing = false;
        EventBus.InvokeGameEnded();
    }
}