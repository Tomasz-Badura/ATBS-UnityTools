using System;
namespace ATBS.Core
{
    [Serializable]
    public class FloatReference : GenericReference<float>
    { public FloatReference(float value) : base(value) { } }
}