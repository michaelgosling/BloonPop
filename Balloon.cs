using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloonPop {

    class Balloon {
        /** Fields */
        private static int idCounter;
        private Vector2 position;
        private Vector2 velocity;
        private int speed = Constants.BALLOON_SPEED;
        private BalloonColor color;
        private Texture2D sprite;
        private Rectangle boundingBox;

        /** Properties */
        public int ID { get; private set; }
        public List<int> MatchedIDs { get; }
        public bool Matched { get; set; }
        public BalloonColor BlnColor { get { return color; } }
        public Rectangle BoundingBox { get { return boundingBox; } }
        public bool Moving { get; private set; }
        public float X {
            get { return position.X; }
            set {
                position.X = value;
                boundingBox.X = (int) value;
            }
        }
        public float Y {
            get { return position.Y; }
            set {
                position.Y = value;
                boundingBox.Y = (int) value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">Color of the balloon</param>
        /// <param name="sprite">Sprite for the balloon</param>
        /// <param name="x">Integer representing x location</param>
        /// <param name="y">Integer representing y location</param>
        public Balloon(BalloonColor color, Texture2D sprite, int x, int y) {
            // get new ID from factory
            this.ID = getNewId();

            // set passed in variables to fields/properties
            this.color = color;
            this.position = new Vector2(x, y);
            this.Matched = false;
            this.sprite = sprite;

            // boundingbox matches sprites bounds
            this.boundingBox = new Rectangle((int) position.X, (int) position.Y, sprite.Width, sprite.Height);

            // velocity should start at 0, moving should be false, and matched IDs should be a new blank list
            this.velocity = Vector2.Zero;
            this.Moving = false;
            this.MatchedIDs = new List<int>();
        }

        /** Methods */

        /// <summary>
        /// Adds a balloon id to the list of matches in the chain for this balloon
        /// </summary>
        /// <param name="id">Integer representing matches IDs</param>
        public void AddToMatched(int id) {
            if (!MatchedIDs.Contains(id))
                MatchedIDs.Add(id);
        }

        /// <summary>
        /// Provides a new ID from a static counter
        /// </summary>
        /// <returns>Integer representing ID</returns>
        private int getNewId() {
            idCounter++;
            return idCounter;
        }

        /// <summary>
        /// Draws the balloon
        /// </summary>
        /// <param name="spriteBatch">spriteBatch used to draw the sprite</param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, new Vector2(this.X, this.Y), Color.White);
        }

        /// <summary>
        /// Updates the balloon
        /// </summary>
        public void Update(GameTime gameTime) {
            if (Moving) {
                position += velocity * speed;
                boundingBox.X = (int) position.X;
                boundingBox.Y = (int) position.Y;
            }
        }

        /// <summary>
        /// Detect collision with stage and respond accordingly
        /// </summary>
        public void DetectStageCollision() {
            // if the balloon hits the left or right of the play area, flip X velocity
            if (this.BoundingBox.Right >= Constants.PLAY_AREA.Right ||
                this.BoundingBox.Left <= Constants.PLAY_AREA.Left) {
                this.velocity.X *= -1;
            }

            // if the balloon hits the top of the play area, snap it to the top, stop it's movement
            if (this.BoundingBox.Top <= 1) {
                this.Y = 1;
                this.velocity = Vector2.Zero;
                this.Moving = false;
            }
        }

        /// <summary>
        /// Detect collision with a balloon
        /// </summary>
        /// <param name="bln">Balloon it might collide with</param>
        public void DetectBlnCollision(Balloon bln) {
            if (this.boundingBox.Intersects(bln.BoundingBox)) {
                this.Moving = false;
                this.velocity = Vector2.Zero;

                // kinda squish this balloon into the one it intersected with
                Point blnCenter = bln.BoundingBox.Center;
                Point thisCenter = this.BoundingBox.Center;
                if (blnCenter.X >= thisCenter.X) {
                    this.X += 1;
                } else if (blnCenter.X <= thisCenter.X) {
                    this.X -= 1;
                }
                if (blnCenter.Y >= thisCenter.Y) {
                    this.Y += 1;
                } else if (blnCenter.Y <= thisCenter.Y) {
                    this.Y -= 1;
                }
            }
        }

        /// <summary>
        /// Fire the balloon, presumably from a cannon but not required
        /// </summary>
        /// <param name="angle">Angle to fire at</param>
        public void Fire(double angle) {
            this.Moving = true;
            angle -= 1.55;
            this.velocity = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }

        /// <summary>
        /// Tell Balloon to fall.
        /// </summary>
        public void Fall() {
            this.Moving = true;
            this.velocity = new Vector2((float) Math.Cos(1.5), (float) Math.Sin(1.5));
        }

    }
}