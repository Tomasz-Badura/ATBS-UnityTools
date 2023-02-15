using UnityEngine;
[CreateAssetMenu(menuName = "DialogSystem/DialogSpeakerData")]
public class DialogSpeakerData : ScriptableObject 
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    public Transform transform { get; set; }
    // TODO: extend
}