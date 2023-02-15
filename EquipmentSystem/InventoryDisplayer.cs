using ATBS.Core.UIUtility;
namespace ATBS.InventorySystem
{
    public abstract class InventoryDisplayer : Displayer
    {
        public Inventory Inventory
        {
            get
            {
                return Inventory;
            }
            set
            {
                OnDisable();
                Inventory = value;
                OnEnable();
            }
        }
        protected override void OnEnable() 
        {
            Inventory.OnChanged += UpdateVisuals;
            UpdateVisuals();
        }
        protected override void OnDisable() => Inventory.OnChanged -= UpdateVisuals;
    }
}