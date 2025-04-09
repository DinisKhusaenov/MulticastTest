using Gameplay.Sound.Config;

namespace Gameplay.Sound
{
    public interface ISoundService
    {
        void Play(SoundType type);
        void Stop(SoundType type);
        void Mute(bool isMute, SoundType type = SoundType.None);
    }
}