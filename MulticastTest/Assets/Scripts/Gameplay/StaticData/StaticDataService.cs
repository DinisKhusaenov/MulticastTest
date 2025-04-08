using Gameplay.Levels.Configs;
using Gameplay.Sound.Config;
using UnityEngine;

namespace Gameplay.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelConfigPath = "Configs/Level/LevelConfig";
        private const string SoundConfigPath = "Configs/Sound/SoundConfig";
        
        public LevelConfig LevelConfig { get; private set; }
        public SoundConfig SoundConfig { get; private set; }

        public StaticDataService()
        {
            LoadLevelConfig();
            LoadSoundConfig();
        }

        private void LoadSoundConfig()
        {
            SoundConfig = Resources.Load<SoundConfig>(SoundConfigPath);
        }

        public void LoadLevelConfig()
        {
            LevelConfig = Resources.Load<LevelConfig>(LevelConfigPath);
        }
    }
}