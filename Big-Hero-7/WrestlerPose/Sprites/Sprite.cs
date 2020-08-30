using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrestlerPose.Managers;
using WrestlerPose.Models;

namespace WrestlerPose.Sprites
{
    public class Sprite
    {
        protected AnimationManager _animationManager;
        Animation _animation;
        protected Vector2 _position;
        protected Texture2D _texture;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
        
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float GetAnimationTime()
        {
            float animationTime = _animation.FrameCount * _animation.FrameSpeed;
            return animationTime;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(
                    _texture, 
                    Position, 
                    null,
                    Color.White,

                    0f,
                    new Vector2(_texture.Width / 2, _texture.Height / 2),
                    0.3f,
                    SpriteEffects.None,
                    0f

                    );

            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(spriteBatch);//it's position is the sprite position
            }
            else throw new Exception("This ain't right..!");
        }

        public Sprite(Animation animation)
        {
            _animation = animation;
            _animationManager = new AnimationManager(_animation);
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, Vector2 pos)
        {

            Position = pos;

            _animationManager.Play(_animation);

            _animationManager.Update(gameTime);

        }

    }
}
