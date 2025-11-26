using System.Threading.Tasks;
using Othello.Engine;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Desktop.Orchestration;

public class Coordinator
{
    private readonly Core _core = new();
    
    private Colour _player;

    public Board Board { get; private set; }

    private Task _moveTask;

    private bool BlackIsCpu;

    private bool WhiteIsCpu;

    private int _bestMove;

    public void StartGame()
    {
        _core.StartGame();

        Board = new Board(_core.Board);

        _player = Colour.Black;
    }

    public void Update(double elapsedMilliseconds)
    {
        if (_moveTask == null)
        {
            if ((BlackIsCpu && _player == Colour.Black) || (WhiteIsCpu && _player == Colour.White))
            {
                _moveTask = Task.Run(() => _bestMove = _core.GetBestMove(_player).Cell);
            }
        }
        else if (_moveTask.IsCompleted)
        {
            MakeMove(_bestMove);
        }
    }

    public void CellClicked(int cell)
    {
        if ((! BlackIsCpu && _player == Colour.Black) || (! WhiteIsCpu && _player == Colour.White))
        {
            MakeMove(cell);
        }
    }

    private void MakeMove(int cell)
    {
        _core.MakeMove(_player, cell);

        _player = _player.Invert();
    }
}