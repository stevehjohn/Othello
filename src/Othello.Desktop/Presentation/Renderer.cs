using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Othello.Desktop.Orchestration;
using Othello.Engine.Infrastructure;

namespace Othello.Desktop.Presentation;

public class Renderer : Game
{
    private const int GameOverDelay = 1_000;
    
    private const int Width = 902;

    private const int Height = 894;

    private const int ArenaTop = 63;

    private const int ArenaLeft = 68;

    private const int CellStride = 98;

    private readonly Coordinator _coordinator = new();
    
    private readonly int _players;

    private readonly int _level;
    
    private List<int> _playerLegalMoves;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private Texture2D _background;

    private Texture2D _black;

    private Texture2D _white;

    private MouseState _previousMouseState;

    private double _gameOverTime = -1;

    public Renderer(int players, int level)
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Width,
            PreferredBackBufferHeight = Height
        };

        Content.RootDirectory = "_Content";

        IsMouseVisible = true;

        _players = players;

        _level = level;
    }

    protected override void Initialize()
    {
        Window.Title = "Othello";

        StartGame();
        
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

        var cell = -1;

        var clicked = false;
        
        if (mouseState.X > ArenaLeft && mouseState.Y > ArenaTop)
        {
            var x = (mouseState.X - ArenaLeft) / CellStride;

            var y = (mouseState.Y - ArenaTop) / CellStride;

            if (x < Constants.Columns && y < Constants.Rows)
            {
                cell = y * 8 + x;
            }

            if (mouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed && cell > -1)
            {
                clicked = true;
                
                _coordinator.CellClicked(cell);
            }
        }

        _previousMouseState = mouseState;
        
        _coordinator.Update(gameTime.TotalGameTime.TotalMilliseconds);

        _playerLegalMoves = _coordinator.GetPlayerLegalMoves();

        if (_coordinator.CurrentPlayerIsCpu())
        {
            Mouse.SetCursor(MouseCursor.Arrow);            
        }
        else
        {
            Mouse.SetCursor(_playerLegalMoves.Contains(cell) ? MouseCursor.Hand : MouseCursor.Arrow);
        }

        if (_coordinator.GameOver)
        {
            Window.Title = $"Othello. Game over. {_coordinator.Winner} wins by {Math.Abs(_coordinator.Board.BlackScore - _coordinator.Board.WhiteScore)}!";

            if (_gameOverTime < 0)
            {
                _gameOverTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (gameTime.TotalGameTime.TotalMilliseconds - _gameOverTime > GameOverDelay)
            {
                if (clicked || (_coordinator.PlayerIsCpu(Colour.Black) && _coordinator.PlayerIsCpu(Colour.White)))
                {
                    StartGame();

                    _gameOverTime = -1;
                }
            }
        }
        else
        {
            Window.Title = $"Othello. {_coordinator.Player} {(_coordinator.Thinking ? "(thinking...) " : string.Empty)}to move.";
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

                if (! _coordinator.Board[cell])
                {
                    if (_playerLegalMoves.Contains(cell))
                    {
                        _spriteBatch.Draw(_coordinator.Player == Colour.Black ? _black : _white, new Vector2(ArenaLeft + x * CellStride, ArenaTop + y * CellStride), Color.FromNonPremultiplied(255, 255, 255, 92));
                    }

                    continue;
                }

                _spriteBatch.Draw(_coordinator.Board.Black[cell] ? _black : _white, new Vector2(ArenaLeft + x * CellStride, ArenaTop + y * CellStride), Color.White);
            }
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void StartGame()
    {
        var player1IsCpu = _players == 0;

        var player2IsCpu = _players < 2;
        
        _coordinator.StartGame(player1IsCpu, player2IsCpu, _level);
    }
}