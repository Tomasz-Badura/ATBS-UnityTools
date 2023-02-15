using System;
using UnityEngine;

namespace ATBS.Core
{
    [Serializable]
    public class Vector2Reference : GenericReference<Vector2>
    { public Vector2Reference(Vector2 value) : base(value) { } }
}