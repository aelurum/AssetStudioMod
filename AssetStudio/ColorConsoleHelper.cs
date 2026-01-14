// Based on code by tomzorz (https://gist.github.com/tomzorz/6142d69852f831fb5393654c90a1f22e)
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AssetStudio
{
    internal static class ColorConsoleHelper
    {
        public static readonly bool isAnsiCodesSupported;
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        static ColorConsoleHelper()
        {
            var isWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWin)
            {
                isAnsiCodesSupported = TryEnableVTMode();

                if (!isAnsiCodesSupported && IsTerminal)
                {
                    isAnsiCodesSupported = true;
                }
                else if (IsWine)
                {
                    isAnsiCodesSupported = false;
                }
            }
            else
            {
                isAnsiCodesSupported = true;
            }
        }

        // Enable support for ANSI escape codes
        // (but probably only suitable for windows 10+)
        // https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
        private static bool TryEnableVTMode()
        {
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                return false;
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;

            return SetConsoleMode(iStdOut, outConsoleMode);
        }

        //Check for bash terminal emulator. E.g., Git Bash, Cmder
        private static bool IsTerminal => Environment.GetEnvironmentVariable("TERM") != null;

        private static bool IsWine => Process.GetProcessesByName("winlogon").Length == 0;
    }
}
