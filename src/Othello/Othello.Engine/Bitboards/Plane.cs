using System.Numerics;

namespace Othello.Engine.Bitboards;

public struct Plane
{
    public ulong Pieces;
    
    public bool this[int cell]
    {
        get => (Pieces & 1ul << cell) > 0;
        set
        {
            if (value)
            {
                Pieces |= 1ul << cell;
            }
            else
            {
                Pieces &= ~(1ul << cell);
            }
        }
    }

    public int PieceCount => BitOperations.PopCount(Pieces);
}