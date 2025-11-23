using Othello.Engine.Bitboards;
using Othello.Engine.Infrastructure;

namespace Othello.Engine;

public class Core
{
    private readonly Planes _board = new();

    public void InitialiseNewGame()
    {
        _board.Black[27] = true;

        _board.White[28] = true;

        _board.White[35] = true;

        _board.Black[36] = true;
    }

    public bool MakeMove(Colour colour, int cell)
    {
        var flipped = FlipPieces(colour, cell);

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

    private int FlipPieces(Colour colour, int newPiece)
    {
        throw new NotImplementedException();
    }
}