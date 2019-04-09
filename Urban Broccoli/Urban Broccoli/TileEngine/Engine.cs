using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Urban_Broccoli.TileEngine
{
    class Engine
    {
        private static Rectangle viewportRectangle;

        private static int tileWidth = 32;
        private static int tileHeight = 32;

        private TileMap map;

        private static float scrollSPeed = 500f;

        private static Camera camera;

        public static int TileWidth { get; set; }

        public static int TileHeight { get; set; }

        public TileMap Map { get; }

        public static Rectangle ViewportRectangle { get; set; }

        public static Camera Camera { get; }

        #region Constructor Region

        public Engine(Rectangle viewport)
        {
            ViewportRectangle = viewport;
            camera = new Camera();

            TileWidth = 64;
            TileHeight = 64;
        }

        public Engine(Rectangle viewport, int tileWidth, int tileHeight)
            : this(viewport)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        #endregion

        #region Method Region

        public static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / tileWidth, (int)position.Y / tileHeight);
        }

        public void SetMap(TileMap newMap)
        {
            if (newMap == null)
            {
                throw new ArgumentNullException("newMap");
            }

            map = newMap;
        }

        public void Update(GameTime gameTime)
        {
            Map.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Map.Draw(gameTime, spriteBatch, camera);
        }

        #endregion
    }
}
