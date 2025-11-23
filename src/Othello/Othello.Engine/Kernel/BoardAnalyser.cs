using Othello.Engine.Infrastructure;

namespace Othello.Engine.Kernel;

public class BoardAnalyser
{
    private Board _board;

    public BoardAnalyser(Board board)
    {
        _board = board;
    }

    public ulong GetLegalMoves(Colour colour)
    {
        return 0;
    }
}