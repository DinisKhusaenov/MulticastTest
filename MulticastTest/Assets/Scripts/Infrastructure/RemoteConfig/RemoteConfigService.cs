using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.GameLogic;
using Infrastructure.Serialization;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;

namespace Infrastructure.RemoteConfig
{
    public class RemoteConfigService : IRemoteConfigService
    {
        private const string Key = "LevelData";
        public LevelsData Level { get; private set; }
        
        public async UniTask Initialize()
        {
            if (Utilities.CheckForInternetConnection())
            {
                await InitializeRemoteConfigAsync();
            }

            Unity.Services.RemoteConfig.RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            Unity.Services.RemoteConfig.RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }
        
        private async UniTask InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        
        void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            var level = Unity.Services.RemoteConfig.RemoteConfigService.Instance.appConfig.GetJson(Key);
            Level = JsonSerialization.FromJson<LevelsData>(level);
        }
    }
    
    struct userAttributes { }
    struct appAttributes { }
}