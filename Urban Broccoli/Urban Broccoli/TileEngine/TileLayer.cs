using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Urban_Broccoli.Components;

namespace Urban_Broccoli.TileEngine
{
    class TileLayer
    {
        #region Field Region

        [ContentSerializer(CollectionItemName = "Tiles")]
        private int[] tiles;

        private int width;
        private int height;

        private Point cameraPoint;
        private Point viewPoint;
        private Point min;
        private Point max;

        private Rectangle destination;

        #endregion

        #region Property Region

        [ContentSerializerIgnore] public bool Enabled { get; set; }

        [ContentSerializerIgnore] public bool Visible { get; set; }

        [ContentSerializer]
        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        [ContentSerializer]
        public int Height
        {
            get { return height; }
            private set { height = value; }
        }

        #endregion

        #region Constructor Region

        private TileLayer()
        {
            Enabled = true;
            Visible = true;
        }

        public TileLayer(int[] tiles, int width, int height)
            : this()
        {
            this.tiles = (int[]) tiles.Clone();
            this.width = width;
            this.height = height;
        }

        public TileLayer(int width, int height)
            : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = 0;
                }
            }
        }

        public TileLayer(int width, int height, int fill)
            : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = fill;
                }
            }
        }

        #endregion

        #region Method Region

        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return -1;
            }

            if (x >= width || y >= height)
            {
                return -1;
            }

            return tiles[y * width + x];
        }

        public void SetTile(int x, int y, int tileIndex)
        {
            if (x < 0 || y < 0)
            {
                return;
            }

            if (x >= width || y >= height)
            {
                return;
            }

            tiles[y * width + x] = tileIndex;
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, TileSet tileSet, Camera camera)
        {
            if (!Visible)
            {
                return;
            }

            cameraPoint = Engine.VectorToCell(camera.Position);
            viewPoint = Engine.VectorToCell(new Vector2((camera.Position.X + Engine.ViewportRectangle.Width),
                (cameraPosition.Y + Engine.ViewportRectangle.Height)));
            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, Width);
            max.Y = Math.Min(viewPoint.Y + 1, Height);

            destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            int tile;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
            null,
                null,
                null,
                camera.Transformation);

            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * Engine.TileHeight;

                for (int x = min.X; x < max.X; x++)
                {
                    tile = GetTile(x, y);

                    if (tile == -1)
                    {
                        continue;
                    }

                    destination.X = x * Engine.TileWidth;

                    spriteBatch.Draw(
                        tileSet.Texture,
                        destination,
                        tileSet.SourceRectangles[tile],
                        Color.White);
                }
            }

            spriteBatch.End();
        }

        #endregion
    }
}