using System;
using System.Diagnostics;

namespace MonoKB.Main.Hook
{
    public class LowLevelMouseHook : LowLevelImplHook
    {
        private readonly KeyCode[] m_supportedCodes;
        private const int WH_MOUSE_LL = 14;

        public LowLevelMouseHook()
        {
            m_supportedCodes = (KeyCode[])Enum.GetValues(typeof(ValidKeyCodes));
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
                if (wParam == (IntPtr)KeyCode.WM_RBUTTONDOWN)
                {
                    handled = HandleMouseKeyDown(KeyCode.WM_RBUTTONDOWN);
                }
                else if (wParam == (IntPtr)KeyCode.WM_MBUTTONDOWN)
                {
                    handled = HandleMouseKeyDown(KeyCode.WM_MBUTTONDOWN);
                }
                else if (wParam == (IntPtr)KeyCode.WM_LBUTTONDOWN)
                {
                    handled = HandleMouseKeyDown(KeyCode.WM_LBUTTONDOWN);
                }
                else if (wParam == (IntPtr)KeyCode.WM_RBUTTONUP)
                {
                    handled = HandleMouseKeyUp(KeyCode.WM_RBUTTONUP);
                }
                else if (wParam == (IntPtr)KeyCode.WM_LBUTTONDOWN)
                {
                    handled = HandleMouseKeyUp(KeyCode.WM_LBUTTONUP);
                }
                else if (wParam == (IntPtr)KeyCode.WM_MBUTTONDOWN)
                {
                    handled = HandleMouseKeyUp(KeyCode.WM_MBUTTONUP);
                }
            }
            if (handled)
            {
                return (IntPtr)1;
            }
            return CallNextHookEx(m_hookID, nCode, wParam, lParam);
        }
        
        private bool HandleMouseKeyUp(KeyCode buttonCode)
        {
            if (m_hotkeys.ContainsKey(buttonCode - 1)) // TODO: Remove the -1 and handle mouse codes properly
            {
                m_hotkeys[buttonCode] = false;
                return false;
            }
            return false;
        }

        private bool HandleMouseKeyDown(KeyCode buttonCode)
        {
            if (m_hotkeys.ContainsKey(buttonCode))
            {
                m_hotkeys[buttonCode] = true;
                return false;
            }
            return false;
        }

        protected override KeyCode[] SupportedCodes
        {
            get
            {
                return m_supportedCodes;
            }
        }

        private enum ValidKeyCodes : ushort
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208
        }
    }
}