using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Engine;

public class Core
{
    private readonly Board _board;

    private readonly BoardAnalyser _analyser;

    public Core()
    {
        _board = new Board();
        
        _analyser = new BoardAnalyser(_board);
    }
    
    public void StartGame()
    {
        _board.InitialiseNewGame();
    }

    public bool MakeMove(Colour colour, int cell)
    {
        return _board.MakeMove(colour, cell);
    }

    public ulong GetBestMove(Colour colour)
    {
        return _analyser.GetLegalMoves(colour);
    }
}