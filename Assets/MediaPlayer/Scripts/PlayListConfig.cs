using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    [CreateAssetMenu(menuName = "VideoPlayer/PlayListConfig", fileName = "PlayListConfig_")]
    public class PlayListConfig : ScriptableObject
    {
        public List<VideoConfig> Videos = new List<VideoConfig>();
    }
}