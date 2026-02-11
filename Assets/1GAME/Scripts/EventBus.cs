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

    public static event Action<Vector2Int> OnCellSelected;
    public static void InvokeCellSelected(Vector2Int coord) => OnCellSelected?.Invoke(coord);

    public static event Action<int> OnNumberEntered;
    public static void InvokeNumberEntered(int number) => OnNumberEntered?.Invoke(number);

    public static event Action OnBoardUpdated;
    public static void InvokeBoardUpdated() => OnBoardUpdated?.Invoke();

    public static event Action<int> OnNumberSelected;
    public static void InvokeNumberSelected(int number) => OnNumberSelected?.Invoke(number);

    public static event Action<int> OnHighlightNumber;
    public static void InvokeHighlightNumber(int number) => OnHighlightNumber?.Invoke(number);

    public static event Action<Vector2Int,int> OnCellValueChanged;
    public static void InvokeCellValueChanged(Vector2Int coord,int value) 
        => OnCellValueChanged?.Invoke(coord,value);
}