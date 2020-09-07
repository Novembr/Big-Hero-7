using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrestlerPose
{
    class Image : Component
    {
        private Texture2D _texture;
        private Color _color;

        public Rectangle Rectangle
        {
            get; set;
        }
        public Image(Texture2D texture, Color color)
        {
            _texture = texture;
            _color = color;
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, _color);
        }
    }
}
