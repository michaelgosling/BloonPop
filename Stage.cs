using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BloonPop {
    class Stage : IScreen {
        /** Fields */
        private List<Balloon> lvlBalloons = new List<Balloon>();
        private Texture2D[] balloonSprites;
        private Texture2D borderBrush;
        private Balloon currentBalloon, nextBalloon;
        private Rectangle balloonDimens;
        private Cannon player;
        private ScoreKeeper scoreKeeper;
        private Vector2 scoreTextPos = new Vector2(10, 10);
        private SpriteFont font;
        private Random rand = new Random();
        private Game1 game;
        bool swapBalloonKeyPressed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="level">Current Level to Load</param>
        /// <param name="balloonSprites">Sprites of the balloons</param>
        /// <param name="borderBrush">Brush to use for border</param>
        /// <param name="font">Font to use for score</param>
        /// <param name="scoreKeeper">ScoreKeeper</param>
        /// <param name="player">Player cannon</param>
        /// <param name="game">Reference to Game</param>
        public Stage(Level level, Texture2D[] balloonSprites, Texture2D borderBrush,
            SpriteFont font, ScoreKeeper scoreKeeper, Cannon player, Game1 game) {
            this.player = player;
            this.balloonSprites = balloonSprites;
            this.borderBrush = borderBrush;
            this.font = font;
            this.scoreKeeper = scoreKeeper;
            this.game = game;

            // get balloon dimensions based on the sprites used
            balloonDimens = balloonSprites[0].Bounds;

            // start tracking score
            scoreKeeper.StartTracking();
            LoadStage(level);
        }

        /// <summary>
        /// Loads the stage, depending on the current level
        /// </summary>
        private void LoadStage(Level level) {
            // clear the balloons from the previous level
            lvlBalloons.Clear();
            currentBalloon = null;
            nextBalloon = null;

            // set the initial x offset to the play area's left, 
            // and the y offset to 1 so we see the border
            int xOffset = Constants.PLAY_AREA.Left;
            int yOffset = 1;
            int row = 0;
            BalloonColor[] newBalloons = GetStageBalloons(level);
            foreach (var color in newBalloons) {
                // create new balloon using current x and y offsets, and the color
                Balloon balloon = CreateBalloon(color, xOffset, yOffset);
                if (balloon.BoundingBox.Right - 1 >= Constants.PLAY_AREA.Right) {
                    // new row
                    row++;
                    // set x offset appropriate for row
                    xOffset = Constants.PLAY_AREA.Left;
                    if (row % 2 != 0) xOffset += balloon.BoundingBox.Width / 2;
                    // shift y offset to bubble height + old offset
                    yOffset += balloon.BoundingBox.Height - 1;
                    // assign balloon to new offset
                    balloon.X = xOffset;
                    balloon.Y = yOffset;
                }
                // increment x offset for next balloon
                xOffset += balloon.BoundingBox.Width - 1;

                // add balloon to stage
                lvlBalloons.Add(balloon);
            }
            GetNextBalloon();
        }

        /// <summary>
        /// Find every balloon that intersects another and determine if they're matching
        /// </summary>
        private void UpdateMatches() {
            foreach (Balloon bln in lvlBalloons) {
                foreach (Balloon bln2 in lvlBalloons) {
                    if (bln.BlnColor == bln2.BlnColor) {
                        if (bln.BoundingBox.Intersects(bln2.BoundingBox))
                            bln.AddToMatched(bln2.ID);
                    }
                }
            }

            foreach (Balloon balloon in lvlBalloons) {
                if (balloon.MatchedIDs.Count >= 3) {
                    bool idAdded = false;
                    List<int> allMatchedIDs = new List<int>();
                    foreach (int i in balloon.MatchedIDs)
                        allMatchedIDs.Add(i);
                    allMatchedIDs.Add(balloon.ID);
                    do {
                        idAdded = false;
                        for (int i = allMatchedIDs.Count - 1; i >= 0; i--) {
                            var nextMatch = lvlBalloons.Find((Balloon bln) => bln.ID == allMatchedIDs[i]);
                            foreach (int j in nextMatch.MatchedIDs) {
                                if (!allMatchedIDs.Contains(j)) {
                                    allMatchedIDs.Add(j);
                                    idAdded = true;
                                }
                            }
                        }
                    } while (idAdded);
                    foreach (int id in allMatchedIDs) {
                        Balloon blnToCheck = lvlBalloons.Find((Balloon bln) => bln.ID == id);
                        if (blnToCheck.MatchedIDs.Count >= 2) blnToCheck.Matched = true;
                    }
                }
            }
        }

        /// <summary>
        /// Find matched balloons, increment score, and remove the balloons
        /// </summary>
        private void CleanUpMatches() {
            int color1Matched = 0;
            int color2Matched = 0;
            int color3Matched = 0;
            int color4Matched = 0;
            int color5Matched = 0;
            int color6Matched = 0;
            foreach (Balloon bln in lvlBalloons) {
                if (bln.Matched) {
                    switch (bln.BlnColor) {
                        case BalloonColor.Color1:
                            color1Matched++;
                            break;
                        case BalloonColor.Color2:
                            color2Matched++;
                            break;
                        case BalloonColor.Color3:
                            color3Matched++;
                            break;
                        case BalloonColor.Color4:
                            color4Matched++;
                            break;
                        case BalloonColor.Color5:
                            color5Matched++;
                            break;
                        case BalloonColor.Color6:
                            color6Matched++;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (color1Matched > 0) scoreKeeper.IncrementScore(color1Matched);
            if (color2Matched > 0) scoreKeeper.IncrementScore(color2Matched);
            if (color3Matched > 0) scoreKeeper.IncrementScore(color3Matched);
            if (color4Matched > 0) scoreKeeper.IncrementScore(color4Matched);
            if (color5Matched > 0) scoreKeeper.IncrementScore(color5Matched);
            if (color6Matched > 0) scoreKeeper.IncrementScore(color6Matched);

            // List<int> fallChain = new List<int>();
            for (int i = lvlBalloons.Count - 1; i >= 0; i--) {
                Balloon bln = lvlBalloons.ToArray() [i];
                if (bln.Matched) {
                    // before we remove the balloon, if there's a balloon attached below it
                    // add it to the fall chain
                    // while (GetBalloonBelow(bln) != null){
                    //     Balloon blnBelow = GetBalloonBelow(bln);
                    //     if (blnBelow.BlnColor != bln.BlnColor){
                    //         if (!fallChain.Contains(blnBelow.ID))fallChain.Add(blnBelow.ID);
                    //     }
                    //     bln = blnBelow;
                    // }
                    lvlBalloons.RemoveAt(i);
                }
            }

            // for (int i = fallChain.Count - 1; i >= 0; i--)
            //     lvlBalloons.Find((Balloon bln) => bln.ID == fallChain[i]).Fall();
        }

        // private Balloon GetBalloonBelow(Balloon bln){
        //     foreach (Balloon bln2 in lvlBalloons){
        //         if (bln2.BoundingBox.Intersects(bln.BoundingBox)){
        //             if ((bln2.BoundingBox.Right <= bln.BoundingBox.Right & bln2.BoundingBox.Right >= bln.BoundingBox.Left ||
        //                 bln2.BoundingBox.Left >= bln.BoundingBox.Left & bln2.BoundingBox.Left <= bln.BoundingBox.Right) &&
        //                 bln2.BoundingBox.Top <= bln.BoundingBox.Bottom | bln2.BoundingBox.Bottom >= bln.BoundingBox.Top) {
        //                     return bln2;
        //             }
        //         }
        //     }
        //     return null;
        // }

        /// <summary>
        /// Draws a border around a given rectangle
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        /// <param name="rectangleToDraw">Rectangle to border</param>
        /// <param name="thicknessOfBorder">Thickness of border</param>
        /// <param name="borderColor">Color of border</param>
        private void DrawBorder(SpriteBatch spriteBatch, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor) {
            // Draw top line
            spriteBatch.Draw(borderBrush, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(borderBrush, new Rectangle(rectangleToDraw.X - 1, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(borderBrush, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width + 1),
                rectangleToDraw.Y,
                thicknessOfBorder,
                rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(borderBrush, new Rectangle(rectangleToDraw.X,
                rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                rectangleToDraw.Width,
                thicknessOfBorder), borderColor);
        }

        /// <summary>
        /// Returns an array of balloon colors for the provided level
        /// </summary>
        /// <param name="lvl">Integer representing level</param>
        /// <returns>BalloonColor array</returns>
        BalloonColor[] GetStageBalloons(Level lvl) {
            BalloonColor[] balloons;
            switch (lvl) {
                case Level.First:
                    balloons = Constants.LEVEL1_BALLOONS;
                    break;
                default:
                    balloons = new BalloonColor[] { };
                    break;
            }
            return balloons;
        }

        /// <summary>
        /// Creates a balloon based on the color and cooordinates given
        /// </summary>
        /// <param name="color">BalloonColor</param>
        /// <param name="x">Integer of X Location</param>
        /// <param name="y">Integer of Y Location</param>
        /// <returns>Balloon</returns>
        Balloon CreateBalloon(BalloonColor color, int x, int y) {
            Texture2D sprite = balloonSprites[0];
            switch (color) {
                case BalloonColor.Color1:
                    sprite = balloonSprites[0];
                    break;
                case BalloonColor.Color2:
                    sprite = balloonSprites[1];
                    break;
                case BalloonColor.Color3:
                    sprite = balloonSprites[2];
                    break;
                case BalloonColor.Color4:
                    sprite = balloonSprites[3];
                    break;
                case BalloonColor.Color5:
                    sprite = balloonSprites[4];
                    break;
                case BalloonColor.Color6:
                    sprite = balloonSprites[5];
                    break;
                default:
                    break;
            }
            return new Balloon(color, sprite, x, y);
        }

        /// <summary>
        /// Provides a balloon in the "next balloon" area
        /// </summary>
        /// <returns>Balloon</returns>
        private void GetNextBalloon() {
            int idx, x, y;
            BalloonColor nextColor;
            idx = rand.Next(0, lvlBalloons.Count);
            nextColor = lvlBalloons.ToArray() [idx].BlnColor;
            if (currentBalloon == null) {
                x = player.BoundingBox.X - balloonDimens.Width / 2;
                y = player.BoundingBox.Y - player.BoundingBox.Height / 3;
                currentBalloon = CreateBalloon(nextColor, x, y);
                idx = rand.Next(0, lvlBalloons.Count);
                nextColor = lvlBalloons.ToArray() [idx].BlnColor;
            }
            x = Constants.PLAY_AREA.Right - 5 - balloonDimens.Width;
            y = Constants.PLAY_AREA.Bottom - 5 - balloonDimens.Height;

            nextBalloon = CreateBalloon(nextColor, x, y);
        }

        /// <summary>
        /// Check if the player won
        /// </summary>
        /// <returns>Boolean</returns>
        private bool CheckForWin() {
            return lvlBalloons.Count == 0;
        }

        /// <summary>
        /// Check if player lost
        /// </summary>
        /// <returns>boolean</returns>
        private bool CheckForLoss() {
            foreach (Balloon bln in lvlBalloons)
                if (bln.BoundingBox.Bottom >= player.BoundingBox.Top) return true;

            return false;
        }
        /// <summary>
        /// Draw the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(font, "Score: " + scoreKeeper.CurrentScore, scoreTextPos, Color.White);

            // Draw play area border
            DrawBorder(spriteBatch, Constants.PLAY_AREA, 1, Color.White);

            // draw each balloon in lvl balloons
            foreach (Balloon balloon in lvlBalloons)
                balloon.Draw(spriteBatch);

            // draw player cannon
            player.Draw(spriteBatch);

            // draw current and next balloons for player
            currentBalloon.Draw(spriteBatch);
            nextBalloon.Draw(spriteBatch);
        }

        /// <summary>
        /// Update the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {

            // get keyboard state
            KeyboardState kb = Keyboard.GetState();

            // check to see if Left, Right, Space or Z is pressed.
            // Left = angle cannon left
            // Right = angle cannon right
            // Space = fire cannon
            // Z = swap balloon
            if (kb.IsKeyDown(Keys.Left)) {
                if (player.Angle > -1)
                    player.Angle -= Constants.PLAYER_ROTATE_AMT;
            } else if (kb.IsKeyDown(Keys.Right)) {
                if (player.Angle < 1)
                    player.Angle += Constants.PLAYER_ROTATE_AMT;
            } else if (kb.IsKeyDown(Keys.Space) && !currentBalloon.Moving) {
                currentBalloon.Fire(player.Angle);
            } else if (kb.IsKeyDown(Keys.Z)) {
                swapBalloonKeyPressed = true;
            } else if (swapBalloonKeyPressed && kb.IsKeyUp(Keys.Z) && !currentBalloon.Moving) {
                swapBalloonKeyPressed = false;
                Balloon temp = currentBalloon;
                int tempNxtX = (int) nextBalloon.X;
                int tempNxtY = (int) nextBalloon.Y;
                int tempCurX = (int) currentBalloon.X;
                int tempCurY = (int) currentBalloon.Y;
                currentBalloon = nextBalloon;
                nextBalloon = temp;
                currentBalloon.X = tempCurX;
                currentBalloon.Y = tempCurY;
                nextBalloon.X = tempNxtX;
                nextBalloon.Y = tempNxtY;
            }

            // update the current balloon and if it's moving, do stuff
            currentBalloon.Update(gameTime);
            if (currentBalloon.Moving) {
                // make sure it doesn't leave the play area
                currentBalloon.DetectStageCollision();

                // check for balloon collisions
                foreach (Balloon bln in lvlBalloons)
                    currentBalloon.DetectBlnCollision(bln);

                // if the current balloon isn't moving anymore,
                // 1. Add the balloon to the level balloons list,
                // 2. set the current balloon to the next balloon,
                // 3. set the location of the current balloon to the correct one
                // 4. get the next balloon, update matches, and clean up matches
                if (!currentBalloon.Moving) {
                    lvlBalloons.Add(currentBalloon);
                    currentBalloon = nextBalloon;
                    currentBalloon.X = player.BoundingBox.X - balloonDimens.Width / 2;
                    currentBalloon.Y = player.BoundingBox.Y - player.BoundingBox.Height / 3;
                    UpdateMatches();
                    CleanUpMatches();
                    if (lvlBalloons.Count > 0)
                        GetNextBalloon();
                }
            }

            // update any moving balloons in the level balloons
            foreach (Balloon bln in lvlBalloons)
                if (bln.Moving) bln.Update(gameTime);

            if (CheckForWin()) {
                scoreKeeper.StopTracking();
                game.ChangeScreen(GameState.ScoreWin);
            } else if (CheckForLoss()) {
                scoreKeeper.StopTracking();
                game.ChangeScreen(GameState.ScoreFail);
            }
        }
    }
}