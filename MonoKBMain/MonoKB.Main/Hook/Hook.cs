using System.Linq;

namespace MonoKB.Main.Hook
{
    public class Hook : IHook
    {


        private LowLevelImplHook[] m_impls;

        public Hook()
        {
            m_impls = new LowLevelImplHook[] {new LowLevelKeyboardHook(), new LowLevelMouseHook()};
            foreach (LowLevelImplHook impl in m_impls)
            {
                impl.Init();
            }
        }

        public void Dispose()
        {
            foreach (LowLevelImplHook impl in m_impls)
            {
                impl.Dispose();
            }
        }

        public bool SetHotKey(KeyCode[] hotkeys)
        {
            KeyCode[] unregistered = hotkeys;
            foreach (LowLevelImplHook impl in m_impls)
            {
                unregistered = impl.RegisterHotkeys(unregistered);
                if (unregistered == null)
                {
                    return true;
                }
            }
            return unregistered == null;
        }

        public bool MapKey(KeyCode from, KeyCode to)
        {
            return m_impls.Any(impl => impl.MapKey(from, to));
        }
    }

}