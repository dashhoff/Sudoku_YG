using System;
using System.Collections.Generic;

[Serializable]
public class GameSave
{
    public int _seed;

    public List<Move> _moves = new();

    public int _lives;
    public int _score;
    public int _hints;
}

[Serializable]
public class Move
{
    public bool _wrong;
    public int _x;
    public int _y;
    public int _value;
}