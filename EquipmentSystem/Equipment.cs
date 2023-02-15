using System.Collections.Generic;
using ATBS.Extensions;
using UnityEngine;
namespace ATBS.InventorySystem
{
    public class Equipment : ScriptableObject
    {
        [SerializeField] public List<ItemSlot> itemSlots { get; private set; }
        #region Events
        public delegate void EquipmentHandler();
        public EquipmentHandler OnChanged;
        public EquipmentHandler OnItemAdded;
        public EquipmentHandler OnItemRemoved;
        #endregion
        #region Methods
        public virtual Item PutItemInSlot(Item item, ItemSlot slot)
        {
            Item lastItem = null;
            if(slot.item != null) lastItem = slot.item;
            slot.item = item;
            OnItemAdd(slot);
            return lastItem;
        }

        public virtual Item PutItemInSlot(Item item, string id)
        {
            return PutItemInSlot(item, GetSlot(id));
        }

        public virtual Item RemoveItemFromSlot(ItemSlot slot)
        {
            if(slot.item == null) return null;
            Item item = slot.item;
            slot.item = null;
            OnItemRemoved();
            return item;
        }

        public virtual Item RemoveItemFromSlot(string id)
        {
            return RemoveItemFromSlot(GetSlot(id));
        }

        protected virtual void SlotModified(ItemSlot slot) { }

        public ItemSlot GetSlot(string id)
        {
            return itemSlots.Find(x => x.identifier.Clean() == id.Clean());
        }

        protected void OnItemAdd(ItemSlot slot)
        {
            OnItemAdded.Invoke();
            OnChanged.Invoke();
            SlotModified(slot);
        }

        protected void OnItemRemove(ItemSlot slot)
        {
            OnItemRemoved.Invoke();
            OnChanged.Invoke();
            SlotModified(slot);
        }
        #endregion
    }
}