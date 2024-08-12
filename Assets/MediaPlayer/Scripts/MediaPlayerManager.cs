using RenderHeads.Media.AVProVideo;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MediaPlayer
{
    public class MediaPlayerManager : MonoBehaviour
    {
        [SerializeField, Required] private RenderHeads.Media.AVProVideo.MediaPlayer player;
        [SerializeField] private PlayListController playList;
        [SerializeField] private ControlPanel controlPanel;

        // public Action<bool> playerStateChange;

        private VideoConfig currentVideo;

        private void Awake()
        {
            playList.SetVideoConfig += SetVideo;
            playList.Init();
            controlPanel.Click += TogglePlayButton;
            controlPanel.Init();
            player.Events.AddListener(HandleEvent);
        }

        private void OnDestroy()
        {
            playList.SetVideoConfig -= SetVideo;
            controlPanel.Click -= TogglePlayButton;
            player.Events.RemoveListener(HandleEvent);
        }

        public void SetVideo(VideoConfig config)
        {
            if (config == currentVideo)
                return;
            Pause();
            currentVideo = config;
            player.OpenMedia(new MediaPath($"{currentVideo.URLVideo}", MediaPathType.AbsolutePathOrURL), autoPlay: false);
        }

        [Button]
        public void TogglePlayButton()
        {
            if (!player || player.Control == null) return;

            if (player.Control.IsPlaying())
                Pause();
            else
                Play();
        }

        private void HandleEvent(RenderHeads.Media.AVProVideo.MediaPlayer mp, MediaPlayerEvent.EventType eventType, ErrorCode code)
        {
            switch (eventType)
            {
                case MediaPlayerEvent.EventType.FinishedPlaying:
                    // playerStateChange?.Invoke(false);
                    ChangeState(false);
                    break;
            }
        }

        private void Pause()
        {
            player.Pause();
            // playerStateChange?.Invoke(false);
            ChangeState(false);
        }

        [Button]
        private void Play()
        {
            if (player.MediaPath.Path != currentVideo.URLVideo)
                player.OpenMedia(new MediaPath($"{currentVideo.URLVideo}", MediaPathType.AbsolutePathOrURL));
            else
                player.Play();
            // playerStateChange?.Invoke(true);
            ChangeState(true);
        }

        private void ChangeState(bool state)
        {
            controlPanel.StateChange(state);
            // playerStateChange?.Invoke(true);
        }
    }
}