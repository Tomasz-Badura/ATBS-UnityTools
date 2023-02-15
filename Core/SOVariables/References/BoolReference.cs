using System;
namespace ATBS.Core
{
    [Serializable]
    public class BoolReference : GenericReference<bool>
    { public BoolReference(bool value) : base(value) { } }
}