/*
MENU SYSTEM
creator: Tomasz Badura
Version 1.0.0

DESCRIPTION:
    This menu system was created for providing tools for creating new menus
    and menu functionality. It consists of:

    1. Element handling
    ( Selectable or non selectable elements that hold small amount of data and display it
    in usually list-like format. )

    2. Input handling
    ( Handling player input from input fields and other input ways. Creating forms to easily read and validate
    data given by the player. )

    3. Menu manager and menu class
    ( Changing menus and calling menu specific events. Creating new menus in a consistent and easy way )

    aswell as other tools for working with menu UI and optional examples.

REQUIREMENTS:
- TextMeshPro

DOCUMENTATION:
Work in progress...

*/

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ATBS.Extensions;

namespace ATBS.MenuSystem
{
    [DisallowMultipleComponent]
    public class MenuManager : MonoBehaviour
    {
        #region Variables
        [Tooltip("Parent location of menus")]
        [SerializeField] private Transform menusLocation;
        public bool AreActive { get; private set; } // is any menu active
        public Menu Current { get; private set; } // Currently active menu
        public Menu Last { get; private set; } // Last active menu
        private List<Menu> menus = new List<Menu>();
        #endregion
        #region events
        public event EventHandler OnMenusEnabled; // called when opening menu from all menus closed state
        public event EventHandler OnMenusDisabled; // called when all menus closed
        #endregion
        #region methods
        private void Start()
        {
            if (menusLocation == null) menusLocation = transform;
            GetMenus();
            DisableAll();
        }

        /// <summary>
        /// Finds a menu with the specified name
        /// </summary>
        /// <param name="menuName"> name of the menu to find </param>
        /// <returns> found menu or null if no menu was found </returns>
        public Menu GetMenu(string menuName)
        {
            foreach (Menu menu in menus)
            {
                if (menu.Name != menuName.Clean())
                    continue;
                return menu;
            }
            Debug.LogWarning("Couldn't find menu with that name");
            return null;
        }

        /// <summary>
        /// Changes the current menu
        /// </summary>
        /// <param name="menuName"> name of the menu to change to </param>
        /// <returns> menu that it changed to or null </returns>
        public Menu ChangeMenu(string menuName)
        {
            // Find menu
            Menu found = null;
            foreach (Menu menu in menus)
            {
                if (menu.Name != menuName.Clean())
                    continue;
                found = menu;
                break;
            }

            // If not found
            if (found == null)
            {
                Debug.LogWarning("Couldn't find menu with that name, make sure every menu name is normalized");
                return found;
            }

            // Update last menu
            Last = Current;
            Last?.Unload();
            Last?.gameObject.SetActive(false);
            // Update current menu
            Current = found;
            Current.gameObject.SetActive(true);
            Current.Load();
            if (Last?.Previous != Current)
                Current.Previous = Last;

            // if no menu active, trigger menu enabled
            if (!AreActive)
            {
                OnMenusEnabled?.Invoke(this, EventArgs.Empty);
                AreActive = true;
            }

            return Current;
        }

        /// <summary>
        /// Disables the current menu
        /// </summary>
        public void DisableCurrentMenu()
        {
            // Disable current menu
            if (Current != null)
            {
                Current.Unload();
                Current.gameObject.SetActive(false);
                OnMenusDisabled?.Invoke(this, EventArgs.Empty);
            }
            Reset();
        }


        /// <summary>
        /// Creates a new element in the specified location with the specified prefab and type.
        /// </summary>
        /// <param name="elementPrefab">Prefab of the element</param>
        /// <param name="elementLocation">Location in which to create the element</param>
        /// <param name="elementType">Element type identificator</param>
        /// <param name="reference">reference to IElementHandler</param>
        /// <returns>If all went well, returns the created element, otherwise returns null</returns>
        public Element CreateElement(GameObject elementPrefab, Transform elementLocation, ElementType elementType, IElementHandler reference)
        {
            if (elementPrefab == null)
            {
                Debug.LogError("You can't create an element with a null element prefab");
                return null;
            }
            if (elementLocation == null ? true : elementLocation.Equals(null))
            {
                Debug.LogError("You can't create an element in a location that doesn't exist");
                return null;
            }
            if (reference == null)
            {
                Debug.LogError("You can't create an element without linking it to a not null IElementHandler");
                return null;
            }
            Element element = Instantiate(elementPrefab, elementLocation, false).GetComponent<Element>();
            if (element == null)
            {
                Debug.LogError("You're trying to create an element but given prefab doesn't have an Element class at it's root");
                Destroy(element.gameObject);
                return null;
            }
            element.Set(elementType, reference);
            return element;
        }

        /// <summary>
        /// Searches for game objects with the Element class within the specified transform, and reorders them in the hierarchy tree based on the value of the specified key in their data dictionary.
        /// </summary>
        /// <param name="parent">The transform to search for game objects with the Element class.</param>
        /// <param name="key">The key to use for sorting the elements.</param>
        /// <param name="reverse">Indicates whether the element order should be reversed after sorting. If true, the elements will be sorted in reverse order.</param>
        public void SortElements(Transform elementLocation, string key, bool reverse)
        {
            // Find all game objects with the Element class.
            var elements = elementLocation.GetComponentsInChildren<Element>();

            // Sort the elements based on the specified key.
            elements = elements.OrderBy<Element, object>(e =>
            {
                string value = e[key];
                // Try to parse the value as a date.
                if (DateTime.TryParse(value, out DateTime dateValue))
                {
                    // If the value is a valid date, sort by value.
                    return dateValue;
                }
                // Try to parse the value as a number.
                else if (int.TryParse(value, out int intValue))
                {
                    // If the value is a valid number, sort by value.
                    return intValue;
                }
                else if (float.TryParse(value, out float floatValue))
                {
                    // If the value is a valid float, sort by value.
                    return floatValue;
                }
                // else, sort alphabetically.
                else
                {
                    return value;
                }
            }).ToArray();

            // Reverse the elements order
            if (reverse)
            {
                elements = elements.Reverse().ToArray();
            }

            // Reorder the element game objects in the hierarchy tree.
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].transform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// Fills the menus list
        /// </summary>
        private void GetMenus()
        {
            menus.Clear();
            foreach (Transform child in menusLocation)
            {
                Menu menu = child.GetComponent<Menu>();
                if (menu == null)
                    continue;
                menu.MenuManager = this;
                menus.Add(menu);
            }
        }

        /// <summary>
        /// Disables all of the menus
        /// </summary>
        private void DisableAll()
        {
            foreach (Menu menu in menus)
            {
                menu.gameObject.SetActive(false);
            }
            Reset();
        }

        /// <summary>
        /// Resets menu manager state
        /// </summary>
        private void Reset()
        {
            Last = null;
            Current = null;
            AreActive = false;
        }
        #endregion
    }
}