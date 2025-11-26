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

    private bool _blackIsCpu;

    private bool _whiteIsCpu;

    private int _bestMove;

    private int _level;

    public void StartGame(bool blackIsCpu = false, bool whiteIsCpu = true, int level = 10)
    {
        _blackIsCpu = blackIsCpu;

        _whiteIsCpu = whiteIsCpu;

        _level = level;
        
        _core.StartGame();

        Board = new Board(_core.Board);

        _player = Colour.Black;
    }

    public void Update(double elapsedMilliseconds)
    {
        if (_moveTask == null)
        {
            if ((_blackIsCpu && _player == Colour.Black) || (_whiteIsCpu && _player == Colour.White))
            {
                _moveTask = Task.Run(() => _bestMove = _core.GetBestMove(_player, _level).Cell);
            }
        }
        else if (_moveTask.IsCompleted)
        {
            MakeMove(_bestMove);

            _moveTask = null;
        }
    }

    public void CellClicked(int cell)
    {
        if ((! _blackIsCpu && _player == Colour.Black) || (! _whiteIsCpu && _player == Colour.White))
        {
            MakeMove(cell);
        }
    }

    private void MakeMove(int cell)
    {
        if (_core.MakeMove(_player, cell))
        {
            Board.MakeMove(_player, cell);

            _player = _player.Invert();
        }
    }
}