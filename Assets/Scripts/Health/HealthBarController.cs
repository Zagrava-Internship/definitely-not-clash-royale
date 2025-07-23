using Targeting;
using Targeting.Extensions;
using UnityEngine;

namespace Health
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HealthBarController: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer fillRenderer;
        [SerializeField] private SpriteRenderer backgroundRenderer;
        private HealthComponent _health;

        public void Init(HealthComponent healthComponent,Team team)
        {
            if (healthComponent == null || backgroundRenderer == null || fillRenderer == null)
            {
                Debug.LogError("Components are not set up correctly on " + gameObject.name);
                return;
            }
            fillRenderer.color = team.GetColor();
            backgroundRenderer.color = team.GetColor(isDarkMode:true);
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