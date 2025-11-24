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

    public (int Score, int Cell) GetBestMove(Colour player, int depth = 5)
    {
        var playerMoves = _analyser.GetLegalMoves(player);

        if (depth == 0)
        {
            var opponentMoves = _analyser.GetLegalMoves(player.Invert());

            return (EvaluateBoard(player, playerMoves, opponentMoves), -1);
        }
        
        if (playerMoves == 0)
        {
            var opponentMoves = _analyser.GetLegalMoves(player.Invert());

            if (opponentMoves == 0)
            {
                return (EvaluateBoard(player, playerMoves, opponentMoves), -1);
            }

            var result = GetBestMove(player.Invert(), depth - 1);

            return (-result.Score, -1);
        }

        var bestScore = int.MinValue + 1;

        var bestMove = -1;
        
        while (playerMoves > 0)
        {
            var cell = BitOperations.TrailingZeroCount(playerMoves);

            playerMoves ^= 1ul << cell;

            if (! _board.MakeMove(player, cell))
            {
                continue;
            }

            var result = GetBestMove(player.Invert(), depth - 1);

            _board.UndoLastMove();

            var score = -result.Score;

            if (score > bestScore || (score == bestScore && Random.Shared.Next(2) == 1))
            {
                bestScore = score;

                bestMove = cell;
            }
        }

        return (bestScore, bestMove);
    }

    private int EvaluateBoard(Colour player, ulong playerMoves, ulong opponentMoves)
    {
        var opponent = player.Invert();
        
        var playerPlane = _board[player];

        var opponentPlane = _board[opponent];

        var score = 0;

        var delta = BitOperations.PopCount(playerPlane.Pieces & CornerMask);

        delta -= BitOperations.PopCount(opponentPlane.Pieces & CornerMask);

        score += delta * 1_000;

        delta = BitOperations.PopCount(opponentPlane.Pieces & XSquareMask);

        delta -= BitOperations.PopCount(playerPlane.Pieces & XSquareMask);

        score += delta * 80;

        delta = BitOperations.PopCount(playerMoves);

        delta -= BitOperations.PopCount(opponentMoves);

        score += delta * 100;
        
        score += (playerPlane.PieceCount - opponentPlane.PieceCount) * CalculateMaterialWeight();
        
        return score;
    }

    private int CalculateMaterialWeight()
    {
        var pieces = _board.BlackScore + _board.WhiteScore;

        return (int) (pieces * 0.3_125);
    }
}