using ATBS.Extensions;
using UnityEngine;
namespace ATBS.Audio
{
    [CreateAssetMenu(fileName = "SoundManager", menuName = "SoundSystem/SoundManager")]
    public class SoundManager : ScriptableObject
    {
        [field: SerializeField] public System.Collections.Generic.List<Sound> Sounds { get; private set; }
        
        public Sound GetSound(string name)
        {
            Sound sound = Sounds.Find(sound => sound.name.Clean() == name.Clean());
            if (sound == null) this.LogWarning("Can't find a sound with name: " + name);
            return sound;
        }
    }
}