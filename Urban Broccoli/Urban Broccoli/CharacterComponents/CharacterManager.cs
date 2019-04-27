using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urban_Broccoli.CharacterComponents
{
    public sealed class CharacterManager
    {
        private static readonly CharacterManager instance = new CharacterManager();

        private Dictionary<string, ICharacter> characters = new Dictionary<string, ICharacter>();

        public static CharacterManager Instance
        {
            get { return instance; }
        }

        private CharacterManager()
        {

        }

        public ICharacter GetCharacter(string name)
        {
            if (characters.ContainsKey(name))
            {
                return characters[name];
            }

            return null;
        }

        public void AddCharacter(string name, ICharacter character)
        {
            if (!characters.ContainsKey(name))
            {
                characters.Add(name, character);
            }
        }
    }
}
