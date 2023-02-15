using ATBS.Extensions;
using UnityEngine.InputSystem;

//
//  Not tested
//

namespace ATBS.InputSystem.UserInputHandling
{
    /// <summary>
    /// Data about a specific rebind event.
    /// </summary>
    public class RebindData
    {
        public InputAction Action { get; private set; } // affected Action.
        public string ActionNameNormalized { get; private set; } // Normalized action name.
        public string ReadableControlPath { get; private set; } // Control path of the binding, formated.
        public RebindType Type { get; private set; } // type of the rebind.

        public RebindData(InputAction action)
        {
            Type = RebindType.Single;
            Action = action;
            ActionNameNormalized = action.name.Clean();
            int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);
            ReadableControlPath = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }

        public RebindData()
        {
            Type = RebindType.All;
            Action = null;
            ActionNameNormalized = null;
            ReadableControlPath = null;
        }
    }
}