using System;
using System.Collections.Generic;
using System.Text;

namespace schema_based_animator
{
    public class Scale : ITransform
    {
        public float scale = 1.0f;
        public ITransform interpolate(float stage, ITransform target)
        {
            Scale t = target as Scale;
            if (t is null) return null;
            return new Scale
            {
                scale = stage * t.scale + (1.0f - stage) * scale,
            };
        }
    }
}
