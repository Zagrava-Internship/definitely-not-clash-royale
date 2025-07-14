using UnityEngine;

namespace Units.Animation
{
    public class WaitBeforeLoopBehaviour: StateMachineBehaviour
    {
        public float delay = 1.5f;
        private float _timer;
        private bool _delayed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer = 0f;
            _delayed = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_delayed) return;
            _timer += Time.deltaTime;
            if (!(_timer >= delay)) return;
            animator.Play(stateInfo.fullPathHash, layerIndex, 0f); // Restart the animation from the beginning
            _delayed = true;
        }
    }
}