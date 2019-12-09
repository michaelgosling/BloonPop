using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace BloonPop {
    class ScoreScreen : IScreen {
        private bool didWin;
        private bool startNewGame = false;
        private Game1 game;
        private SpriteFont titleFont, scoreFont;
        private ScoreKeeper scoreKeeper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="didWin"></param>
        /// <param name="scoreKeeper"></param>
        /// <param name="titleFont"></param>
        /// <param name="scoreFont"></param>
        /// <param name="game"></param>
        public ScoreScreen(bool didWin, ScoreKeeper scoreKeeper, SpriteFont titleFont, SpriteFont scoreFont, Game1 game) {
            this.didWin = didWin;
            this.scoreKeeper = scoreKeeper;
            this.titleFont = titleFont;
            this.scoreFont = scoreFont;
            this.game = game;
        }

        /// <summary>
        /// Draw Screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            if (didWin) {
                var titleSize = titleFont.MeasureString("WIN!");
                spriteBatch.DrawString(titleFont, "WIN!", new Vector2(Constants.WINDOW_WIDTH / 2 - titleSize.Length() / 2, Constants.WINDOW_HEIGHT / 16), Color.White);
            } else {
                var titleSize = titleFont.MeasureString("LOSE!");
                spriteBatch.DrawString(titleFont, "LOSE!", new Vector2(Constants.WINDOW_WIDTH / 2 - titleSize.Length() / 2, Constants.WINDOW_HEIGHT / 16), Color.White);
            }
            var highScoresSize = titleFont.MeasureString("High Scores: ");
            spriteBatch.DrawString(titleFont, "High Scores:", new Vector2(Constants.WINDOW_WIDTH / 2 - highScoresSize.Length() / 2, Constants.WINDOW_HEIGHT / 16 * 3), Color.White);
            int yOffset = Constants.WINDOW_HEIGHT / 8 * 3;
            float xLoc = 0;
            foreach (int score in scoreKeeper.HighScores) {
                xLoc = Constants.WINDOW_WIDTH / 2 - (scoreFont.MeasureString(score.ToString()).Length() / 2);
                Vector2 location = new Vector2(xLoc, yOffset);
                spriteBatch.DrawString(scoreFont, score.ToString(), location, Color.White);
                yOffset += 30;
            }
            var textSize = scoreFont.MeasureString("Press R to play again!").Length();
            spriteBatch.DrawString(scoreFont, "Press R to play again!", new Vector2(Constants.WINDOW_WIDTH / 2 - textSize / 2, Constants.WINDOW_HEIGHT / 8 * 7), Color.White);
        }

        /// <summary>
        /// Update Screen
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.R)) {
                startNewGame = true;
            } else if (kb.IsKeyUp(Keys.R) && startNewGame) {
                game.ChangeScreen(GameState.Playing);
            }

        }
    }
}