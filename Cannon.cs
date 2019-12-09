using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloonPop
{
  class Cannon
  {
    /** Fields */
    private const int width = 20;
    private const int height = 100;
    private readonly Color color = Color.White;
    private Texture2D sprite;
    private Vector2 position;
    private Rectangle boundingBox;

    /** Properties */
    public Rectangle BoundingBox { get { return boundingBox; } }

    public double Angle { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sprite">Cannon sprite</param>
    public Cannon(Texture2D sprite)
    {
      Angle = 0;
      this.sprite = sprite;
      position = new Vector2(Constants.WINDOW_WIDTH / 2, Constants.WINDOW_HEIGHT / 8 * 7);
      boundingBox = new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    /// <summary>
    /// Draw cannon
    /// </summary>
    /// <param name="spriteBatch"></param>
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(sprite, position, null, color, (float)Angle, new Vector2(sprite.Bounds.Center.X, sprite.Bounds.Center.Y), new Vector2(1, 1), SpriteEffects.None, 0);
    }
  }
}