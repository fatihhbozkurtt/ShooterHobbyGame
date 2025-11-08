using System;
using DG.Tweening;
using UnityEngine;

namespace EssentialManagers.CustomPackages.SlotSystem.Scripts
{
    public class ExampleSlotUser : MonoBehaviour
    {
        [Header("Debug")] public Slot MySlot;
        public SlotUserColor SlotUserColor;

        private void Awake()
        {
            SlotUserColor = GetRandomColor();
            gameObject.name = SlotUserColor.ToString();
        }

        private SlotUserColor GetRandomColor()
        {
            Array values = Enum.GetValues(typeof(SlotUserColor));
            return (SlotUserColor)values.GetValue(UnityEngine.Random.Range(1,
                values.Length));
        }

        private void OnMouseDown()
        {
            if (SlotManager.instance.HasEmptySlots(out Slot emptySlot))
            {
                MySlot = emptySlot;
                emptySlot.SetOccupied(this);
                SlotManager.instance.AddToColorDictionary(this);
                transform.DOMove(emptySlot.transform.position, 0.5f)
                    .OnComplete(() => SlotManager.instance.OnNewOccupierArrives(this));
            }
            else
            {
                Debug.LogWarning(SlotManager.instance.WillMergeHappen()
                    ? "Dont Worry, merge is happening"
                    : "No empty slots exist!");
            }
        }

        public void PerformMergeAnimation()
        {
            float duration = 0.3f;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMoveY(transform.position.y + 1f, duration).SetEase(Ease.OutQuad))
                .Join(transform.DOScale(1.25f, duration))
                .Append(transform.DOScale(Vector3.zero, duration).SetEase(Ease.InQuad))
                .OnComplete(() => Destroy(gameObject));
        }
    }
}

[Serializable]
public enum SlotUserColor
{
    None,
    Red,
    Blue,
    Green,
    Yellow,
    Orange,
}