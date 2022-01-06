using System;
using System.Collections.Generic;
using System.Text;

namespace schema_based_animator
{
    public class CommandSequence<T> where T : ITransform
    {
        List<Command<T>> commands = new List<Command<T>>();
        public T getTransformAtFrame(int frame)
        {
            int i = 0;
            for (; i < commands.Count && commands[i].frame <= frame; i++) ;
            return (T)commands[i - 1].value.interpolate((float)(frame-commands[i - 1].frame)/(commands[i].frame-commands[i - 1].frame), commands[i].value);
        }

        public void addCommand(Command<T> command)
        {
            commands.Add(command);
            commands.Sort(Comparer<Command<T>>.Default);
        }
    }
}
