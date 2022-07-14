namespace Enemy.States
{
    public abstract class BaseEnemyState<TView>
        where TView : BaseEnemyView
    {
        protected TView _view;
        protected BaseEnemyLogic<TView> _logic;
        protected BaseEnemyData _data;

        protected BaseEnemyState(TView view, BaseEnemyLogic<TView> logic, BaseEnemyData data)
        {
            _view = view;
            _logic = logic;
            _data = data;
        }
        
        public virtual void OnEnter()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public abstract void SetNextState();
    }
}