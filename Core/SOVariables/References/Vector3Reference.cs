using System;
using UnityEngine;

namespace ATBS.Core
{
    [Serializable]
    public class Vector3Reference : GenericReference<Vector3>
    { public Vector3Reference(Vector3 value) : base(value) { } }
}