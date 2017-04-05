using WpfKb.Input;

namespace WpfKb.LogicalKeys
{
    public class VirtualKey : LogicalKeyBase
    {
        private VirtualKeyCode _keyCode;

        public virtual VirtualKeyCode KeyCode
        {
            get { return _keyCode; }
            set
            {
                if (value != _keyCode)
                {
                    _keyCode = value;
                    OnPropertyChanged("KeyCode");
                }
            }
        }

        public VirtualKey(VirtualKeyCode keyCode, string displayName)
        {
            DisplayName = displayName;
            KeyCode = keyCode;
        }

        public VirtualKey(VirtualKeyCode keyCode)
        {
            KeyCode = keyCode;
        }

        public VirtualKey()
        {
        }

        public override void Press()
        {
            InputSimulator.SimulateKeyPress(_keyCode);
            base.Press();
        }
    }
}