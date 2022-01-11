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
            string scName = "test.txt";
            if(args.length > 0)scName = args[0];
            while (true)
            {
                dbg.Info("Rendering...");
                Interpereter interpereter = new Interpereter();
                interpereter.RunScript(scName);
                dbg.Succes("Done.");
                GC.Collect();
                Console.ReadKey();
               
            }
        }

    }
}
