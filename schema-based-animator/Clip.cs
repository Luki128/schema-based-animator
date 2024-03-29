﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;

namespace schema_based_animator
{
    public class Clip
    {
        public CommandSequence<Position> position = new CommandSequence<Position>();
        public CommandSequence<Rotation> rotation = new CommandSequence<Rotation>();
        public CommandSequence<Scale> scale = new CommandSequence<Scale>();
        public float origin_x = 0.0f;
        public float origin_y = 0.0f;
        public Bitmap source;

        private Semaphore _pool = new Semaphore(1,1);

        public void LoadImage(string str)
        {
            source = new Bitmap(str);
        }

        public Clip()
        {
            position.addCommand(new Command<Position> { frame = -1, value = new Position() });
            rotation.addCommand(new Command<Rotation> { frame = -1, value = new Rotation() });
            scale.addCommand(new Command<Scale> { frame = -1, value = new Scale() });
        }

        public Bitmap getClipAtFrame(int frame, int width, int height)
        {
            Position pos = position.getTransformAtFrame(frame);
            Rotation rot = rotation.getTransformAtFrame(frame);
            Scale scal = scale.getTransformAtFrame(frame);

            float sin = (float)Math.Sin(rot.angle);
            float cos = (float)Math.Cos(rot.angle);

            Bitmap result = new Bitmap(width, height);

           _pool.WaitOne();
            using (var copy = new Bitmap(source))
            {
               _pool.Release();
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        float x1 = x;
                        float y1 = y;

                        x1 -= pos.x;
                        y1 -= pos.y;

                        x1 /= scal.global_x;
                        y1 /= scal.global_y;

                        float x2 = (x1 * cos - y1 * sin) / scal.local_x;
                        float y2 = (x1 * sin + y1 * cos) / scal.local_y;

                        x2 += origin_x;
                        y2 += origin_y;

                        if (x2 < 0 || x2 >= copy.Width || y2 < 0 || y2 >= copy.Height)
                        {
                            result.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                            continue;
                        }

                        result.SetPixel(x, y, copy.GetPixel((int)x2, (int)y2));
                    }
             }

          /*  for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    float x1 = x;
                    float y1 = y;

                    x1 -= pos.x;
                    y1 -= pos.y;

                    float x2 = (x1 * cos - y1 * sin) / scal.scale;
                    float y2 = (x1 * sin + y1 * cos) / scal.scale;

                    x2 += origin_x;
                    y2 += origin_y;

                    if (x2 < 0 || x2 >= source.Width || y2 < 0 || y2 >= source.Height)
                    {
                        result.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                        continue;
                    }

                    result.SetPixel(x, y, source.GetPixel((int)x2, (int)y2));
                }*/
            return result;
        }
    }
}
