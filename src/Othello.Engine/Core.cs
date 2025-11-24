using System.Numerics;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Engine;

public class Core
{
    private const ulong CornerMask = 0b1000000100000000000000000000000000000000000000000000000010000001ul;
    
    private const ulong XSquareMask = 0b0000000001000010000000000000000000000000000000000100001000000000ul;
    
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
        if (depth == 0)
        {
            return (EvaluateBoard(colour), -1);
        }
        
        var moves = _analyser.GetLegalMoves(colour);

        if (moves == 0)
        {
            var opponentMoves = _analyser.GetLegalMoves(colour.Invert());

            if (opponentMoves == 0)
            {
                return (EvaluateBoard(colour), -1);
            }

            var result = GetBestMove(colour.Invert(), depth - 1);

            return (-result.Score, -1);
        }

        var bestScore = int.MinValue + 1;

        var bestMove = -1;
        
        while (moves > 0)
        {
            var cell = BitOperations.TrailingZeroCount(moves);

            moves ^= 1ul << cell;

            if (! _board.MakeMove(colour, cell))
            {
                continue;
            }

            var result = GetBestMove(colour.Invert(), depth - 1);

            _board.UndoLastMove();

            var score = -result.Score;

            if (score > bestScore)
            {
                bestScore = score;

                bestMove = cell;
            }
        }

        return (bestScore, bestMove);
    }

    private int EvaluateBoard(Colour colour)
    {
        var score = 4;

        var plane = _board[colour];

        var mask = plane.Pieces & XSquareMask;

        score -= BitOperations.PopCount(mask);

        mask = plane.Pieces & CornerMask;

        score += BitOperations.PopCount(mask) * 100;
        
        return score;
    }
}