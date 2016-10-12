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

        for (int i = 0; i < white.Count; i++){
            Console.WriteLine($"{white[i].FigureType} {white[i].x} {white[i].y}");
        }
        for (int i = 0; i < black.Count; i++)
        {
            Console.WriteLine($"{black[i].FigureType} {black[i].x} {black[i].y}");
        }
        BishopMoves(white, black, white.First());
        //DrawBoard(Board(white, black));
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
            board[figure.y, figure.x] = types[(int)figure.FigureType];
        foreach (var figure in opponent)
            board[figure.y, figure.x] = Char.ToLower(types[(int)figure.FigureType]);
        return board;
    }

    static void DrawBoard(char[,] board)
    {
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 0; j < 4; j++)
                Console.Write($"{board[j, i]}");
            Console.WriteLine("");
        }
    }

    static bool CanBeatQueen(List<Figure> myFigures, List<Figure> opponent)
    {
        Figure opponentQueen = opponent.First(x => x.FigureType == FigureType.Queen);
        foreach (var figure in myFigures)
        {
            var col = myFigures.ToList();
            col.Remove(figure);
            var board = Board(col, opponent);
            //if (figure.CanHit(opponentQueen.x, opponentQueen.y))
            //    return true;
        }
        return false;
    }

    static List<Cell> BishopMoves(List<Figure> myFigures, List<Figure> opponent, Figure figure)
    {
        var col = myFigures.ToList();
        //col.Remove(figure);
        var board = Board(col, opponent);
        var moves = new List<Cell>();
        int x, y;
        x = figure.x;
        y = figure.y;
        while (true)
        {
            x++;
            y++;
            if (x < 4 && y < 4 && (board[x, y] == '.' || Char.IsLower(board[x,y])))
                moves.Add(new Cell {x = x, y = y});
            else
                break;
        }
        x = figure.x;
        y = figure.y;
        while (true)
        {
            x--;
            y--;
            if (x >= 0 && y >= 0 && (board[x, y] == '.' || Char.IsLower(board[x, y])))
                moves.Add(new Cell { x = x, y = y });
            else
                break;
        }
        x = figure.x;
        y = figure.y;
        while (true)
        {
            x--;
            y++;
            if (x >= 0 && y < 4 && (board[x, y] == '.' || Char.IsLower(board[x, y])))
                moves.Add(new Cell { x = x, y = y });
            else
                break;
        }
        x = figure.x;
        y = figure.y;
        while (true)
        {
            x++;
            y--;
            if (x < 4 && y >= 0 && (board[x, y] == '.' || Char.IsLower(board[x, y])))
                moves.Add(new Cell { x = x, y = y });
            else
                break;
        }
        foreach (var move in moves)
            board[move.y, move.x] = 'x';
        DrawBoard(board);
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