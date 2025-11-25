using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Othello.Desktop.Presentation;

public class Renderer : Game
{
    private const int Width = 902;

    private const int Height = 894;

    private const int ArenaTop = 63;

    private const int ArenaLeft = 66;

    private const int CellStride = 98;
    
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

        IsMouseVisible = true;
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

        for (var i = 0; i < 8; i++)
        {
            _spriteBatch.Draw(_white, new Vector2(ArenaLeft, ArenaTop + i * CellStride), Color.White);
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}