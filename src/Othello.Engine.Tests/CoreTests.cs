using System.Diagnostics;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;
using Xunit.Abstractions;

namespace Othello.Engine.Tests;

public class CoreTests
{
    private readonly ITestOutputHelper _outputHelper;

    public CoreTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void EngineCanPlayItself()
    {
        var core = new Core();
        
        core.StartGame();

        var ply = 1;

        var player = Colour.White;

        var passCount = 0;

        var stopwatch = new Stopwatch();
        
        while (passCount < 2)
        {
            _outputHelper.WriteLine($"\nPly {ply}:\n");
            
            _outputHelper.WriteLine(core.Board.ToString());
            
            stopwatch.Start();

            var move = core.GetBestMove(player);

            stopwatch.Stop();
            
            _outputHelper.WriteLine($"\nThinking time: {stopwatch.ElapsedMilliseconds:N0}ms");

            passCount = core.MakeMove(player, move.Cell) ? 0 : passCount + 1;

            player = player.Invert();

            ply++;
        }
    }
}