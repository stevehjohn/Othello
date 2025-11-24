using System.Numerics;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Engine;

public class Core
{
    private readonly Board _board;

    private readonly BoardAnalyser _analyser;

    public Core()
    {
        _board = new Board();
        
        _analyser = new BoardAnalyser(_board);
    }
    
    public void StartGame()
    {
        _board.InitialiseNewGame();
    }

    public bool MakeMove(Colour colour, int cell)
    {
        return _board.MakeMove(colour, cell);
    }

    public (int Score, int Cell) GetBestMove(Colour colour, int depth = 5)
    {
        var moves = _analyser.GetLegalMoves(colour);

        if (moves == 0 || depth == 0)
        {
            return (-1, -1);
        }
        
        var bestScore = int.MinValue;

        var bestMove = -1;
        
        while (moves > 0)
        {
            var cell = BitOperations.TrailingZeroCount(moves);

            moves ^= 1ul << cell;

            if (! _board.MakeMove(colour, cell))
            {
                continue;
            }

            var score = ScoreMove(colour, cell);
            
            _board.UndoLastMove();

            if (score > bestScore)
            {
                bestScore = score;

                bestMove = cell;
            }
        }

        return (bestScore, bestMove);
    }

    private int ScoreMove(Colour colour, int cell)
    {
        return 0;
    }
}