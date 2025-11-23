using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;

namespace Othello.Engine.Kernel;

public class BoardAnalyser
{
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

        for (var i = 0; i < Constants.Directions.Length; i++)
        {
            legalMoves |= GetLegalMovesForDirection(friendly, opponent, empty, i);
        }

        return legalMoves;
    }
    
    private static ulong GetLegalMovesForDirection(ulong friendly, ulong opponent, ulong empty, int direction)
    {
        var candidates = friendly.Shift(direction) & opponent;
    
        candidates |= candidates.Shift(direction) & opponent;
        
        candidates |= candidates.Shift(direction) & opponent;
        
        candidates |= candidates.Shift(direction) & opponent;
        
        candidates |= candidates.Shift(direction) & opponent;
        
        candidates |= candidates.Shift(direction) & opponent;
    
        return candidates.Shift(direction) & empty;
    }
}