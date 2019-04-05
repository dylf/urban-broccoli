using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Urban_Broccoli.StateManager;

namespace Urban_Broccoli.GameStates
{
    public interface ITitleIntroState : IGameState
    {

    }
    class TitleIntroState : BaseGameState, ITitleIntroState
    {
        #region Field Region

        private Texture2D background;
        private Rectangle backgroundDestination;
        private SpriteFont font;
        private TimeSpan elapsed;
        private Vector2 position;
        private string message;

        #endregion

        #region Constructor Region

        public TitleIntroState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ITitleIntroState), this);
        }

        #endregion

        #region Method Region

        public override void Initialize()
        {
            backgroundDestination = Game1.ScreenRectangle;
            elapsed = TimeSpan.Zero;
            message = "PRESS SPACE TO CONTINUE";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            background = content.Load<Texture2D>(@"GameScreens\titlescreen");
            font = content.Load<SpriteFont>(@"Fonts\InterfaceFont");

            Vector2 size = font.MeasureString(message);
            position = new Vector2((Game1.ScreenRectangle.Width - size.X) / 2,Game1.ScreenRectangle.Bottom - 50 - font.LineSpacing);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerIndex index = PlayerIndex.One;
            elapsed += gameTime.ElapsedGameTime;
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.Draw(background, backgroundDestination, Color.White);
            Color color = new Color(1f, 1f, 1f) * (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 2));

            GameRef.SpriteBatch.DrawString(font, message, position, color);

            GameRef.SpriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion
    }
}
