using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private GameObject _cashedObject;

        public async UniTask<T> Load<T>(string key)
        {
            _cashedObject = await Addressables.InstantiateAsync(key).ToUniTask();

            if (_cashedObject.TryGetComponent(out T component) == false)
            {
                throw new NullReferenceException($"Object of type {typeof(T)} is null");
            }

            return component;
        }

        protected void Unload()
        {
            if (_cashedObject != null)
            {
                return;
            }

            _cashedObject.SetActive(false);
            Addressables.ReleaseInstance(_cashedObject);
            _cashedObject = null;
        }
    }
}