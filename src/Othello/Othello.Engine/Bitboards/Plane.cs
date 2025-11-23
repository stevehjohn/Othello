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
}