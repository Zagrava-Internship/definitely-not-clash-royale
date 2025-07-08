using Grid;

namespace Units.UnitStates
{
    public class IdleState : UnitState
    {
        public IdleState(Unit unit) : base(unit) { }
        public override void Enter() {  }

        public override void Update()
        {
            if (Unit.CurrentTarget == null || Unit.CurrentTarget.IsDead)
            {
                var target = TargetFinder.FindClosestTarget(Unit.transform.position);
                if (target != null)
                    Unit.SetTarget(target);
            }
            if (Unit.CurrentTarget is { IsDead: false })
            {
                var node = GridManager.Instance.GetClosestNode(Unit.CurrentTarget.Position);
                if (node != null) Unit.SetState(new MoveState(Unit, node));
            }
        }

        public override void Exit() { }
    }

}