using System.Diagnostics;
using Othello.Engine;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;

using static System.Console;

namespace Othello.Console;

public static class EntryPoint
{
    public static void Main()
    {
        var core = new Core();
        
        core.StartGame();

        var ply = 1;

        var player = Colour.White;

        var passCount = 0;

        var stopwatch = new Stopwatch();
        
        Clear();
        
        while (passCount < 2 && ! core.Board.IsFull)
        {
            CursorTop = 1;
            
            WriteLine($"Ply {ply}, player {player}:\n");
            
            WriteLine(core.Board.ToString());
            
            stopwatch.Restart();

            var move = core.GetBestMove(player, 9);

            stopwatch.Stop();
            
            WriteLine($"\nThinking time: {stopwatch.ElapsedMilliseconds:N0}ms");

            passCount = core.MakeMove(player, move.Cell) ? 0 : passCount + 1;

            player = player.Invert();

            ply++;
        }
        
        Clear();

        CursorTop = 1;
        
        WriteLine($"Black: {core.Board.BlackScore}, White: {core.Board.WhiteScore}.\n");

        WriteLine(core.Board.ToString());

        if (core.Board.BlackScore == core.Board.WhiteScore)
        {
            WriteLine("\nIt's a draw.\n");
        }
        else
        {
            WriteLine($"\n{(core.Board.BlackScore > core.Board.WhiteScore ? "Black" : "White")} wins!\n");
        }
    }
}