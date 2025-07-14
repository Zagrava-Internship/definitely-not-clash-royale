using UnityEngine;

namespace Health
{
    public class HealthBarController: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer fillRenderer;
        private HealthComponent _health;

        public void Init(HealthComponent healthComponent)
        {
            _health = healthComponent;
            _health.OnHealthChanged += UpdateHp;
            UpdateHp();
        }

        private void UpdateHp()
        {
            var ratio = (float)_health.Current / _health.Max;
            
            var scale = fillRenderer.transform.localScale;
            scale.x = ratio;
            fillRenderer.transform.localScale = scale;
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.OnHealthChanged -= UpdateHp;
        }
    }
}