using System;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    [Serializable]
    public class ControlPanel
    {
        [SerializeField] private Image imagePlayPause;
        [SerializeField] private Sprite playSprite;
        [SerializeField] private Sprite pauseSprite;


        [SerializeField] private Button buttonPlayPause;

        // private MediaPlayerManager mediaPlayerManager;

        public Action Click;

        public void Init()//MediaPlayerManager mediaPlayerManager
        {
            // this.mediaPlayerManager = mediaPlayerManager;
            
            //mediaPlayerManager.TogglePlayButton
            buttonPlayPause.onClick.AddListener(OnClick);
            // this.mediaPlayerManager.playerStateChange += OnPlayerStateChange;
        }

        ~ControlPanel()
        {
            // if (mediaPlayerManager == null)
            //     return;
            buttonPlayPause.onClick.RemoveAllListeners();
            // this.mediaPlayerManager.playerStateChange -= OnPlayerStateChange;
        }

        public void StateChange(bool state)
        {
            imagePlayPause.sprite = (!state) ? playSprite : pauseSprite;
        }

        private void OnClick()
        {
            Click?.Invoke();
        }
    }
}