using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Engine;

public class Core
{
    private readonly Board _board = new();

    public void StartGame()
    {
        _board.InitialiseNewGame();
    }

    public bool MakeMove(Colour colour, int cell)
    {
        var flipped = _board.FlipPieces(colour, cell);

        if (flipped == 0)
        {
            return false;
        }

        switch (colour)
        {
            case Colour.Black:
                _board.Black[cell] = true;
                
                break;
            
            case Colour.White:
            default:
                _board.White[cell] = true;
                
                break;
        }

        return true;
    }

    public int GetBestMove(Colour colour)
    {
        throw new NotImplementedException();
    }
}