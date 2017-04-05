using WpfKb.Input;

namespace WpfKb.LogicalKeys
{
    public class InstantaneousModifierKey : ModifierKeyBase
    {
        public InstantaneousModifierKey(string displayName, VirtualKeyCode keyCode) :
            base(keyCode)
        {
            DisplayName = displayName;
        }

        public override void Press()
        {
            if (IsInEffect) InputSimulator.SimulateKeyUp(KeyCode);
            else InputSimulator.SimulateKeyDown(KeyCode);

            // We need to use IsKeyDownAsync here so we will know exactly what state the key will be in
            // once the active windows read the input from the MessagePump.  IsKeyDown will only report
            // the correct value after the input has been read from the MessagePump and will not be correct
            // by the time we set IsInEffect.
            IsInEffect = InputSimulator.IsKeyDownAsync(KeyCode);
            OnKeyPressed();
        }

        public override void SynchroniseKeyState()
        {
            IsInEffect = InputSimulator.IsKeyDownAsync(KeyCode);
        }
    }
}