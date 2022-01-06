using System;
using System.Collections.Generic;
using System.Text;

namespace schema_based_animator
{
   public class Command<T> : IComparable where T : ITransform
    {
        public int frame;
        public T value;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Command<T> otherTemperature = obj as Command<T>;
            if (otherTemperature != null)
                return this.frame.CompareTo(otherTemperature.frame);
            else
                throw new ArgumentException("Object is not a Command");
        }
    }
}
