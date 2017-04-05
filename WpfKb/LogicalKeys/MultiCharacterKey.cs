using System;
using System.Collections.Generic;
using WpfKb.Input;

namespace WpfKb.LogicalKeys
{
    public class MultiCharacterKey : VirtualKey
    {
        private int _selectedIndex;
        public IList<string> KeyDisplays { get; protected set; }
        public string SelectedKeyDisplay { get; protected set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value != _selectedIndex)
                {
                    _selectedIndex = value;
                    SelectedKeyDisplay = KeyDisplays[value];
                    DisplayName = SelectedKeyDisplay;
                    OnPropertyChanged("SelectedIndex");
                    OnPropertyChanged("SelectedKeyDisplay");
                }
            }
        }

        public MultiCharacterKey(VirtualKeyCode keyCode, IList<string> keyDisplays) :
            base(keyCode)
        {
            if (keyDisplays == null) throw new ArgumentNullException("keyDisplays");
            if (keyDisplays.Count <= 0)
                throw new ArgumentException("Please provide a list of one or more keyDisplays", "keyDisplays");
            KeyDisplays = keyDisplays;
            DisplayName = keyDisplays[0];
        }
    }
}