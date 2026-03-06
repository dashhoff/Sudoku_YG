using UnityEngine;
using YG;

public class GameController : MonoBehaviour
{
    [SerializeField] private int _startLives = 5;
    [SerializeField] private int _pointsPerCorrect = 10;
    [SerializeField] private int _penaltyPoints = 5;
    [SerializeField] private int _startHints = 3;

    [SerializeField] private ScreenBorderFlash _screenBorderFlash;
    
    [SerializeField] private GameObject _hintUI;
    
    private GameSave _save;
    private int _seed;

    private int[,] _current;
    private int[,] _solution;
    private Vector2Int _selected;
    private bool _playing;
    private int _selectedNumber;
    private bool _eraseMode;

    private int _lives;
    private int _score;
    private int _hints;

    private void OnEnable()
    {
        EventBus.OnCellSelected += SelectCell;
        EventBus.OnNumberSelected += SelectNumber;
        EventBus.OnEraseSelected += SelectErase;
        EventBus.OnHintRequested += OnHintRequested;
    }

    private void OnDisable()
    {
        EventBus.OnCellSelected -= SelectCell;
        EventBus.OnNumberSelected -= SelectNumber;
        EventBus.OnEraseSelected -= SelectErase;
        EventBus.OnHintRequested -= OnHintRequested;
    }

    public void InitBoard(int[,] puzzle, int[,] solution)
    {
        _solution = solution;
        _current = (int[,])puzzle.Clone();

        _lives = _startLives;
        _score = 0;
        _hints = _startHints;
        _playing = true;
        _eraseMode = false;
        
        if (_save == null)
            _save = new GameSave();

        EventBus.InvokeGameStarted();
        EventBus.InvokeLivesChanged(_lives);
        EventBus.InvokeScoreChanged(_score);
        EventBus.InvokeHintsChanged(_hints);
    }
    
    public void StartNewGame()
    {
        _seed = Random.Range(int.MinValue,int.MaxValue);

        SudokuBoard _board = FindObjectOfType<SudokuBoard>();

        _board.GenerateFromSeed(_seed,10);
        _board.SpawnFromPuzzle();

        _solution = _board.GetSolution();
        _current = (int[,])_board.GetPuzzle().Clone();

        _save = new GameSave();
        _save._seed = _seed;
        _save._moves.Clear();
        _save._moves = new System.Collections.Generic.List<Move>();

        _lives = _startLives;
        _score = 0;
        _hints = _startHints;

        EventBus.InvokeGameStarted();
        EventBus.InvokeLivesChanged(_lives);
        EventBus.InvokeScoreChanged(_score);
        EventBus.InvokeHintsChanged(_hints);
    }

    public void SaveGame()
    {
        if (_save == null)
            _save = new GameSave();

        _save._seed = _seed;

        _save._lives = _lives;
        _save._score = _score;
        _save._hints = _hints;

        _save._moves.Clear();

        for (int y = 0; y < 9; y++)
        for (int x = 0; x < 9; x++)
        {
            if (_current[x,y] == 0) continue;

            Move m = new Move();
            m._x = x;
            m._y = y;
            m._value = _current[x,y];

            _save._moves.Add(m);
        }

        SaveSystem.Save(_save);
    }
    
    public void LoadGameButton()
    {
        bool _loaded = LoadGame();

        if (!_loaded)
            StartNewGame();
    }
    
