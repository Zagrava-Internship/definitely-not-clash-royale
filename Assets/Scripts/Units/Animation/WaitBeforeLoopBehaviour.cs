using Combat.Interfaces;
using UnityEngine;

namespace Units.Animation
{
    public class WaitBeforeLoopBehaviour: StateMachineBehaviour
    {
        private float _delay = 1.5f;
        private float _timer;
        private bool _delayed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //_timer = 0f;
            //_delayed = false;
            //var attacker = animator.GetComponent<IAttacker>();
            //if (attacker is not null)
            //    _delay = attacker.AttackerDelay; // Use the weapon's attack delay if available
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Check if the animation is still playing and not yet delayed
            //if (stateInfo.normalizedTime < 1f) return;
            //if (_delayed) return;
            //_timer += Time.deltaTime;
            //if (!(_timer >= _delay)) return;
            //animator.Play(stateInfo.fullPathHash, layerIndex, 0f); // Restart the animation from the beginning
            //_delayed = true;
        }
    }
}