using System;
using System.Runtime.InteropServices;

namespace MonoKB.Main
{


    public enum HotKeyCode : ushort
    {
        /// <summary>
        /// Left control
        /// </summary>
        LCONTROL = 0xa2,

        /// <summary>
        /// Left shift
        /// </summary>
        LSHIFT = 160,

        /// <summary>
        /// left windows key
        /// </summary>
        LWIN = 0x5b,

        /// <summary>
        /// Right shift
        /// </summary>
        RSHIFT = 0xa1,

        /// <summary>
        /// Right windows key
        /// </summary>
        RWIN = 0x5c,

        /// <summary>
        /// Right control
        /// </summary>
        RCONTROL = 0xa3,

    }

    public enum MouseHotKeyCode : ushort
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
    }

    public enum KeyCode : ushort
    {
      
        #region math

        /// <summary>Key "+"</summary>
        ADD = 0x6b,
        /// <summary>
        /// "*" key
        /// </summary>
        MULTIPLY = 0x6a,

        /// <summary>
        /// "/" key
        /// </summary>
        DIVIDE = 0x6f,

        /// <summary>
        /// Subtract key "-"
        /// </summary>
        SUBTRACT = 0x6d,

        #endregion

        #region KEYS

        /// <summary>
        /// 
        /// </summary>
        KEY_0 = 0x30,
        /// <summary>
        /// 
        /// </summary>
        KEY_1 = 0x31,
        /// <summary>
        /// 
        /// </summary>
        KEY_2 = 50,
        /// <summary>
        /// 
        /// </summary>
        KEY_3 = 0x33,
        /// <summary>
        /// 
        /// </summary>
        KEY_4 = 0x34,
        /// <summary>
        /// 
        /// </summary>
        KEY_5 = 0x35,
        /// <summary>
        /// 
        /// </summary>
        KEY_6 = 0x36,
        /// <summary>
        /// 
        /// </summary>
        KEY_7 = 0x37,
        /// <summary>
        /// 
        /// </summary>
        KEY_8 = 0x38,
        /// <summary>
        /// 
        /// </summary>
        KEY_9 = 0x39,
        /// <summary>
        /// 
        /// </summary>
        KEY_A = 0x41,
        /// <summary>
        /// 
        /// </summary>
        KEY_B = 0x42,
        /// <summary>
        /// 
        /// </summary>
        KEY_C = 0x43,
        /// <summary>
        /// 
        /// </summary>
        KEY_D = 0x44,
        /// <summary>
        /// 
        /// </summary>
        KEY_E = 0x45,
        /// <summary>
        /// 
        /// </summary>
        KEY_F = 70,
        /// <summary>
        /// 
        /// </summary>
        KEY_G = 0x47,
        /// <summary>
        /// 
        /// </summary>
        KEY_H = 0x48,
        /// <summary>
        /// 
        /// </summary>
        KEY_I = 0x49,
        /// <summary>
        /// 
        /// </summary>
        KEY_J = 0x4a,
        /// <summary>
        /// 
        /// </summary>
        KEY_K = 0x4b,
        /// <summary>
        /// 
        /// </summary>
        KEY_L = 0x4c,
        /// <summary>
        /// 
        /// </summary>
        KEY_M = 0x4d,
        /// <summary>
        /// 
        /// </summary>
        KEY_N = 0x4e,
        /// <summary>
        /// 
        /// </summary>
        KEY_O = 0x4f,
        /// <summary>
        /// 
        /// </summary>
        KEY_P = 80,
        /// <summary>
        /// 
        /// </summary>
        KEY_Q = 0x51,
        /// <summary>
        /// 
        /// </summary>
        KEY_R = 0x52,
        /// <summary>
        /// 
        /// </summary>
        KEY_S = 0x53,
        /// <summary>
        /// 
        /// </summary>
        KEY_T = 0x54,
        /// <summary>
        /// 
        /// </summary>
        KEY_U = 0x55,
        /// <summary>
        /// 
        /// </summary>
        KEY_V = 0x56,
        /// <summary>
        /// 
        /// </summary>
        KEY_W = 0x57,
        /// <summary>
        /// 
        /// </summary>
        KEY_X = 0x58,
        /// <summary>
        /// 
        /// </summary>
        KEY_Y = 0x59,
        /// <summary>
        /// 
        /// </summary>
        KEY_Z = 90,

        #endregion

        /// <summary>
        /// "." key
        /// </summary>
        DECIMAL = 110,

        /// <summary>
        /// Return key
        /// </summary>
        ENTER = 13,

        /// <summary>
        /// Go Back or delete
        /// </summary>
        BACKSPACE = 8,

    }
}