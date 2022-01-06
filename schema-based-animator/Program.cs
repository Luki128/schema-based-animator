using System;
using System.Linq;
using FFMpegCore;
using LewyDiagnostic;
using System.Drawing;


namespace schema_based_animator
{
    class Program
    {
        

        static void Main(string[] args)
        {
            

            dbg.Title("test", 60,'-', ConsoleColor.Blue);

            //   var t = FFmpeg.Conversions.New();
           
            FFMpeg.JoinImageSequence(@"joined_video.mp4", frameRate: 1,
                ImageInfo.FromPath(@"1.png"),
                ImageInfo.FromPath(@"2.png"),
                ImageInfo.FromPath(@"3.png")          
            );

            dbg.Title("done", 60, '-', ConsoleColor.Blue);

           
            //  t.SetFrameRate(2);
            //   var strem = Cr

        }

    }
}
