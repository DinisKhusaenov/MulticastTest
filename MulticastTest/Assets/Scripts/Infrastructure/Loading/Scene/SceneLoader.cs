using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.LogService;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Loading.Scene
{
    public class SceneLoader : ISceneLoadService
    {
        private readonly ZenjectSceneLoader _sceneLoader;
        private readonly ILogService _logService;

        private bool IsLoadingSceneLoaded => SceneManager.GetSceneByName(SceneNames.Loading).isLoaded;

        public SceneLoader(ZenjectSceneLoader sceneLoader, ILogService logService)
        {
            _sceneLoader = sceneLoader;
            _logService = logService;
        }

        public async UniTask LoadScene(string name, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == name)
            {
                _logService.Log($"Reloading the scene {name}");
                onLoaded?.Invoke();
                return;
            }

            await LoadScenes(name, onLoaded);
        }

        private async UniTask LoadScenes(string sceneName, Action onLoaded = null, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!IsLoadingSceneLoaded)
            {
                await LoadLoadingScene();
            }

            await _sceneLoader.LoadSceneAsync(sceneName, mode);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            onLoaded?.Invoke();
        }

        private async UniTask LoadLoadingScene(LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var loadingSceneLoad = _sceneLoader.LoadSceneAsync(SceneNames.Loading, mode);
            await loadingSceneLoad;
        }
    }
}
