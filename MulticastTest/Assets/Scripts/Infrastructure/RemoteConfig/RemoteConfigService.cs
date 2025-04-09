using System;
using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.GameLogic;
using Infrastructure.Serialization;
using Infrastructure.Services.LogService;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;

namespace Infrastructure.RemoteConfig
{
    public class RemoteConfigService : IRemoteConfigService
    {
        private readonly ILogService _logService;
        private const string Key = "LevelData";
        private const string Environment = "production";
        
        public LevelsData Level { get; private set; }

        public RemoteConfigService(ILogService logService)
        {
            _logService = logService;
        }
        
        public async UniTask Initialize()
        {
            try
            {
                if (Utilities.CheckForInternetConnection())
                {
                    await InitializeRemoteConfigAsync();
                }

                Unity.Services.RemoteConfig.RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
                Unity.Services.RemoteConfig.RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
            }
            catch (Exception ex)
            {
                _logService.LogError("RemoteConfig Initialize error: " + ex.Message);
            }
        }
        
        private async UniTask InitializeRemoteConfigAsync()
        {
            try
            {
                var options = new InitializationOptions();
                options.SetEnvironmentName(Environment);
                await UnityServices.InitializeAsync(options);

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error during Unity Services init/auth: " + ex.Message);
            }
        }
        
        void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            if (configResponse.status == ConfigRequestStatus.Success)
            {
                var level = Unity.Services.RemoteConfig.RemoteConfigService.Instance.appConfig.GetJson(Key);
                Level = JsonSerialization.FromJson<LevelsData>(level);
            }
            else
            {
                Level = null;
            }
        }
    }
    
    struct userAttributes { }
    struct appAttributes { }
}