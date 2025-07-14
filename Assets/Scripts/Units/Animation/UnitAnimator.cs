using System;
using UnityEngine;

namespace Units.Animation
{
    public class UnitAnimator:MonoBehaviour
    {
        private Animator _animator;
        public event Action OnAttackAnimationEnd;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        // ResetState is used to reset the animator state when changing states or when the unit is idle
        public void ResetState()
        {
            _animator.SetBool(UnitAnimationParameters.IsMoving, false);
            _animator.SetBool(UnitAnimationParameters.IsAttacking, false);
        }
        // PlayMove is used to play the moving animation with the given direction
        public void PlayMove(Vector2 direction)
        {
            _animator.SetBool(UnitAnimationParameters.IsMoving, true);
            _animator.SetFloat(UnitAnimationParameters.MoveX, direction.x);
            _animator.SetFloat(UnitAnimationParameters.MoveY, direction.y);
        }
        // ChangeMovingDirection is used to change the direction of the moving animation
        public void ChangeMovingDirection(Vector2 direction)
        {
            _animator.SetFloat(UnitAnimationParameters.MoveX, direction.x);
            _animator.SetFloat(UnitAnimationParameters.MoveY, direction.y);
        }
        // PlayAttack is used to play the attacking animation
        public void PlayAttack()
        {
            _animator.SetBool(UnitAnimationParameters.IsAttacking, true);
        }
        // PlayIdle is used to play the idle animation
        public void PlayIdle()
        {
            ResetState();
        }
        
        // This method is called by the animation event at the end of the attack animation
        public void OnAttackAnimationEndEvent()
        {
            OnAttackAnimationEnd?.Invoke();
        }
    }
}