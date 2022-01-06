using System;
using System.Collections.Generic;
using System.Text;

namespace schema_based_animator
{
   public class Rotation : ITransform
    {
        public float angle = 0.0f;
        public ITransform interpolate(float stage, ITransform target)
        {
            Rotation t = target as Rotation;
            if (t is null) return null;
            return new Rotation
            {
                angle = stage * t.angle + (1.0f - stage) * angle,
            };
        }
    }
}
