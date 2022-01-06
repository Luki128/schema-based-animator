using System;
using System.Collections.Generic;
using System.Text;

namespace schema_based_animator
{
    public interface ITransform
    {
        ITransform interpolate(float stage, ITransform target);
    }
}
