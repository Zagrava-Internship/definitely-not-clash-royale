using System;
using UnityEngine;

namespace Towers.Animation
{
    public class TowerAnimator : MonoBehaviour
    {
        private Animator _animator;
        public event Action OnAttackAnimationEnd;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ResetState()
        {
            _animator.SetBool(TowerAnimationParameters.IsAttacking, false);
        }
        public void Initialize(float attackSpeed)
        {
            _animator.SetFloat(TowerAnimationParameters.AttackSpeed, attackSpeed);
        }
        
        public void PlayAttack()
        {
            _animator.SetBool(TowerAnimationParameters.IsAttacking,true); 
        }
        
        public void OnAttackAnimationEndEvent()
        {
            OnAttackAnimationEnd?.Invoke();
        }
    }
}