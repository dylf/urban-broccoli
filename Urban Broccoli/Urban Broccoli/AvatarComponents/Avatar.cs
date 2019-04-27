using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Urban_Broccoli.AvatarComponents
{
    public enum AvatarElement
    {
        Dark, Earth, Fire, Light, Water, Wind
    }

    public enum Stat
    {
        Attack, Defense, Speed, Health
    }

    public class Avatar
    {

        #region Field Region

        private static Random random = new Random();
        private Texture2D texture;
        private string name;
        private AvatarElement element;
        private int level;
        private long experience;
        private int costToBuy;
        private int speed;
        private int attack;
        private int defense;
        private int health;
        private int currentHealth;
        private List<IMove> effects;
        private Dictionary<string, IMove> knownMoves;

        #endregion

        #region Property Region

        public string Name
        {
            get => name;
        }

        public int Level
        {
            get => level;
            set => level = (int) MathHelper.Clamp(value, 1, 100);
        }

        public long Experience
        {
            get => experience;
        }

        public Texture2D Texture
        {
            get => texture;
        }

        public Dictionary<string, IMove> KnownMoves
        {
            get => knownMoves;
        }

        public AvatarElement Element
        {
            get => element;
        }

        public List<IMove> Effects
        {
            get => effects;
        }

        public static Random Random
        {
            get => random;
        }

        public int BaseAttack
        {
            get => attack;
        }

        public int BaseDefense
        {
            get => defense;
        }

        public int BaseSpeed
        {
            get => speed;
        }

        public int BaseHealth
        {
            get => health;
        }

        public int CurrentHealth
        {
            get => currentHealth;
        }

        public bool Alive
        {
            get => (currentHealth > 0);
        }

        #endregion

        #region Constructor Region

        private Avatar()
        {
            level = 1;
            knownMoves = new Dictionary<string, IMove>();
            effects = new List<IMove>();
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Apply the outcomes of a move.
        /// </summary>
        /// <param name="move">The attackers move object.</param>
        /// <param name="target">The target of the move.</param>
        public void ResolveMove(IMove move, Avatar target)
        {
            bool found = false;
            switch (move.Target)
            {
                case Target.Self:
                    if (move.MoveType == MoveType.Buff)
                    {
                        found = false;
                        for (int i = 0; i < effects.Count; i++)
                        {
                            if (effects[i].Name == move.Name)
                            {
                                effects[i].Duration += move.Duration;
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            effects.Add((IMove)move.Clone());
                        }
                    }
                    else if (move.MoveType == MoveType.Heal)
                    {
                        currentHealth += move.Health;
                        if (currentHealth > health)
                        {
                            currentHealth = health;
                        }
                    }
                    else if (move.MoveType == MoveType.Status)
                    {
                    }

                    break;

                case Target.Enemy:
                    if (move.MoveType == MoveType.Debuff)
                    {
                        found = false;
                        for (int i = 0; i < target.Effects.Count; i++)
                        {
                            if (target.Effects[i].Name == move.Name)
                            {
                                target.Effects[i].Duration += move.Duration;
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            target.Effects.Add((IMove)move.Clone());
                        }
                    }
                    else if (move.MoveType == MoveType.Attack)
                    {
                        float modifier = GetMoveModifier(move.MoveElement, target.Element);
                        float tDamage = GetAttack() + move.Health * modifier - target.GetDefense();

                        if (tDamage < 1f)
                        {
                            tDamage = 1f;
                        }

                        target.ApplyDamage((int) tDamage);
                    }

                    break;
            }
        }

        /// <summary>
        /// Get the damage modifier given both the move and target elements.
        /// </summary>
        /// <param name="moveElement">Element of the move being performed</param>
        /// <param name="targetElement">Element of the target</param>
        /// <returns></returns>
        public static float GetMoveModifier(MoveElement moveElement, AvatarElement targetElement)
        {
            float modifier = 1f;

            switch (moveElement)
            {
                case MoveElement.Dark:
                    if (targetElement == AvatarElement.Light)
                    {
                        modifier += .25f;
                    }
                    else if (targetElement == AvatarElement.Wind)
                    {
                        modifier -= .25f;
                    }

                    break;

                case MoveElement.Earth:
                    if (targetElement == AvatarElement.Water)
                    {
                        modifier += .25f;
                    }
                    else if (targetElement == AvatarElement.Wind)
                    {
                        modifier -= .25f;
                    }

                    break;

                case MoveElement.Fire:
                    if (targetElement == AvatarElement.Wind)
                    {
                        modifier += .25f;
                    }
                    else if (targetElement == AvatarElement.Water)
                    {
                        modifier -= .25f;
                    }

                    break;

                case MoveElement.Light:
                    if (targetElement == AvatarElement.Dark)
                    {
                        modifier += .25f;
                    }
                    else if (targetElement == AvatarElement.Earth)
                    {
                        modifier -= .25f;
                    }

                    break;

                case MoveElement.Wind:
                    if (targetElement == AvatarElement.Light)
                    {
                        modifier += .25f;
                    }
                    else if (targetElement == AvatarElement.Earth)
                    {
                        modifier -= .25f;
                    }

                    break;
            }

            return modifier;
        }

        /// <summary>
        /// Reduce the current Avatars health.
        /// </summary>
        /// <param name="tDamage">The total damage.</param>
        public void ApplyDamage(int tDamage)
        {
            currentHealth -= tDamage;
        }


        /// <summary>
        /// Update the Avatar.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Duration--;

                if (effects[i].Duration < 1)
                {
                    effects.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Get the current attack.
        /// </summary>
        /// <returns>The total attack stat.</returns>
        public int GetAttack()
        {
            int attackMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    attackMod += move.Attack;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    attackMod -= move.Attack;
                }
            }

            return attack + attackMod;
        }

        /// <summary>
        /// Get the current Defense.
        /// </summary>
        /// <returns>The total defense stat.</returns>
        public int GetDefense()
        {
            int defenseMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    defenseMod += move.Defense;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    defenseMod -= move.Defense;
                }
            }

            return defense + defenseMod;
        }

        /// <summary>
        /// Get the current Speed.
        /// </summary>
        /// <returns>The current speed.</returns>
        public int GetSpeed()
        {
            int speedMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    speedMod += move.Speed;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    speedMod -= move.Speed;
                }
            }

            return speed + speedMod;
        }

        /// <summary>
        /// Get the current health.
        /// </summary>
        /// <returns>The current health.</returns>
        public int GetHealth()
        {
            int healthMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    healthMod += move.Health;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    healthMod -= move.Health;
                }
            }

            return health + healthMod;
        }

        /// <summary>
        /// Set up function called when the battle starts.
        /// </summary>
        public void StartCombat()
        {
            effects.Clear();
            currentHealth = health;
        }

        /// <summary>
        /// Handle the battle winning logic.
        /// </summary>
        public long WinBattle(Avatar target)
        {
            int levelDiff = target.Level - level;
            long expGained = 0;

            if (levelDiff <= -10)
            {
                expGained = 10;
            }
            else if (levelDiff <= -5)
            {
                expGained = (long) (100f * (float) Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 0)
            {
                expGained = (long) (50f * (float) Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 5)
            {
                expGained = (long) (5f * (float) Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 10)
            {
                expGained = (long) (10f * (float) Math.Pow(2, levelDiff));
            }
            else
            {
                expGained = (long) (50f * (float) Math.Pow(2, target.Level));
            }

            return expGained;
        }

        /// <summary>
        /// Handle battle losing logic.
        /// </summary>
        public long LoseBattle(Avatar target)
        {
            // They are still going to gain exp for losing, probably delete later
            return (long) ((float) WinBattle(target) * .5f);
        }

        /// <summary>
        /// Check if the avatar has leveled up.
        /// </summary>
        public bool CheckLevelUp()
        {
            bool leveled = false;
            if (experience >= 50 * (1 + (long)Math.Pow(level, 2.5)))
            {
                leveled = true;
                level++;
            }

            return leveled;
        }

        /// <summary>
        /// Function for a assigning skill points
        /// </summary>
        public void AsssignPoint(Stat stat, int points)
        {
            switch (stat)
            {
                case Stat.Attack:
                    attack += points;
                    break;

                case Stat.Defense:
                    defense += points;
                    break;

                case Stat.Speed:
                    speed += points;
                    break;

                case Stat.Health:
                    health += points;
                    break;
            }
        }

        public object Clone()
        {
            Avatar avatar = new Avatar();

            avatar.name = this.name;
            avatar.texture = this.texture;
            avatar.element = this.element;
            avatar.costToBuy = this.costToBuy;
            avatar.level = this.level;
            avatar.experience = this.experience;
            avatar.attack = this.attack;
            avatar.defense = this.defense;
            avatar.speed = this.speed;
            avatar.health = this.health;
            avatar.currentHealth = this.health;

            foreach (string s in this.knownMoves.Keys)
            {
                avatar.knownMoves.Add(s, this.knownMoves[s]);
            }

            return avatar;
        }

        #endregion

    }
}
