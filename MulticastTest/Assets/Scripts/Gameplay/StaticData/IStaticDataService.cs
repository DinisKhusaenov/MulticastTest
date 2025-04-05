using Gameplay.Levels.Configs;

namespace Gameplay.StaticData
{
    public interface IStaticDataService
    {
        LevelConfig LevelConfig { get; }
        void LoadLevelConfig();
    }
}