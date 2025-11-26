using Othello.Engine;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Desktop.Orchestration;

public class Coordinator
{
    private readonly Core _core = new();
    
    private Colour _player = Colour.Black;

    public Board Board { get; private set; }

    public void CellClicked(int cell)
    {
    }
}