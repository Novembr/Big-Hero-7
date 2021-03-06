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
        #region define
        private MouseState currentMouse;
        private MouseState previousMouse;
        
        private bool isHovering;  

        private Texture2D _texture;
        private Texture2D _textureActive;

        private string buttonName = "";

        public event EventHandler Click;
        public bool Clicked { get; private set; }
        #endregion
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
            // load textures
            _texture = texture;
            _textureActive = textureActive;
            buttonName = _texture.Name;
        }


        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            // not hoving then texture1, hover then texture2
            if (!isHovering)
                _spriteBatch.Draw(_texture, Rectangle, Color.White);
            else
                _spriteBatch.Draw(_textureActive, Rectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            //track mouse movement
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            //Hover then invoke.
            isHovering = false;
            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

            GamePadCapabilities capabilities = GamePad.GetCapabilities(0);
            GamePadState state = GamePad.GetState(0);
            if (capabilities.HasAButton && capabilities.HasDPadDownButton && capabilities.HasDPadUpButton && capabilities.HasStartButton)
            {
                if(state.DPad.Down == ButtonState.Pressed)
                {
                    if(buttonName == "START")
                    {
                        isHovering = false;
                    }
                    else
                    {
                        isHovering = true;
                        if (state.Buttons.A == ButtonState.Pressed)
                        {
                            Click?.Invoke(this, new EventArgs());
                        }
                    }
                }

                if (state.DPad.Up == ButtonState.Pressed)
                {
                    if (buttonName == "EXIT")
                    {
                        isHovering = false;
                    }
                    else
                    {
                        isHovering = true;
                        if (state.Buttons.A == ButtonState.Pressed)
                        {
                            Click?.Invoke(this, new EventArgs());
                        }
                    }
                }
            }

        }

    }
}