    public bool LoadGame()
    {
        _save = SaveSystem.Load();

        if (_save == null)
            return false;

        SudokuBoard _board = FindObjectOfType<SudokuBoard>();

        if (_board == null)
            return false;

        // ----- Восстанавливаем поле -----
        _seed = _save._seed;

        _board.GenerateFromSeed(_seed, 10);
        _board.SpawnFromPuzzle();

        _solution = _board.GetSolution();
        _current = (int[,])_board.GetPuzzle().Clone();

        // ----- Восстанавливаем ходы игрока -----
        if (_save._moves != null)
        {
            foreach (var move in _save._moves)
            {
                _current[move._x, move._y] = move._value;
                EventBus.InvokeCellValueChanged(
                    new Vector2Int(move._x, move._y),
                    move._value,
                    false
                );
            }
        }

        // ----- Восстанавливаем статистику -----
        _lives = _save._lives;
        _score = _save._score;
        _hints = _save._hints;

        _playing = true;
        _eraseMode = false;

        // ----- Обновляем UI -----
        EventBus.InvokeGameStarted();
        EventBus.InvokeLivesChanged(_lives);
        EventBus.InvokeScoreChanged(_score);
        EventBus.InvokeHintsChanged(_hints);
        EventBus.InvokeBoardUpdated();

        return true;
    }

    // ---- изменение: при неверном ходе и при снижении _lives вызываем запрос пополнения, если 1 осталась ----
    private void TryPlaceNumber(int number)
    {
        if (_current == null || _solution == null) return;

        int prev = _current[_selected.x, _selected.y];
        int correctValue = _solution[_selected.x, _selected.y];
        bool alreadyCorrect = prev == correctValue;

        if (alreadyCorrect) return;

        bool correct = (number == correctValue);

        _current[_selected.x, _selected.y] = number;
        EventBus.InvokeCellValueChanged(_selected, number, !correct);
        EventBus.InvokeBoardUpdated();

        if (correct)
        {
            _score += _pointsPerCorrect;
            EventBus.InvokeScoreChanged(_score);
            EventBus.InvokeSound(SoundType.Correct);
            _screenBorderFlash.PlaySuccess();
            
            Move _move = new Move();
            _move._x = _selected.x;
            _move._y = _selected.y;
            _move._value = number;

            _save._moves.Add(_move);
            
            SaveGame();

            if (CheckWin())
            {
                EventBus.InvokeWin();
                EndGame();
            }
            return;
        }

        // неверный ход
        _lives--;
        _score -= _penaltyPoints;
        if (_score < 0) _score = 0;

        EventBus.InvokeLivesChanged(_lives);
        EventBus.InvokeScoreChanged(_score);
        EventBus.InvokeSound(SoundType.Wrong);
        _screenBorderFlash.PlayFail();
        
        // если осталась 1 жизнь — предложить пополнение
        if (_lives == 1)
            EventBus.InvokeRequestLifeRefill();

        if (_lives <= 0)
        {
            EventBus.InvokeGameOver();
            EndGame();
        }
    }
    
    // обработчик запроса подсказки
    private void OnHintRequested()
    {
        if (!_playing) return;

        if (_hints > 0)
        {
            UseHintDirect();

            if (_hints < 1)
            {
                _hintUI.gameObject.SetActive(true);
                
            }
            return;
        }

        // если подсказок нет — показываем рекламу и даём 3 подсказки по вознаграждению
        AdsService.Instance.ShowReward(() =>
        {
            AddHints(3);
            // сразу дать подсказку после рекламы (опционально). Если не нужно — убрать следующую строку:
            UseHintDirect();
        });
    }

// добавить подсказки и оповестить UI
    private void AddHints(int amount)
    {
        _hints += amount;
        EventBus.InvokeHintsChanged(_hints);
        if (_hints > 0)
            _hintUI.gameObject.SetActive(false);
    }

// непосредственно ставим случайную правильную цифру
    private void UseHintDirect()
    {
        Vector2Int cell = FindRandomEmptyOrWrongCell();
        if (cell.x == -1) return;

        int value = _solution[cell.x, cell.y];
        _current[cell.x, cell.y] = value;
        EventBus.InvokeCellValueChanged(cell, value, false);
        EventBus.InvokeBoardUpdated();

        _hints--;
        EventBus.InvokeHintsChanged(_hints);

        _score += _pointsPerCorrect;
        EventBus.InvokeScoreChanged(_score);

        SaveGame();
        
        if (CheckWin())
        {
            EventBus.InvokeWin();
            EndGame();
        }
    }

// выбор клетки для подсказки: пустая или не-правильная
    private Vector2Int FindRandomEmptyOrWrongCell()
    {
        var list = new System.Collections.Generic.List<Vector2Int>();
        for (int y = 0; y < 9; y++)
        for (int x = 0; x < 9; x++)
        {
            if (_current[x,y] == 0 || _current[x,y] != _solution[x,y])
                list.Add(new Vector2Int(x,y));
        }

        if (list.Count == 0) return new Vector2Int(-1,-1);
        return list[Random.Range(0, list.Count)];
    }
    
