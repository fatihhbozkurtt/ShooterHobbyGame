using System;
using EssentialManagers.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EssentialManagers.Scripts.Controllers
{
    public class SettingsItemController : MonoBehaviour
    {
        [Header("Config")] [SerializeField] private SettingsItemType type;

        [Header("References")] [SerializeField]
        private Button onButton;

        [SerializeField] private Button offButton;
        [SerializeField] private TextMeshProUGUI nameText;
        private string _itemSaveKey;

        private bool IsEnabled
        {
            get => PlayerPrefs.GetInt(_itemSaveKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(_itemSaveKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        private void Awake()
        {
            _itemSaveKey = $"{type}_Settings";
            nameText.text = type.ToString();

            onButton.onClick.AddListener(() => SetState(false));
            offButton.onClick.AddListener(() => SetState(true));
        }

        private void Start()
        {
            SetState(IsEnabled, false);
        }

        private void SetState(bool enable, bool save = true)
        {
            onButton.gameObject.SetActive(enable);
            offButton.gameObject.SetActive(!enable);

            switch (type)
            {
                case SettingsItemType.Sound:
                    AudioManager.instance.SetSoundBlocked(!enable);
                    break;
                case SettingsItemType.Music:
                    AudioManager.instance.SetMusicBlocked(!enable);
                    break;  
                case SettingsItemType.Haptics:
                    HapticManager.instance.SetHapticsStatus(!enable);
                    break;
            }

            Debug.Log($"{(enable ? "Enabled" : "Disabled")} settings item: {type}");

            if (save)
                IsEnabled = enable;
        }
    }
}

[Serializable]
public enum SettingsItemType
{
    Music,
    Sound,
    Haptics
}