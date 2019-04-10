using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Urban_Broccoli.TileEngine
{
    class Camera
    {
        #region Field Region

        private Vector2 position;
        private float speed;

        #endregion

        #region PropertyRegion

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = (float)MathHelper.Clamp(speed, 1f, 16f); }
        }

        public Matrix Transformation
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)); }
        }

        #endregion

        #region Constructor Region

        public Camera()
        {
            speed = 4f;
        }

        public Camera(Vector2 position)
        {
            speed = 4f;
            Position = position;
        }

        #endregion

        public void LockCamera(TileMap map, Rectangle viewport)
        {
            position.X = MathHelper.Clamp(position.X, 0, map.WidthInPixels - viewport.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, map.HeightInPixels - viewport.Height);
        }

        public void LockToSprite(TileMap map, AnimatedSprite sprite, Rectangle viewPort)
        {
            position.X = (sprite.Position.X + sprite.Width / 2) - (viewPort.Width / 2);
            position.Y = (sprite.Position.Y + sprite.Height / 2) - (viewPort.Height / 2);
            LockCamera(map, viewPort);
        }
    }
}
