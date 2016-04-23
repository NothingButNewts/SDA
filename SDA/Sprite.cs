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
        
        public Sprite(Vector2 startPos, string asset)
        {
            position = startPos;
            textureAsset = asset;

        }

        /// <summary>
        /// Loads whatever assetname was put as a paramater and assigns it's startPosition(subject to change)   
        /// </summary>
        /// <param name="content"></param>
        /// <param name="asset"></param>
        /// <param name="startPos"></param>
        public void LoadContent(ContentManager content)
        {
            
            texture = content.Load<Texture2D>(textureAsset);
            size = new Rectangle((int)position.X
                ,(int)position.Y,64,64);
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
