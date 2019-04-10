using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Urban_Broccoli.TileEngine
{
    public class Animation
    {
        #region Field Region

        private Rectangle[] frames;
        private int framesPerSecond;
        private TimeSpan frameLength;
        private TimeSpan frameTimer;
        private int currentFrame;
        private int frameWidth;
        private int frameHeight;

        #endregion

        #region Property Region

        public int FramesPerSecond
        {
            get => framesPerSecond;
            set
            {
                if (value < 1)
                {
                    framesPerSecond = 1;
                }
                else if (value > 60)
                {
                    framesPerSecond = 60;
                }
                else
                {
                    framesPerSecond = value;
                }

                frameLength = TimeSpan.FromSeconds(1 / (double) framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect
        {
            get => frames[currentFrame]; 
        }

        public int CurrentFrame
        {
            get => currentFrame;
            set => currentFrame = (int) MathHelper.Clamp(value, 0, frames.Length - 1);
        }

        public int FrameWidth
        {
            get => frameWidth;
        }

        public int FrameHeight
        {
            get => frameHeight;
        }

        #endregion

        #region Constructor Region

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            frames = new Rectangle[frameCount];
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(xOffset + (frameWidth * i), yOffset, frameWidth, frameHeight);
            }

            FramesPerSecond = 5;
            Reset();
        }

        private Animation(Animation animation)
        {
            this.frames = animation.frames;
            FramesPerSecond = 5;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;

            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            frameTimer = TimeSpan.Zero;
        }

        #endregion

        #region Interface Method Region

        public object Clone()
        {
            Animation animationClone = new Animation(this);

            animationClone.frameWidth = this.frameWidth;
            animationClone.frameHeight = this.frameHeight;
            animationClone.Reset();

            return animationClone;
        }

        #endregion
    }
}