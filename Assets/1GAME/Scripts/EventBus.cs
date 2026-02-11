using System;
using UnityEngine;

public static class EventBus
{
    public static event Action OnGameStarted;
    public static void InvokeGameStarted() => OnGameStarted?.Invoke();

    public static event Action OnGameEnded;
    public static void InvokeGameEnded() => OnGameEnded?.Invoke();

    public static event Action OnWin;
    public static void InvokeWin() => OnWin?.Invoke();

    public static event Action OnGameOver;
    public static void InvokeGameOver() => OnGameOver?.Invoke();

    public static event Action<Vector2Int> OnCellSelected;
    public static void InvokeCellSelected(Vector2Int coord) => OnCellSelected?.Invoke(coord);

    public static event Action<int> OnNumberSelected;
    public static void InvokeNumberSelected(int number) => OnNumberSelected?.Invoke(number);

    public static event Action OnBoardUpdated;
    public static void InvokeBoardUpdated() => OnBoardUpdated?.Invoke();

    public static event Action<Vector2Int,int,bool> OnCellValueChanged;
    public static void InvokeCellValueChanged(Vector2Int coord,int value,bool wrong) 
        => OnCellValueChanged?.Invoke(coord,value,wrong);

    public static event Action<int> OnHighlightNumber;
    public static void InvokeHighlightNumber(int number) => OnHighlightNumber?.Invoke(number);

    public static event Action<int> OnLivesChanged;
    public static void InvokeLivesChanged(int lives) => OnLivesChanged?.Invoke(lives);

    public static event Action<int> OnScoreChanged;
    public static void InvokeScoreChanged(int score) => OnScoreChanged?.Invoke(score);

    public static event Action OnEraseSelected;
    public static void InvokeEraseSelected() => OnEraseSelected?.Invoke();
    
    public static event Action OnHintRequested;
    public static void InvokeHintRequested() => OnHintRequested?.Invoke();

    public static event Action<int> OnHintsChanged;
    public static void InvokeHintsChanged(int hints) => OnHintsChanged?.Invoke(hints);
    
    public static event Action OnRequestLifeRefill;
    public static void InvokeRequestLifeRefill() => OnRequestLifeRefill?.Invoke();
}