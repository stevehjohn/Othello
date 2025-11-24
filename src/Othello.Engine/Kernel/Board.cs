using System.Numerics;
using System.Text;
using Othello.Engine.Bitboards;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Plane = Othello.Engine.Bitboards.Plane;

namespace Othello.Engine.Kernel;

public class Board
{
    private Planes _planes = new();

    public Plane Black => _planes.Black;

    public Plane White => _planes.White;

    public int BlackScore => Black.PieceCount;

    public int WhiteScore => White.PieceCount;

    public Plane this[Colour colour] => colour == Colour.Black ? _planes.Black : _planes.White;

    private readonly Stack<Planes> _history = new();

    private bool this[int cell] => _planes[cell];

    public void InitialiseNewGame()
    {
        Clear();
        
        _planes.White[27] = true;

        _planes.Black[28] = true;

        _planes.Black[35] = true;

        _planes.White[36] = true;
    }

    public void InitialiseFromBoardState(ulong blackPieces, ulong whitePieces)
    {
        Clear();

        _planes.Black.Pieces = blackPieces;

        _planes.White.Pieces = whitePieces;
    }

    public bool MakeMove(Colour colour, int cell)
    {
        SaveState();
        
        var flipped = FlipPieces(colour, cell);

        if (flipped == 0)
        {
            _history.Pop();
            
            return false;
        }

        switch (colour)
        {
            case Colour.Black:
                _planes.Black[cell] = true;

                break;

            case Colour.White:
            default:
                _planes.White[cell] = true;

                break;
        }

        return true;
    }

    public void UndoLastMove() => _planes = _history.Count > 0 ? _history.Pop() : _planes;

    private void SaveState() => _history.Push(_planes);

    private int FlipPieces(Colour colour, int cell)
    {
        if (_planes[cell])
        {
            return 0;
        }

        var friendly = colour == Colour.Black ? _planes.Black.Pieces : _planes.White.Pieces;
        
        var opponent = colour == Colour.Black ? _planes.White.Pieces : _planes.Black.Pieces;
    
        var flips = 0ul;
    
        for (var i = 0; i < Constants.Directions.Length; i++)
        {
            flips |= GetFlipsInDirection(cell, i, friendly, opponent);
        }

        if (flips == 0)
        {
            return 0;
        }
    
        if (colour == Colour.Black)
        {
            _planes.Black.Pieces |= flips;
            
            _planes.White.Pieces &= ~flips;
        }
        else
        {
            _planes.White.Pieces |= flips;
            
            _planes.Black.Pieces &= ~flips;
        }
    
        return BitOperations.PopCount(flips);
    }
    
    private static ulong GetFlipsInDirection(int cell, int direction, ulong friendly, ulong opponent)
    {
        var flips = 0ul;
        
        for (var i = 0; i < 6; i++)
        {
            var shifted = (1ul << cell).Shift(direction);
            
            if (shifted == 0)
            {
                break;
            } 
        
            cell = BitOperations.TrailingZeroCount(shifted);
        
            if ((opponent & shifted) != 0)
            {
                flips |= shifted;
            }
            else if ((friendly & shifted) != 0 && flips != 0)
            {
                return flips;
            }
            else
            {
                break;
            }
        }
    
        return 0;
    }
    
    private void Clear()
    {
        for (var i = 0; i < Constants.Cells; i++)
        {
            _planes.Black[i] = false;

            _planes.White[i] = false;
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var cell = 0; cell < Constants.Cells; cell++)
        {
            if (cell > 0 && cell % Constants.Rows == 0)
            {
                builder.AppendLine();
            }

            if (! this[cell])
            {
                builder.Append('·');
                
                continue;
            }

            if (_planes.Black[cell])
            {
                builder.Append('○');
                
                continue;
            }
            
            builder.Append('●');
        }

        return builder.ToString();
    }
}