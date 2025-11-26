using Othello.Engine;
using Othello.Engine.Kernel;

namespace Othello.Desktop.Orchestration;

public class Coordinator
{
    private readonly Core _core = new();
    
    public Board Board { get; set; }
}