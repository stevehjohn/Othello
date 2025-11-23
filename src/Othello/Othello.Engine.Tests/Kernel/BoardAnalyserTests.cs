using Othello.Engine.Infrastructure;
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
    
    [Theory]
    [InlineData(Colour.White, 0b0000000000000000000100000010000000000100000010000000000000000000ul)]
    [InlineData(Colour.Black, 0b0000000000000000000010000000010000100000000100000000000000000000ul)]
    public void GetLegalMovesReturnsCorrectMovesForInitialBoardState(Colour colour, ulong expected)
    {
        _board.InitialiseNewGame();

        var moves = _boardAnalyser.GetLegalMoves(colour);
        
        Assert.Equal(expected, moves);
    }
}