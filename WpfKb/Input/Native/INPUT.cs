using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace WpfKb.Input.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
	{
        internal InputType Type;
		internal MOUSEKEYBDHARDWAREINPUT Data;
        internal static int Size => Marshal.SizeOf(typeof(INPUT));
	}

    internal enum InputType : uint
    {
        MOUSE = 0,
        KEYBOARD = 1,
        HARDWARE = 2
    }
}
