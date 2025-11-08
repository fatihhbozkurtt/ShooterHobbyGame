using System.Collections;
using DG.Tweening;
using EssentialManagers.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EssentialManagers.CustomPackages.ResourceManager.Scripts
{
    public enum ResourceType
    {
        Gold,
        Diamond,
        Money,
        Hp
    }

    public class ResourceController : MonoBehaviour
    {
        public delegate void ResourceChangedDelegate(ResourceController resource);

        public delegate void DelayedResourceChangedDelegate(ResourceController resource);

        public delegate void ResourceSpendCallbackDelegate(Transform spendTarget);

        public delegate void ResourceAddCallbackDelegate();

        public event ResourceChangedDelegate ResourceChangedEvent;
        public event DelayedResourceChangedDelegate ResourceChangedDelayedEvent;

        [Header("Config")] public int startingCount;
        public ResourceType resourceType;

        [Header("Editor")] [SerializeField] bool overrideEditorCount;
        [SerializeField] int editorStartingCount;

        [Header("References")] public RectTransform resourceIcon;
        public TextMeshProUGUI resourceCountText;

        [Header("Debug")] [SerializeField] bool needsSave;
        [SerializeField] int resourceCount;
        [SerializeField] float lastSaveTimestamp;
        [SerializeField] string saveName;
        [SerializeField] public Sprite iconSprite;

        private void Awake()
        {
            saveName = "n_" + resourceType + "_count";
            iconSprite = resourceIcon.GetComponent<Image>().sprite;
            resourceIcon.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;

            if (Application.isEditor & overrideEditorCount)
            {
                resourceCount = editorStartingCount;
            }

            else
            {
                resourceCount = PlayerPrefs.GetInt(saveName, startingCount);
            }

            PerformSave();
            UpdateText();
        }

        private void Update()
        {
            if (lastSaveTimestamp < Time.time & needsSave)
            {
                lastSaveTimestamp = Time.time + 0.2f;
                needsSave = false;

                PerformSave();

                ResourceChangedDelayedEvent?.Invoke(this);
            }
        }

        #region Adders

        public void AddResource(int amount)
        {
            ModifyResource(amount);
            AnimateIconAndText(amount > 0 ? Color.green : Color.red);
        }

        public void AddResource(int amount, Vector3 worldPos, bool showAmount = false,
            ResourceAddCallbackDelegate callback = null)
        {
            Vector3 screenPos = CameraManager.instance.mainCam.WorldToScreenPoint(worldPos);

            RectTransform clonedIcon;
            if (showAmount) clonedIcon = CloneResourceIcon(amount);
            else clonedIcon = CloneResourceIcon();

            clonedIcon.position = screenPos;
            MoveCloneAndCollect(amount, clonedIcon, callback: callback);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void AddScatteredResource(int amount, Vector3 worldPos, int iconCount = 50, float scatterScale = 10f,
            ResourceAddCallbackDelegate callback = null, float iconScale = 1, float moveDelay = 0.25f)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            for (int i = 0; i < iconCount; i++)
            {
                RectTransform clonedIcon = CloneResourceIcon(scale: iconScale);

                Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                Vector3 randomDir = randomRot * Vector3.up;
                randomDir *= Screen.width * Random.Range(0.1f, 0.4f) * scatterScale;
                Vector3 randomPos = screenPos + randomDir;

                clonedIcon.position = screenPos;

                Tween tween = clonedIcon.DOMove(randomPos, 0.5f + Random.Range(-0.1f, 0.1f))
                    .SetEase(Ease.OutCubic);

                if (i == iconCount - 1)
                {
                    tween.OnComplete(() => { MoveCloneAndCollect(amount, clonedIcon, callback: callback, moveDelay); });
                }

                else
                {
                    tween.OnComplete(() => { MoveCloneAndCollect(0, clonedIcon); });
                }
            }
        }

        #endregion

        #region Spenders

        public bool TrySpendResource(int amount)
        {
            if (resourceCount >= amount)
            {
                ModifyResource(-amount);
                AnimateIconAndText(Color.red);

                return true;
            }

            else
            {
                return false;
            }
        }

        public bool TrySpendResource(int amount, Transform spendTarget, ResourceSpendCallbackDelegate callback,
            bool showAmount = false)
        {
            bool isSpent = TrySpendResource(amount);

            if (isSpent)
            {
                RectTransform clonedIcon;
                if (showAmount) clonedIcon = CloneResourceIcon(-amount);
                else clonedIcon = CloneResourceIcon();

                StartCoroutine(SpendIconAnimationRoutine(spendTarget, callback, clonedIcon));
            }

            return isSpent;
        }

        #endregion

        #region Getters/Setters

        public void SetResource(int count)
        {
            resourceCount = count;

            UpdateText();
            SaveCurrency();

            ResourceChangedEvent?.Invoke(this);
        }

        public void ModifyResource(int amount)
        {
            if (amount == 0) return;
            SetResource(resourceCount + amount);
        }

        public int GetResource()
        {
            return resourceCount;
        }

        #endregion

        #region Internal

        private RectTransform CloneResourceIcon(int amount)
        {
            RectTransform clonedIconWithAmount = CloneResourceIcon();

            TextMeshProUGUI iconText = clonedIconWithAmount.GetComponentInChildren<TextMeshProUGUI>();

            if (amount > 0)
            {
                iconText.text = "+" + amount;
                iconText.color = Color.green;
            }

            else if (amount < 0)
            {
                iconText.text = amount.ToString();
                iconText.color = Color.red;
            }

            return clonedIconWithAmount;
        }

        private RectTransform CloneResourceIcon(float scale = 1.5f)
        {
            RectTransform clonedIcon = Instantiate(resourceIcon, resourceIcon.parent);

            clonedIcon.DOScale(scale, 0.1f)
                .From(0);

            return clonedIcon;
        }

        private void MoveCloneAndCollect(int amount, RectTransform clonedIcon,
            ResourceAddCallbackDelegate callback = null, float moveDelay = 0.5f)
        {
            float moveDuration = 0.5f;
            float scaleDelay = moveDuration + moveDelay;

         
            clonedIcon.DOLocalMove(resourceIcon.localPosition, moveDuration)
                .SetEase(Ease.InOutCubic)
                .SetDelay(moveDelay)
                .OnComplete(() =>
                {
                    ModifyResource(amount);
                    AnimateIconAndText(Color.green);
                    callback?.Invoke();
                });

            clonedIcon.DOScale(0, 0.1f)
                .SetDelay(scaleDelay);

            Destroy(clonedIcon.gameObject, 2);
        }

        private IEnumerator SpendIconAnimationRoutine(Transform target, ResourceSpendCallbackDelegate callback,
            RectTransform clonedIcon)
        {
            Camera mainCam = CameraManager.instance.mainCam;

            clonedIcon.DOScale(0, 0.15f)
                .SetDelay(0.7f);

            yield return new WaitForSeconds(0.25f);

            Vector3 startPos = resourceIcon.position;

            float duration = 0.5f;
            float currentDuration = 0;

            while (currentDuration < duration)
            {
                float t = currentDuration / duration;

                Vector3 endPos = mainCam.WorldToScreenPoint(target.position);
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, t * t);
                clonedIcon.position = currentPos;

                currentDuration += Time.deltaTime;
                yield return null;
            }

            clonedIcon.position = mainCam.WorldToScreenPoint(target.position);

            yield return null;

            clonedIcon.gameObject.SetActive(false);
            Destroy(clonedIcon.gameObject, 1f);

            callback?.Invoke(target);
        }

        private void AnimateIconAndText(Color textFlashColor)
        {
            resourceCountText.DOColor(Color.white, 0.5f)
                .From(textFlashColor);

            resourceIcon.DOScale(0.55f, 0.05f)
                .From(1)
                .OnComplete(() =>
                {
                    resourceIcon.DOScale(1, 0.2f)
                        .From(0.55f)
                        .SetEase(Ease.OutBack)
                        .easeOvershootOrAmplitude = 2;
                })
                .SetEase(Ease.OutCubic);
        }

        private void UpdateText()
        {
            resourceCountText.text = resourceCount.ToString();
        }

        #endregion

        #region Saving

        private void SaveCurrency()
        {
            needsSave = true;
        }

        private void PerformSave()
        {
            PlayerPrefs.SetInt(saveName, resourceCount);
        }

        #endregion
    }
}