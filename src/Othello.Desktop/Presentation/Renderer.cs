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

    private Texture2D _board;

    private Texture2D _black;

    private Texture2D _white;

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

        _board = Content.Load<Texture2D>("board");

        _black = Content.Load<Texture2D>("black");

        _white = Content.Load<Texture2D>("white");
        
        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        
        _spriteBatch.Draw(_board, new Vector2(0, 0), Color.White);
        
        _spriteBatch.Draw(_black, new Vector2(66, 63), Color.White);
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}