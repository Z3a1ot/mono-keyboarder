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

            bool result = m_hook.RegisterHotkeys((ushort)HotKeyCode.LSHIFT);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestKeyMap()
        {
            Assert.IsNotNull(m_hook);

            bool result = false;
            result = m_hook.MapKey(KeyCode.KEY_A, KeyCode.KEY_B);
            Assert.AreEqual(true, result);



        }
    }
}
