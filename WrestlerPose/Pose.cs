using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WrestlerPose
{
    
    public enum PoseName
    {
        Idle,
        LowHands,
        HighHands,
        OneHandUp,
        Pointing,
        Kneeling
    }

    //pose needs to be set with its texture

    public class Pose
    {
        Texture2D _wrestlerTexture;
        PoseName _poseName;

        public Pose(Texture2D wrestlerTexture, PoseName poseName)
        {
            _wrestlerTexture = wrestlerTexture;
            _poseName = poseName;
        }

        public PoseName GetPoseName() { return _poseName; }
        public Texture2D GetTexture() { return _wrestlerTexture; }

    }
}
