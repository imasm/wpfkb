using WpfKb.Input;

namespace WpfKb.LogicalKeys
{
    public abstract class ModifierKeyBase : VirtualKey
    {
        private bool _isInEffect;

        public bool IsInEffect
        {
            get { return _isInEffect; }
            set
            {
                if (value != _isInEffect)
                {
                    _isInEffect = value;
                    OnPropertyChanged("IsInEffect");
                }
            }
        }

        protected ModifierKeyBase(VirtualKeyCode keyCode) :
            base(keyCode)
        {
        }

        public abstract void SynchroniseKeyState();
    }
}