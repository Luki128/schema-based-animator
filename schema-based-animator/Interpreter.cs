using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LewyDiagnostic;
using System.Reflection;

namespace schema_based_animator
{
    public class Interpereter
    {
        Canvas currentCanvas = null;
        Clip currentClip = null;

        public Dictionary<string, MethodBase> commandBase = new Dictionary<string, MethodBase>();
        public Interpereter()
        {
            addCommand("canvas");
        }
        void ExeCommand(int line, MethodBase f, params string[] args)// arg 0 skip is name function
        {
            float fRef = 0;
            int iRef = 0;
            string sRef = "";

            var p = f.GetParameters();
            if (p.Length != (args.Length - 1))
            {
                dbg.Error($"[Line {line}]Incorrect number of agruments for {f.Name} expected {p.Length} recived {args.Length - 1}");
                return;
            }
            object[] Pasarg = new object[p.Length];

            for (int i = 0; i < p.Length; i++)
            {
                //Console.WriteLine($"{args[i].GetType()} - {p[i].ParameterType}");
                var t = p[i].ParameterType;

                bool sucess = false;
                if (fRef.GetType() == t)
                {
                    sucess = float.TryParse(args[i + 1], out fRef);
                    Pasarg[i] = fRef;
                }
                else if (iRef.GetType() == t)
                {
                    sucess = int.TryParse(args[i + 1], out iRef);
                    Pasarg[i] = iRef;
                }
                else if (sRef.GetType() == t)
                {
                    Pasarg[i] = args[i + 1];
                    sucess = true;
                }
                if (!sucess)
                {
                    dbg.Error($"[Line {line}] Incorrect type of agrument nr:{i + 1} for {f.Name} expected argument type: {p[i].ParameterType.Name} ");
                    return;
                }
            }
            f.Invoke(this, Pasarg);
        }

        void addCommand(string name)
        {
            if (commandBase.ContainsKey(name)) return;
            MethodBase Mymethodbase = this.GetType().GetMethod(name);
            commandBase.Add(name, Mymethodbase);
        }

        public void RunScript(string name)
        {
            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(name);
            }
            catch (Exception e)
            {
                dbg.Error($"Cannot open the script file:{e.Message}");
                // throw;
                return;
            }
            int line = 1;
            foreach (var item in lines)
            {
                string[] arg = item.Split(' ');
                if (commandBase.ContainsKey(arg[0]))
                {
                    ExeCommand(line, commandBase[arg[0]], arg);
                }
                else
                {
                    dbg.Error($"[Line {line}]unknown or inaccessible command {arg[0]}");
                }
                line++;
            }
            dbg.ClassicRaport();
        }
        void InitStage2()
        {
            addCommand("clip");
        }
        void InitStage3()
        {
            addCommand("position");
            addCommand("place");
            addCommand("move");
            addCommand("shift");
            addCommand("rotation");
            addCommand("rotate");
            addCommand("scale");
            addCommand("rescale");
            addCommand("video");
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
            InitStage3();
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

        public void rotation(int frame, float angle)
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

        public void scale(int frame, float local_x, float local_y, float global_x, float global_y)
        {
            Scale scal = new Scale()
            {
                local_x = local_x,
                local_y = local_y,
                global_x = global_x,
                global_y = global_y,
            };

            Command<Scale> com = new Command<Scale>()
            {
                value = scal,
                frame = frame,
            };

            currentClip.scale.addCommand(com);
        }

        public void rescale(int frame, float local_x, float local_y, float global_x, float global_y)
        {
            Command<Scale> last = currentClip.scale.commands.LastOrDefault();
            if (last is null) return;
            Scale scal = new Scale()
            {
                local_x = local_x* last.value.local_x,
                local_y = local_y * last.value.local_y,
                global_x = global_x * last.value.global_x,
                global_y = global_y * last.value.global_y,
            };

            Command<Scale> com = new Command<Scale>()
            {
                value = scal,
                frame = frame,
            };

            currentClip.scale.addCommand(com);
        }
    
        public void video(string name)
        {
            currentCanvas.saveAsVideo(name);
        }
    }
}
