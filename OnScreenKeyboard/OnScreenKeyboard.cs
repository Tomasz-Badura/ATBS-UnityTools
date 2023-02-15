using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class OnScreenKeyboard : MonoBehaviour
{
    [SerializeField] private List<OnScreenKey> keys;
    public List<TMP_InputField> InputFields { get; set; } = new();
    public event System.Action OnShow;
    public event System.Action OnHide;
    public bool Caps { get; set; } 
    public string CurrentText
    {
        get { return currentText; }
        set
        {
            currentText = value;
            UpdateFields();
        }
    }

    private string currentText;

    public void Focus() => keys.First()?.Button.Select();
    public void Show() 
    {
        gameObject.SetActive(true);
        Focus();
        OnShow?.Invoke();
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
        OnHide?.Invoke();
    }

    private void UpdateFields()
    {
        for (int i = 0; i < InputFields.Count; i++)
            InputFields[i].text = currentText;
    }

    private void Awake()
    {
        UpdateFields();
        FillReferences();
    }

    private void FillReferences()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            keys[i].Keyboard = this;
        }
    }
}
