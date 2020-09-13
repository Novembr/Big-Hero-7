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
        protected float _scale;
        protected float _layer;

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

        public float Scale { get; set; }

        public float GetAnimationTime()
        {
            float animationTime = _animation.FrameCount * _animation.FrameSpeed;
            return animationTime;
        }

        public virtual void Draw(SpriteBatch spriteBatch, bool isColoredLight, Player currentAI)
        {
            Color color = Color.White;
            PoseName aIPose = currentAI.GetPose().GetPoseName();

            if (isColoredLight)
            {
                switch (aIPose)
                {
                    case PoseName.Idle:
                        color = Color.White;//don't actually need this one but just in case will put here
                        break;
                    case PoseName.LowHands:
                        color = Color.FromNonPremultiplied(254, 242, 141, 256);
                        break;
                    case PoseName.Pointing:
                        color = Color.FromNonPremultiplied(131, 129, 190, 256);
                        break;
                    case PoseName.OneHandUp:
                        color = Color.FromNonPremultiplied(155, 205, 236, 256);
                        break;
                    case PoseName.HighHands:
                        color = Color.FromNonPremultiplied(205, 153, 155, 256);
                        break;
                    case PoseName.Hercules:
                        color = Color.FromNonPremultiplied(164, 202, 156, 256);
                        break;
                    default:
                        break;
                }
            }

            if (_texture != null)
            {
                spriteBatch.Draw(
                    _texture, 
                    Position, 
                    null,
                    color,
                    0f,
                    new Vector2(_texture.Width / 2, _texture.Height / 2),
                    0.3f,
                    SpriteEffects.None,
                    .93f

                    );

            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(spriteBatch, _scale, _layer, color);//it's position is the sprite position
            }
            else throw new Exception("This ain't right..!");
        }

        public Sprite(Animation animation, float scale, float layer)
        {
            _animation = animation;
            _animationManager = new AnimationManager(_animation);
            _scale = scale;
            _layer = layer;
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
