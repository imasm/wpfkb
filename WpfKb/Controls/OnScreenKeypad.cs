using System.Collections.ObjectModel;
using WpfKb.Input;
using WpfKb.LogicalKeys;


namespace WpfKb.Controls
{
    public class OnScreenKeypad : UniformOnScreenKeyboard
    {
        public OnScreenKeypad()
        {
            Keys = new ObservableCollection<OnScreenKey>
                       {
                           new OnScreenKey { GridRow = 0, GridColumn = 0, Key = new VirtualKey(VirtualKeyCode.VK_7, "7") },
                           new OnScreenKey { GridRow = 0, GridColumn = 1, Key = new VirtualKey(VirtualKeyCode.VK_8, "8") },
                           new OnScreenKey { GridRow = 0, GridColumn = 2, Key = new VirtualKey(VirtualKeyCode.VK_9, "9") },
                           new OnScreenKey { GridRow = 1, GridColumn = 0, Key = new VirtualKey(VirtualKeyCode.VK_4, "4") },
                           new OnScreenKey { GridRow = 1, GridColumn = 1, Key = new VirtualKey(VirtualKeyCode.VK_5, "5") },
                           new OnScreenKey { GridRow = 1, GridColumn = 2, Key = new VirtualKey(VirtualKeyCode.VK_6, "6") },
                           new OnScreenKey { GridRow = 2, GridColumn = 0, Key = new VirtualKey(VirtualKeyCode.VK_1, "1") },
                           new OnScreenKey { GridRow = 2, GridColumn = 1, Key = new VirtualKey(VirtualKeyCode.VK_2, "2") },
                           new OnScreenKey { GridRow = 2, GridColumn = 2, Key = new VirtualKey(VirtualKeyCode.VK_3, "3") },
                           new OnScreenKey { GridRow = 3, GridColumn = 0, Key = new VirtualKey(VirtualKeyCode.DELETE, "Clear") },
                           new OnScreenKey { GridRow = 3, GridColumn = 1, Key = new VirtualKey(VirtualKeyCode.VK_0, "0") },
                           new OnScreenKey { GridRow = 3, GridColumn = 2, Key = new VirtualKey(VirtualKeyCode.BACK, "Del") },
                       };
        }
    }
}