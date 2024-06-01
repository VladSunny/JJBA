using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace JJBA.UI
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private float _stayDuration = 1f;
        [SerializeField] private float startRotation = 90f;

        public Vector3 randomizePositionFrom = new Vector3(1f, 1f, 0);
        public Vector3 randomizePositionTo = new Vector3(1f, 1f, 0);

        private Vector3 _startScale = Vector3.zero;
        private Vector3 _endScale;

        private TextMeshProUGUI damageText;

        public void Initialize(float damageAmount)
        {
            RandomizePosition();

            damageText = GetComponentInChildren<TextMeshProUGUI>();
            damageText.text = "-" + damageAmount.ToString();
            _endScale = transform.localScale;

            StartPopup().Forget();
        }

        public void DestroyPopup()
        {
            Destroy(gameObject);
        }

        private void RandomizePosition()
        {
            transform.position = new Vector3
            (
                Random.Range(randomizePositionFrom.x, randomizePositionTo.x),
                Random.Range(randomizePositionFrom.y, randomizePositionTo.y),
                Random.Range(randomizePositionFrom.z, randomizePositionTo.z)
            );
        }

        private async UniTaskVoid StartPopup()
        {
            await AnimatedUIElementIn();
            await UniTask.Delay((int)(_stayDuration * 1000));
            await AnimatedUIElementOut();
        }

        private async UniTask AnimatedUIElementIn()
        {
            transform.localRotation = Quaternion.Euler(0, 0, startRotation);
            transform.localScale = _startScale;

            var rotateTween = transform.DORotate(Vector3.zero, _animationDuration).SetEase(Ease.OutBack).Play().ToUniTask();
            var scaleTween = transform.DOScale(_endScale, _animationDuration).SetEase(Ease.OutBack).Play().ToUniTask();

            await UniTask.WhenAll(rotateTween, scaleTween);
        }

        private async UniTask AnimatedUIElementOut()
        {
            var rotateTween = transform.DORotate(new Vector3(0, 0, startRotation), _animationDuration).SetEase(Ease.InBack).Play().ToUniTask();
            var scaleTween = transform.DOScale(_startScale, _animationDuration).SetEase(Ease.InBack).Play().ToUniTask();

            await UniTask.WhenAll(rotateTween, scaleTween);
        }
    }
}