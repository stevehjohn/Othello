using Othello.Engine.Infrastructure;

namespace Othello.Engine.Kernel;

public class BoardAnalyser
{
    private static readonly int[] _directions = { -9, -8, -7, -1, 1, 7, 8, 9 };
    
    private const ulong NOT_A_FILE = 0xFEFEFEFEFEFEFEFEUL;
    
    private const ulong NOT_H_FILE = 0x7F7F7F7F7F7F7F7FUL;
    
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

        foreach (var direction in _directions)
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
    
    private ulong Shift(ulong bits, int dir)
    {
    
        return dir switch
        {
            -9 => (bits & NOT_A_FILE) >> 9,
            -8 => bits >> 8,
            -7 => (bits & NOT_H_FILE) >> 7,
            -1 => (bits & NOT_A_FILE) >> 1,
            1  => (bits & NOT_H_FILE) << 1,
            7  => (bits & NOT_A_FILE) << 7,
            8  => bits << 8,
            9  => (bits & NOT_H_FILE) << 9,
            _ => 0
        };
    }
}