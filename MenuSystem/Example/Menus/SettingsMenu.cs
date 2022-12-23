using UnityEngine;
using TMPro;
using ATBS.MenuSystem;
using ATBS.Extensions;

public class SettingsMenu : Menu
{
    [SerializeField] private InputForm myForm; // get a reference to a form
    [SerializeField] private TextMeshProUGUI validateText, agree, age, nameText, nickname, gender;
    private void Start()
    {
        SetupButtons();
    }
    // assign methods to the buttons using GetButton, you can also use the built-in unity OnClick event
    // in the button inspector or create a variable in this script containing the button you need.
    private void SetupButtons()
    {
        GetButton("main")?.onClick.AddListener(()=>{ MenuManager.ChangeMenu("main"); });
        GetButton("validate")?.onClick.AddListener(OnClickValidateButton);
        GetButton("send")?.onClick.AddListener(OnClickSendButton);
        GetButton("example")?.onClick.AddListener(()=>{MenuManager.ChangeMenu("example");});
    }
    public void OnClickValidateButton()
    {
        string output;
        //Validate a form and get the string error message if something went wrong
        myForm.ValidateForm(out output);
        validateText.text = output;
    }
    public void OnClickSendButton()
    {
        // check if the form is valid
        if(!myForm.ValidateForm())
            return;
        // If form is valid, update the dictionary
        myForm.UpdateDictionary();
        // set new viuals
        SetVisuals();
    }

    // Set the default menu visuals on the refresh.
    // Refresh is called on each load menu if load menu is not overriden.
    public override void Refresh()
    {
        agree.text = "do you agree";
        age.text = "your age";
        nameText.text = "your name";
        nickname.text = "your nickname";
        gender.text = "your gender";
    }
    // change the visuals to the values in the validated form, use Clean() to make sure the key is normalized
    private void SetVisuals()
    {
        agree.text = myForm["agree".Clean()];
        age.text = myForm["Age".Clean()];
        nameText.text = myForm["name".Clean()];
        nickname.text = myForm["nickname".Clean()];
        gender.text = myForm["gender".Clean()];
    }

    /*
    Besides using a form you can use just the InputWrapper class to get and validate one input.
    This is usefull when trying to create a smaller input check.
    It's recommended to use a form when creating more complicated checks that consist of many inputs.
    */
}