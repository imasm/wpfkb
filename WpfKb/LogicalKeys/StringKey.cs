using System;
using System.Linq;
using WindowsInput;

namespace WpfKb.LogicalKeys
{
    public class StringKey : LogicalKeyBase
    {
        private string _stringToSimulate;

        public virtual string StringToSimulate
        {
            get { return _stringToSimulate; }
            set
            {
                if (value != _stringToSimulate)
                {
                    _stringToSimulate = value;
                    OnPropertyChanged("StringToSimulate");
                }
            }
        }

        public StringKey(string displayName, string stringToSimulate)
        {
            DisplayName = displayName;
            _stringToSimulate = stringToSimulate;
        }

        public StringKey()
        {
        }

        public override void Press()
        {
            InputSimulator.SimulateTextEntry(_stringToSimulate);
            base.Press();
        }
    }
}
