using System.Linq;
using System.Windows.Forms;

namespace MonoKB.Main
{
    public class LowLevelHook : IHook
    {


        private LowLevelHookImpl[] m_impls;

        public LowLevelHook()
        {
            m_impls = new LowLevelHookImpl[] {new LowLevelKeyboardHook(), new LowLevelMouseHook()};
            foreach (LowLevelHookImpl impl in m_impls)
            {
                impl.Init();
            }
        }

        public void Dispose()
        {
            foreach (LowLevelHookImpl impl in m_impls)
            {
                impl.Dispose();
            }
        }

        public bool SetHotKey(KeyCode[] hotkeys)
        {
            KeyCode[] unregistered = hotkeys;
            foreach (LowLevelHookImpl impl in m_impls)
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