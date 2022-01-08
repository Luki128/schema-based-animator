using System;
using System.Linq;
using FFMpegCore;
using System.Collections.Generic;
using LewyDiagnostic;

namespace schema_based_animator
{
    class Program
    {     

        static void Main(string[] args)
        {
            while (true)
            {
                dbg.Info("Rendering...");
                Interpereter interpereter = new Interpereter();
                interpereter.RunScript("test.txt");
                dbg.Succes("Done.");
                GC.Collect();
                Console.ReadKey();
               

            }
          //  interpereter.video("test.mp4");
        }

    }
}
