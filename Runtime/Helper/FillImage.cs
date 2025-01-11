using UnityEngine;
using UnityEngine.UI;

namespace DivineSkies.Tools.Helper
{{
    public class FillImage : MonoBehaviour
    {
        private const float _flowDuration = 0.5f;
        [SerializeField] private Image _fill;
        [SerializeField] private Image _flow;

        private Coroutine _flowRoutine;

        private float _value = 0;
        public float Value
        {
            get => _value;
            set
            {
                float oldValue = _value;
                _value = Mathf.Clamp(value, 0f, 1f);
                OnValueChanged(oldValue, _value);
            }
        }

        public void SetValueSilent(float newValue)
        {
            this.StopTickRoutine(ref _flowRoutine);

            _value = Mathf.Clamp(newValue, 0f, 1f); ;
            _flow.fillAmount = newValue;
            _fill.fillAmount = newValue;
        }

        private void OnValueChanged(float oldValue, float newValue)
        {
            if (oldValue == newValue)
                return;

            this.StopTickRoutine(ref _flowRoutine);

            Image fill = newValue > oldValue ? _flow : _fill;
            Image flow = newValue > oldValue ? _fill : _flow;

            fill.fillAmount = newValue;
            flow.fillAmount = oldValue;

            _flowRoutine = this.StartTickRoutine(dt =>
            {
                flow.fillAmount += (newValue - oldValue) * (1f / _flowDuration) * dt;
                return 1f / _flowDuration * dt;
            }, () => SetValueSilent(newValue));
        }
    }
}