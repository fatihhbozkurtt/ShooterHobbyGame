using EssentialManagers.Scripts.SO;
using UnityEngine;
using UnityEngine.UI;

namespace EssentialManagers.Scripts.Managers
{
    public class SettingsPanelManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Button settingsOpenerButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private GameObject settingsPanel;

        private void Awake()
        {
            settingsOpenerButton.onClick.AddListener(ShowSettingsPanel);
            exitButton.onClick.AddListener(HideSettingsPanel);
        }

        private void ShowSettingsPanel()
        {
            settingsPanel.SetActive(true);
            AudioManager.instance.PlaySound(AudioTag.ButtonClick);
        }

        private void HideSettingsPanel()
        {
            settingsPanel.SetActive(false);
            AudioManager.instance.PlaySound(AudioTag.ButtonClick);
        }
    }
}