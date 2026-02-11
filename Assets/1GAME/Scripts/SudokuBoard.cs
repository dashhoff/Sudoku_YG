using UnityEngine;

public class SudokuBoard : MonoBehaviour
{
    [SerializeField] private SudokuCell _cellPrefab;
    [SerializeField] private Transform _container;

    private SudokuCell[,] _cells = new SudokuCell[9,9];

    private int[,] _puzzle;
    private int[,] _solution;

    public int holes = 30;

    private void Start()
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

    private void InitController()
    {
        var controller = FindObjectOfType<GameController>();
        if (controller != null)
            controller.InitBoard(_puzzle,_solution);
    }
}