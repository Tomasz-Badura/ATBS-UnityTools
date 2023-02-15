using System;
namespace ATBS.Core
{
    [Serializable]
    public class StringReference : GenericReference<string>
    { public StringReference(string value) : base(value) { } }
}