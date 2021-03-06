﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrestlerPose
{
    class Background : Component
    {
        private Texture2D _texture;
        private Color _color;

        public Rectangle Rectangle
        {
            get; set;
        }
        public Background(Texture2D texture, Color color)
        {
            _texture = texture;
            _color = color;
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, Rectangle, _color);
        }
    }
}
