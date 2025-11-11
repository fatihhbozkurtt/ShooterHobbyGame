using System;
using Cysharp.Threading.Tasks;
using EssentialManagers.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace DEVELOPER.Scripts.Managers
{
    public class TimerManager : MonoSingleton<TimerManager>
    {
        [Header("Debug")] [SerializeField] private float totalGameTime = 180f;
        [SerializeField] private float remainingTime;

        [Header("UI Reference")] [SerializeField]
        private TextMeshProUGUI timerText;

        public event Action<float> OnTimerTick;
        public event Action OnTimerEnd;

        private bool isRunning;

        private void Start()
        {
            GameManager.instance.LevelInstantiatedEvent += OnLevelInstantiated;
            totalGameTime = GameManager.instance.GetGeneralLevelData().Time;
            GameManager.instance.LevelFailedEvent += StopTimer;
            
            StartTimer();
        }

        private void OnLevelInstantiated(LevelData data)
        {
            totalGameTime = GameManager.instance.GetGeneralLevelData().Time;
            StartTimer();
        }
        
        private void StartTimer()
        {
            remainingTime = totalGameTime;
            isRunning = true;
            RunTimerAsync().Forget();
        }

        private async UniTaskVoid RunTimerAsync()
        {
            while (remainingTime > 0f && isRunning)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                remainingTime -= 1f;

                UpdateTimerUI();
                OnTimerTick?.Invoke(remainingTime);
            }

            StopTimer();
            OnTimerEnd?.Invoke();
        }

        private void UpdateTimerUI()
        {
            if (timerText == null) return;

            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void StopTimer() => isRunning = false;
        public float GetTotalGameTime() => totalGameTime;
        public float GetRemainingTime() => remainingTime;
    }
}