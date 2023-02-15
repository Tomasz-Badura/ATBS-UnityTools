using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnScreenKey : MonoBehaviour
{
    public OnScreenKeyboard Keyboard { get; set; }
    [SerializeField] private bool characterRemove;
    [SerializeField] private bool checkForCaps = true;
    [SerializeField] private string inputString;
    [SerializeField] private UnityEvent action;
    [SerializeField] private UnityEvent selectAction;
    [SerializeField] private UnityEvent deselectAction;
    private bool clicked = false;
    public Button Button { get; private set; }
    private void Awake()
    {   
        Button = GetComponent<Button>();
        if (characterRemove)
            Button?.onClick.AddListener(RemoveClick);
        else
            Button?.onClick.AddListener(Click);
    }

    private void RemoveClick()
    {
        HandleSelect();
        string text = Keyboard.CurrentText;
        Keyboard.CurrentText = text.Remove(text.Length - 1);
        action?.Invoke();
    }

    private void Click()
    {
        HandleSelect();
        Keyboard.CurrentText += checkForCaps ? Keyboard.Caps ? inputString.ToUpper() : inputString.ToLower() : inputString;
        action?.Invoke();
    }

    private void HandleSelect()
    {
        if(clicked)
        {
            deselectAction?.Invoke();
            clicked = false;
        }
        else
        {
            selectAction?.Invoke();
            clicked = true;
        }
    }

    public void ChangeColorSelect()
    {
        Button.GetComponent<Image>().color = Button.colors.selectedColor;
    }
    public void ChangeColorDeselect()
    {
        Button.GetComponent<Image>().color = Button.colors.normalColor;
    }
}