using System;
using System.Collections.Generic;
using System.Text;

namespace schema_based_animator
{
   public class Position : ITransform
    {
        public float x = 0.0f;
        public float y = 0.0f;
        public ITransform interpolate(float stage, ITransform target)
        {
            Position t = target as Position;
            if (t is null) return null;
            return new Position
            {
                x = stage * t.x + (1.0f - stage) * x,
                y = stage * t.y + (1.0f - stage) * y,
            };
        }
    }
}
