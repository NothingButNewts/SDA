using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SDA
{
    class Sprite
    {
        public Vector2 position;
        protected Texture2D texture;
        public Rectangle size;
        public string textureAsset;


        public void LoadContent(ContentManager content, string asset,Vector2 startPos)
        {
            position = startPos;
            texture = content.Load<Texture2D>(asset);
            textureAsset = asset;
            size = new Rectangle(1,1, texture.Width+100,texture.Height);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, size, Color.White, 0.0f, Vector2.Zero, .25f, SpriteEffects.None, 0);
        }
    }
}
