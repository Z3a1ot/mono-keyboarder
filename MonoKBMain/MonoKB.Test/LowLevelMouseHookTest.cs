using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoKB.Main;
using MonoKB.Main.Hook;

namespace MonoKB.Test
{


    [TestClass]
    public class LowLevelMouseHookTest
    {

        protected static LowLevelImplHook m_hook;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            m_hook = new LowLevelMouseHook();
            m_hook.Init();
        }
        [ClassCleanup]
        public static void CleanUp()
        {
            m_hook.Dispose();
        }

        [TestMethod]
        public void TestHotkeyRegistration()
        {
            Assert.IsNotNull(m_hook);

            bool result = m_hook.RegisterHotkeys((ushort)MouseHotKeyCode.WM_LBUTTONDOWN);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestKeyMap()
        {
            Assert.IsNotNull(m_hook);

            bool result = false;
            result = m_hook.MapKey(KeyCode.KEY_0, KeyCode.KEY_2);
            Assert.IsFalse(result);
            
        }
    }
}
