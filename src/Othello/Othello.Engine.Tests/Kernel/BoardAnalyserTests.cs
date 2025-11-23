using Othello.Engine.Kernel;

namespace Othello.Engine.Tests.Kernel;

public class BoardAnalyserTests
{
    private readonly Board _board;

    private readonly BoardAnalyser _boardAnalyser;

    public BoardAnalyserTests()
    {
        _board = new Board();
        
        _boardAnalyser = new BoardAnalyser(_board);
    }
    
    [Fact]
    public void GetLegalMovesReturnsCorrectMoves()
    {
        _board.InitialiseNewGame();
    }
}