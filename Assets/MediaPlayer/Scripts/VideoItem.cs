using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class VideoItem : MonoBehaviour
    {
        private const float height = 30;
        private const float duration = 1f;

        [SerializeField] private Button button;
        [SerializeField] private Image selector;
        [SerializeField] private Image prevImage;
        [SerializeField] private Image loadingImage;

        public Action<VideoItem> OnClickAction;

        private Tweener _tweener;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
            var startPosition = loadingImage.transform.localPosition;
            startPosition.y = height;
            loadingImage.transform.localPosition = startPosition;
            _tweener = loadingImage.transform.DOLocalMoveY(-height, duration).SetLoops(-1, LoopType.Yoyo);
            selector.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        public void SetPrevImage(Sprite sprite)
        {
            _tweener.Kill();
            loadingImage.gameObject.SetActive(false);
            prevImage.sprite = sprite;
        }

        public void SetState(bool isSelected)
        {
            selector.gameObject.SetActive(isSelected);
        }

        private void OnClick()
        {
            OnClickAction?.Invoke(this);
        }
    }
}