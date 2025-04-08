using Gameplay.Levels.Configs;
using Gameplay.Sound.Config;

namespace Gameplay.StaticData
{
    public interface IStaticDataService
    {
        LevelConfig LevelConfig { get; }
        SoundConfig SoundConfig { get; }
        void LoadLevelConfig();
    }
}