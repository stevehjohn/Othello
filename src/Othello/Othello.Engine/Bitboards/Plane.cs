namespace Othello.Engine.Infrastructure;

public class Plane
{
    private ulong _pieces;

    public bool this[int cell]
    {
        get => (_pieces & 1ul << (cell - 1)) > 0;
        set
        {
            if (value)
            {
                _pieces |= 1ul << (cell - 1);
            }
            else
            {
                _pieces &= ~(1ul << (cell - 1));
            }
        }
    }
}