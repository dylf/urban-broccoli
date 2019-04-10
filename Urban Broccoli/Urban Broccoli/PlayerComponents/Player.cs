using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Urban_Broccoli.TileEngine;

namespace Urban_Broccoli.PlayerComponents
{
    class Player : DrawableGameComponent
    {
        #region Field Region

        private Game1 gameRef;
        private string name;
        private bool gender;
        private string mapName;
        private Point tile;
        private AnimatedSprite sprite;
        private Texture2D texture;
        private float speed = 180f;

        private Vector2 position;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get => sprite.Position;
            set => sprite.Position = value;
        }

        public AnimatedSprite Sprite
        {
            get => sprite;
        }

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        #endregion

        #region Constructor Region

        private Player(Game game)
            : base(game)
        {
        }

        public Player(Game game, string name, bool gender, Texture2D texture)
            : base(game)
        {
            gameRef = (Game1) game;
            this.name = name;
            this.gender = gender;

            this.texture = texture;
            this.sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);
            this.sprite.CurrentAnimation = AnimationKey.WalkDown;
        }

        #endregion

        #region Method Region

        public void SavePlayer()
        {

        }

        public static Player Load(Game game)
        {
            Player player = new Player(game);

            return player;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            sprite.Draw(gameTime, gameRef.SpriteBatch);
        }

        #endregion
    }
}
