namespace Units.StateMachine
{
    /// <summary>
    /// Base class for all unit states (Idle, Move, Attack, etc.).
    /// Each state controls unit behavior for a period of time.
    /// </summary>
    public abstract  class UnitState
    {
        //  Reference to the unit controlled by this state.
        protected readonly Unit Unit;
        
        protected UnitState(Unit unit) { Unit = unit; }
        
        // Called once when entering this state.
        public abstract void Enter();
        
        // Called every frame while this state is active.
        public abstract void Update();
        
        // Called once when exiting this state.
        public abstract void Exit();
        
        // used for UI menus
        public abstract string DisplayName { get; }
    }
}