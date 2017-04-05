using System.Collections.Generic;
using WpfKb.Input;

namespace WpfKb.LogicalKeys
{
    public class ChordKey : LogicalKeyBase
    {
        public IList<VirtualKeyCode> ModifierKeys { get; private set; }
        public IList<VirtualKeyCode> Keys { get; private set; }

        public ChordKey(string displayName, VirtualKeyCode modifierKey, VirtualKeyCode key)
            : this(displayName, new List<VirtualKeyCode> { modifierKey }, new List<VirtualKeyCode> { key })
        {
        }

        public ChordKey(string displayName, IList<VirtualKeyCode> modifierKeys, VirtualKeyCode key)
            : this(displayName, modifierKeys, new List<VirtualKeyCode> { key })
        {
        }

        public ChordKey(string displayName, VirtualKeyCode modifierKey, IList<VirtualKeyCode> keys)
            : this(displayName, new List<VirtualKeyCode> { modifierKey }, keys)
        {
        }

        public ChordKey(string displayName, IList<VirtualKeyCode> modifierKeys, IList<VirtualKeyCode> keys)
        {
            DisplayName = displayName;
            ModifierKeys = modifierKeys;
            Keys = keys;
        }

        public override void Press()
        {
            InputSimulator.SimulateModifiedKeyStroke(ModifierKeys, Keys);
            base.Press();
        }
    }
}