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

        private MediaPlayerManager mediaPlayerManager;

        public void Init(MediaPlayerManager mediaPlayerManager)
        {
            this.mediaPlayerManager = mediaPlayerManager;
            buttonPlayPause.onClick.AddListener(mediaPlayerManager.TogglePlayButton);
            this.mediaPlayerManager.playerStateChange += OnPlayerStateChange;
        }

        ~ControlPanel()
        {
            if (mediaPlayerManager == null)
                return;
            buttonPlayPause.onClick.RemoveAllListeners();
            this.mediaPlayerManager.playerStateChange -= OnPlayerStateChange;
        }

        private void OnPlayerStateChange(bool state)
        {
            imagePlayPause.sprite = (!state) ? playSprite : pauseSprite;
        }
    }
}