using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.GameLogic;

namespace Infrastructure.RemoteConfig
{
    public interface IRemoteConfigService
    {
        LevelsData Level { get; }
        UniTask Initialize();
    }
}