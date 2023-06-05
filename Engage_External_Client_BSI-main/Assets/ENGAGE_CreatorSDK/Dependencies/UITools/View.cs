using UnityEditor;

namespace Engage.UI.Editor
{
    public abstract class View<T> : EditorWindow where T : ViewModel, new()
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

                OnDisable();
                viewModel = value;
                OnEnable();
            }
        }

        public event System.Action OnClose;

        protected virtual void OnEnable()
        {
            ViewModel.OnPropertyChanged += OnViewModelUpdate;
        }

        protected virtual void OnDisable()
        {
            ViewModel.OnPropertyChanged -= OnViewModelUpdate;
            OnClose?.Invoke();
        }

        protected virtual void OnViewModelUpdate()
        {
            Repaint();
        }
    }
}