using System;
namespace ATBS.Core
{
    [Serializable]
    public class IntReference : GenericReference<int>
    { public IntReference(int value) : base(value) { } }
}