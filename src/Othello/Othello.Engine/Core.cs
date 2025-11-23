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
}