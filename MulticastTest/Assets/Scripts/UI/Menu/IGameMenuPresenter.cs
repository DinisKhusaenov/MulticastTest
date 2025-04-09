using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Menu
{
    public interface IGameMenuPresenter
    {
        void StartGame();
        UniTask ShowSettings(Transform parent);
    }
}