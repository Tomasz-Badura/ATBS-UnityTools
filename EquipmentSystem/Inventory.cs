using System.Collections.Generic;
using UnityEngine;
namespace ATBS.InventorySystem
{
    public class Inventory : ScriptableObject
    {
        public List<ItemContainer> storedItems = new List<ItemContainer>();
        [SerializeField] private ItemContainer defaultStoredItem;
        public delegate void InventoryHandler();
        public InventoryHandler OnChanged;
        public InventoryHandler OnItemAdded;
        public InventoryHandler OnItemRemoved;

        public virtual ItemContainer AddItem(ItemContainer item)
        {
            storedItems.Add(item);
            OnItemAdded();
            return item;
        }

        public virtual ItemContainer AddItem(Item item)
        {
            ItemContainer newStored = defaultStoredItem;
            newStored.item = item;
            return AddItem(defaultStoredItem);
        }

        public virtual Item RemoveItem(ItemContainer item)
        {
            storedItems.Remove(item);
            OnItemRemoved();
            return item.item;
        }

        public virtual Item RemoveItem(Item item)
        {
            ItemContainer found = FindItem(item);
            if(found != null)
                return RemoveItem(found);
            return null;
        }

        public virtual void InventoryChanged() { }

        protected ItemContainer FindItem(Item item)
        {
            return storedItems.Find(x => x.item == item);
        }

        protected void OnItemRemove()
        {
            OnItemRemoved.Invoke();
            OnChanged.Invoke();
            InventoryChanged();
        }

        protected void OnItemAdd()
        {
            OnItemAdded.Invoke();
            OnChanged.Invoke();
            InventoryChanged();
        }
    }
}