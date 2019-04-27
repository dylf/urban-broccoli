using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Urban_Broccoli.TileEngine
{
    public enum AnimationKey
    {
        IdleLeft,
        IdleRight,
        IdleDown,
        IdleUp,
        WalkLeft,
        WalkRight,
        WalkDown,
        WalkUp,
        ThrowLeft,
        ThrowRight,
        DuckLeft,
        DuckRight,
        JumpLeft,
        JumpRight,
        Dieing
    }
    public class AnimatedSprite
    {

        #region Field Region

        private Dictionary<AnimationKey, Animation> animations;
        private AnimationKey currentAnimation;
        private bool isAnimating;
        private float scale = 2.0f;

        private Texture2D texture;
        public Vector2 Position;
        private Vector2 velocity;
        private float speed = 200.0f;

        #endregion

        #region Property Region

        public bool IsActive { get; set; }

        public AnimationKey CurrentAnimation
        {
            get => currentAnimation;
            set => currentAnimation = value;
        }

        public bool IsAnimating
        {
            get => isAnimating;
            set => isAnimating = value;
        }

        public int Width
        {
            get => animations[currentAnimation].FrameWidth;
        }

        public int Height
        {
            get => animations[currentAnimation].FrameHeight;
        }

        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        public float Speed
        {
            get => speed;
            set => speed = MathHelper.Clamp(value, 1.0f, 400.0f);
        }

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        #endregion

        #region Constructor Region

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            texture = sprite;
            animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
            {
                animations.Add(key, (Animation)animation[key].Clone());
            }
        }

        #endregion

        #region Method Region


        public void ResetAnimation()
        {
            animations[currentAnimation].Reset();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (isAnimating)
            {
                animations[currentAnimation].Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                Position,
                animations[currentAnimation].CurrentFrameRect,
                Color.White,
                0.0f,
                Vector2.One, 
                scale,
                SpriteEffects.None,
                1f
                    );
        }

        private Rectangle GetDestinationRectangle()
        {
            int width = (int)((float)animations[currentAnimation].CurrentFrameRect.Width * scale);
            int height = (int)((float)animations[currentAnimation].CurrentFrameRect.Height * scale);
            return new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }

        public void LockToMap(Point mapSize)
        {
            Position.X = MathHelper.Clamp(Position.X, 0, mapSize.X - Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, mapSize.Y - Height);
        }

        #endregion
    }
}
