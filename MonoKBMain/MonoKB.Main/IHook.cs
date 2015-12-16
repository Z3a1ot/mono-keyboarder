using System;

namespace MonoKB.Main
{
    public interface IHook : IDisposable
    {
        /// <summary>
        /// Register keyboard hotkey the application will listen to
        /// </summary>
        /// <param name="hotkey">Key to listen to</param>
        /// <returns></returns>
        bool RegisterHotKey(HotKeyCode hotkey);

        /// <summary>
        /// Register mouse hotkey the application will listen to
        /// </summary>
        /// <param name="hotkey">Key to listen to</param>
        /// <returns></returns>
        bool RegisterHotKey(MouseHotKeyCode hotkey);

        /// <summary>
        /// Method to map the keys.
        /// </summary>
        /// <param name="from">Original Keyboard physical key</param>
        /// <param name="to">Logic key to be remapped to</param>
        /// <returns></returns>
        bool MapKey(KeyCode from, KeyCode to);
    }
}