using System;
using EssentialManagers.Scripts.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EssentialManagers.Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public event Action LevelStartedEvent;
        public event Action LevelEndedEvent; // fired regardless of fail or success
        public event Action LevelSuccessEvent; // fired only on success
        public event Action LevelFailedEvent; // fired only on fail
        public event Action LevelAboutToChangeEvent; // fired just before next level load

        public static readonly string lastPlayedStageKey = "n_lastPlayedStage";
        public static readonly string randomizeStagesKey = "n_randomizeStages";
        public static readonly string cumulativeStagePlayedKey = "n_cumulativeStages";

        [HideInInspector] public bool isLevelActive;
        [HideInInspector] public bool isLevelSuccessful;

        [Header("Debug")] [SerializeField] private LevelDataSO levelDataSo;
    
        protected override void Awake()
        {
            base.Awake();
            LoadLevelDataList();

            if (!PlayerPrefs.HasKey(cumulativeStagePlayedKey)) PlayerPrefs.SetInt(cumulativeStagePlayedKey, 0);

            Application.targetFrameRate = 120;
            QualitySettings.vSyncCount = 0;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void LoadLevelDataList()
        {
            levelDataSo = Resources.Load<LevelDataSO>("LevelData");

            if (levelDataSo == null)
            {
                Debug.LogError("Failed to load LevelDataSO! Make sure it's in the Resources folder and named correctly.");
                return;
            }

            Debug.Log("LevelDataSO loaded successfully!");

            InstantiateLevel(GetTotalStagePlayed());
        }

        private void InstantiateLevel(int targetIndex)
        {
            GameObject levelPrefab = levelDataSo.levelData[targetIndex].LevelPrefab;
            Instantiate(levelPrefab);

            Debug.LogWarning("LevelPrefab Instantiated: " + levelPrefab.name);
        }

        public void StartGame()
        {
            isLevelActive = true;
            LevelStartedEvent?.Invoke();
        }

        public void EndGame(bool success)
        {
            isLevelActive = false;
            isLevelSuccessful = success;

            LevelEndedEvent?.Invoke();
            if (success)
            {
                LevelSuccessEvent?.Invoke();
            }
            else
            {
                LevelFailedEvent?.Invoke();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N)) NextLevel();
            if (Input.GetKeyDown(KeyCode.P)) PreviousLevel();
            if (Input.GetKeyDown(KeyCode.W)) EndGame(true);
            if (Input.GetKeyDown(KeyCode.F)) EndGame(false);
        }

        public void NextLevel()
        {
            //Analytics.LevelPassed(PlayerPrefs.GetInt(cumulativeStagePlayedKey));

            int nextLevelIndex = GetTotalStagePlayed() + 1;

            if (nextLevelIndex <= levelDataSo.levelData.Count - 1)
                PlayerPrefs.SetInt(cumulativeStagePlayedKey, GetTotalStagePlayed() + 1);
            else
            {
                nextLevelIndex = 0;
                PlayerPrefs.SetInt(cumulativeStagePlayedKey, 0);
            }

            InstantiateLevel(nextLevelIndex);

            PlayerPrefs.SetInt(lastPlayedStageKey, nextLevelIndex);
            LevelAboutToChangeEvent?.Invoke();
        }
    
        public void PreviousLevel()
        {
            //Analytics.LevelPassed(PlayerPrefs.GetInt(cumulativeStagePlayedKey));

            int prevLevelIndex = GetTotalStagePlayed() - 1;

            if (prevLevelIndex >= 0)
                PlayerPrefs.SetInt(cumulativeStagePlayedKey, prevLevelIndex);
            else
            {
                prevLevelIndex = 0;
                PlayerPrefs.SetInt(cumulativeStagePlayedKey, 0);
            }

            InstantiateLevel(prevLevelIndex);

            PlayerPrefs.SetInt(lastPlayedStageKey, prevLevelIndex);
            LevelAboutToChangeEvent?.Invoke();
        }

        public void RestartStage()
        {
            LevelAboutToChangeEvent?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public int GetTotalStagePlayed()
        {
            return PlayerPrefs.GetInt(cumulativeStagePlayedKey, 0);
        }
    }
}