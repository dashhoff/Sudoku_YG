using UnityEngine;

public static class SudokuGenerator
{
    private static int[,] _grid = new int[9,9];

    public static int[,] GenerateSolved()
    {
        _grid = new int[9,9];
        Fill(0,0);
        return (int[,])_grid.Clone();
    }

    private static bool Fill(int x, int y)
    {
        if (y == 9) return true;

        int nextX = (x + 1) % 9;
        int nextY = y + (x == 8 ? 1 : 0);

        int[] numbers = Shuffle();

        foreach (int n in numbers)
        {
            if (IsValid(x,y,n))
            {
                _grid[x,y] = n;

                if (Fill(nextX,nextY))
                    return true;

                _grid[x,y] = 0;
            }
        }
        return false;
    }

    private static bool IsValid(int x,int y,int n)
    {
        for (int i=0;i<9;i++)
        {
            if (_grid[i,y]==n) return false;
            if (_grid[x,i]==n) return false;
        }

        int boxX=(x/3)*3;
        int boxY=(y/3)*3;

        for(int i=0;i<3;i++)
        for(int j=0;j<3;j++)
            if(_grid[boxX+i,boxY+j]==n) return false;

        return true;
    }

    private static int[] Shuffle()
    {
        int[] arr={1,2,3,4,5,6,7,8,9};
        for(int i=0;i<9;i++)
        {
            int r=Random.Range(i,9);
            (arr[i],arr[r])=(arr[r],arr[i]);
        }
        return arr;
    }
}