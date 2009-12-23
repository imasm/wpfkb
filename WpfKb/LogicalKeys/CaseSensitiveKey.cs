using System.Collections.Generic;
using WindowsInput;

namespace WpfKb.LogicalKeys
{
    public class CaseSensitiveKey : MultiCharacterKey
    {
        public CaseSensitiveKey(VirtualKeyCode keyCode, IList<string> keyDisplays)
            : base(keyCode, keyDisplays)
        {
        }
    }
}