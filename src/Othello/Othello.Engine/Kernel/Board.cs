using Othello.Engine.Bitboards;
using Othello.Engine.Infrastructure;

namespace Othello.Engine.Kernel;

public class Board
{
    private readonly Planes _planes = new();

    public Plane Black => _planes.Black;

    public Plane White => _planes.White;

    public bool this[int cell] => _planes[cell];
    
    public void InitialiseNewGame()
    {
        for (var i = 0; i < Constants.Cells; i++)
        {
            _planes.Black[i] = false;

            _planes.White[i] = false;
        }

        _planes.Black[27] = true;

        _planes.White[28] = true;

        _planes.White[35] = true;

        _planes.Black[36] = true;
    }

    public int FlipPieces(Colour colour, int cell)
    {
        if (! _planes[cell])
        {
            return 0;
        }

        throw new NotImplementedException();
    }
}