using UnityEngine;
using UnityEngine.InputSystem;
namespace ATBS.InputSystem.UserInputHandling
{
    [RequireComponent(typeof(PlayerInput))]
    public class UserInputManager : MonoBehaviour
    {
        #region Variables
        public RebindManager RebindManager = new();
        public InputActionMap CurrentMap { get { return playerInput.currentActionMap; } }
        public InputActionMap PreviousMap { get; private set; }
        [SerializeField] UIActionMap defaultUI;
        private PlayerInput playerInput;
        #endregion
        #region methods
        public static UserInputManager Instance; //Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);

            playerInput = GetComponent<PlayerInput>();
            PreviousMap = playerInput.currentActionMap;
            RebindManager.playerInput = playerInput;
        }

        /// <summary>
        /// Changes the current action map
        /// </summary>
        /// <param name="MapName"></param>
        public void SwitchActionMap(string MapName)
        {
            if (MapName == null)
            {
                CurrentMap.Disable();
                PreviousMap = null;
                return;
            }
            if (CurrentMap.name != MapName)
            {
                PreviousMap = CurrentMap;
                playerInput.SwitchCurrentActionMap(MapName);
            }
        }

        /// <summary>
        /// Changes the current UI navigation inputs
        /// </summary>
        /// <param name="uiMap"></param>
        public void CustomUINavigation(UIActionMap uiMap)
        {
            if(uiMap == null) return;
            if(playerInput == null) return;
            if(playerInput.uiInputModule == null)
            {
                Debug.LogError("There is no uiInputModule in the PlayerInput, make sure to assign it before using CustomUINavigation");
                return;
            }

            playerInput.uiInputModule.point = uiMap.Point;
            playerInput.uiInputModule.leftClick = uiMap.LeftClick;
            playerInput.uiInputModule.middleClick = uiMap.MiddleClick;
            playerInput.uiInputModule.rightClick = uiMap.RightClick;
            playerInput.uiInputModule.scrollWheel = uiMap.ScrollWheel;
            playerInput.uiInputModule.move = uiMap.Move;
            playerInput.uiInputModule.submit = uiMap.Submit;
            playerInput.uiInputModule.cancel = uiMap.Cancel;
            playerInput.uiInputModule.trackedDevicePosition = uiMap.TrackedPosition;
            playerInput.uiInputModule.trackedDeviceOrientation = uiMap.TrackedOrientation;
        }

        /// <summary>
        /// Sets current UI navigation inputs to default
        /// </summary>
        public void DefaultUINavigation() 
        {
            CustomUINavigation(defaultUI);
        }
        #endregion
    }
}