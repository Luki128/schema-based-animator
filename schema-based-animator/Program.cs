using System;
using System.Linq;
using FFMpegCore;
using LewyDiagnostic;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;

namespace schema_based_animator
{
    public class Interpereter
    {
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
            if (p.Length != (args.Length-1))
            {
                dbg.Error($"Incorrect number of agruments for {f.Name} expected {p.Length} recived {args.Length-1}");
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
                    sucess = float.TryParse(args[i+1], out fRef);
                    Pasarg[i] = fRef;
                } else if (iRef.GetType() == t) {
                    sucess = int.TryParse(args[i+1], out iRef);
                    Pasarg[i] = iRef;
                } else if (sRef.GetType() == t) {
                    Pasarg[i] = args[i+1];
                    sucess = true;
                }
                if (!sucess) { 
                    dbg.Error($"Incorrect type of agrument nr:{i + 1} for {f.Name} expected argument type: {p[i].ParameterType.Name} ");
                    return;
                }
            }
            f.Invoke(this, Pasarg);
        }

        void addCommand(string name)
        {
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
                    dbg.Error($"unknown or inaccessible command {arg[0]}");
                }
                line++;
            }
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
        }
    }

    class Program
    {




        

        static void Main(string[] args)
        {
            Interpereter interpereter = new Interpereter();
            interpereter.RunScript("test.txt");
        }

    }
}
