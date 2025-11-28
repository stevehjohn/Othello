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
        CursorVisible = false;

        var gameCount = 1;
        
        while (! KeyAvailable)
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

                WriteLine($" Game {gameCount}: Ply: {ply}\n");
                
                WriteLine($" Player: {player}\n");

                WriteLine($" {core.Board.ToString().Replace("\n", "\n ")}");

                stopwatch.Restart();

                var move = core.GetBestMove(player, 10);

                stopwatch.Stop();

                WriteLine($"\n Thinking time: {stopwatch.ElapsedMilliseconds:N0}ms               ");

                passCount = core.MakeMove(player, move.Cell) ? 0 : passCount + 1;

                player = player.Invert();

                ply++;
            }

            Clear();

            CursorTop = 1;

            WriteLine($" Game {gameCount}: Ply: {ply}\n");
                
            WriteLine($" Player: {player}\n");

            WriteLine($" {core.Board.ToString().Replace("\n", "\n ")}");

            WriteLine(core.Board.BlackScore == core.Board.WhiteScore 
                ? "\n It's a draw.\n" 
                : $" \n {(core.Board.BlackScore > core.Board.WhiteScore ? "Black" : "White")} wins!\n");

            gameCount++;

            Thread.Sleep(2_000);
        }

        CursorVisible = true;
    }
}