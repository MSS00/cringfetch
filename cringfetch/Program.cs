﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.VisualBasic.Devices;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Diagnostics;

namespace cringfetch
{
    class Program
    {
        static void Main(string[] args)
        {
            SelectQuery Sq = new SelectQuery("Win32_Processor");
            ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher(Sq);
            ManagementObjectCollection osDetailsCollection = objOSDetails.Get();
            StringBuilder sb = new StringBuilder();
            var totalGBRam = Convert.ToInt32((new ComputerInfo().TotalPhysicalMemory / (Math.Pow(1024, 3))) + 0.5);
            var machine = System.Environment.MachineName;
            var cores = System.Environment.ProcessorCount;                   
            string user = System.Environment.UserName;
            var color = ConsoleColor.Cyan;                                                  // Here you can set your custom color theme
            bool arch = System.Environment.Is64BitOperatingSystem;
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = color; Console.Write("                           000000000    " + user);
            Console.ResetColor();
            Console.Write("@");
            Console.ForegroundColor = color; Console.Write(machine + "\n");
            Console.ResetColor();
            Console.ForegroundColor = color; Console.Write("               000 00000000000000000");
            Console.ResetColor();
            Console.WriteLine("    -------------");
            Console.ForegroundColor = color;
            Console.WriteLine("     0000000000000 00000000000000000");
            Console.WriteLine("     0000000000000 00000000000000000");
            Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    OS");
            Console.ResetColor();
            Console.WriteLine(": " + GetOSVersion());
            Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    Kernel");
            Console.ResetColor();
            Console.WriteLine(": NT " + GetNTVersion());
            switch (arch)
            {
                case(true):                                              
                    Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    Architecture");
                    Console.ResetColor();
                    Console.WriteLine(": 64-bit");
                    break;
                case(false):
                    Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    Architecture");        
                    Console.ResetColor();
                    Console.WriteLine(": 32-bit");
                    break;
            }
            foreach (ManagementObject mo in osDetailsCollection)
            {
                Console.ForegroundColor = color; Console.Write("                                        CPU");
                Console.ResetColor();
                Console.WriteLine(string.Format(": {0}", (string)mo["Name"]));
            }
            Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    CPU cores");    
            Console.ResetColor();
            Console.WriteLine(": " + cores);
            Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    RAM");
            Console.ResetColor();
            Console.WriteLine(": " + totalGBRam + " GB");
            Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    LAN IPv4");
            Console.ResetColor();
            Console.WriteLine(": " + GetLocalIPAddress());
            Console.ForegroundColor = color; Console.Write("     0000000000000 00000000000000000    Theme");
            Console.ResetColor();
            Console.WriteLine(": " + GetTheme());
            Process p = Process.GetCurrentProcess();
            PerformanceCounter parent = new PerformanceCounter("Process", "Creating Process ID", p.ProcessName);
            int ppid = (int)parent.NextValue();

            if (Process.GetProcessById(ppid).ProcessName == "powershell")
            {
                Console.ForegroundColor = color; Console.Write("               000 00000000000000000    Shell");
                Console.ResetColor();
                Console.WriteLine(": PowerShell");
            }
            else
            {
                Console.ForegroundColor = color; Console.Write("               000 00000000000000000    Shell");
                Console.ResetColor();
                Console.WriteLine(": cmd");
            }
            Console.ForegroundColor = color; Console.Write("                           000000000    GPU");
            Console.ResetColor();
            ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");

            foreach (ManagementObject obj in myVideoObject.Get())
            {
                Console.WriteLine(": " + obj["Name"]);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.Read(); //REMOVE AFTER FINISHING WORK GODDAMIT
        }
        
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No IPv4!");
        }
        public static string GetTheme()
        {
            string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes";
            string theme;
            theme = (string)Registry.GetValue(RegistryKey, "CurrentTheme", string.Empty);
            theme = theme.Split('\\').Last().Split('.').First().ToString();
            return theme;
        }
        public static string GetOSVersion()
        {
            string RegistryKey = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion";
            string osver;
            osver = (string)Registry.GetValue(RegistryKey, "ProductName", string.Empty);
            return osver;
        }
        public static string GetNTVersion()
        {
            string RegistryKey = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion";
            string osver;
            osver = (string)Registry.GetValue(RegistryKey, "CurrentVersion", string.Empty);
            return osver;
        }
    }
}