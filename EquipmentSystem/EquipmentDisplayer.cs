using ATBS.Core.UIUtility;
namespace ATBS.InventorySystem
{
    public abstract class EquipmentDisplayer : Displayer
    {
        public Equipment Equipment
        {
            get
            {
                return Equipment;
            }
            set
            {
                OnDisable();
                Equipment = value;
                OnEnable();
            }
        }
        protected override void OnEnable() 
        { 
            Equipment.OnChanged += UpdateVisuals;            
            UpdateVisuals();
        }
        protected override void OnDisable() => Equipment.OnChanged -= UpdateVisuals;
    }
}