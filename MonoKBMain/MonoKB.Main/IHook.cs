using System;

namespace MonoKB.Main
{
    public interface IHook : IDisposable
    {
        bool SetHotKey(KeyCode[] hotkey);

        bool MapKey(KeyCode from, KeyCode to);
    }
}