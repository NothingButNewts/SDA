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

        /// <summary>
        /// Loads whatever assetname was put as a paramater and assigns it's startPosition(subject to change)   
        /// </summary>
        /// <param name="content"></param>
        /// <param name="asset"></param>
        /// <param name="startPos"></param>
        public void LoadContent(ContentManager content, string asset,Vector2 startPos)
        {
            position = startPos;
            texture = content.Load<Texture2D>(asset);
            textureAsset = asset;
            size = new Rectangle((int)startPos.X
                ,(int)startPos.Y,64,64);
        }

        /// <summary>
        /// Draws the sprite onto the screen
        /// </summary>
        /// <param name="spritebatch"></param>
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, size, new Rectangle(0,0,texture.Width,texture.Height), Color.White);
        }
    }
}
