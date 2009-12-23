using System.Collections.Generic;
using WindowsInput;

namespace WpfKb.LogicalKeys
{
    public class ShiftSensitiveKey : MultiCharacterKey
    {
        public ShiftSensitiveKey(VirtualKeyCode keyCode, IList<string> keyDisplays)
            : base(keyCode, keyDisplays)
        {
        }
    }
}