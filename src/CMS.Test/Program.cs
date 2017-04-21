using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Test
{
    using CMS.Models;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin to init database...");
            //try
            {
                DbInitializer.Init();
                Console.WriteLine("Done");
            }
            //catch (Exception ex)
            {
                //   Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Q to exit.");
            while (Console.ReadLine() != "Q") { }
        }
    }
}
