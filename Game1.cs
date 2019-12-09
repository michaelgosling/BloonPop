using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BloonPop {
    public class Game1 : Game {
        /** Fields */
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont scoreFont, titleFont, subtitleFont;
        Texture2D borderBrush;
        Texture2D[] balloonSprites = new Texture2D[6];
        Cannon playerCannon;
        Level currentLevel;
        IScreen currentScreen;
        PlayerPalette playerPalette;
        ScoreKeeper scoreKeeper = new ScoreKeeper();

        /** Properties */
        GameState State { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
        }

        /// <summary>
        /// Initialize game
        /// </summary>
        protected override void Initialize() {
            // Set player pallete and current state
            playerPalette = PlayerPalette.Default;
            State = GameState.Title;
            base.Initialize();
        }

        /// <summary>
        /// Load game content
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            if (playerPalette != PlayerPalette.Default) {
                // Custom Palette code here
            } else {
                // default palette
                balloonSprites[0] = this.Content.Load<Texture2D>("sprites/balloons/red_balloon");
                balloonSprites[1] = this.Content.Load<Texture2D>("sprites/balloons/green_balloon");
                balloonSprites[2] = this.Content.Load<Texture2D>("sprites/balloons/blue_balloon");
                balloonSprites[3] = this.Content.Load<Texture2D>("sprites/balloons/orange_balloon");
                balloonSprites[4] = this.Content.Load<Texture2D>("sprites/balloons/purple_balloon");
                balloonSprites[5] = this.Content.Load<Texture2D>("sprites/balloons/yellow_balloon");
            }
            // players cannon sprite. local cause it'll only be loaded once
            Texture2D playerCannonSprite = this.Content.Load<Texture2D>("sprites/player_cannon");

            // load the fonts
            scoreFont = this.Content.Load<SpriteFont>("fonts/Score");
            titleFont = this.Content.Load<SpriteFont>("fonts/Title");
            subtitleFont = this.Content.Load<SpriteFont>("fonts/Subtitle");

            // create a new 2D Texture, 1x1 pixels, White in color and color as the format.
            // we use this for drawing the border later
            borderBrush = new Texture2D(base.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            borderBrush.SetData(new [] { Color.White });

            // create player cannon
            playerCannon = new Cannon(playerCannonSprite);

            // set the current screen to a title screen
            currentScreen = new TitleScreen(this, titleFont, subtitleFont);
        }

        /// <summary>
        /// Update game, runs as a loop before draw
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime) {

            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw game, runs as a loop after Update
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            currentScreen.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Change the games screen based on a given GameState
        /// </summary>
        /// <param name="state">GameState</param>
        public void ChangeScreen(GameState state) {
            this.State = state;
            if (this.State == GameState.Title) {
                currentScreen = new TitleScreen(this, titleFont, subtitleFont);
            } else if (this.State == GameState.Playing) {
                currentScreen = new Stage(Level.First, balloonSprites, borderBrush, scoreFont, scoreKeeper, playerCannon, this);
            } else if (this.State == GameState.ScoreWin) {
                currentScreen = new ScoreScreen(true, scoreKeeper, titleFont, scoreFont, this);
            } else if (this.State == GameState.ScoreFail) {
                currentScreen = new ScoreScreen(false, scoreKeeper, titleFont, scoreFont, this);
            }
        }
    }

}