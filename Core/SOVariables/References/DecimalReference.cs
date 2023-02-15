using System;
namespace ATBS.Core
{
    [Serializable]
    public class DecimalReference : GenericReference<decimal>
    { public DecimalReference(decimal value) : base(value) { } }
}