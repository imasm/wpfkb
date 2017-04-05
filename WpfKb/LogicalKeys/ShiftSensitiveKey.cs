using System.Collections.Generic;
using WpfKb.Input;

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