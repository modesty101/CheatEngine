using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

namespace CheatEngine_Benchmarking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Edit Process Name : ");
            string procName = Console.ReadLine();

            Process[] p = Process.GetProcessesByName(procName);

            uint DELETE = 0x00010000;
            uint READ_CONTROL = 0x00020000;
            uint WRITE_DAC = 0x00040000;
            uint WRITE_OWNER = 0x00080000;
            uint SYNCHRONIZE = 0x00100000;
            uint END = 0xFFF; //if you have WinXP or Windows Server 2003 you must change this to 0xFFFF
            uint PROCESS_ALL_ACCESS = (DELETE |
                       READ_CONTROL |
                       WRITE_DAC |
                       WRITE_OWNER |
                       SYNCHRONIZE |
                       END
                     );

            int processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, p[0].Id);

            Console.Write("Changing Value : ");
            string procSize = Console.ReadLine();
            int processSize = GetObjectSize(procSize);

            // Here.. 
            int OFFSET = 0x001CAB38;

            Console.WriteLine((Encoding.Unicode.GetString(ReadMemory(OFFSET, processSize, processHandle))));

            Console.Write("Changed Value : ");
            string text = Console.ReadLine();
            // 문자열 잘림 현상
            WriteMemory(OFFSET, Encoding.Unicode.GetBytes(text), processHandle);
        }

        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);

        public static byte[] ReadMemory(int adress, int processSize, int processHandle)
        {
            byte[] buffer = new byte[processSize];
            ReadProcessMemory(processHandle, adress, buffer, processSize, 0);
            return buffer;
        }

        public static void WriteMemory(int adress, byte[] processBytes, int processHandle)
        {
            WriteProcessMemory(processHandle, adress, processBytes, processBytes.Length, 0);
        }


        public static int GetObjectSize(object TestObject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, TestObject);
            Array = ms.ToArray();
            return Array.Length;
        }
    }
}
