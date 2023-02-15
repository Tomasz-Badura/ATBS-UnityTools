using UnityEngine;

namespace ATBS.MenuSystem
{
    public class TooltipErrorDisplayer : InputErrorDisplayer
    {
        protected override void DisplayError(ValidationCode validationCode)
        {
            Debug.LogWarning(validationCode.ErrorMessage);
        }
    }
}