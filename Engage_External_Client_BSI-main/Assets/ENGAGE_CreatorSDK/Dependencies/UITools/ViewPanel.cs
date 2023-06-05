namespace Engage.UI.Editor
{
    public class ViewPanel<T> : IView where T : ViewModel, new()
    {
        protected T viewModel;
        public T ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new T();
                }

                return viewModel;
            }
            protected set
            {
                if (value == null)
                    return;

                Disable();
                viewModel = value;
                Enable();
            }
        }

        public event System.Action OnClose;
        public event System.Action OnViewUpdate;

        public ViewPanel()
        {
            Initialize();
        }

        public ViewPanel(T viewModel)
        {
            ViewModel = viewModel;
            Initialize();
        }

        public virtual void Initialize() { }

        public virtual void Enable()
        {
            UnityEngine.Debug.Log("View Enabled");
            ViewModel.OnPropertyChanged += OnViewModelUpdate;
        }

        public virtual void Disable()
        {
            UnityEngine.Debug.Log("View Disabled");
            ViewModel.OnPropertyChanged -= OnViewModelUpdate;
        }

        public virtual void Close()
        {
            OnClose?.Invoke();
        }

        public virtual void Draw() { }

        protected virtual void OnViewModelUpdate()
        {
            OnViewUpdate?.Invoke();
        }
    }
}