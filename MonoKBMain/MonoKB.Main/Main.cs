using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class MonoKBMain
{

    private static IHook m_hook;

    public static void Main()
    {
        using (m_hook = new LowLevelHook())
        {
            Application.Run();
        };
        
    }


}