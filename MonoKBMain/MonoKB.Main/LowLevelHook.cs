using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MonoKB.Main
{
    public class LowLevelHook : IHook
    {

        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private LowLevelKeyboardProc m_keyBoardProc;
        private LowLevelMouseProc m_mouseProc;
        private IntPtr m_keyBoardHookID = IntPtr.Zero;
        private IntPtr m_mouseHookID = IntPtr.Zero;
        private Dictionary<KeyCode, KeyCode> m_map;
        private Dictionary<KeyCode, bool> m_hotkeys;

//        private enum MouseMessages
//        {
//            WM_LBUTTONDOWN = 0x0201,
//            WM_LBUTTONUP = 0x0202,
//            WM_MOUSEMOVE = 0x0200,
//            WM_MOUSEWHEEL = 0x020A,
//            WM_RBUTTONDOWN = 0x0204,
//            WM_RBUTTONUP = 0x0205,
//            WM_MBUTTONDOWN = 0x0207,
//            WM_MBUTTONUP = 0x0208
//        }

        public LowLevelHook()
        {
            m_keyBoardProc = KeyBoardHookCallback;
            m_mouseProc = MouseHookCallback;
            m_keyBoardHookID = SetHook(m_keyBoardProc);
            m_mouseHookID = SetHook(m_mouseProc);
            m_map = new Dictionary<KeyCode, KeyCode>();
            m_hotkeys = new Dictionary<KeyCode, bool>();
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr KeyBoardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool handled = false;
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    KEYBDINPUT input = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
                    handled = HandleKeyDown(input);
                }
                else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                {
                    KEYBDINPUT input = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
                    handled = HandleKeyUp(input);
                }
            }
            if (handled)
            {
                return (IntPtr)1;
            }
            return CallNextHookEx(m_keyBoardHookID, nCode, wParam, lParam);
        }

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
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
            return CallNextHookEx(m_keyBoardHookID, nCode, wParam, lParam);
        }

        private bool HandleMouseKeyUp(KeyCode buttonCode)
        {
            if (m_hotkeys.ContainsKey(buttonCode-1)) // TODO: Remove the -1 and handle mouse codes properly
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

        private bool HandleKeyUp(KEYBDINPUT keybdinput)
        {
            KeyCode keyCode = (KeyCode)keybdinput.Vk;
            if (m_hotkeys.ContainsKey(keyCode))
            {
                m_hotkeys[keyCode] = false;
                return true;
            }
            return false;
        }

        private bool HandleKeyDown(KEYBDINPUT keybdinput)
        {
            KeyCode keyCode = (KeyCode)keybdinput.Vk;
            if (m_hotkeys.ContainsKey(keyCode))
            {
                m_hotkeys[keyCode] = true;
                return true;
            }
            if (!m_map.ContainsKey(keyCode) || m_hotkeys.ContainsValue(false))
            {
                //no remap for key, go on as intended
                return false;
            }
            keyCode = m_map[keyCode];
            INPUT[] inputs = BuildNewInput(keyCode);
            if (SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
                throw new Exception();
            return true;
        }

        private INPUT[] BuildNewInput(KeyCode keyCode)
        {
            //key down
            INPUT input = new INPUT
            {
                Type = 1,
                Data =
                {
                    Keyboard = new KEYBDINPUT()
                    {
                        Vk = (ushort) keyCode,
                        Scan = 0,
                        Flags = 0,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero,
                    }
                }
            };


            //key up
            INPUT input2 = new INPUT
            {
                Type = 1,
                Data =
                {
                    Keyboard = new KEYBDINPUT()
                    {
                        Vk = (ushort) keyCode,
                        Scan = 0,
                        Flags = 2,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            INPUT[] inputs = {input, input2};
            return inputs;
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(m_keyBoardHookID);
            UnhookWindowsHookEx(m_mouseHookID);
        }

        public bool SetHotKey(KeyCode[] hotkeys)
        {
            foreach (KeyCode keycode in hotkeys)
            {
                m_hotkeys.Add(keycode,false);
            }
            return true;
        }

        public bool MapKey(KeyCode from, KeyCode to)
        {
            m_map.Add(from,to);
            return true;
        }

        #region exports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        #endregion
    }
}