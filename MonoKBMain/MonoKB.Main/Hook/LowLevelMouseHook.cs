using System;
using System.Diagnostics;

namespace MonoKB.Main.Hook
{
    public class LowLevelMouseHook : LowLevelImplHook
    {
        private readonly KeyCode[] m_SupportedKeyCodes;
        private readonly ushort[] m_SupportedHotKeyCodes;
        private const int WH_MOUSE_LL = 14;

        public LowLevelMouseHook()
        {
            m_SupportedKeyCodes = new KeyCode[0];
            m_SupportedHotKeyCodes = (ushort[])Enum.GetValues(typeof(MouseHotKeyCode));
        }
        protected override IntPtr SetHook(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// Callback function which executes each type a mouse key is presed
        /// </summary>
        /// <param name="nCode">hook code, should pass to CallNextHookEx without further processing if less than 0</param>
        /// <param name="wParam">mouse message id</param>
        /// <param name="lParam">mouse input structure</param>
        /// <returns></returns>
        protected override IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool handled = false;
            if (nCode >= 0)
            {
                switch ((MouseHotKeyCode)wParam)
                {
                    case MouseHotKeyCode.WM_RBUTTONDOWN:
                    case MouseHotKeyCode.WM_MBUTTONDOWN:
                    case MouseHotKeyCode.WM_LBUTTONDOWN:
                        handled = HandleMouseKeyDown((ushort)wParam);
                        break;
                    case MouseHotKeyCode.WM_RBUTTONUP:
                    case MouseHotKeyCode.WM_MBUTTONUP:
                    case MouseHotKeyCode.WM_LBUTTONUP:
                        handled = HandleMouseKeyUp((ushort)wParam);
                        break;
                    default:
                        handled = false;
                        break;
                }

            }
            if (handled)
            {
                return (IntPtr)1;
            }
            return CallNextHookEx(m_hookID, nCode, wParam, lParam);
        }

        private bool HandleMouseKeyUp(ushort buttonCode)
        {
            if (m_hotkeys.ContainsKey((ushort)(buttonCode - 1))) // TODO: Remove the -1 and handle mouse codes properly
            {
                m_hotkeys[(ushort)(buttonCode - 1)] = false;
                return false;
            }
            return false;
        }

        private bool HandleMouseKeyDown(ushort buttonCode)
        {
            if (m_hotkeys.ContainsKey(buttonCode))
            {
                m_hotkeys[buttonCode] = true;
                return false;
            }
            return false;
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
            get
            {
                return m_SupportedHotKeyCodes;
            }
        }
    }
}