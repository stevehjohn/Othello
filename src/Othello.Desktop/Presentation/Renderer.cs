using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Othello.Desktop.Orchestration;
using Othello.Engine.Infrastructure;

namespace Othello.Desktop.Presentation;

public class Renderer : Game
{
    private readonly Coordinator _coordinator = new();
    
    private const int Width = 902;

    private const int Height = 894;

    private const int ArenaTop = 63;

    private const int ArenaLeft = 68;

    private const int CellStride = 98;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private Texture2D _background;

    private Texture2D _black;

    private Texture2D _white;

    private MouseState _previousMouseState;
    
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
        var mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
        {
            var x = (mouseState.X - ArenaLeft) / CellStride;

            var y = (mouseState.Y - ArenaTop) / CellStride;

            if (x < Constants.Columns && y < Constants.Rows)
            {
                var cell = y * 8 + x;
                
                _coordinator.CellClicked(cell);
            }
        }
        
        Mouse.SetCursor(MouseCursor.Hand);

        _previousMouseState = mouseState;
        
        // var delay = _passCount > 1 ? 1_000 : 200;
        //
        // if (gameTime.TotalGameTime.TotalMilliseconds - _lastActionMilliseconds < delay)
        // {
        //     return;
        // }
        //
        // _lastActionMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
        //
        // if (_passCount > 1)
        // {
        //     _core.StartGame();
        //
        //     _board = new Board(_core.Board);
        //     
        //     _passCount = 0;
        //     
        //     return;
        // }
        //
        // if (_moveTask != null && _moveTask.IsCompleted)
        // {
        //     _passCount = _core.MakeMove(_player, _bestMove) ? 0 : _passCount + 1;
        //
        //     _board.MakeMove(_player, _bestMove);
        //     
        //     _player = _player.Invert();
        //
        //     _moveTask = null;
        // }
        //
        // _moveTask ??= Task.Run(() => _bestMove = _core.GetBestMove(_player, 11).Cell);

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

                if (! _coordinator.Board[cell])
                {
                    continue;
                }

                _spriteBatch.Draw(_coordinator.Board.Black[cell] ? _black : _white, new Vector2(ArenaLeft + x * CellStride, ArenaTop + y * CellStride), Color.White);
            }
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}