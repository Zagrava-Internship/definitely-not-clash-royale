using System;

namespace Mana
{
    public interface  IManaReadOnly
    {
        float CurrentMana { get; }
        int MaxMana { get; }
        bool CanSpend(int amount);
        event Action<float> OnManaChanged;
    }
}