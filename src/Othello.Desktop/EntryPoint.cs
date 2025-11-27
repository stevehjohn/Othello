using Othello.Desktop.Presentation;

namespace Othello.Desktop;

public static class EntryPoint
{
    public static void Main(params string[] arguments)
    {
        var players = 1;

        var level = 10;
        
        for (var i = 0; i < arguments.Length; i++)
        {
            if (int.TryParse(arguments[i], out var value))
            {
                switch (i)
                {
                    case 1:
                        level = value;
                        break;
                    
                    default:
                        players = value;
                        break;
                }
            }
        }

        var renderer = new Renderer(players, level);

        renderer.Run();
    }
}