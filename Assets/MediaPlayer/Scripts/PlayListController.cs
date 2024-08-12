using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace MediaPlayer
{
    [System.Serializable]
    public class PlayListController
    {
        [SerializeField] private PlayListConfig Config;
        [SerializeField] private TextMeshProUGUI videoName;
        [SerializeField] private VideoItem prefab;
        [SerializeField] private Transform root;


        [SerializeField] private bool testingMod = false;
        [SerializeField, ShowIf("testingMod")] private Vector2 prevLoadingDelayTime = new Vector2(1, 10);
        [SerializeField, ShowIf("testingMod")] private bool forceDownload = false;

        private Dictionary<VideoItem, VideoConfig> videoList = new Dictionary<VideoItem, VideoConfig>();


        private MediaPlayerManager mediaPlayerManager;

        public void Init(MediaPlayerManager mediaPlayerManager)
        {
            this.mediaPlayerManager = mediaPlayerManager;

            foreach (var config in Config.Videos)
            {
                var newItem = GameObject.Instantiate(prefab, root);
                newItem.OnClickAction += OnVideoClick;
                UpdatePrevImage(newItem, config);
                videoList.Add(newItem, config);
            }

            if (videoList.Count > 0)
                OnVideoClick(videoList.FirstOrDefault().Key);
        }

        ~PlayListController()
        {
            foreach (var video in videoList)
                video.Key.OnClickAction -= OnVideoClick;
        }


        private void UpdatePrevImage(VideoItem item, VideoConfig config)
        {
            if ((!testingMod || !forceDownload) && TryGetPrevImage(GetFileName(config.URLImage), out var sprite))
            {
                item.SetPrevImage(sprite);
                return;
            }
            
            DownloadImageAsync(config.URLImage, item);
        }

        private bool TryGetPrevImage(string imageName, out Sprite sprite)
        {
            sprite = null;

            var filePath = Path.Combine(Application.persistentDataPath, $"{imageName}");
            try
            {
                if (File.Exists(filePath))
                {
                    var bytes = File.ReadAllBytes(filePath);
                    var texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(bytes);
                    sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 1f);
                    return true;
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"has prev exception: {exception}");
            }

            return false;
        }

        private async UniTaskVoid DownloadImageAsync(string imageUrl, VideoItem item)
        {
            try
            {
                using var www = UnityWebRequestTexture.GetTexture(imageUrl);
                await www.SendWebRequest();
                if (testingMod)
                    await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(prevLoadingDelayTime.x, prevLoadingDelayTime.y)), ignoreTimeScale: false);

                if (www.result == UnityWebRequest.Result.Success)
                {
                    var t = DownloadHandlerTexture.GetContent(www);
                    var sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
                    item.SetPrevImage(sprite);
                    SaveTexture(t, GetFileName(imageUrl));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Download Image Exception: {ex}");
            }
        }

        private string GetFileName(string urlString)
        {
            if (urlString.Contains("/"))
                return urlString.Substring(urlString.LastIndexOf('/') + 1);
            // if (urlString.Contains("\\"))
            //     return urlString.Substring(urlString.LastIndexOf('\\'));
            return urlString;
        }

        private void SaveTexture(Texture2D texture2D, string fileName)
        {
            Debug.Log($"file name {fileName}");
            try
            {
                if (texture2D == null)
                    return;

                var pngData = texture2D.EncodeToJPG();
                if (pngData == null)
                    return;

                var filePath = Path.Combine(Application.persistentDataPath, $"{fileName}");

                File.WriteAllBytes(filePath, pngData);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Save Texture exception: {exception}");
            }
        }

        private void SetVideo(VideoConfig config)
        {
            videoName.text = config.Name;
            mediaPlayerManager.SetVideo(config);
        }


        private void OnVideoClick(VideoItem videoItem)
        {
            foreach (var video in videoList)
            {
                if (videoItem == video.Key)
                {
                    video.Key.SetState(true);
                    SetVideo(video.Value);
                    continue;
                }

                video.Key.SetState(false);
            }
        }
    }
}