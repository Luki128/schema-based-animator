using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace schema_based_animator
{
    class Interpreter
    {
        Canvas currentCanvas = null;
        Clip currentClip = null;

        void InitStage2()
        {

        }

        void InitStage3()
        {

        }

        public void canvas(int width, int height, int frames)
        {
            currentCanvas = new Canvas()
            {
                width = width,
                height = height,
                frames = frames,
            };
            InitStage2();
        }

        public void clip(string path, int origin_x, int origin_y)
        {
            currentClip = new Clip()
            {
                origin_x = origin_x,
                origin_y = origin_y,
            };
            currentClip.LoadImage(path);
            currentCanvas.clips.Add(currentClip);
            InitStage2();
        }

        public void position(int frame, int x, int y)
        {
            Position pos = new Position()
            {
                x = x,
                y = y,
            };

            Command<Position> com = new Command<Position>()
            {
                value = pos,
                frame = frame,
            };

            currentClip.position.addCommand(com);
        }

        public void place(int frame, float x, float y)
        {
            Position pos = new Position()
            {
                x = x * currentCanvas.width,
                y = y * currentCanvas.height,
            };

            Command<Position> com = new Command<Position>()
            {
                value = pos,
                frame = frame,
            };

            currentClip.position.addCommand(com);
        }

        public void move(int frame, int x, int y)
        {
            Command<Position> last = currentClip.position.commands.LastOrDefault();
            if (last is null) return;
            Position pos = new Position()
            {
                x = x + last.value.x,
                y = y + last.value.y,
            };

            Command<Position> com = new Command<Position>()
            {
                value = pos,
                frame = frame,
            };

            currentClip.position.addCommand(com);
        }

        public void shift(int frame, float x, float y)
        {
            Command<Position> last = currentClip.position.commands.LastOrDefault();
            if (last is null) return;
            Position pos = new Position()
            {
                x = x * currentCanvas.width + last.value.x,
                y = y * currentCanvas.height + last.value.y,
            };

            Command<Position> com = new Command<Position>()
            {
                value = pos,
                frame = frame,
            };

            currentClip.position.addCommand(com);
        }

        public void rotation(int frame, int angle)
        {
            Rotation rot = new Rotation()
            {
                angle = angle
            };

            Command<Rotation> com = new Command<Rotation>()
            {
                value = rot,
                frame = frame,
            };

            currentClip.rotation.addCommand(com);
        }

        public void rotate(int frame, float angle)
        {
            Command<Rotation> last = currentClip.rotation.commands.LastOrDefault();
            if (last is null) return;
            Rotation rot = new Rotation()
            {
                angle = angle + last.value.angle
            };

            Command<Rotation> com = new Command<Rotation>()
            {
                value = rot,
                frame = frame,
            };

            currentClip.rotation.addCommand(com);
        }

        public void scale(int frame, float scale)
        {
            Scale scal = new Scale()
            {
                scale = scale
            };

            Command<Scale> com = new Command<Scale>()
            {
                value = scal,
                frame = frame,
            };

            currentClip.scale.addCommand(com);
        }

        public void rescale(int frame, float scale)
        {
            Command<Scale> last = currentClip.scale.commands.LastOrDefault();
            if (last is null) return;
            Scale scal = new Scale()
            {
                scale = scale * last.value.scale
            };

            Command<Scale> com = new Command<Scale>()
            {
                value = scal,
                frame = frame,
            };

            currentClip.scale.addCommand(com);
        }
    }
}
