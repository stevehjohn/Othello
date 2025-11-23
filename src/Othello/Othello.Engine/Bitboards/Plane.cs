using System.Numerics;

namespace Othello.Engine.Bitboards;

public struct Plane
{
    private ulong _pieces;
    
    public bool this[int cell]
    {
        get => (_pieces & 1ul << cell) > 0;
        set
        {
            if (value)
            {
                _pieces |= 1ul << cell;
            }
            else
            {
                _pieces &= ~(1ul << cell);
            }
        }
    }

    public int PieceCount => BitOperations.PopCount(_pieces);
}