using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MonoKB.Main
{
    public class LowLevelHook : IHook
    {

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private LowLevelKeyboardProc m_proc;
        private IntPtr m_hookID = IntPtr.Zero;
        private KeyCode[] m_hotkey;
        private Dictionary<KeyCode, KeyCode> m_map;

        public LowLevelHook()
        {
            m_proc = HookCallback;
            m_hookID = SetHook(m_proc);
            m_map = new Dictionary<KeyCode, KeyCode>();
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

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                KeyCode keyCode = (KeyCode)vkCode;

                if (!m_map.ContainsKey(keyCode))
                {
                    //no remap for key, go on as intended
                    return CallNextHookEx(m_hookID, nCode, wParam, lParam);
                }
                m_map.TryGetValue(keyCode, out keyCode);

                INPUT[] inputs = BuildNewInput(keyCode);
                if (SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
                    throw new Exception();
                Console.WriteLine((Keys)keyCode);
            }
            //absorb key
            return (IntPtr) 1;
        }

        private INPUT[] BuildNewInput(KeyCode keyCode)
        {
            //key down
            INPUT input = new INPUT
            {
                Type = 1
            };

            input.Data.Keyboard = new KEYBDINPUT()
            {
                Vk = (ushort) keyCode,
                Scan = 0,
                Flags = 0,
                Time = 0,
                ExtraInfo = IntPtr.Zero,
            };

            //key up
            INPUT input2 = new INPUT
            {
                Type = 1
            };
            input2.Data.Keyboard = new KEYBDINPUT()
            {
                Vk = (ushort) keyCode,
                Scan = 0,
                Flags = 2,
                Time = 0,
                ExtraInfo = IntPtr.Zero
            };

            INPUT[] inputs = {input, input2};
            return inputs;
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(m_hookID);
        }

        public bool SetHotKey(KeyCode[] hotkey)
        {
            m_hotkey = hotkey;
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