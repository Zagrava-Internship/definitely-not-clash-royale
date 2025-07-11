using System.Collections.Generic;
using Units;

namespace Targeting
{
    public static class TargetRegistry
    {
        public static readonly List<ITargetable> AllTargets = new();
    }
}