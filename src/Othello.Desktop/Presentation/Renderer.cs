using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Othello.Engine;
using Othello.Engine.Extensions;
using Othello.Engine.Infrastructure;

namespace Othello.Desktop.Presentation;

public class Renderer : Game
{
    private const int Width = 902;

    private const int Height = 894;

    private const int ArenaTop = 63;

    private const int ArenaLeft = 68;

    private const int CellStride = 98;
    
    private readonly Core _core;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private Texture2D _background;

    private Texture2D _black;

    private Texture2D _white;

    private Task<(int Score, int Cell)> _moveTask;

    private Colour _player = Colour.Black;

    public Renderer()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Width,
            PreferredBackBufferHeight = Height
        };

        Content.RootDirectory = "_Content";

        IsMouseVisible = true;

        _core = new Core();
        
        _core.StartGame();
    }

    protected override void Initialize()
    {
        Window.Title = "Othello";
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("board");

        _black = Content.Load<Texture2D>("black");

        _white = Content.Load<Texture2D>("white");
        
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (_moveTask == null)
        {
            _moveTask = Task.Run(() => _core.GetBestMove(_player));
        }

        if (_moveTask.IsCompleted)
        {
            _player = _player.Invert();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        
        _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
            {
                var cell = y * 8 + x;

                if (! _core.Board[cell])
                {
                    continue;
                }

                _spriteBatch.Draw(_core.Board.Black[cell] ? _black : _white, new Vector2(ArenaLeft + x * CellStride, ArenaTop + y * CellStride), Color.White);
            }
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}