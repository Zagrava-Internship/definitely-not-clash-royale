using System.Collections;
using Targeting;
using UnityEngine;

namespace Units.StateMachine
{
    public class AttackState: UnitState
    {
        private readonly ITargetable _target;
        private Coroutine _attackCoroutine;
        public AttackState(Unit unit, ITargetable target) : base(unit)
        {
            this._target = target;
        }

        public override void Enter()
        {
            Unit.Animator.PlayAttack();
            _attackCoroutine = Unit.StartCoroutine(AttackSequence());
        }
        
        private IEnumerator AttackSequence() 
        {
            while (true) 
            {
                if (_target == null || _target.IsTargetDead || _target != Unit.AttackerCurrentTarget)
                {
                    Unit.StateMachine.SetState(new IdleState(Unit));
                    yield break; 
                }
                if (Vector3.Distance(Unit.ObjectTransform.position, _target.ObjectTransform.position) > Unit.AttackStrategy.Range)
                {
                    Unit.StateMachine.SetState(new MoveState(Unit));
                    yield break; 
                }
                Unit.Animator.PlayAttack();
                
                var animationEnded = false;
                System.Action onAnimEnd = () => animationEnded = true;
                Unit.Animator.OnAttackAnimationEnd += onAnimEnd;
                
                yield return new WaitUntil(() => animationEnded); 
                Unit.Animator.OnAttackAnimationEnd -= onAnimEnd;
                
                Unit.AttackStrategy.Attack(Unit, _target);
                
                yield return new WaitForSeconds(Unit.AttackStrategy.AttackDelay);
            }
        }
        public override void Update()
        {
            if(_target== null || _target.IsTargetDead)
                Unit.StateMachine.SetState(new IdleState(Unit));
        }

        public override void Exit()
        {
            Unit.Animator.ResetState();
            if (_attackCoroutine != null)
            {
                Unit.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }
        
        public override string DisplayName => "Attack";

    }
}