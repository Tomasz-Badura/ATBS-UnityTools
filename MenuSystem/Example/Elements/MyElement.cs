using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ATBS.MenuSystem;
using ATBS.Extensions;

public class MyElement : Element
{
    [SerializeField] private TextMeshProUGUI title;
    private void Awake()
    {
        // set this element to allow for selecting
        IsSelectable = true;
    }
    public override void UpdateVisuals()
    {
        // update the title text
        title.text = Data["title text".Clean()];
    }

    public override void SetDefaultData()
    {
        // clear and repopulate the dictionary to a default state
        Data.Clear();
        Data.Add("TITLETEXT".Clean(), "placeholder");
    }

    public override void OnDeselectElement()
    {
        // change visuals for deselecting
        GetComponent<Image>().color = new Color32(90, 90, 90, 255);
    }
    public override void OnSelectElement()
    {
        // change visuals for selecting
        GetComponent<Image>().color = new Color32(100, 100, 100, 255);
    }
}