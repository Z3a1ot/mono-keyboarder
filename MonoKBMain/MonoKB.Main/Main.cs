using System.Windows.Forms;

namespace MonoKB.Main
{
    class MonoKBMain
    {

        private static IHook m_hook;

        public static void Main()
        {
            using (m_hook = new LowLevelHook())
            {
                m_hook.SetHotKey(new KeyCode[] {KeyCode.LCONTROL, KeyCode.LSHIFT});
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