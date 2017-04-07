using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CheatEngine_Benchmarking
{

    class Program
    {
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        public static extern IntPtr OpenProcess(Int32 Access, Boolean InheritHandle, Int32 ProcessId);

        [DllImport("kernel32")]
        public static extern void CloseHandle(IntPtr hProcess);

        public const Int32 PROCESS_VM_READ = 0x10;
        public const Int32 PROCESS_VM_WRITE = 0x20;


        void BindToRunningProcesses()
        {
            // 현재 실행중인 프로세스를 가져옴
            Process currentProcess = Process.GetCurrentProcess();

            // 로컬 상에서 실행중인 모든 프로세스를 가져옴
            Process[] localAll = Process.GetProcesses();
            
            // 특정 이름으로 프로세스를 찾음
            Process[] localByName = Process.GetProcessesByName("notepad");

            Console.WriteLine("notepad process : {0}", localByName);

            // 특정 PID로 프로세스를 찾음
            Process localById = Process.GetProcessById(9600);

        }

        static void Main(string[] args)
        {
            //Program pro = new Program();
            //pro.BindToRunningProcesses();

        EnterPID:
            Int32 ProcessId;
            int procID = Process.GetProcessesByName("notepad")[0].Id;
            Console.Write("종료할 프로세스의 식별자(PID)를 입력하세요: ");
            try
            {
                ProcessId = Int32.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("다시 입력하세요");
                goto EnterPID;
            }

            IntPtr hProcess = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE, false, ProcessId);

            if (hProcess == IntPtr.Zero)
            {
                Console.WriteLine("프로세스 핸들 열기 실패!");
                return;
            }
            /*
            if (TerminateProcess(hProcess, 0) != 0)
            {
                Console.WriteLine("프로세스 종료 성공!");
                result = true;
            }
            else
            {
                Console.WriteLine("프로세스 종료 실패!");
            }
            */
            CloseHandle(hProcess);
            Console.ReadKey(true);
        }
    }
}
