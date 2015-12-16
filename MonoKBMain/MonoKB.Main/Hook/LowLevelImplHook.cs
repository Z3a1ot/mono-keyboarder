using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MonoKB.Main.Hook
{
    public abstract class LowLevelImplHook : IDisposable
    {
        protected static Dictionary<ushort, bool> m_hotkeys = new Dictionary<ushort, bool>();
        protected LowLevelProc m_proc;

        protected delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
        protected IntPtr m_hookID = IntPtr.Zero;
        protected Dictionary<KeyCode, KeyCode> m_map;

        protected LowLevelImplHook()
        {
            m_map = new Dictionary<KeyCode, KeyCode>();
            m_proc = HookCallBack;
        }

        /// <summary>
        /// Init function to set up the hook. Must be called prior working with the hook
        /// </summary>
        public void Init()
        {
            m_hookID = SetHook(m_proc);
        }

        protected abstract IntPtr SetHook(LowLevelProc proc);

        protected abstract IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Register hotkeys the application will listen to
        /// </summary>
        /// <param name="keyCodes">Key combination to listen to</param>
        /// <returns>Collection of keycodes the hook failed to register as hotkey</returns>
        public bool RegisterHotkeys(ushort keyCode)
        {
            if (!SupportedHotKeyCodes.Contains(keyCode)) 
                return false;
            m_hotkeys.Add(keyCode, false);
            return true;
        }

        /// <summary>
        /// Method to map the keys.
        /// </summary>
        /// <param name="from">Original Keyboard physical key</param>
        /// <param name="to">Logic key to be remapped to</param>
        /// <returns>Was the mapping successful</returns>
        public bool MapKey(KeyCode from, KeyCode to)
        {
            if (!SupportedKeyCodes.Contains(from) || !SupportedKeyCodes.Contains(to))
                return false;
            m_map.Add(from, to);
            return true;
        }

        /// <summary>
        /// Collection of keycodes the hook supports
        /// </summary>
        protected abstract KeyCode[] SupportedKeyCodes { get; }        
        
        /// <summary>
        /// Collection of keycodes the hook supports
        /// </summary>
        protected abstract ushort[] SupportedHotKeyCodes { get; }

        #region native methods
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        #endregion

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct INPUT
        {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        protected struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
        public void Dispose()
        {
            UnhookWindowsHookEx(m_hookID);
        }
    }
}