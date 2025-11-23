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
        
        Assert.Equal(0b0000000000000000000000000001000000001000000000000000000000000000ul, board.Black.Pieces);
        
        Assert.Equal(0b0000000000000000000000000000100000010000000000000000000000000000ul, board.White.Pieces);

        board.MakeMove(Colour.White, 37);
        
        Assert.Equal(0b0000000000000000000000000000000000001000000000000000000000000000ul, board.Black.Pieces);
        
        // TODO: Modify after flip implementation
        Assert.Equal(0b0000000000000000000000000011100000010000000000000000000000000000ul, board.White.Pieces);
        
        board.UndoLastMove();
        
        Assert.Equal(0b0000000000000000000000000001000000001000000000000000000000000000ul, board.Black.Pieces);
        
        Assert.Equal(0b0000000000000000000000000000100000010000000000000000000000000000ul, board.White.Pieces);
    }
}