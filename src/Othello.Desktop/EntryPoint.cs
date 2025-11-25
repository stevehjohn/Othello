using Othello.Desktop.Presentation;

namespace Othello.Desktop;

public static class EntryPoint
{
    public static void Main()
    {
        var renderer = new Renderer();
        
        renderer.Run();
    }
}