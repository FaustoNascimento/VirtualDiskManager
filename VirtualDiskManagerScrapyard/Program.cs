using System;
using VirtualDiskManager;

namespace VirtualDiskManagerScrapyard
{
    class Program
    {
        static void Main(string[] args)
        {
            var iso = VirtualHardDisk2.Open("D:\\Test.vhd");
            iso.Attach();
            iso.Attach();
            Console.WriteLine(iso);
            iso.Detach();
        }
    }
}
