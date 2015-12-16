using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonoKB.Main.Hook
{
    public class LowLevelKeyboardHook : LowLevelImplHook
    {
        private readonly KeyCode[] m_SupportedKeyCodes;
        private readonly ushort[] m_SupportedHotKeyCodes;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        public LowLevelKeyboardHook() : base()
        {
            m_map = new Dictionary<KeyCode, KeyCode>();
            m_SupportedKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
            m_SupportedHotKeyCodes = (ushort[]) Enum.GetValues(typeof (HotKeyCode));
        }

        protected override IntPtr SetHook(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// Callback function which executes each type a key is presed
        /// </summary>
        /// <param name="nCode">hook code, should pass to CallNextHookEx without further processing if less than 0</param>
        /// <param name="wParam">keyboard message id</param>
        /// <param name="lParam">key input structure</param>
        /// <returns></returns>
        protected override IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool handled = false;
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    handled = HandleKey(lParam, down: true);
                }
                else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                {
                    handled = HandleKey(lParam, down: false);
                }
            }
            if (handled)
            {
                return (IntPtr)1;
            }
            return CallNextHookEx(m_hookID, nCode, wParam, lParam);
        }

        protected override KeyCode[] SupportedKeyCodes
        {
            get
            {
                return m_SupportedKeyCodes;
            }
        }

        protected override ushort[] SupportedHotKeyCodes
        {
            get { return m_SupportedHotKeyCodes; }
        }

        private bool HandleKey(IntPtr lParam, bool down)
        {
            KEYBDINPUT keybdinput = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
            if (m_hotkeys.ContainsKey(keybdinput.Vk))
            {
                m_hotkeys[keybdinput.Vk] = down;
                return true;
            }
            KeyCode keyCode = (KeyCode)keybdinput.Vk;
            if (!m_map.ContainsKey(keyCode) || m_hotkeys.ContainsValue(false))
            {
                //no remap for key, go on as intended
                return false;
            }
            keyCode = m_map[keyCode];
            INPUT[] inputs = {BuildNewInput(keyCode, down)};
            if (SendInput((uint) inputs.Length, inputs, Marshal.SizeOf(typeof (INPUT))) == 0)
                return false;
            return true;
        }

        private INPUT BuildNewInput(KeyCode keyCode, bool down)
        {
            INPUT input = new INPUT
            {
                Type = 1,
                Data =
                {
                    Keyboard = new KEYBDINPUT()
                    {
                        Vk = (ushort)keyCode,
                        Scan = 0,
                        Flags = down ? 0u : 2u,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero,
                    }
                }
            };

            return input;
        }

    }
}