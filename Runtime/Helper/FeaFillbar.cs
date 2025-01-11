using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace DivineSkies.Tools.Helper
{
    public class Fillbar : MonoBehaviour
    {
        public const float FlowDuration = 0.5f;
        [SerializeField] private RectTransform _fill;
        [SerializeField] private RectTransform _flow;
        [SerializeField] private float _startValue = 0;
        [SerializeField] private Direction _fillDirection;

        private Vector2 _direction;
        private float _size;

        private float _value = -1;
        public float Value
        {
            get => _value;
            set
            {
                float oldValue = _value;
                _value =  Mathf.Clamp01(value);
                OnValueChanged(oldValue, _value);
            }
        }

        public void SetSilent(float value)
        {
            _value = Mathf.Clamp01(value);
            SetBarToValue(_fill, _value);
            SetBarToValue(_flow, _value);
        }

        private void Start() => StartCoroutine(WaitForLayoutInit());

        private IEnumerator WaitForLayoutInit()
        {
            yield return new WaitForEndOfFrame();
            Init();
        }

        private void Init()
        {
            _direction = _fillDirection switch
            {
                Direction.Left => Vector2.left,
                Direction.Up => Vector2.up,
                Direction.Right => Vector2.right,
                Direction.Down => Vector2.down,
                _ => Vector2.zero,
            };
            _size = _fillDirection switch
            {
                Direction.Left or Direction.Right => (transform as RectTransform).rect.width,
                Direction.Up or Direction.Down => (transform as RectTransform).rect.height,
                _ => 0f,
            };
            Vector2 staticAxis = Vector2.one - _direction.Abs();
            _fill.sizeDelta = _direction.Abs() * _size + staticAxis * Vector3.Scale(_fill.sizeDelta, staticAxis);
            _flow.sizeDelta = _direction.Abs() * _size + staticAxis * Vector3.Scale(_fill.sizeDelta, staticAxis);
            SetSilent(_startValue);
        }

        private void OnValueChanged(float oldValue, float newValue)
        {
            if (oldValue == newValue)
                return;

            RectTransform fill = newValue > oldValue ? _flow : _fill;
            RectTransform flow = newValue > oldValue ? _fill : _flow;

            SetBarToValue(fill, newValue);
            SetBarToValue(flow, oldValue);

            flow.MoveLocal(FlowDuration, _direction * _size * (newValue - oldValue));
        }

        private void SetBarToValue(RectTransform bar, float value) => bar.anchoredPosition = _direction * _size * (value - 1f);
    }
}