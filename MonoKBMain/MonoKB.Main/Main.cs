using System.Windows.Forms;
using MonoKB.Main.Hook;

namespace MonoKB.Main
{
    class MonoKBMain
    {

        private static IHook m_hook;

        public static void Main()
        {
            using (m_hook = new Hook.Hook())
            {
                m_hook.RegisterHotKey(HotKeyCode.LCONTROL);
                m_hook.RegisterHotKey(MouseHotKeyCode.WM_MBUTTONDOWN);

                m_hook.MapKey(KeyCode.KEY_Q, KeyCode.KEY_Y);
                m_hook.MapKey(KeyCode.KEY_A, KeyCode.KEY_H);
                m_hook.MapKey(KeyCode.KEY_Z, KeyCode.KEY_N);
                m_hook.MapKey(KeyCode.KEY_W, KeyCode.KEY_U);
                m_hook.MapKey(KeyCode.KEY_S, KeyCode.KEY_J);
                m_hook.MapKey(KeyCode.KEY_X, KeyCode.KEY_M);
                Application.Run();
            };
        
        }


    }
}