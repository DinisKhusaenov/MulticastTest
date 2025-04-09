using Gameplay.Sound;
using Gameplay.StaticData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu.Settings
{
    public class SettingsView : MonoBehaviour, ISettingsView
    {
        [SerializeField] private Button _soundMute;
        [SerializeField] private Button _back;
        [SerializeField] private Sprite _soundOn, _soundOff;
        
        private ISoundService _soundService;
        private bool _isMuted;

        [Inject]
        private void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public void Show()
        {
            gameObject.SetActive(true);

            _isMuted = PlayerPrefs.GetInt(DataBaseConstants.Volume) == 0;
            _soundMute.image.sprite = _isMuted
                ? _soundOff
                : _soundOn;

            _soundMute.onClick.AddListener(OnMuteClicked);
            _back.onClick.AddListener(Hide);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _soundMute.onClick.RemoveListener(OnMuteClicked);
            _back.onClick.RemoveListener(Hide);
        }

        private void OnMuteClicked()
        {
            _isMuted = !_isMuted;
            _soundService.Mute(_isMuted);
            _soundMute.image.sprite = _isMuted
                ? _soundOff
                : _soundOn;
            PlayerPrefs.SetInt(DataBaseConstants.Volume, _isMuted ? 0 : 1 );
        }
    }
}