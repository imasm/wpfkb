using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
namespace WpfKb.Input.Native
{

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        internal VirtualKeyCode Vk; // ushort
        internal ushort Scan; //ScanCodeShort
        internal KEYEVENTF Flags;
        internal uint Time;
        internal UIntPtr ExtraInfo;
    }

    [Flags]
    internal enum KEYEVENTF : uint
    {
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        SCANCODE = 0x0008,
        UNICODE = 0x0004
    }
}
