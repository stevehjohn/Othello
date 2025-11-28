using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Othello.Engine;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Desktop.Orchestration;

public class Coordinator
{
    private const int MoveDelay = 200;
    
    private readonly Core _core = new();

    private readonly bool[] _playerIsCpu = new bool[2];

    private readonly List<int> _playerLegalMoves = [];

    private readonly BoardAnalyser _boardAnalyser = new();

    private Task _moveTask;

    private int _bestMove = -1;

    private int _level;

    private Colour? _playerLegalMovesColour;

    private double _moveCalculatedTime = -1;

    private Stack<Colour> _turns = [];

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

        _playerLegalMovesColour = null;
        
        _core.StartGame();

        Board = new Board(_core.Board);

        Player = Colour.Black;

        _moveCalculatedTime = -1;

        _moveTask = null;

        _bestMove = -1;
        
        _playerLegalMoves.Clear();

        _playerLegalMovesColour = null;
        
        _turns.Clear();
    }

    public List<int> GetPlayerLegalMoves()
    {
        if (Player == _playerLegalMovesColour)
        {
            return _playerLegalMoves;
        }

        _playerLegalMoves.Clear();

        _boardAnalyser.Board = Board;
        
        var moves = _boardAnalyser.GetLegalMoves(Player);

        while (moves != 0)
        {
            var cell = BitOperations.TrailingZeroCount(moves);

            moves ^= 1ul << cell;
            
            _playerLegalMoves.Add(cell);
        }

        _playerLegalMovesColour = Player;

        return _playerLegalMoves;
    }

    public void Update(double elapsedMilliseconds)
    {
        if (_moveTask == null)
        {
            if (PlayerIsCpu(Player))
            {
                _moveTask = Task.Run(() => _bestMove = _core.GetBestMove(Player, _level).Cell);
            }
            else if (! _core.HasLegalMoves(Player) && ! GameOver)
            {
                Player = Player.Invert();

                _playerLegalMovesColour = _playerLegalMovesColour?.Invert();
            }
            
            return;
        }
        
        if (_moveTask != null && _moveTask.IsFaulted)
        {
            throw _moveTask.Exception;
        }

        if (_moveTask.IsCompleted)
        {
            if (_moveCalculatedTime < 0)
            {
                _moveCalculatedTime = PlayerIsCpu(Player.Invert()) ? elapsedMilliseconds + MoveDelay : elapsedMilliseconds;

                MakeMove(_bestMove);
            }

            if (elapsedMilliseconds - _moveCalculatedTime > MoveDelay)
            {
                _moveTask = null;

                _moveCalculatedTime = -1;
            }
        }
    }

    public void CellClicked(int cell)
    {
        if (! PlayerIsCpu(Player))
        {
            MakeMove(cell);
        }
    }

    public bool PlayerIsCpu(Colour colour)
    {
        return _playerIsCpu[(int) colour];
    }

    public bool CurrentPlayerIsCpu()
    {
        return PlayerIsCpu(Player);
    }

    public void UndoLastMove()
    {
        if (_moveTask != null)
        {
            return;
        }

        Colour? player = null;

        while (_turns.Count > 0 && Player != player)
        {
            player ??= Player;
            
            Board.UndoLastMove();
        
            _core.Board.UndoLastMove();

            Player = _turns.Pop();
        }

        _playerLegalMovesColour = Player.Invert();
        
        GetPlayerLegalMoves();
    }

    private void PlayerIsCpu(Colour colour, bool isCpu)
    {
        _playerIsCpu[(int) colour] = isCpu;
    }

    private void MakeMove(int cell)
    {
        _turns.Push(Player);
        
        if (cell == -1)
        {
            Player = Player.Invert();

            _moveTask = null;

            _moveCalculatedTime = -1;
            
            return;
        }

        if (_core.MakeMove(Player, cell))
        {
            Board.MakeMove(Player, cell);

            Player = Player.Invert();
        }
    }
}