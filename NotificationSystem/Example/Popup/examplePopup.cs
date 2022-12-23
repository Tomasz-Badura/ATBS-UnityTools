using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBS.Notifications;
using UnityEngine.UI;
using ATBS.Extensions;
public class examplePopup : Popup
{
    [SerializeField] private Button closePopup, testAction, anotherAction;

    private void Awake()
    {
        closePopup.onClick.AddListener(() => {
            PopupManager.ClosePopup(gameObject); // use ClosePopup to close this popup on button click
        });

        testAction.onClick.AddListener(returnValue);

        anotherAction.onClick.AddListener(() =>
        {
            Actions["ano ther".Clean()](); // fire actions from Actions dictionary
        });
    }

    private void returnValue()
    {
        Data.Clear();

        Data.Insert(0, "My custom passed data"); // fill Data Dictionary in your own way

        if(Data.Count > 0) // Checks if data is correct
            Actions["test".Clean()](); // fire the action back 
        else
            Debug.Log("Data not captured correctly"); // throw errors
    }
}
