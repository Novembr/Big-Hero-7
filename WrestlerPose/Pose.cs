using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WrestlerPose.Models;
using WrestlerPose.Sprites;

namespace WrestlerPose
{
    
    public enum PoseName
    {
        Idle,
        LowHands,
        Pointing,
        OneHandUp,
        HighHands,
        Hercules
    }

    public class Pose
    {
        PoseName _poseName;
        Sprite _sprite;

        //public Pose(Texture2D singleFrameAnimation, PoseName poseName)
        //{
        //    _poseName = poseName;
        //    _sprite = new Sprite(singleFrameAnimation);
        //}

        public Pose(Animation animation, PoseName poseName, float scale, float layer)
        {
            _poseName = poseName;
            _sprite = new Sprite(animation, scale, layer);
        }

        public PoseName GetPoseName() { return _poseName; }
        public Sprite GetSprite() { return _sprite; }
    }
}
