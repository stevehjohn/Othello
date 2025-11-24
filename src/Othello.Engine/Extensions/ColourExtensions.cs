using Othello.Engine.Infrastructure;

namespace Othello.Engine.Extensions;

public static class ColourExtensions
{
    extension(Colour colour)
    {
        public Colour Invert()
        {
            return colour == Colour.White ? Colour.Black : Colour.White;
        }
    }
}