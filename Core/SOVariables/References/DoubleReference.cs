using System;
namespace ATBS.Core
{
    [Serializable]
    public class DoubleReference : GenericReference<double>
    { public DoubleReference(double value) : base(value) { } }
}