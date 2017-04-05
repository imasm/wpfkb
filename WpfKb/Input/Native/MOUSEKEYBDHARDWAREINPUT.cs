using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
namespace WpfKb.Input.Native
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct MOUSEKEYBDHARDWAREINPUT
	{
		[FieldOffset(0)]
        internal MOUSEINPUT Mouse;

		[FieldOffset(0)]
        internal KEYBDINPUT Keyboard;

		[FieldOffset(0)]
        internal HARDWAREINPUT Hardware;
	}
}
