using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LewyDiagnostic;
using System.Reflection;

namespace schema_based_animator
{
    public class AbstractInterpereter
    {
        struct loopInfo
        {
            public int startLine;
            public int times;
            public loopInfo(int line,int t)
            {
                startLine = line;
                times = t;
            }
        }

        private Dictionary<string, string> variable = new Dictionary<string, string>();
        private CommandEngine cmd = null;
        private Stack<loopInfo> reaptStack = new Stack<loopInfo>();
        private int currentLine = 1;

        public AbstractInterpereter()
        {
            cmd = new CommandEngine(this);
            cmd.addCommand("var");
            cmd.addCommand("repeat");
            cmd.addCommand("end");
            cmd.addCommand("add");
            cmd.addCommand("sub");
            cmd.addCommand("mul");
            cmd.addCommand("div");
            cmd.addCommand("addf");
            cmd.addCommand("subf");
            cmd.addCommand("mulf");
            cmd.addCommand("divf");
        }

        private string processClassicCommand(string[] cmd)
        {
            string wnk = cmd[0];
            for (int i = 1; i < cmd.Length; i++)
            {
                wnk += " ";
                if (variable.ContainsKey($"{cmd[i]}")) wnk += variable[$"{cmd[i]}"]; else wnk += cmd[i];
            }
          //  dbg.Info(wnk);
            return wnk;
        }

        public (string[],int[]) RunAbstractIntrprter(string[] srcipt)
        {
            List<string> proccedCode = new List<string>();
            List<int> linieTrack = new List<int>();
            while ((currentLine-1) < srcipt.Length)
            {
                var args = srcipt[currentLine - 1].Split(' ');
                if (cmd.ExeSrciptCommand(currentLine, args) == -1)
                {
                    proccedCode.Add(processClassicCommand(args));
                    linieTrack.Add(currentLine);
                    //add null comand to proper line conut in lover level
                }
                currentLine++;
            }
            return (proccedCode.ToArray(), linieTrack.ToArray());
        }

        enum OpTypes
        {
            Add = 0,
            Sub = 1,
            Mul = 3,
            Div = 4
        }


        float TakeVarOrConstF(string arg)
        {
            if (variable.ContainsKey(arg)) arg = variable[arg];
            float wnk = 0;
            if (!float.TryParse(arg, out wnk)) dbg.Wraning($"[Line {currentLine}]Fail to convert to int: {arg}");
            return wnk;
        }
        int TakeVarOrConst(string arg)
        {
            if (variable.ContainsKey(arg)) arg = variable[arg];
            int wnk = 0;
            if (!int.TryParse(arg, out wnk)) dbg.Wraning($"[Line {currentLine}]Fail to convert to int: {arg}");
            return wnk;
        }
        void IntOp(string aVar, string bVar, OpTypes op)
        {
            int a = TakeVarOrConst(aVar), b = TakeVarOrConst(bVar);
            if (variable.ContainsKey(aVar))
            {
                switch (op) {
                    case OpTypes.Add: variable[aVar] = (a + b).ToString(); break;
                    case OpTypes.Sub: variable[aVar] = (a - b).ToString(); break;
                    case OpTypes.Mul: variable[aVar] = (a * b).ToString(); break;
                    case OpTypes.Div:
                        if (b != 0)
                            variable[aVar] = (a / b).ToString();
                        else dbg.Wraning($"[Line {currentLine}] arg nr 2 is not allowed to be 0");
                    break;
                }       
            }
            else dbg.Wraning("[Line {currentLine}] arg nr 1 must be variable");
        }
        void IntOpF(string aVar, string bVar, OpTypes op)
        {
            float a = TakeVarOrConstF(aVar), b = TakeVarOrConstF(bVar);
            if (variable.ContainsKey(aVar))
            {
                switch (op)
                {
                    case OpTypes.Add: variable[aVar] = (a + b).ToString(); break;
                    case OpTypes.Sub: variable[aVar] = (a - b).ToString(); break;
                    case OpTypes.Mul: variable[aVar] = (a * b).ToString(); break;
                    case OpTypes.Div:
                        if (b != 0)
                            variable[aVar] = (a / b).ToString();
                        else dbg.Wraning($"[Line {currentLine}] arg nr 2 is not allowed to be 0");
                        break;
                }
            }
            else dbg.Wraning("[Line {currentLine}] arg nr 1 must be variable");
        }
        public void add(string aVar,string bVar)
        {
            IntOp(aVar, bVar, OpTypes.Add);
        }
        public void sub(string aVar, string bVar)
        {
            IntOp(aVar, bVar, OpTypes.Sub);
        }
        public void mul(string aVar, string bVar)
        {
            IntOp(aVar, bVar, OpTypes.Mul);
        }
        public void div(string aVar, string bVar)
        {
            IntOp(aVar, bVar, OpTypes.Div);
        }
        public void addf(string aVar, string bVar)
        {
            IntOpF(aVar, bVar, OpTypes.Add);
        }
        public void subf(string aVar, string bVar)
        {
            IntOpF(aVar, bVar, OpTypes.Sub);
        }
        public void mulf(string aVar, string bVar)
        {
            IntOpF(aVar, bVar, OpTypes.Mul);
        }
        public void divf(string aVar, string bVar)
        {
            IntOpF(aVar, bVar, OpTypes.Div);
        }

        public void var(string name, string value)
        {
           // dbg.Succes($"add var {name}, {value}");
            variable[$"${name}"] = value;
        }
        public void repeat(int times)
        {
            reaptStack.Push(new loopInfo(currentLine, times - 1));
        }
        public void end()
        {
            if(reaptStack.Count > 0)
            {
                loopInfo loopInfo = reaptStack.Pop();
                if(loopInfo.times > 0)
                {
                    loopInfo.times--;
                  //  dbg.Wraning($"{currentLine}->{loopInfo.startLine}");
                    currentLine = loopInfo.startLine;
                    reaptStack.Push(loopInfo);
                }
            }
            else
            {
                dbg.Error($"[Line {currentLine}] unexptcted end of loop");
            }
        }
    }

    public class Interpereter
    {
        Canvas currentCanvas = null;
        Clip currentClip = null;
        CommandEngine cmd = null;
        public Interpereter()
        {
            cmd = new CommandEngine(this);
            cmd.addCommand("canvas");
        }
     
        public void RunScript(string name)
        {
            string[] lines;
            int[] lineCnt;
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
            AbstractInterpereter abi = new AbstractInterpereter();
            (lines, lineCnt) = abi.RunAbstractIntrprter(lines);

            int line = 1;
            foreach (var item in lines)
            {
                string[] arg = item.Split(' ');//commandBase.ContainsKey(arg[0])
                if (cmd.ExeSrciptCommand(lineCnt[line - 1], arg) == -1)
                {
                    dbg.Error($"[Line {lineCnt[line - 1]}]unknown or inaccessible command {arg[0]}");
                }
                line++;
            }
            dbg.ClassicRaport();
        }
        void InitStage2()
        {
            cmd.addCommand("clip");
        }
        void InitStage3()
        {
            cmd.addCommand("position");
            cmd.addCommand("place");
            cmd.addCommand("move");
            cmd.addCommand("shift");
            cmd.addCommand("rotation");
            cmd.addCommand("rotate");
            cmd.addCommand("scale");
            cmd.addCommand("rescale");
            cmd.addCommand("video");
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
