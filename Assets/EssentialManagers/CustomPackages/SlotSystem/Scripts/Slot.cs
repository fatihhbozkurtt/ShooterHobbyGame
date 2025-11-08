using UnityEngine;

namespace EssentialManagers.CustomPackages.SlotSystem.Scripts
{
    public class Slot : MonoBehaviour
    {
        [Header("Debug")] public bool IsOccupied;
        [SerializeField] private ExampleSlotUser OccupierObject;
        
        public void SetOccupied(ExampleSlotUser newOccupier)
        {
            OccupierObject = newOccupier;
            IsOccupied = true;
        }
        
        public void SetFree()
        {
            OccupierObject = null;
            IsOccupied = false;
        }
        
        public ExampleSlotUser GetOccupierObject()
        {
            return OccupierObject;
        }
    }
}