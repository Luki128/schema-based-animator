using System;
using System.Collections.Generic;
using System.Text;
using FFMpegCore;
using LewyDiagnostic;
using System.Drawing;
using System.IO;


namespace schema_based_animator
{
    public class Canvas
    {
        List<Clip> clips;
        int width ;
        int heiht;
        int frames;
        int FramesPreSecond = 30;

        public const string ProcessingTempFolder = "Tmp";

        Image getCanvasAt(int x)
        {

            return Image.FromFile("1.png");
        }
        void saveAsVideo(string path)
        {
            if (!Directory.Exists(ProcessingTempFolder))
            {
                dbg.Wraning($"Foder {ProcessingTempFolder} not exisitng, so is auto created");
                Directory.CreateDirectory(ProcessingTempFolder);
            }
            ImageInfo[] imageInfos = new ImageInfo[frames];

            for (int i = 0; i < frames; i++)
            {
                Image img = getCanvasAt(i);
                string tmpImagePath = $"{ProcessingTempFolder}/frame_{i}.png";
                img.Save(tmpImagePath, System.Drawing.Imaging.ImageFormat.Png);
                imageInfos[i] = ImageInfo.FromPath(tmpImagePath);
            }

            FFMpeg.JoinImageSequence(path, frameRate: FramesPreSecond,
                imageInfos
            );
        }
    }
}
