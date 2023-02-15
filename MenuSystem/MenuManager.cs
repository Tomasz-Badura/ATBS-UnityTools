// MENU SYSTEM
// creator: Tomasz Badura
// Version 1.0.0
// DOCUMENTATION:
// Work in progress...

using System;
using UnityEngine;
using System.Linq;
using ATBS.Extensions;
using ATBS.Core;
namespace ATBS.MenuSystem
{
    [CreateAssetMenu(menuName = "MenuSystem/MenuManager")]
    public class MenuManager : ScriptableObject
    {
        #region Variables
        public bool AreActive { get; private set; } // is any menu active
        public Menu Current { get; private set; }
        public Menu Last { get; private set; }
        [field: SerializeField] public RuntimeSet<Menu> Menus { get; private set; }
        [SerializeField] private string defaultMenu;
        #endregion
        #region events
        public delegate void MenuHandler();
        public event MenuHandler OnMenusEnabled;
        public event MenuHandler OnMenusDisabled;
        #endregion
        #region methods

        /// <summary>
        /// Should be called at Awake in the scene containing the defaultMenu
        /// </summary>
        public void Setup()
        {
            DisableAll();
            ChangeMenu(defaultMenu);
        }

        /// <summary>
        /// Finds a menu with the specified name
        /// </summary>
        /// <param name="menuName"> name of the menu to find </param>
        /// <returns> found menu or null if no menu was found </returns>
        public Menu GetMenu(string menuName)
        {
            foreach (Menu menu in Menus.Items)
            {
                if (menu.Name.Clean() != menuName.Clean())
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
            foreach (Menu menu in Menus.Items)
            {
                if (menu.Name.Clean() != menuName.Clean())
                    continue;
                found = menu;
                break;
            }

            // If not found
            if (found == null)
            {
                Debug.LogWarning("Couldn't find menu with that name: " + menuName);
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
                OnMenusEnabled?.Invoke();
                AreActive = true;
            }

            return Current;
        }

        public void ChangeMenu(Menu menu)
        {
            if (menu == null)
            {
                Debug.LogError("menu can't be null");
                return;
            }
            // Update last menu
            Last = Current;
            if(!(Last == null ? true : Last.Equals(null)))
            {
                Last.Unload();
                Last.gameObject.SetActive(false);
            }
            // Update current menu
            Current = menu;
            Current.gameObject.SetActive(true);
            Current.Load();
            if (Last?.Previous != Current)
                Current.Previous = Last;

            // if no menu active, trigger menu enabled
            if (!AreActive)
            {
                OnMenusEnabled.Invoke();
                AreActive = true;
            }
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
                OnMenusDisabled.Invoke();
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
                if (DateTime.TryParse(value, out DateTime dateValue))
                    return dateValue;
                else if (int.TryParse(value, out int intValue))
                    return intValue;
                else if (float.TryParse(value, out float floatValue))
                    return floatValue;
                else // sort alphabetically.
                    return value;
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
        /// Disables all of the menus
        /// </summary>
        private void DisableAll()
        {
            foreach (Menu menu in Menus.Items)
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