using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;
using Xunit.Abstractions;

namespace Othello.Engine.Tests.Kernel;

public class BoardTests
{
    private readonly ITestOutputHelper _outputHelper;

    public BoardTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void UndoLastMoveRestoresPreviousState()
    {
        var board = new Board();
        
        board.InitialiseNewGame();
        
        _outputHelper.WriteLine("\nBlack intended move:\n");

        var boardString = board.ToString();

        boardString = boardString.SuperimposeLegalMoves(1ul << 37);
        
        _outputHelper.WriteLine(boardString);
        
        Assert.Equal(0b0000000000000000000000000001000000001000000000000000000000000000ul, board.White.Pieces);
        
        Assert.Equal(0b0000000000000000000000000000100000010000000000000000000000000000ul, board.Black.Pieces);

        _outputHelper.WriteLine("\nBoard state after black move:\n");

        board.MakeMove(Colour.Black, 37);
        
        _outputHelper.WriteLine(board.ToString());
        
        Assert.Equal(0b0000000000000000000000000000000000001000000000000000000000000000ul, board.White.Pieces);
        
        Assert.Equal(0b0000000000000000000000000011100000010000000000000000000000000000ul, board.Black.Pieces);
        
        _outputHelper.WriteLine("\nBoard state after undo move:\n");

        board.UndoLastMove();
                
        _outputHelper.WriteLine(board.ToString());

        Assert.Equal(0b0000000000000000000000000001000000001000000000000000000000000000ul, board.White.Pieces);
        
        Assert.Equal(0b0000000000000000000000000000100000010000000000000000000000000000ul, board.Black.Pieces);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(27, false)]
    [InlineData(29, true)]
    [InlineData(30, false)]
    public void BoardDetectsMoveValidityFromInitialState(int cell, bool isValid)
    {
        var board = new Board();
        
        board.InitialiseNewGame();

        var result = board.MakeMove(Colour.White, cell);
        
        Assert.Equal(isValid, result);
    }

    [Fact]
    public void BlackScoreReturnsCorrectValue()
    {
        var board = new Board();
        
        board.InitialiseNewGame();
        
        Assert.Equal(2, board.BlackScore);
    }

    [Fact]
    public void WhiteScoreReturnsCorrectValue()
    {
        var board = new Board();
        
        board.InitialiseNewGame();
        
        Assert.Equal(2, board.WhiteScore);
    }
}