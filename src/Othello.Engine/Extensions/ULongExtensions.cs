using Othello.Engine.Infrastructure;

namespace Othello.Engine.Extensions;

public static class ULongExtensions
{
    private const ulong ClearEastMask = 0xFEFEFEFEFEFEFEFEul;
    
    private const ulong ClearWestMask = 0x7F7F7F7F7F7F7F7Ful;
    
    extension(ulong value)
    {
        public ulong Shift(int direction)
        {
            return Constants.Directions[direction] switch
            {
                -9 => (value & ClearEastMask) >> 9,
                -8 => value >> 8,
                -7 => (value & ClearWestMask) >> 7,
                -1 => (value & ClearEastMask) >> 1,
                1  => (value & ClearWestMask) << 1,
                7  => (value & ClearEastMask) << 7,
                8  => value << 8,
                _  => (value & ClearWestMask) << 9
            };
        }
    }
}