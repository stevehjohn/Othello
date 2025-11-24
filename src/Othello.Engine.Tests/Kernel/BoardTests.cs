using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;

namespace Othello.Engine.Tests.Kernel;

public class BoardTests
{
    [Fact]
    public void UndoLastMoveRestoresPreviousState()
    {
        var board = new Board();
        
        board.InitialiseNewGame();
        
        Assert.Equal(0b0000000000000000000000000001000000001000000000000000000000000000ul, board.White.Pieces);
        
        Assert.Equal(0b0000000000000000000000000000100000010000000000000000000000000000ul, board.Black.Pieces);

        board.MakeMove(Colour.Black, 37);
        
        Assert.Equal(0b0000000000000000000000000000000000001000000000000000000000000000ul, board.White.Pieces);
        
        // TODO: Modify after flip implementation
        Assert.Equal(0b0000000000000000000000000011100000010000000000000000000000000000ul, board.Black.Pieces);
        
        board.UndoLastMove();
        
        Assert.Equal(0b0000000000000000000000000001000000001000000000000000000000000000ul, board.White.Pieces);
        
        Assert.Equal(0b0000000000000000000000000000100000010000000000000000000000000000ul, board.Black.Pieces);
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