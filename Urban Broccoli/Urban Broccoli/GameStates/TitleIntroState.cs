using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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

        private readonly List<Texture2D> _backgrounds = new List<Texture2D>();
        private Rectangle backgroundDestination;
        private SpriteFont titleFont;
        private SpriteFont messageFont;
        private TimeSpan elapsed;
        private Vector2 messagePosition;
        private Vector2 titlePosition;
        private string message;
        private string title;

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
            title = "URBAN BROCCOLI";
            message = "PRESS SPACE TO CONTINUE";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadBackground();
            messageFont = content.Load<SpriteFont>(@"Fonts\InterfaceFont");
            titleFont = content.Load<SpriteFont>(@"Fonts\TitleFont");

            Vector2 size = messageFont.MeasureString(message);
            messagePosition = new Vector2((Game1.ScreenRectangle.Width - size.X) / 2,Game1.ScreenRectangle.Bottom - 80 - messageFont.LineSpacing);

            size = titleFont.MeasureString(title);
            titlePosition = new Vector2((Game1.ScreenRectangle.Width - size.X) / 2, Game1.ScreenRectangle.Top + 80 + titleFont.LineSpacing);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerIndex index = PlayerIndex.One;
            elapsed += gameTime.ElapsedGameTime;
            base.Update(gameTime);
            UpdateTitlePosition();
        }


        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();
            int i = 1;
            Color bgColor;
            foreach (Texture2D background in _backgrounds)
            {
            
                bgColor = (i < 4) ? Color.White : Color.Green;
                GameRef.SpriteBatch.Draw(background, backgroundDestination, bgColor);
                i++;

            }
            Color color = new Color(1f, 1f, 1f) * (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 2));

            GameRef.SpriteBatch.DrawString(messageFont, message, messagePosition, color);
            GameRef.SpriteBatch.DrawString(titleFont, title, new Vector2(titlePosition.X - 5 , titlePosition.Y + 5), Color.Black);
            GameRef.SpriteBatch.DrawString(titleFont, title, new Vector2(titlePosition.X - 4 , titlePosition.Y + 4), Color.DarkGray);

            GameRef.SpriteBatch.DrawString(titleFont, title, titlePosition, Color.White);

            GameRef.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void UpdateTitlePosition()
        {
            Vector2 size = titleFont.MeasureString(title);
            titlePosition = new Vector2((Game1.ScreenRectangle.Width - size.X) / 2, Game1.ScreenRectangle.Top + 80 + titleFont.LineSpacing + (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 2)) * 3);
        }

        private void LoadBackground()
        {

            _backgrounds.Add(content.Load<Texture2D>(@"GameScreens\Title\Background"));
            _backgrounds.Add(content.Load<Texture2D>(@"GameScreens\Title\Cloud-Layer-2"));
            _backgrounds.Add(content.Load<Texture2D>(@"GameScreens\Title\Cloud-Layer-1"));
            _backgrounds.Add(content.Load<Texture2D>(@"GameScreens\Title\Mushroom-Layer-4"));
            _backgrounds.Add(content.Load<Texture2D>(@"GameScreens\Title\Mushroom-Layer-3"));
            _backgrounds.Add(content.Load<Texture2D>(@"GameScreens\Title\Mushroom-Layer-2"));
            _backgrounds.Add(content.Load<Texture2D>(@"GameScreens\Title\Mushroom-Layer-1"));
        }

        #endregion
    }
}
