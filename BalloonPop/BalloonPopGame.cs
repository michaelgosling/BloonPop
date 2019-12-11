using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BalloonPop {
  public class BalloonPopGame : Game {
    /** Fields */
    GraphicsDeviceManager graphics;
    ScreenManager screenManager;
    SpriteBatch spriteBatch;
    SpriteFont scoreFont, titleFont, subtitleFont;
    static Color bgColor = new Color(47, 49, 54);

    /// <summary>
    /// Constructor
    /// </summary>
    public BalloonPopGame() {
      // Initialize Graphics Device Manager
      graphics = new GraphicsDeviceManager(this);

      // Set the mouse to visible and the the width/height of the window
      IsMouseVisible = true;
      graphics.PreferredBackBufferHeight = Globals.WINDOW_HEIGHT;
      graphics.PreferredBackBufferWidth = Globals.WINDOW_WIDTH;

      // Set content directory
      Content.RootDirectory = "Content";

      // Initialize Screen Manager
      screenManager = new ScreenManager(this);
      Components.Add(screenManager);
    }

    /// <summary>
    /// Initialize game
    /// </summary>
    protected override void Initialize() {
      base.Initialize();

      // Load initial screens
      screenManager.AddScreen(/* new BackgroundScreen() */ null);
      screenManager.AddScreen(/* new MainMenuScreen() */ null);
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