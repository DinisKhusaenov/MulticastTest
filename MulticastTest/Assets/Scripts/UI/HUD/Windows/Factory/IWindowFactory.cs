using Cysharp.Threading.Tasks;
using UI.Menu.Settings;
using UnityEngine;

namespace UI.HUD.Windows.Factory
{
    public interface IWindowFactory
    {
        UniTask<IGameOverView> CreateGameOverView(Canvas canvas);
        UniTask<ISettingsView> CreateSettingsView(Transform parent);
    }
}