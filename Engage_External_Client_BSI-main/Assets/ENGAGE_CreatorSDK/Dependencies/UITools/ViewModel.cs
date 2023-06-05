namespace Engage.UI.Editor
{
    public abstract class ViewModel : System.IDisposable
    {
        public event System.Action OnPropertyChanged;

        public ViewModel() { Initialize(); }

        protected virtual void Initialize() { }

        public void NotifyPropertyChange()
        {
            OnPropertyChanged?.Invoke();
        }

        public virtual void Dispose()
        {
            OnPropertyChanged = null;
        }
    }
}
