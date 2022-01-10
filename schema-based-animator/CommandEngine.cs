using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using LewyDiagnostic;
using System.Globalization;

namespace schema_based_animator
{
    public class CommandEngine
    {
        private object CommandsSource;
        private Dictionary<string, MethodBase> commandBase = new Dictionary<string, MethodBase>();
        public CommandEngine(object commadSrc)
        {
            CommandsSource = commadSrc;
        }
        public int ExeSrciptCommand(int line,params string[] args)// arg 0 skip is name function
        {
            float fRef = 0;
            int iRef = 0;
            string sRef = "";
            string commandName = args[0];
            if (!commandBase.ContainsKey(commandName)) return -1;

            MethodBase f = commandBase[commandName];

            var p = f.GetParameters();
            if (p.Length != (args.Length - 1))
            {
                dbg.Error($"[Line {line}]Incorrect number of agruments for {f.Name} expected {p.Length} recived {args.Length - 1}");
                return -2;
            }
            object[] Pasarg = new object[p.Length];

            for (int i = 0; i < p.Length; i++)
            {
                //Console.WriteLine($"{args[i].GetType()} - {p[i].ParameterType}");
                var t = p[i].ParameterType;

                bool sucess = false;
                if (fRef.GetType() == t)
                {
                    sucess = float.TryParse(args[i + 1], NumberStyles.Any, CultureInfo.InvariantCulture, out fRef);
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
                    return -3;
                }
            }
            f.Invoke(CommandsSource, Pasarg);
            return 1;
        }
        public void addCommand(string name)
        {
            if (commandBase.ContainsKey(name)) return;
            MethodBase Mymethodbase = CommandsSource.GetType().GetMethod(name);
            commandBase.Add(name, Mymethodbase);
        }

    }
}
