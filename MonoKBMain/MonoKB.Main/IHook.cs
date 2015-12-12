using System;

namespace MonoKB.Main
{
    public interface IHook : IDisposable
    {
        /// <summary>
        /// Register hotkeys the application will listen to
        /// </summary>
        /// <param name="hotkeys">Key combination to listen to</param>
        /// <returns></returns>
        bool SetHotKey(KeyCode[] hotkeys);

        /// <summary>
        /// Method to map the keys.
        /// </summary>
        /// <param name="from">Original Keyboard physical key</param>
        /// <param name="to">Logic key to be remapped to</param>
        /// <returns></returns>
        bool MapKey(KeyCode from, KeyCode to);
    }
}