    private Vector2Int FindRandomEmptyCell()
    {
        System.Collections.Generic.List<Vector2Int> empty = new();

        for (int y = 0; y < 9; y++)
        for (int x = 0; x < 9; x++)
            if (_current[x, y] != _solution[x, y])
                empty.Add(new Vector2Int(x, y));

        if (empty.Count == 0)
            return new Vector2Int(-1, -1);

        return empty[Random.Range(0, empty.Count)];
    }
    
    private void UseHint()
    {
        if (!_playing) return;
        if (_hints <= 0) return;

        Vector2Int cell = FindRandomEmptyCell();
        if (cell.x == -1) return;

        int value = _solution[cell.x, cell.y];

        _current[cell.x, cell.y] = value;
        EventBus.InvokeCellValueChanged(cell, value, false);
        EventBus.InvokeBoardUpdated();

        _hints--;
        EventBus.InvokeHintsChanged(_hints);

        _score += _pointsPerCorrect;
        EventBus.InvokeScoreChanged(_score);

        if (CheckWin())
        {
            EventBus.InvokeWin();
            EndGame();
        }
    }

    private void SelectNumber(int number)
    {
        _eraseMode = false;
        _selectedNumber = number;
        EventBus.InvokeHighlightNumber(number);
    }

    private void SelectErase()
    {
        _eraseMode = true;
        _selectedNumber = 0;
        EventBus.InvokeHighlightNumber(0);
    }

    private void SelectCell(Vector2Int coord)
    {
        if (!_playing || _current == null) return;
        _selected = coord;

        if (_eraseMode)
        {
            TryErase();
            return;
        }

        if (_selectedNumber == 0) return;

        TryPlaceNumber(_selectedNumber);
    }

    private void TryErase()
    {
        if (_current == null || _solution == null) return;

        int prev = _current[_selected.x, _selected.y];
        if (prev == 0) return;

        bool wrong = prev != _solution[_selected.x, _selected.y];
        if (!wrong) return;

        _current[_selected.x, _selected.y] = 0;
        EventBus.InvokeCellValueChanged(_selected, 0, false);
        EventBus.InvokeBoardUpdated();
        EventBus.InvokeSound(SoundType.Click2);
    }

    private bool CheckWin()
    {
        if (_current == null || _solution == null) return false;

        for (int y = 0; y < 9; y++)
            for (int x = 0; x < 9; x++)
                if (_current[x, y] != _solution[x, y])
                    return false;

        return true;
    }

    public void AddLives(int amount)
    {
        _lives += amount;
        EventBus.InvokeLivesChanged(_lives);
    }

    public void RestoreToFullLives()
    {
        _lives = _startLives;
        EventBus.InvokeLivesChanged(_lives);
    }

    // метод, который AdsService может вызвать в callback
    public void OnLifeRewardGranted(int amount)
    {
        AddLives(amount);
    }

    public void EndGame()
    {
        _playing = false;
        EventBus.InvokeGameEnded();

        _seed = 0;
        _save = null;

        SaveSystem.Clear();

        YG2.saves.MaxScore += _score;
        YG2.SaveProgress();
        YG2.SetLeaderboard("Score",YG2.saves.MaxScore);
    }
}
