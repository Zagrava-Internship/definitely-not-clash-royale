using Grid;

namespace Units.UnitStates
{
    public class MoveState : UnitState
    {
        private readonly GridNode _targetNode;

        public MoveState(Unit unit, GridNode node) : base(unit)
        {
            _targetNode = node;
        }

        public override void Enter()
        {
            if (_targetNode == null)
            {
                Unit.SetState(new IdleState(Unit));
                return;
            }

            Unit.Mover.OnPathComplete += OnPathComplete;
            Unit.Mover.MoveTo(_targetNode);
        }

        public override void Update() { }

        public override void Exit()
        {
            Unit.Mover.OnPathComplete -= OnPathComplete;
        }

        private void OnPathComplete()
        {
            if (Unit.CurrentTarget != null)
                Unit.SetState(new AttackState(Unit, Unit.CurrentTarget));
            else
                Unit.SetState(new IdleState(Unit));
        }
    }


}