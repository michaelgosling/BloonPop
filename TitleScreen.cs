using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BloonPop {
    class TitleScreen : IScreen {
        /** Fields */
        private SpriteFont titleFont;
        private SpriteFont subtitleFont;
        private Game1 game;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="titleFont"></param>
        /// <param name="subtitleFont"></param>
        public TitleScreen(Game1 game, SpriteFont titleFont, SpriteFont subtitleFont) {
            this.game = game;
            this.titleFont = titleFont;
            this.subtitleFont = subtitleFont;
        }

        /// <summary>
        /// Draw the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            var titleSize = titleFont.MeasureString("Bloon Pop!").Length();
            var subtitleSize = subtitleFont.MeasureString("Press Enter to Play!").Length();
            spriteBatch.DrawString(titleFont, "Bloon Pop!", new Vector2(Constants.WINDOW_WIDTH / 2 - titleSize / 2, Constants.WINDOW_HEIGHT / 4), Color.White);
            spriteBatch.DrawString(subtitleFont, "Press Enter to Play!", new Vector2(Constants.WINDOW_WIDTH / 2 - subtitleSize / 2, Constants.WINDOW_HEIGHT / 2), Color.White);
        }

        /// <summary>
        /// Update the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Enter)) {
                game.ChangeScreen(GameState.Playing);
            }
        }
    }
}