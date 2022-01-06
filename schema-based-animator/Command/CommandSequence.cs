using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace schema_based_animator
{
    public class CommandSequence<T> where T : ITransform, new()
    {
        public List<Command<T>> commands = new List<Command<T>>();
        public T getTransformAtFrame(int frame)
        {
            if (commands.Count == 0) return new T();
            if (commands.Count == 1) return commands.FirstOrDefault().value;
            int i = 0;
            for (; i < commands.Count && commands[i].frame < frame; i++) ;
            if (i >= commands.Count()) return commands.LastOrDefault().value;
            return (T)commands[i - 1].value.interpolate((float)(frame-commands[i - 1].frame)/(commands[i].frame-commands[i - 1].frame), commands[i].value);
        }

        public void addCommand(Command<T> command)
        {
            commands.Add(command);
            commands.Sort(Comparer<Command<T>>.Default);
        }
    }
}
