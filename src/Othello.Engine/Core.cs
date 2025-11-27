using System.Numerics;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Engine;

public class Core
{
    private const ulong CornerMask = 0b1000000100000000000000000000000000000000000000000000000010000001ul;

    private const ulong XSquareMask = 0b0000000001000010000000000000000000000000000000000100001000000000ul;

    private readonly BoardAnalyser _analyser;

    public Board Board { get; }

    public bool GameOver { get; private set; }

    public Core()
    {
        Board = new Board();

        _analyser = new BoardAnalyser(Board);
    }

    public void StartGame()
    {
        Board.InitialiseNewGame();

        GameOver = false;
    }

    public bool MakeMove(Colour colour, int cell)
    {
        if (cell is < 0 or >= Constants.Cells)
        {
            return false;
        }

        return Board.MakeMove(colour, cell);
    }

    public (int Score, int Cell) GetBestMove(Colour player, int depth = 7)
    {
        if (Board.EmptyCells < 18)
        {
            depth = int.MaxValue;
        }

        var result = GetBestMove(player, depth, int.MinValue, int.MaxValue);

        GameOver = _analyser.GetLegalMoves(Colour.Black) == 0 && _analyser.GetLegalMoves(Colour.White) == 0;

        return result;
    }

    public bool HasLegalMoves(Colour colour) => _analyser.GetLegalMoves(colour) > 0;

    private (int Score, int Cell) GetBestMove(Colour player, int depth, int alpha, int beta)
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

            var result = GetBestMove(player.Invert(), depth - 1, -beta, -alpha);

            return (-result.Score, -1);
        }

        var bestScore = int.MinValue + 1;

        var bestMove = -1;

        var bestMoveCount = 0;

        while (playerMoves > 0)
        {
            var cell = PickNextMove(playerMoves);

            playerMoves ^= 1ul << cell;

            if (! Board.MakeMove(player, cell))
            {
                continue;
            }

            var result = GetBestMove(player.Invert(), depth - 1, -beta, -alpha);

            Board.UndoLastMove();

            var score = -result.Score;

            if (score > bestScore)
            {
                bestScore = score;

                bestMove = cell;

                bestMoveCount = 1;
            }
            else if (score == bestScore)
            {
                bestMoveCount++;

                if (Random.Shared.Next(bestMoveCount) == 0)
                {
                    bestMove = cell;
                }
            }

            if (score > alpha)
            {
                alpha = score;
            }

            if (alpha >= beta)
            {
                break;
            }
        }

        return (bestScore, bestMove);
    }
    
    private static int PickNextMove(ulong moves)
    {
        var corners = moves & CornerMask;
        
        if (corners != 0)
        {
            return BitOperations.TrailingZeroCount(corners);
        }

        var xSquares = moves & XSquareMask;
        
        if (xSquares == 0)
        {
            return BitOperations.TrailingZeroCount(moves);
        }

        var nonXSquares = moves & ~XSquareMask;
        
        if (nonXSquares != 0)
        {
            return BitOperations.TrailingZeroCount(nonXSquares);
        }

        return BitOperations.TrailingZeroCount(xSquares);
    }


    private int EvaluateBoard(Colour player, ulong playerMoves, ulong opponentMoves)
    {
        var opponent = player.Invert();

        var playerPlane = Board[player];

        var opponentPlane = Board[opponent];

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
        var pieces = Board.BlackScore + Board.WhiteScore;

        return (int) (pieces * 0.3_125);
    }
}