using System.Collections.Generic;
using ATBS.Extensions;
using ATBS.MenuSystem;
using UnityEngine;
public class MainMenu : Menu, IElementHandler
{
    public Element SelectedElement { get; set; } // Implement IElementHandler
    public bool IgnoreDeselect { get; set; } // Implement IElementHandler
    [SerializeField] private GameObject elementPrefab, secondElementPrefab;
    [SerializeField] private Transform elementLocation, secondElementLocation;
    [SerializeField] private List<Element> elements, elementsTwo = new();

    protected override void Awake()
    {
        base.Awake();
        IgnoreDeselect = false; // make sure ignore deselect starts as false
        SetupButtons();
    }

    // assign methods to the buttons using GetButton, you can also use the built-in unity OnClick event
    // in the button inspector or create a variable in this script containing the button you need.
    private void SetupButtons()
    {
        GetButton("settings")?.onClick.AddListener(OnClickSettingsButton);
        GetButton("Add")?.onClick.AddListener(OnClickAddButton);
        GetButton("remove")?.onClick.AddListener(OnClickRemoveButton);
        GetButton("Randomise")?.onClick.AddListener(OnClickRandomise);
        GetButton("example")?.onClick.AddListener(()=>{MenuManager.ChangeMenu("example");});
    }

    private void OnClickSettingsButton()
    {
        if(MenuManager.ChangeMenu("Settings") == null)
            Debug.LogWarning("You assigned the settings button for the main menu, try creating a menu for settings menu with the name 'settings'");
    }
    private void OnClickAddButton()
    {
        if(elements.Count != 30)
        {
            // create element using the CreateElement method.
            Element element = MenuManager.CreateElement(elementPrefab, elementLocation, ElementType.Placeholder01, this);
            // assign a random number for the title text, use Clean() to make sure the key is normalized.
            element["title text".Clean()] = Random.Range(1, 1_000_000).ToString();
            // add this element to a list to track it and use later.
            elements.Add(element);
        }

        if(elementsTwo.Count != 12)
        {
            // create element using the CreateElement method.
            Element secondElement = MenuManager.CreateElement(secondElementPrefab, secondElementLocation, ElementType.Placeholder02, this);
            // assign a random number for the title text, use Clean() to make sure the key is normalized.
            secondElement["title text".Clean()] = Random.Range(-1000, 0).ToString();
            // add this element to a list to track it and use later.
            elementsTwo.Add(secondElement);
        }  
    }
    private void OnClickRemoveButton()
    {
        if(SelectedElement == null ? true : SelectedElement.Equals(null))
        {
            if(elementsTwo.Count == 0) return;
            // to remove an element you can simply call Destroy().
            Destroy(elementsTwo[0].gameObject);
            // remove the element from the list.
            elementsTwo.RemoveAt(0);
        }
        else
        {
            elements.Remove(SelectedElement);
            // to remove an element you can simply call Destroy().
            Destroy(SelectedElement.gameObject);
        }
    }
    private void OnClickRandomise()
    {
        foreach (Element element in elements)
        {
            // randomise all title texts, use Clean() to make sure the key is normalized.
            element["titletext".Clean()] = Random.Range(1, 1_000_000).ToString();
        }
        // use SortElements to sort elements in the hierarchy, most useful when using a layout group.
        MenuManager.SortElements(elementLocation, "titletext".Clean(), false);
    }
}