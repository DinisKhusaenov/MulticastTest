using System.Collections.Generic;
using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters;

namespace Gameplay.Levels
{
    public interface ILevelCompletionChecker
    {
        bool IsCompleted(List<IClusterContainer> clusterContainers, Level level);
    }
}