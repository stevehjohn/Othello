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
        
        while (passCount < 2 && ! core.Board.IsFull)
        {
            WriteLine($"\nPly {ply}, player {player}:\n");
            
            WriteLine(core.Board.ToString());
            
            stopwatch.Restart();

            var move = core.GetBestMove(player, 9);

            stopwatch.Stop();
            
            WriteLine($"\nThinking time: {stopwatch.ElapsedMilliseconds:N0}ms");

            passCount = core.MakeMove(player, move.Cell) ? 0 : passCount + 1;

            player = player.Invert();

            ply++;
        }
        
        WriteLine(string.Empty);
                    
        WriteLine(core.Board.ToString());

        WriteLine($"\nBlack: {core.Board.BlackScore}, White: {core.Board.WhiteScore}.\n");

        if (core.Board.BlackScore == core.Board.WhiteScore)
        {
            WriteLine("It's a draw.");
        }
        else
        {
            WriteLine($"{(core.Board.BlackScore > core.Board.WhiteScore ? "Black" : "White")} wins!");
        }
    }
}