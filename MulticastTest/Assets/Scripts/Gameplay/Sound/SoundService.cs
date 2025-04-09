using System;
using Gameplay.Sound.Config;
using Gameplay.StaticData;
using Infrastructure.PoolService;
using Infrastructure.PoolService.Factory;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Gameplay.Sound
{
    public class SoundService : MonoBehaviour, ISoundService
    {
        [SerializeField] private AudioMixer _audioMixer;
        
        private IPoolFactory _poolFactory;
        private SoundConfig _config;
        
        private ObjectPool<SoundItem> _pool;

        [Inject]
        private void Construct(IPoolFactory poolFactory, IStaticDataService staticDataService)
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
            prefab.OnEnded += ReturnToPool;
        }

        public void Stop(SoundType type)
        {
            foreach (SoundItem item in _pool.Entries)
            {
                if (item.SoundData.SoundType == type)
                    item.Stop();
            }
        }

        public void Mute(bool isMute, SoundType type = SoundType.None)
        {
            switch (type)
            {
                case SoundType.None:
                    if (isMute)
                        _audioMixer.SetFloat(SoundGroupName.Master, -80f);
                    else
                        _audioMixer.SetFloat(SoundGroupName.Master, 0f);
                    return;
                
                case SoundType.BackgroundMusic:
                    if (isMute)
                        _audioMixer.SetFloat(SoundGroupName.BackgroundMusic, -80f);
                    else
                        _audioMixer.SetFloat(SoundGroupName.BackgroundMusic, 0f);
                    return;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void ReturnToPool(SoundItem item)
        {
            _pool.Return(item);
        }
    }
}