using System;
using System.Linq;
using FFMpegCore;
using System.Collections.Generic;

namespace schema_based_animator
{
    class Program
    {     

        static void Main(string[] args)
        {
            Interpereter interpereter = new Interpereter();
            interpereter.RunScript("test.txt");
            interpereter.video("test.mp4");
        }

    }
}
