using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Othello.Engine.Kernel;
using Xunit.Abstractions;

namespace Othello.Engine.Tests.Kernel;

public class BoardAnalyserTests
{
    private readonly ITestOutputHelper _outputHelper;

    public BoardAnalyserTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Theory]
    [InlineData(Colour.Black, 0b0000000000000000000100000010000000000100000010000000000000000000ul)]
    [InlineData(Colour.White, 0b0000000000000000000010000000010000100000000100000000000000000000ul)]
    public void GetLegalMovesReturnsCorrectMovesForInitialBoardState(Colour colour, ulong expected)
    {
        var board = new Board();
        
        var boardAnalyser = new BoardAnalyser(board);

        board.InitialiseNewGame();
        
        _outputHelper.WriteLine($"\n{colour} legal moves:\n");

        var moves = boardAnalyser.GetLegalMoves(colour);

        var boardString = board.ToString();

        boardString = boardString.SuperimposeLegalMoves(moves);
        
        _outputHelper.WriteLine(boardString);
        
        Assert.Equal(expected, moves);
    }

    [Theory]
    [InlineData(Colour.White,
        0b0100000011000000000000000000000000000000000000000000000000000000ul,
        0b1000000000000000000000000000000000000000000000000000000000000000ul,
        0b0010000000000000101000000000000000000000000000000000000000000000ul)]
    [InlineData(Colour.White,
        0b0111111000000000000000000000000000000000000000000000000000000000ul,
        0b1000000000000000000000000000000000000000000000000000000000000000ul,
        0b0000000100000000000000000000000000000000000000000000000000000000ul)]
    [InlineData(Colour.White,
        0b0111111010000000000000000000000000000000000000000000000000000000ul,
        0b1000000000000000000000000000000000000000000000000000000000000000ul,
        0b0000000100000000100000000000000000000000000000000000000000000000ul)]
    [InlineData(Colour.White,
        0b0111111011000000000000000000000000000000000000000000000000000000ul,
        0b1000000000000000000000000000000000000000000000000000000000000000ul,
        0b0000000100000000101000000000000000000000000000000000000000000000ul)]
    [InlineData(Colour.White,
        0b0000000001000000000000000000000000000000000000000000000000000000ul,
        0b0000000010000000000000000000000000000000000000000000000000000000ul,
        0b0000000000100000000000000000000000000000000000000000000000000000ul)]
    public void GetLegalMovesReturnsCorrectMovesForArbitraryBoardState(Colour colour, ulong blackPieces, ulong whitePieces, ulong expected)
    {
        var board = new Board();
        
        var boardAnalyser = new BoardAnalyser(board);
    
        board.InitialiseFromBoardState(blackPieces, whitePieces);
                
        _outputHelper.WriteLine($"\n{colour} legal moves:\n");

        var moves = boardAnalyser.GetLegalMoves(colour);
        
        var boardString = board.ToString();

        boardString = boardString.SuperimposeLegalMoves(moves);
        
        _outputHelper.WriteLine(boardString);

        Assert.Equal(expected, moves);
    }
}