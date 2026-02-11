public static class SudokuSolver
{
    public static int CountSolutions(int[,] grid)
    {
        int[,] copy=(int[,])grid.Clone();
        return Solve(copy,0,0,0);
    }

    private static int Solve(int[,] g,int x,int y,int count)
    {
        if(count>1) return count;
        if(y==9) return count+1;

        int nx=(x+1)%9;
        int ny=y+(x==8?1:0);

        if(g[x,y]!=0)
            return Solve(g,nx,ny,count);

        for(int n=1;n<=9;n++)
        {
            if(IsValid(g,x,y,n))
            {
                g[x,y]=n;
                count=Solve(g,nx,ny,count);
                g[x,y]=0;
            }
        }
        return count;
    }

    private static bool IsValid(int[,] g,int x,int y,int n)
    {
        for(int i=0;i<9;i++)
        {
            if(g[i,y]==n) return false;
            if(g[x,i]==n) return false;
        }

        int bx=(x/3)*3;
        int by=(y/3)*3;

        for(int i=0;i<3;i++)
        for(int j=0;j<3;j++)
            if(g[bx+i,by+j]==n) return false;

        return true;
    }
}