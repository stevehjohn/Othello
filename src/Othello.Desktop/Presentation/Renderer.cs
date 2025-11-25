using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Othello.Desktop.Presentation;

public class Renderer : Game
{
    private const int Width = 902;

    private const int Height = 894;
    
    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    public Renderer()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Width,
            PreferredBackBufferHeight = Height
        };

        Content.RootDirectory = "_Content";
    }
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        base.Draw(gameTime);
    }
}