using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Threading;
using System.Diagnostics;
using System.IO.Ports;

namespace FapMonitor
{
    class Program
    {

        [DllImport("user32.dll")]
        static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        static void Main(string[] args)
        {
            Console.Title = "Fap Monitor";
            bool incognito;
            while (true)
            {
                var proc = Process.GetProcesses().OrderBy(x => x.ProcessName);
                incognito = false;

                foreach (Process prs in proc)
                {
                    if (!incognito)
                    {
                        incognito = (prs.ProcessName == "chrome" && WmiTest(prs.Id));
                    }
                    else {

                    }
                }
               LockDoor(incognito);
            }
        }

        //sample code for detecting chrome mode from here: http://stackoverflow.com/questions/14132142/using-c-sharp-to-close-google-chrome-incognito-windows-only 
        private static bool WmiTest(int processId)
        {
            try {
                using (ManagementObjectSearcher mos = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", processId)))
                    foreach (ManagementObject mo in mos.Get())
                        if (mo["CommandLine"].ToString().Contains("--disable-databases"))
                            return true;
            }
            catch (Exception ex) {

            }
            return false;
        }

        private static void LockDoor(bool lockIt)
        {
            using (SerialPort serialPort1 = new SerialPort())
            {
                serialPort1.PortName = "COM4"; //set the port name you see in arduino IDE
                serialPort1.BaudRate = 9600;   //set the Baud you see in arduino IDE

                serialPort1.Open();

                Thread.Sleep(5);
                if (serialPort1.IsOpen)
                {
                    Console.WriteLine(lockIt ? "l" : "u");
                    serialPort1.WriteLine(lockIt ? "u" : "l");
                    serialPort1.Close();
                }
            }
        }
    }
}
