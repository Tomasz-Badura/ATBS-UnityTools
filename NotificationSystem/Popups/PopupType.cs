namespace ATBS.Notifications
{
    /// <summary>
    /// Types of popups.
    /// </summary>
    public enum PopupType
    {
        Important, // displayed first in the queue
        NonImportant,
        System, // displayed over every other popup, can be multiple of them open at once
        Custom // your custom popup
    }
}