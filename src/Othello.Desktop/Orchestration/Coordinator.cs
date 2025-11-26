using System.Threading.Tasks;
using Othello.Engine;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Desktop.Orchestration;

public class Coordinator
{
    private readonly Core _core = new();

    private Task _moveTask;

    private bool[] _playerIsCpu = new bool[2];

    private int _bestMove;

    private int _level;

    private double _moveCalculatedTime;

    public bool GameOver => _core.GameOver;

    public Colour Winner => _core.Board.BlackScore > _core.Board.WhiteScore ? Colour.Black : Colour.White;

    public Board Board { get; private set; }
    
    public Colour Player { get; private set; }

    public bool Thinking => _moveTask != null;

    public void StartGame(bool blackIsCpu = false, bool whiteIsCpu = true, int level = 10)
    {
        PlayerIsCpu(Colour.Black, blackIsCpu);

        PlayerIsCpu(Colour.White, whiteIsCpu);

        _level = level;
        
        _core.StartGame();

        Board = new Board(_core.Board);

        Player = Colour.Black;
    }

    public void Update(double elapsedMilliseconds)
    {
        if (_moveTask == null)
        {
            if (PlayerIsCpu(Player))
            {
                _moveTask = Task.Run(() => _bestMove = _core.GetBestMove(Player, _level).Cell);
            }
        }
        else if (_moveTask.IsCompleted)
        {
            if (PlayerIsCpu(Player.Invert()))
            {
            }

            MakeMove(_bestMove);

            _moveTask = null;
        }
    }

    public void CellClicked(int cell)
    {
        if (! PlayerIsCpu(Player))
        {
            MakeMove(cell);
        }
    }

    private bool PlayerIsCpu(Colour colour)
    {
        return _playerIsCpu[(int) colour];
    }

    private void PlayerIsCpu(Colour colour, bool isCpu)
    {
        _playerIsCpu[(int) colour] = isCpu;
    }

    private void MakeMove(int cell)
    {
        if (_core.MakeMove(Player, cell))
        {
            Board.MakeMove(Player, cell);

            Player = Player.Invert();
        }
    }
}