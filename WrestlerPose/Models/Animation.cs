using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace WrestlerPose.Models
{
    public class Animation
    {
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public int FrameHeight { get { return Texture.Height; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public bool IsLooping { get; set; }
        public Texture2D Texture { get; private set; }

        public Animation(Texture2D texture, int frameCount, bool isLooping)
        {
            Texture = texture;
            FrameCount = frameCount;
            IsLooping = isLooping;
            FrameSpeed = 0.06f;
        }

        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;
            FrameCount = frameCount;
            IsLooping = true;
            FrameSpeed = 0.06f;
        }
    }
}
