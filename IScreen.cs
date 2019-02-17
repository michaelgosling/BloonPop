using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BloonPop {
    interface IScreen {

        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}