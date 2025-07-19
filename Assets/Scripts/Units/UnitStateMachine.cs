using Units.StateMachine;
using UnityEngine;

namespace Units
{
    public class UnitStateMachine : MonoBehaviour
    {
        private UnitState _currentState;

        public void Initialize(Unit unit)
        {
            SetState(new IdleState(unit));
        }
        
        private void Update() => _currentState?.Update();

        public UnitState GetState()
        {
            return _currentState;
        }
        
        public void SetState(UnitState next)
        {
            _currentState?.Exit();
            _currentState = next;
            _currentState.Enter();
        }
    }
}