using UnityEngine;
using TMPro;
using ATBS.MenuSystem;
using ATBS.Extensions;

public class MySecondElement : Element
{
    [SerializeField] private TextMeshProUGUI title;
    private void Awake()
    {
        // set this element to not allow for selecting
        IsSelectable = false;
    }
    public override void SetDefaultData()
    {
        // clear and repopulate the dictionary to a default state
        Data.Clear();
        Data.Add("titletext".Clean(), "placeholder");
    }

    public override void UpdateVisuals()
    {
        // update the title text
        title.text = Data["titletext".Clean()];
    }
}
