using System.Collections.Generic;
using System.Linq;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace EssentialManagers.CustomPackages.SlotSystem.Scripts
{
    public class SlotManager : MonoSingleton<SlotManager>
    {
        [Header("Debug")] [SerializeField] private int mergeCount = 3;
        [SerializeField] private List<Slot> slots;
        private Dictionary<SlotUserColor, List<ExampleSlotUser>> colorOccupiers = new();

        protected override void Awake()
        {
            base.Awake();

            slots = transform.GetComponentsInChildren<Slot>().ToList();
        }
        
        /// <summary>
        /// ExampleSlotUser nesnesini renk tabanlı sözlüğe ekler ve güncellenmiş Dictionary döndürür.
        /// </summary>
        public void AddToColorDictionary(ExampleSlotUser occupier)
        {
            if (!colorOccupiers.ContainsKey(occupier.SlotUserColor))
            {
                colorOccupiers[occupier.SlotUserColor] = new List<ExampleSlotUser>();
            }

            colorOccupiers[occupier.SlotUserColor].Add(occupier);
        }
        
        /// <summary>
        /// Belirtilen kullanıcıyı sözlükten kaldırır.
        /// </summary>
        public void RemoveFromDictionary(ExampleSlotUser occupier)
        {
            if (colorOccupiers.ContainsKey(occupier.SlotUserColor))
            {
                colorOccupiers[occupier.SlotUserColor].Remove(occupier);

                if (colorOccupiers[occupier.SlotUserColor].Count == 0)
                {
                    colorOccupiers.Remove(occupier.SlotUserColor);
                }
            }
        }

        public void OnNewOccupierArrives(ExampleSlotUser occupier)
        {
            List<ExampleSlotUser> sameColoredOccupiers = GetSameColoredOccupiers(occupier.SlotUserColor);

            if (sameColoredOccupiers.Count < mergeCount) return;

            for (int i = 0; i < mergeCount; i++)
            {
                ExampleSlotUser slotUser = sameColoredOccupiers[i];
                RemoveFromDictionary(slotUser);
                slotUser.MySlot.SetFree();
                slotUser.PerformMergeAnimation();
            }
        }

        #region BOOLEANS

        public bool HasEmptySlots(out Slot emptySlot)
        {
            emptySlot = null;
            bool hasSlots = false;
            foreach (var slot in slots)
            {
                if (slot.IsOccupied) continue;

                hasSlots = true;
                emptySlot = slot;
                break;
            }

            return hasSlots;
        }

        public bool WillMergeHappen()
        {
            foreach (var pair in colorOccupiers)
            {
                if (pair.Value.Count >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
        #region GETTERS

        private List<ExampleSlotUser> GetSameColoredOccupiers(SlotUserColor targetColor)
        {
            List<ExampleSlotUser> sameColoredOccupiers = new List<ExampleSlotUser>();

            foreach (var slot in slots)
            {
                if (!slot.IsOccupied) continue;
                if (slot.GetOccupierObject().SlotUserColor == targetColor)
                    sameColoredOccupiers.Add(slot.GetOccupierObject());
            }

            return sameColoredOccupiers;
        }

        public List<Slot> GetSlots()
        {
            return slots;
        }

        #endregion
    }
}