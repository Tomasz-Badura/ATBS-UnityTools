using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

//
//  Not tested
//

namespace ATBS.InputSystem.UserInputHandling
{
    [Serializable]
    public class RebindManager
    {
        #region Variables
        [Tooltip("Used to exit of rebinding")]
        [SerializeField] private string[] cancelKeys = { "<Keyboard>/escape", "<Gamepad>/start" };
        [Tooltip("The key for player prefs bindings")]
        [SerializeField] private string bindingsKey = "Bindings";
        public PlayerInput playerInput { get; set; }
        #endregion
        #region Events
        public event EventHandler<RebindData> OnStartedRebinding;
        public event EventHandler<RebindData> OnCanceledRebinding;
        public event EventHandler<RebindData> OnPerformedRebinding;
        #endregion
        #region Methods

        /// <summary>
        /// Loads bindings from PlayerPrefs.
        /// </summary>
        public void LoadBindings()
        {
            string rebinds = PlayerPrefs.GetString(bindingsKey, string.Empty);
            if (string.IsNullOrEmpty(rebinds)) return;
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
            OnPerformedRebinding?.Invoke(this, new RebindData());
        }

        /// <summary>
        /// Saves current bindings to PlayerPrefs.
        /// </summary>
        public void SaveBindings()
        {
            string rebinds = playerInput.actions.SaveBindingOverridesAsJson();

            PlayerPrefs.SetString(bindingsKey, rebinds);
        }

        /// <summary>
        /// Start rebinding an action.
        /// </summary>
        /// <param name="action">Action to rebind.</param>
        /// <param name="bindingId">binding id to rebind.</param>
        /// <param name="excluded">Excluded bind choices.</param>
        public void StartRebind(InputAction action, string bindingId, string[] excluded = null)
        {
            if (ResolveActionAndBinding(action, bindingId, out var bindingIndex))
                return;

            if (action.bindings[bindingIndex].isComposite)
            {
                int firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    Rebind(action, bindingIndex, allCompositeParts: true, excluded: excluded);
                return;
            }
            Rebind(action, bindingIndex, excluded: excluded);
        }

        /// <summary>
        /// Rebinds the specified action binding.
        /// </summary>
        /// <param name="action">Action to rebind to.</param>
        /// <param name="bindingIndex">Index of the binding to rebind.</param>
        /// <param name="allCompositeParts">Is this action binding a composite.</param>
        /// <param name="excluded">Excluded bind choices.</param>
        private void Rebind(InputAction action, int bindingIndex, bool allCompositeParts = false, string[] excluded = null)
        {
            InputActionMap currentMap = playerInput.currentActionMap;
            currentMap.Disable();
            // start rebind
            RebindingOperation rebind = action.PerformInteractiveRebinding();

            void CleanUp()
            {
                currentMap?.Enable();
                rebind?.Dispose();
            }

            // Excluded, cancel keys
            foreach (string exclude in excluded)
            {
                rebind.WithControlsExcluding(exclude);
            }
            foreach (string cancelThrough in cancelKeys)
            {
                rebind.WithCancelingThrough(cancelThrough);
            }
            rebind.OnMatchWaitForAnother(0.1f);
            rebind.OnComplete(_ =>
            {
                // if duplicate, ask for rebind again
                if (CheckDuplicateBindings(action, bindingIndex, allCompositeParts))
                {
                    action.RemoveBindingOverride(bindingIndex);
                    CleanUp();
                    Rebind(action, bindingIndex, allCompositeParts);
                    return;
                }
                // action is composite
                if (allCompositeParts)
                {
                    int nextBindingIndex = bindingIndex + 1;
                    if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                    {
                        Rebind(action, nextBindingIndex, true, excluded);
                        return;
                    }
                }

                CleanUp();
                OnPerformedRebinding?.Invoke(this, new RebindData(action));
            });
            rebind.OnCancel(_ =>
            {
                CleanUp();
                OnCanceledRebinding?.Invoke(this, new RebindData(action));
            });
            rebind.Start();
            OnStartedRebinding?.Invoke(this, new RebindData(action));
        }

        /// <summary>
        /// Checks if an action has duplicate bindings.
        /// </summary>
        /// <param name="action">action to check.</param>
        /// <param name="bindingIndex">index of the new binding.</param>
        /// <param name="allCompositeParts">is the action a composite.</param>
        /// <returns>True if found a duplicate, false otherwise.</returns>
        private bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            InputBinding newBinding = action.bindings[bindingIndex];
            foreach (InputBinding binding in action.actionMap.bindings)
            {
                if (binding.action == newBinding.action)
                {
                    continue;
                }
                if (binding.effectivePath == newBinding.effectivePath)
                {
                    return true;
                }
            }

            if (!allCompositeParts)
                return false;

            for (int i = 1; i < bindingIndex; ++i)
            {
                if (action.bindings[i].effectivePath != newBinding.effectivePath)
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if an actions is right and binding exists withing this action.
        /// </summary>
        /// <param name="action">Action to check.</param>
        /// <param name="bindingId">Actions binding id.</param>
        /// <param name="bindingIndex">Binding index based on binding id.</param>
        /// <returns>True if everything is right, false otherwise</returns>
        private bool ResolveActionAndBinding(InputAction action, string bindingId, out int bindingIndex)
        {
            bindingIndex = -1;

            if (action == null)
                return false;

            if (string.IsNullOrEmpty(bindingId))
                return false;

            var _bindingId = new Guid(bindingId);
            bindingIndex = action.bindings.IndexOf(x => x.id == _bindingId);
            if (bindingIndex == -1)
            {
                Debug.LogError("Cannot find binding with ID " + _bindingId + " on " + action);
                return false;
            }
            return true;
        }
        #endregion
    }
}