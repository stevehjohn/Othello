using Othello.Engine.Infrastructure;

namespace Othello.Engine.Kernel;

public class BoardAnalyser
{
    private static readonly int[] _directions = { -9, -8, -7, -1, 1, 7, 8, 9 };
    
    private readonly Board _board;

    public BoardAnalyser(Board board)
    {
        _board = board;
    }

    public ulong GetLegalMoves(Colour colour)
    {
        var friendly = colour == Colour.Black ? _board.Black.Pieces : _board.White.Pieces;

        var opponent = colour == Colour.Black ? _board.White.Pieces : _board.Black.Pieces;

        var empty = ~(friendly | opponent);

        var legalMoves = 0ul;

        foreach (var dir in _directions)
        {
            legalMoves |= GetLegalMovesInDirection(friendly, opponent, empty, dir);
        }

        return legalMoves;
    }
}