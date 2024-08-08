using UnityEngine;

namespace MediaPlayer
{
    [CreateAssetMenu(menuName = "VideoPlayer/CreateVideoConfig", fileName = "VideoConfig_")]
    public class VideoConfig : ScriptableObject
    {
        public string Name;
        public string URLVideo;
        public string URLImage;
    }


    
}