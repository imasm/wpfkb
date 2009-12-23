using WindowsInput;

namespace WpfKb.LogicalKeys
{
    public class TogglingModifierKey : ModifierKeyBase
    {
        public TogglingModifierKey(string displayName, VirtualKeyCode keyCode) :
            base(keyCode)
        {
            DisplayName = displayName;
        }

        public override void Press()
        {
            // This is a bit tricky because we can only get the state of a toggling key after the input has been
            // read off the MessagePump.  Ergo if we make that assumption that in the time it takes to run this method
            // we will be toggling the state of the key, set IsInEffect to the new state and then press the key.
            IsInEffect = !InputSimulator.IsTogglingKeyInEffect(KeyCode);
            base.Press();
        }

        public override void SynchroniseKeyState()
        {
            IsInEffect = InputSimulator.IsTogglingKeyInEffect(KeyCode);
        }
    }
}