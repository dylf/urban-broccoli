using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Urban_Broccoli.AvatarComponents;
using Urban_Broccoli.TileEngine;

namespace Urban_Broccoli.CharacterComponents
{
    class Character : ICharacter
    {
        #region Constants

        public const float SpeakingRadius = 40f;

        #endregion

        #region Field Region

        private string name;
        private Avatar battleAvatar;
        private Avatar givingAvatar;
        private AnimatedSprite sprite;

        private string conversation;

        private static Game1 gameRef;
        private static Dictionary<AnimationKey, Animation> characterAnimations = new Dictionary<AnimationKey, Animation>();

        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }

        public Avatar BattleAvatar
        {
            get { return battleAvatar; }
        }

        public Avatar GiveAvatar
        {
            get { return givingAvatar; }
        }

        public string Conversation
        {
            get { return conversation; }
        }

        #endregion

        #region Constructor Region

        private Character()
        {
        }

        #endregion

        #region Method Region

        private static void BuildAnimations()
        {

        }

        public static Character FromString(Game game, string characterString)
        {
            if (gameRef == null)
            {
                gameRef = (Game1) game;
            }

            if (characterAnimations.Count == 0)
            {
                BuildAnimations();;
            } 

            Character character = new Character();
            string[] parts = characterString.Split(',');

            character.name = parts[0];
            Texture2D texture = game.Content.Load<Texture2D>(@"CharacterSprites\" + parts[1]);

            character.sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);

            AnimationKey key = AnimationKey.WalkDown;
            Enum.TryParse<AnimationKey>(parts[2], true, out key);

            character.sprite.CurrentAnimation = key;

            character.conversation = parts[3];

            return character;
        }

        public void SetConversation(string newConversation)
        {
            this.conversation = newConversation;
        }

        public static void Save(string characterName)
        {

        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }

        #endregion


    }
}
