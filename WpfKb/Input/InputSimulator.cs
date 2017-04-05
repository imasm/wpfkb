using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WpfKb.Input.Native;

namespace WpfKb.Input
{
    public static class InputSimulator
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern short GetAsyncKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern short GetKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        public static bool IsKeyDownAsync(VirtualKeyCode keyCode)
        {
            short result = GetAsyncKeyState((ushort)keyCode);
            return result < 0;
        }

        public static bool IsKeyDown(VirtualKeyCode keyCode)
        {
            short result = GetKeyState((ushort)keyCode);
            return result < 0;
        }

        public static bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            short result = GetKeyState((ushort)keyCode);
            return (result & 1) == 1;
        }

        public static void SimulateKeyDown(VirtualKeyCode keyCode)
        {
            INPUT down = default(INPUT);
            down.Type = InputType.KEYBOARD;
            down.Data.Keyboard = default(KEYBDINPUT);
            down.Data.Keyboard.Vk = keyCode;
            down.Data.Keyboard.Scan = 0;
            down.Data.Keyboard.Flags = 0u;
            down.Data.Keyboard.Time = 0u;
            down.Data.Keyboard.ExtraInfo = UIntPtr.Zero;
            uint numberOfSuccessfulSimulatedInputs = SendInput(1u, new INPUT[]
            {
                down
            }, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0u)
            {
                throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
            }
        }

        public static void SimulateKeyUp(VirtualKeyCode keyCode)
        {
            INPUT up = default(INPUT);
            up.Type = InputType.KEYBOARD;
            up.Data.Keyboard = default(KEYBDINPUT);
            up.Data.Keyboard.Vk = keyCode;
            up.Data.Keyboard.Scan = 0;
            up.Data.Keyboard.Flags = KEYEVENTF.KEYUP;
            up.Data.Keyboard.Time = 0u;
            up.Data.Keyboard.ExtraInfo = UIntPtr.Zero;
            uint numberOfSuccessfulSimulatedInputs = SendInput(1u, new INPUT[]
            {
                up
            }, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0u)
            {
                throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
            }
        }

        public static void SimulateKeyPress(VirtualKeyCode keyCode)
        {
            INPUT down = default(INPUT);
            down.Type = InputType.KEYBOARD;
            down.Data.Keyboard = default(KEYBDINPUT);
            down.Data.Keyboard.Vk = keyCode;
            down.Data.Keyboard.Scan = 0;
            down.Data.Keyboard.Flags = 0u;
            down.Data.Keyboard.Time = 0u;
            down.Data.Keyboard.ExtraInfo = UIntPtr.Zero;

            INPUT up = default(INPUT);
            up.Type = InputType.KEYBOARD;
            up.Data.Keyboard = default(KEYBDINPUT);
            up.Data.Keyboard.Vk = keyCode;
            up.Data.Keyboard.Scan = 0;
            up.Data.Keyboard.Flags = KEYEVENTF.KEYUP;
            up.Data.Keyboard.Time = 0u;
            up.Data.Keyboard.ExtraInfo = UIntPtr.Zero;
            uint numberOfSuccessfulSimulatedInputs = SendInput(2u, new INPUT[]
            {
                down,
                up
            }, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0u)
            {
                throw new Exception(string.Format("The key press simulation for {0} was not successful.", keyCode));
            }
        }

        public static void SimulateTextEntry(string text)
        {
            if ((uint) text.Length > int.MaxValue)
            {
                throw new ArgumentException(
                    string.Format("The text parameter is too long. It must be less than {0} characters.", 2147483647u), "text");
            }

            byte[] chars = Encoding.ASCII.GetBytes(text);
            int len = chars.Length;
            INPUT[] inputList = new INPUT[len * 2];
            for (int x = 0; x < len; x++)
            {
                ushort scanCode = (ushort)chars[x];
                INPUT down = default(INPUT);
                down.Type = InputType.KEYBOARD;
                down.Data.Keyboard = default(KEYBDINPUT);
                down.Data.Keyboard.Vk = 0;
                down.Data.Keyboard.Scan = scanCode;
                down.Data.Keyboard.Flags = KEYEVENTF.UNICODE;
                down.Data.Keyboard.Time = 0u;
                down.Data.Keyboard.ExtraInfo = UIntPtr.Zero;

                INPUT up = default(INPUT);
                up.Type = InputType.KEYBOARD;
                up.Data.Keyboard = default(KEYBDINPUT);
                up.Data.Keyboard.Vk = 0;
                up.Data.Keyboard.Scan = scanCode;
                up.Data.Keyboard.Flags = KEYEVENTF.UNICODE | KEYEVENTF.KEYUP;
                up.Data.Keyboard.Time = 0u;
                up.Data.Keyboard.ExtraInfo = UIntPtr.Zero;
                if ((scanCode & 65280) == 57344)
                {
                    down.Data.Keyboard.Flags = (down.Data.Keyboard.Flags | KEYEVENTF.EXTENDEDKEY);
                    up.Data.Keyboard.Flags = (up.Data.Keyboard.Flags | KEYEVENTF.EXTENDEDKEY);
                }
                inputList[2 * x] = down;
                inputList[2 * x + 1] = up;
            }
            uint numberOfSuccessfulSimulatedInputs = InputSimulator.SendInput((uint)(len * 2), inputList,
                Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            SimulateKeyDown(modifierKeyCode);
            SimulateKeyPress(keyCode);
            SimulateKeyUp(modifierKeyCode);
        }

        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes,
            VirtualKeyCode keyCode)
        {
            List<VirtualKeyCode> modifierKeyCodesList = modifierKeyCodes?.ToList();
            modifierKeyCodesList?.ForEach(SimulateKeyDown);
            SimulateKeyPress(keyCode);
            modifierKeyCodesList?.Reverse<VirtualKeyCode>().ToList().ForEach(SimulateKeyUp);
        }


        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            SimulateKeyDown(modifierKey);
            keyCodes?.ToList().ForEach(SimulateKeyPress);
            SimulateKeyUp(modifierKey);
        }

        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes,
            IEnumerable<VirtualKeyCode> keyCodes)
        {
            List<VirtualKeyCode> modifierKeyCodesList = modifierKeyCodes?.ToList();

            modifierKeyCodesList?.ForEach(SimulateKeyDown);

            keyCodes?.ToList().ForEach(SimulateKeyPress);

            modifierKeyCodesList?.Reverse<VirtualKeyCode>().ToList().ForEach(SimulateKeyUp);

        }
    }
}
