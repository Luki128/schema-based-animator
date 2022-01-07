using System;
using System.Collections.Generic;
using System.Text;

namespace schema_based_animator
{
    public class Scale : ITransform
    {
        public float local_x = 1.0f;
        public float local_y = 1.0f;
        public float global_x = 1.0f;
        public float global_y = 1.0f;

        public ITransform interpolate(float stage, ITransform target)
        {
            Scale t = target as Scale;
            if (t is null) return null;
            return new Scale
            {
                local_x = stage * t.local_x + (1.0f - stage) * local_x,
                local_y = stage * t.local_y + (1.0f - stage) * local_y,
                global_x = stage * t.global_x + (1.0f - stage) * global_x,
                global_y = stage * t.global_y + (1.0f - stage) * global_y,
            };
        }
    }
}
