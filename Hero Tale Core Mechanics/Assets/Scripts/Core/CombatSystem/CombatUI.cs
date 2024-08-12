using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CombatUI
    {
        private readonly Image _fillImage;
        private float _timer;

        private bool _isPause;

        public CombatUI(Image fillImage)
        {
            _fillImage = fillImage;
        }

        public void Resume()
        {
            _isPause = false;
        }

        public void Pause()
        {
            _isPause = true;
        }

        public IEnumerator UpdateFillImage(float duration, float startDuration = 0)
        {
            _timer = startDuration == 0 ? duration : startDuration;
            _fillImage.fillAmount = (duration - startDuration) / duration;

            while (_timer > 0)
            {
                _timer -= _isPause == true ? 0 : Time.deltaTime;

                if (_fillImage != null)
                    _fillImage.fillAmount = _timer / duration;
                else
                    yield break;

                yield return null;
            }

            _fillImage.fillAmount = 0f;
        }

        public void DestroyUI()
        {
            if (_fillImage != null && _fillImage.gameObject != null)
            {
                GameObject.Destroy(_fillImage.gameObject);
            }
        }
    }
}