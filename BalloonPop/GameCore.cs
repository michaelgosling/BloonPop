using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BalloonPop {
  public class GameCore : Game {
    /** Fields */
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteFont scoreFont, titleFont, subtitleFont;
    Color bgColor;

    /// <summary>
    /// Constructor
    /// </summary>
    public GameCore() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
      graphics.PreferredBackBufferHeight = Globals.WINDOW_HEIGHT;
      graphics.PreferredBackBufferWidth = Globals.WINDOW_WIDTH;
      bgColor = new Color(47, 49, 54);
    }

    /// <summary>
    /// Initialize game
    /// </summary>
    protected override void Initialize() {

      base.Initialize();
    }

    /// <summary>
    /// Load game content
    /// </summary>
    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // load the fonts
      scoreFont = this.Content.Load<SpriteFont>("fonts/Score");
      titleFont = this.Content.Load<SpriteFont>("fonts/Title");
      subtitleFont = this.Content.Load<SpriteFont>("fonts/Subtitle");

    }

    /// <summary>
    /// Update game, runs as a loop before draw
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Update(GameTime gameTime) {
      base.Update(gameTime);
    }

    /// <summary>
    /// Draw game, runs as a loop after Update
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(bgColor);

      spriteBatch.Begin();

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}