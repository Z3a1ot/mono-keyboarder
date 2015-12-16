using System;
using System.Linq;

namespace MonoKB.Main.Hook
{
    public class Hook : IHook
    {


        private readonly LowLevelImplHook[] m_impls;

        public Hook()
        {
            m_impls = new LowLevelImplHook[] {new LowLevelKeyboardHook(), new LowLevelMouseHook()};
            foreach (var impl in m_impls)
            {
                impl.Init();
            }
        }

        public void Dispose()
        {
            foreach (var impl in m_impls)
            {
                impl.Dispose();
            }
        }



        public bool RegisterHotKey(HotKeyCode hotkey)
        {
            return RegisterHotKey((ushort)hotkey);
        }

        public bool RegisterHotKey(MouseHotKeyCode hotkey)
        {
            return RegisterHotKey((ushort)hotkey);
        }

        private bool RegisterHotKey(ushort hotkey)
        {
            return m_impls.Any(impl => impl.RegisterHotkeys(hotkey));
        }

        public bool MapKey(KeyCode from, KeyCode to)
        {
            return m_impls.Any(impl => impl.MapKey(from, to));
        }
    }

}