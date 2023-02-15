using UnityEngine;
using UnityEditor;
using ATBS.Audio;

[CustomEditor(typeof(Sound), true)]
public class SoundEditor : Editor 
{
    [SerializeField] AudioSource _previewer;
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Inspect"))
        {
            Sound sound = (Sound) target;
            sound.Play(_previewer);
        }
    }

    private void OnEnable() 
    {
        _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio reviewer", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    private void OnDisable() 
    {
        DestroyImmediate(_previewer.gameObject);
    }
}