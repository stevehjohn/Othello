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
        return _board.MakeMove(colour, cell);
    }

    public int GetBestMove(Colour colour)
    {
        throw new NotImplementedException();
    }
}