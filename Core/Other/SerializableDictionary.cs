using System;
using System.Collections.Generic;
using UnityEngine;
using ATBS.Core;
namespace ATBS.Core
{
    public abstract class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();

        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
            {
                this[this.keyData[i]] = this.valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            this.valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
    }
}

namespace ATBS.SerializableDictionaries
{
    #region Dictionary types
    //STRING
    [Serializable] public class StringStringDictionary : SerializableDictionary<string, string> { }
    [Serializable] public class StringIntDictionary : SerializableDictionary<string, int> { }
    [Serializable] public class StringBoolDictionary : SerializableDictionary<string, bool> { }
    [Serializable] public class StringFloatDictionary : SerializableDictionary<string, float> { }
    [Serializable] public class StringObjectDictionary : SerializableDictionary<string, object> { }
    [Serializable] public class StringGameobjectDictionary : SerializableDictionary<string, GameObject> { }
    [Serializable] public class StringTransformDictionary : SerializableDictionary<string, Transform> { }
    //INT
    [Serializable] public class IntIntDictionary : SerializableDictionary<int, int> { }
    [Serializable] public class IntStringDictionary : SerializableDictionary<int, string> { }
    [Serializable] public class IntBoolDictionary : SerializableDictionary<int, bool> { }
    [Serializable] public class IntFloatDictionary : SerializableDictionary<int, float> { }
    [Serializable] public class IntObjectDictionary : SerializableDictionary<int, object> { }
    [Serializable] public class IntGameobjectDictionary : SerializableDictionary<int, GameObject> { }
    [Serializable] public class IntTransformDictionary : SerializableDictionary<int, Transform> { }
    //BOOL
    [Serializable] public class BoolBoolDictionary : SerializableDictionary<bool, bool> { }
    [Serializable] public class BoolIntDictionary : SerializableDictionary<bool, int> { }
    [Serializable] public class BoolStringDictionary : SerializableDictionary<bool, string> { }
    [Serializable] public class BoolFloatDictionary : SerializableDictionary<bool, float> { }
    [Serializable] public class BoolObjectDictionary : SerializableDictionary<bool, object> { }
    [Serializable] public class BoolGameobjectDictionary : SerializableDictionary<bool, GameObject> { }
    [Serializable] public class BoolTransformDictionary : SerializableDictionary<bool, Transform> { }
    //FLOAT
    [Serializable] public class FloatFloatDictionary : SerializableDictionary<float, float> { }
    [Serializable] public class FloatIntDictionary : SerializableDictionary<float, int> { }
    [Serializable] public class FloatStringDictionary : SerializableDictionary<float, string> { }
    [Serializable] public class FloatBoolDictionary : SerializableDictionary<float, bool> { }
    [Serializable] public class FloatObjectDictionary : SerializableDictionary<float, object> { }
    [Serializable] public class FloatGameobjectDictionary : SerializableDictionary<float, GameObject> { }
    [Serializable] public class FloatTransformDictionary : SerializableDictionary<float, Transform> { }
    //TRANSFORM
    [Serializable] public class TransformTransformDictionary : SerializableDictionary<Transform, Transform> { }
    [Serializable] public class TransformIntDictionary : SerializableDictionary<Transform, int> { }
    [Serializable] public class TransformStringDictionary : SerializableDictionary<Transform, string> { }
    [Serializable] public class TransformBoolDictionary : SerializableDictionary<Transform, bool> { }
    [Serializable] public class TransformFloatDictionary : SerializableDictionary<Transform, float> { }
    [Serializable] public class TransformObjectDictionary : SerializableDictionary<Transform, object> { }
    [Serializable] public class TransformGameobjectDictionary : SerializableDictionary<Transform, GameObject> { }
    //GAMEOBJECT
    [Serializable] public class GameobjectGameobjectDictionary : SerializableDictionary<GameObject, GameObject> { }
    [Serializable] public class GameobjectIntDictionary : SerializableDictionary<GameObject, int> { }
    [Serializable] public class GameobjectStringDictionary : SerializableDictionary<GameObject, string> { }
    [Serializable] public class GameobjectBoolDictionary : SerializableDictionary<GameObject, bool> { }
    [Serializable] public class GameobjectFloatDictionary : SerializableDictionary<GameObject, float> { }
    [Serializable] public class GameobjectObjectDictionary : SerializableDictionary<GameObject, object> { }
    [Serializable] public class GameobjectTransformDictionary : SerializableDictionary<GameObject, Transform> { }
    //OBJECT
    [Serializable] public class ObjectObjectDictionary : SerializableDictionary<object, object> { }
    [Serializable] public class ObjectGameobjectDictionary : SerializableDictionary<object, GameObject> { }
    [Serializable] public class ObjectIntDictionary : SerializableDictionary<object, int> { }
    [Serializable] public class ObjectStringDictionary : SerializableDictionary<object, string> { }
    [Serializable] public class ObjectBoolDictionary : SerializableDictionary<object, bool> { }
    [Serializable] public class ObjectFloatDictionary : SerializableDictionary<object, float> { }
    [Serializable] public class ObjectTransformDictionary : SerializableDictionary<object, Transform> { }
    #endregion
}