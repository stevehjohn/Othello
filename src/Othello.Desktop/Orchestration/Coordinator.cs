using Othello.Engine;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Desktop.Orchestration;

public class Coordinator
{
    private readonly Core _core = new();
    
    private Colour _player = Colour.Black;

    public Board Board { get; private set; }

    public void StartGame()
    {
        _core.StartGame();

        Board = new Board(_core.Board);
    }

    public void CellClicked(int cell)
    {
    }
}