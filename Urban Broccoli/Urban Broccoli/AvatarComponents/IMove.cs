using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urban_Broccoli.AvatarComponents
{
    public enum Target
    {
        Self, Enemy
    }

    public enum MoveType
    {
        Attack, Heal, Buff, Debuff, Status
    }

    public enum Status
    {
        Normal, Sleep, Poison, Paralysis
    }

    public enum MoveElement
    {
        None, Dark, Earth, Fire, Light, Water, Wind
    }

    public interface IMove
    {
        string Name { get; }
        Target Target { get; }
        MoveType MoveType { get; }
        MoveElement MoveElement { get; }
        Status Status { get; }
        int UnlockedAt { get; set; }
        bool Unlocked { get; }
        int Duration { get; set; }
        int Attack { get; }
        int Defense { get; }
        int Speed { get; }
        int Health { get; }
        void Unlock();
        object Clone();
    }
}
