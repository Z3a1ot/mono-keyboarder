using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoKB.Main;
using MonoKB.Main.Hook;

namespace MonoKB.Test
{


    [TestClass]
    public class LowLevelKeyBoardHookTest
    {
        
        protected static LowLevelImplHook m_hook;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            m_hook = new LowLevelKeyboardHook();
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

            KeyCode[] codes = new KeyCode[] { KeyCode.WM_LBUTTONDOWN, KeyCode.LSHIFT };
            KeyCode[] unsupported = m_hook.RegisterHotkeys(codes);

            Assert.AreEqual(1, unsupported.Length);
            Assert.AreEqual(KeyCode.WM_LBUTTONDOWN, unsupported[0]);
        }

        [TestMethod]
        public void TestKeyMap()
        {
            Assert.IsNotNull(m_hook);

            bool result = false;
            result = m_hook.MapKey(KeyCode.KEY_A, KeyCode.KEY_B);
            Assert.AreEqual(true, result);

            result = m_hook.MapKey(KeyCode.WM_MBUTTONDOWN, KeyCode.KEY_B);
            Assert.AreEqual(false, result);


        }
    }
}
