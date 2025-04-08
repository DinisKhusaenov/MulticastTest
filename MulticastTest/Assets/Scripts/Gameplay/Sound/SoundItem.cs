using Gameplay.Sound.Config;
using UnityEngine;

namespace Gameplay.Sound
{
    public class SoundItem : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        public SoundData SoundData { get; private set; }

        public void Play(SoundData soundData)
        {
            SoundData = soundData;

            _audioSource.clip = SoundData.AudioClip;
            _audioSource.outputAudioMixerGroup = SoundData.MixerGroup;
            _audioSource.loop = SoundData.isLoop;

            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}