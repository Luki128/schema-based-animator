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
        public List<Clip> clips = new List<Clip>();
        public int width = 500;
        public int height = 500;
        public int frames = 30;
        public int FramesPreSecond = 30;

        public const string ProcessingTempFolder = "Tmp";

        Image getCanvasAt(int frames)
        {
            Bitmap canvasFrame = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(canvasFrame))
            {
                foreach (var item in clips)
                {
                    gr.DrawImage(item.getClipAtFrame(frames, width, height), Point.Empty);
                }
            }    
            return canvasFrame;
        }
       public void saveAsVideo(string path)
        {
            if (!Directory.Exists(ProcessingTempFolder))
            {
                dbg.Wraning($"Folder {ProcessingTempFolder} not exisitng, so is auto created");
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
            for (int i = 0; i < frames; i++)
            {
                string tmpImagePath = $"{ProcessingTempFolder}/frame_{i}.png";
                File.Delete(tmpImagePath);
            }
        }
    }
}
