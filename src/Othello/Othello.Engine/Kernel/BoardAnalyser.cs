using Othello.Engine.Infrastructure;

namespace Othello.Engine.Kernel;

public class BoardAnalyser
{
    private static readonly int[] Directions = { -9, -8, -7, -1, 1, 7, 8, 9 };
    
    private const ulong ClearWestMask = 0xFEFEFEFEFEFEFEFEUL;
    
    private const ulong ClearEastMask = 0x7F7F7F7F7F7F7F7FUL;
    
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

        foreach (var direction in Directions)
        {
            legalMoves |= GetLegalMovesForDirection(friendly, opponent, empty, direction);
        }

        return legalMoves;
    }
    
    private ulong GetLegalMovesForDirection(ulong friendly, ulong opponent, ulong empty, int direction)
    {
        var candidates = Shift(friendly, direction) & opponent;
    
        candidates |= Shift(candidates, direction) & opponent;
        
        candidates |= Shift(candidates, direction) & opponent;
        
        candidates |= Shift(candidates, direction) & opponent;
        
        candidates |= Shift(candidates, direction) & opponent;
        
        candidates |= Shift(candidates, direction) & opponent;
    
        return Shift(candidates, direction) & empty;
    }
    
    private static ulong Shift(ulong bits, int dir)
    {
        return dir switch
        {
            -9 => (bits & ClearWestMask) >> 9,
            -8 => bits >> 8,
            -7 => (bits & ClearEastMask) >> 7,
            -1 => (bits & ClearWestMask) >> 1,
            1  => (bits & ClearEastMask) << 1,
            7  => (bits & ClearWestMask) << 7,
            8  => bits << 8,
            9  => (bits & ClearEastMask) << 9,
            _ => 0
        };
    }
}