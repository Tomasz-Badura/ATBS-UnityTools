using ATBS.MenuSystem;
using UnityEngine;
public class ExampleMenu : Menu
{
    // you can override the custom Awake of Menu class
    protected override void Awake()
    {
        // Still include the Menu awake for making sure the name is normalized and not empty
        base.Awake();
        SetupButtons();
    }

    // assign methods to the buttons using GetButton, you can also use the built-in unity OnClick event
    // in the button inspector or create a variable in this script containing the button you need.
    private void SetupButtons()
    {
        GetButton("back")?.onClick.AddListener(OnClickBackButton);
        GetButton("next")?.onClick.AddListener(OnClickNextButton);
    }

    private void OnClickBackButton()
    {
        // Change menu to the previous one, automatically avoids creating menu "soft lock" situations
        MenuManager.ChangeMenu(Previous.Name); 
    }
    private void OnClickNextButton()
    {
        // Changes to another menu, requires another menu with examplesecond name.
        if(MenuManager.ChangeMenu("examplesecond") == null)
            Debug.LogWarning("You assigned the next button for the example menu, try creating another menu with this class but giving it a name 'examplesecond'");
    }
}
