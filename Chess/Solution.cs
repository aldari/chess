using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Solution
{
    static void Main()
    {
        Console.SetIn(new StreamReader("input.txt"));

        int g = Convert.ToInt32(Console.ReadLine());
        while (g > 0)
        {
            SolveTask();
            g--;
        }
        Console.ReadKey();
    }

    enum FigureType { Queen, Knight, Bishop, Rook };
    struct Figure
    {
        public FigureType FigureType;
        public int x;
        public int y;
    }

    struct Cell
    {
        public int x;
        public int y;
    }

    enum SortOutResult
    {
        OpponentWin,
        MyWin,
        Raw
    };

    static void SolveTask()
    {
        
        int[] d = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
        int w = d[0];
        int b = d[1];
        int m = d[2];

        var white = new List<Figure>();
        var black = new List<Figure>();
        for (int i = 0; i < w; i++)
            white.Add(ReadFigure(Console.ReadLine()));
        for (int i = 0; i < b; i++)
            black.Add(ReadFigure(Console.ReadLine()));

        //DrawBoard(Board(white, black));
        Console.WriteLine(AllPossibleMoves(white, black, 1, 3)==SortOutResult.MyWin ? "YES" : "NO" );
    }

    static char[,] Board(List<Figure> myFigures, List<Figure> opponent)
    {
        var board = new char[4, 4] {
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' }
        };
        char[] types = { 'Q', 'N', 'B', 'R' };
        foreach (var figure in myFigures)
            board[figure.x, figure.y] = types[(int)figure.FigureType];
        foreach (var figure in opponent)
            board[figure.x, figure.y] = Char.ToLower(types[(int)figure.FigureType]);
        return board;
    }

    static void DrawBoard(char[,] board)
    {
        Console.WriteLine("");
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 0; j < 4; j++)
                Console.Write($"{board[i, j]}");
            Console.WriteLine("");
        }
    }

    static SortOutResult AllPossibleMoves(List<Figure> myFigures, List<Figure> opponent, int count, int limit)
    {
        bool AllMyWin = true;
        bool AllOpponentWin = true;

        foreach (var figure in myFigures)
        {
            var col = myFigures.ToList();
            col.Remove(figure);
            List<Cell> moves = null;
            switch (figure.FigureType)
            {
                case FigureType.Bishop:
                    moves = BishopMoves(col, opponent, figure);
                    break;
                case FigureType.Knight:
                    moves = KnightMoves(col, opponent, figure);
                    break;
                case FigureType.Queen:
                    moves = QueenMoves(col, opponent, figure);
                    break;
                case FigureType.Rook:
                    moves = RookMoves(col, opponent, figure);
                    break;
            }
            var queen = opponent.First(x => x.FigureType == FigureType.Queen);
            var t = moves.Where(x => x.x == queen.x && x.y == queen.y).ToList();
            if (t.Count > 0)
                return SortOutResult.MyWin;
        }

        foreach (var figure in myFigures)
        {
            var col = myFigures.ToList();
            col.Remove(figure);
            List<Cell> moves = null;
            switch (figure.FigureType)
            {
                case FigureType.Bishop:
                    moves = BishopMoves(col, opponent, figure);
                    break;
                case FigureType.Knight:
                    moves = KnightMoves(col, opponent, figure);
                    break;
                case FigureType.Queen:
                    moves = QueenMoves(col, opponent, figure);
                    break;
                case FigureType.Rook:
                    moves = RookMoves(col, opponent, figure);
                    break;
            }

            if (count != limit)
            {
                foreach (var move in moves)
                {
                    var c = col.ToList();
                    var o = opponent.ToList();
                    var f = new Figure { FigureType = figure.FigureType, x = move.x, y = move.y };
                    c.Add(f);
                    if (o.Any(s => s.x == move.x && s.y == move.y))
                    {
                        var w = o.Single(s => s.x == move.x && s.y == move.y);
                        o.Remove(w);
                    }

                    //DrawBoard(Board(c, o));
                    var result = AllPossibleMoves(o, c, count+1, limit);

                    if (result == SortOutResult.OpponentWin)
                        return SortOutResult.MyWin;

                    if (result != SortOutResult.MyWin)
                        AllMyWin = false;
                }
            }
        }
        if (AllMyWin)
            return SortOutResult.OpponentWin;
        return SortOutResult.Raw;
    }

    static List<Cell> KnightMoves(List<Figure> myFigures, List<Figure> opponent, Figure figure)
    {
        var col = myFigures.ToList();
        var board = Board(col, opponent);
        var moves = new List<Cell>();

        var _x = figure.x + 2;
        var _y = figure.y + 1;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

        _x = figure.x - 2;
        _y = figure.y + 1;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

        _x = figure.x + 2;
        _y = figure.y - 1;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

        _x = figure.x - 2;
        _y = figure.y - 1;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

        _x = figure.x + 1;
        _y = figure.y + 2;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

        _x = figure.x - 1;
        _y = figure.y + 2;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

        _x = figure.x + 1;
        _y = figure.y - 2;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

        _x = figure.x - 1;
        _y = figure.y - 2;
        if (_x >= 0 && _x < 4 && _y >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            moves.Add(new Cell { x = _x, y = _y });

//        foreach (var move in moves)
//            board[move.x, move.y] = 'x';
//        DrawBoard(board);
        return moves;
    }

    static List<Cell> QueenMoves(List<Figure> myFigures, List<Figure> opponent, Figure figure)
    {
        return HorizontalMoves(myFigures, opponent, figure).Concat(DiagonalMoves(myFigures, opponent, figure)).ToList();
    }

    static List<Cell> RookMoves(List<Figure> myFigures, List<Figure> opponent, Figure figure)
    {
        return HorizontalMoves(myFigures, opponent, figure);
    }

    static List<Cell> BishopMoves(List<Figure> myFigures, List<Figure> opponent, Figure figure)
    {
        return DiagonalMoves(myFigures, opponent, figure);
    }

    static List<Cell> DiagonalMoves(List<Figure> myFigures, List<Figure> opponent, Figure figure)
    {
        var col = myFigures.ToList();
        var board = Board(col, opponent);
        var moves = new List<Cell>();
        var _x = figure.x;
        var _y = figure.y;
        while (true)
        {
            _x++;
            _y++;
            if (_x < 4 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x,_y])))
            {
                moves.Add(new Cell { x = _x, y = _y });
                if (Char.IsLower(board[_x, _y]))
                    break;
            }
            else
                break;
        }
        _x = figure.x;
        _y = figure.y;
        while (true)
        {
            _x--;
            _y--;
            if (_x >= 0 && _y >= 0 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            {
                moves.Add(new Cell { x = _x, y = _y });
                if (Char.IsLower(board[_x, _y]))
                    break;
            }
            else
                break;
        }
        _x = figure.x;
        _y = figure.y;
        while (true)
        {
            _x--;
            _y++;
            if (_x >= 0 && _y < 4 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            {
                moves.Add(new Cell { x = _x, y = _y });
                if (Char.IsLower(board[_x, _y]))
                    break;
            }
            else
                break;
        }
        _x = figure.x;
        _y = figure.y;
        while (true)
        {
            _x++;
            _y--;
            if (_x < 4 && _y >= 0 && (board[_x, _y] == '.' || Char.IsLower(board[_x, _y])))
            {
                moves.Add(new Cell { x = _x, y = _y });
                if (Char.IsLower(board[_x, _y]))
                    break;
            }
            else
                break;
        }
//        foreach (var move in moves)
//            board[move.x, move.y] = 'x';
//        DrawBoard(board);
        return moves;
    }

    static List<Cell> HorizontalMoves(List<Figure> myFigures, List<Figure> opponent, Figure figure)
    {
        var col = myFigures.ToList();
        var board = Board(col, opponent);
        var moves = new List<Cell>();
        int x, y;

        x = figure.x;
        y = figure.y;
        while (true)
        {
            x++;
            if (x < 4 && (board[x, y] == '.' || Char.IsLower(board[x, y])))
            {
                moves.Add(new Cell { x = x, y = y });
                if (Char.IsLower(board[x, y]))
                    break;
            }
            else
                break;
        }

        x = figure.x;
        y = figure.y;
        while (true)
        {
            x--;
            if (x >= 0 && (board[x, y] == '.' || Char.IsLower(board[x, y])))
            {
                moves.Add(new Cell { x = x, y = y });
                if (Char.IsLower(board[x, y]))
                    break;
            }
            else
                break;
        }

        x = figure.x;
        y = figure.y;
        while (true)
        {
            y++;
            if (y < 4 && (board[x, y] == '.' || Char.IsLower(board[x, y])))
            {
                moves.Add(new Cell { x = x, y = y });
                if (Char.IsLower(board[x, y]))
                    break;
            }
            else
                break;
        }

        x = figure.x;
        y = figure.y;
        while (true)
        {
            y--;
            if (y >= 0 && (board[x, y] == '.' || Char.IsLower(board[x, y])))
            {
                moves.Add(new Cell { x = x, y = y });
                if (Char.IsLower(board[x, y]))
                    break;
            }
            else
                break;
        }
//        foreach (var move in moves)
//            board[move.x, move.y] = 'x';
//        DrawBoard(board);
        return moves;
    }

    static Figure ReadFigure(string s)
    {
        string[] t = s.Split(' ');
        Figure figure = new Figure();
        switch (t[0])
        {
            case "Q":
                figure.FigureType = FigureType.Queen;
                break;
            case "N":
                figure.FigureType = FigureType.Knight;
                break;
            case "B":
                figure.FigureType = FigureType.Bishop;
                break;
            case "R":
                figure.FigureType = FigureType.Rook;
                break;
        }
        figure.y = t[1][0] - 'A';
        figure.x = Convert.ToInt32(t[2]) - 1;
        return figure;
    }
}