using System.ComponentModel;

namespace WpfKb.LogicalKeys
{
    public interface ILogicalKey : INotifyPropertyChanged
    {
        string DisplayName { get; }
        void Press();
        event LogicalKeyPressedEventHandler LogicalKeyPressed;
    }
}