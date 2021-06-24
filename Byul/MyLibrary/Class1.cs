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
        [DllImport("shlifedev.dll", CallingConvention = CallingConvention.FastCall)]
        public extern static void Initialize(IntPtr hWnd, float tick);

        [DllImport("shlifedev.dll")]
        public extern static void InstallHIDDriver();


    }
}
