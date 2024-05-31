using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace JJBA.UI
{
    public class CooldownTimer : MonoBehaviour
    {
        [SerializeField] private float _cooldownTime = 1f;
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private float startRotation = 90f;

        private Image _image;
        private RectTransform _rectTransform;

        private Vector3 _startScale = Vector3.zero;
        private Vector3 _endScale;
        private float _timer = 0f;
        private bool _isCoolingDown = false;

        public void Initialize(float cooldown)
        {
            _cooldownTime = cooldown;
            _isCoolingDown = true;
            _timer = 0f;

            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();

            _endScale = _rectTransform.localScale;

            AnimatedUIElementIn();
            StartCooldown().Forget();
        }

        private void Update()
        {
            if (_isCoolingDown)
            {
                _timer += Time.deltaTime;
                _image.fillAmount = 1 - _timer / _cooldownTime;
            }
        }

        private void AnimatedUIElementIn()
        {
            _rectTransform.localRotation = Quaternion.Euler(0, 0, startRotation);
            _rectTransform.localScale = _startScale;

            _rectTransform.DORotate(Vector3.zero, _animationDuration).SetEase(Ease.OutBack);
            _rectTransform.DOScale(_endScale, _animationDuration).SetEase(Ease.OutBack);
        }

        private async UniTaskVoid StartCooldown()
        {
            await UniTask.Delay((int)(_cooldownTime * 1000));
            _isCoolingDown = false;

            await AnimatedUIElementOut();
            Destroy(gameObject);
        }

        private async UniTask AnimatedUIElementOut()
        {
            var rotateTween = _rectTransform.DORotate(new Vector3(0, 0, startRotation), _animationDuration).SetEase(Ease.InBack).Play().ToUniTask();
            var scaleTween = _rectTransform.DOScale(_startScale, _animationDuration).SetEase(Ease.InBack).Play().ToUniTask();

            await UniTask.WhenAll(rotateTween, scaleTween);
        }
    }
}
