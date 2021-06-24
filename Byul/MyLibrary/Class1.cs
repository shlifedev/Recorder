using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    public static class Caller
    {
        /// <summary>
        /// Driver Load From My Custom C++ Library
        /// 0 = Loaded
        /// -1 = Not Installed
        /// -2 = Unknown, Please Reboot or Window Update Latest
        /// </summary>
        /// <param name="hWnd">Your Program Handle</param>
        /// <param name="tick">Caller Update Thread Tick, Recommand 0.05f ~ 0.1f</param>
        [DllImport("shlifedev.dll", CallingConvention = CallingConvention.FastCall)]
        public extern static int Initialize(IntPtr hWnd, float tick);

        /// <summary>
        /// Require PC Reboot
        /// -3 Unknown, Please Update Window Latest
        /// -2 = Already Installed
        /// -1 = Install Failed
        /// 0 = Install Succesfully 
        /// </summary>
        [DllImport("shlifedev.dll")]
        public extern static int InstallHIDDriver();


    }
}
