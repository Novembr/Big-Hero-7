﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WrestlerPose
{
    public class Button : Component
    {

        private MouseState currentMouse;
        private MouseState previousMouse;
        
        private bool isHovering;  

        private Texture2D _texture;
        private Texture2D _textureActive;

        public event EventHandler Click;
        public bool Clicked { get; private set; }

        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Button(Texture2D texture, Texture2D textureActive)
        {
            _texture = texture;
            _textureActive = textureActive;
        }

        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            if (!isHovering)
                _spriteBatch.Draw(_texture, Rectangle, Color.White);
            else
                _spriteBatch.Draw(_textureActive, Rectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            isHovering = false;
            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

        }

    }
}
