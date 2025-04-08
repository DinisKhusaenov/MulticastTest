using Gameplay.Sound.Config;
using Gameplay.StaticData;
using Infrastructure.PoolService;
using Infrastructure.PoolService.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.Sound
{
    public class SoundSystem : MonoBehaviour
    {
        private IPoolFactory _poolFactory;
        private SoundConfig _config;
        
        private ObjectPool<SoundItem> _pool;

        [Inject]
        public void Construct(IPoolFactory poolFactory, IStaticDataService staticDataService)
        {
            _poolFactory = poolFactory;
            _pool = new ObjectPool<SoundItem>(_poolFactory);
            _config = staticDataService.SoundConfig;
            _pool.Initialize(_config.PoolCapacity, PoolObjectType.Sound, transform);
        }

        public void Play(SoundType type)
        {
            var prefab = _pool.Get(Vector3.zero, transform);
            prefab.Play(_config.GetDataBy(type));
        }
    }
}