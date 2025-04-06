using Gameplay.Levels.Configs;
using UnityEngine;

namespace Gameplay.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelConfigPath = "Configs/Level/LevelConfig";
        
        public LevelConfig LevelConfig { get; private set; }

        public StaticDataService()
        {
            LoadLevelConfig();
        }

        public void LoadLevelConfig()
        {
            LevelConfig = Resources.Load<LevelConfig>(LevelConfigPath);
        }
    }
}