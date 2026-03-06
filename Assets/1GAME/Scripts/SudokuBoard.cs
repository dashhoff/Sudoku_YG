using UnityEngine;

public class SudokuBoard : MonoBehaviour
{
    [SerializeField] private SudokuCell _cellPrefab;
    [SerializeField] private Transform _container;

    private SudokuCell[,] _cells = new SudokuCell[9,9];

    private int[,] _puzzle;
    private int[,] _solution;

    public int holes = 30;

    public void Init()
    {
        GeneratePuzzle(holes);
        Spawn();
        InitController();
    }

    private void GeneratePuzzle(int holes)
    {
        _solution = SudokuGenerator.GenerateSolved();
        _puzzle = (int[,])_solution.Clone();

        int attempts = holes;
        while (attempts > 0)
        {
            int x = Random.Range(0,9);
            int y = Random.Range(0,9);
            if (_puzzle[x,y]==0) continue;

            int backup = _puzzle[x,y];
            _puzzle[x,y] = 0;

            int solutions = SudokuSolver.CountSolutions(_puzzle);
            if (solutions != 1)
                _puzzle[x,y] = backup;
            else
                attempts--;
        }
    }
    
    public void GenerateFromSeed(int seed, int holes)
    {
        Random.InitState(seed);

        _solution = SudokuGenerator.GenerateSolved();
        _puzzle = (int[,])_solution.Clone();

        int attempts = holes;

        while (attempts > 0)
        {
            int x = Random.Range(0,9);
            int y = Random.Range(0,9);

            if (_puzzle[x,y]==0) 
                continue;

            int backup = _puzzle[x,y];
            _puzzle[x,y] = 0;

            int solutions = SudokuSolver.CountSolutions(_puzzle);

            if (solutions != 1)
                _puzzle[x,y] = backup;
            else
                attempts--;
        }
    }
    
    public int[,] GetPuzzle() => _puzzle;
    public int[,] GetSolution() => _solution;

    private void Spawn()
    {
        for(int y=0;y<9;y++)
        for(int x=0;x<9;x++)
        {
            SudokuCell cell = Instantiate(_cellPrefab,_container);
            int value = _puzzle[x,y];
            bool fixedCell = value != 0;
            cell.Init(new Vector2Int(x,y), value, fixedCell);
            _cells[x,y] = cell;
        }
    }

    public void SpawnFromPuzzle()
    {
        foreach (Transform _child in _container)
            Destroy(_child.gameObject);

        for(int y=0;y<9;y++)
        for(int x=0;x<9;x++)
        {
            SudokuCell _cell = Instantiate(_cellPrefab,_container);

            int _value = _puzzle[x,y];
            bool _fixed = _value != 0;

            _cell.Init(new Vector2Int(x,y), _value, _fixed);
            _cells[x,y] = _cell;
        }
        
        var controller = FindObjectOfType<GameController>();
        if (controller != null)
            controller.InitBoard(_puzzle, _solution);
    }

    private void InitController()
    {
        var controller = FindObjectOfType<GameController>();
        if (controller != null)
            controller.InitBoard(_puzzle,_solution);
    }
}