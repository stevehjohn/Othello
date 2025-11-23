using Othello.Engine.Bitboards;
using Othello.Engine.Infrastructure;

namespace Othello.Engine.Kernel;

public class Board
{
    private Planes _planes = new();

    public Plane Black => _planes.Black;

    public Plane White => _planes.White;

    public bool this[int cell] => _planes[cell];

    private Stack<Planes> _history = new();

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
                _planes.Black[cell] = true;

                break;

            case Colour.White:
            default:
                _planes.White[cell] = true;

                break;
        }

        return true;
    }

    public void UndoLastMove()
    {
        if (_history.Count == 0)
        {
            return;
        }

        _planes = _history.Pop();
    }

    private void SaveState()
    {
        _history.Push(_planes);
    }

    private int FlipPieces(Colour colour, int cell)
    {
        if (! _planes[cell])
        {
            return 0;
        }

        throw new NotImplementedException();
    }
}