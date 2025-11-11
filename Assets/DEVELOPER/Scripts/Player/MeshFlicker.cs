using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using EssentialManagers.Scripts.Managers;

namespace DEVELOPER.Scripts.Player
{
    public class MeshFlicker : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float flickerDuration = 3f;
        [SerializeField] private float flickerInterval = 0.15f;

        [Header("Debug")]
        [SerializeField] private MeshRenderer meshRenderer;

        private PlayerHealthHandler _playerHealthHandler;
        private bool isFlickering;
        private CancellationTokenSource flickerCTS;

        private void Awake()
        {
            _playerHealthHandler = GetComponentInParent<PlayerHealthHandler>();
            _playerHealthHandler.OnDamaged += StartFlicker;
            meshRenderer = GetComponent<MeshRenderer>();

            GameManager.instance.LevelEndedEvent += StopFlicker;
        }

        private void OnDestroy()
        {
            GameManager.instance.LevelEndedEvent -= StopFlicker;
        }

        private void StartFlicker()
        {
            if (isFlickering) return;

            flickerCTS = new CancellationTokenSource();
            FlickerAsync(flickerCTS.Token).Forget();
        }

        private async UniTaskVoid FlickerAsync(CancellationToken token)
        {
            isFlickering = true;
            float elapsed = 0f;

            try
            {
                while (elapsed < flickerDuration)
                {
                    token.ThrowIfCancellationRequested();

                    ToggleMeshes(false);
                    await UniTask.Delay((int)(flickerInterval * 1000), cancellationToken: token);

                    ToggleMeshes(true);
                    await UniTask.Delay((int)(flickerInterval * 1000), cancellationToken: token);

                    elapsed += flickerInterval * 2f;
                }
            }
            catch (OperationCanceledException)
            {
                ToggleMeshes(true); // Mesh açık kalsın
            }

            isFlickering = false;
            _playerHealthHandler.ResetInvincibility();
        }

        private void StopFlicker()
        {
            if (isFlickering)
            {
                flickerCTS?.Cancel();
                flickerCTS?.Dispose();
                flickerCTS = null;
            }
        }

        private void ToggleMeshes(bool visible)
        {
            meshRenderer.enabled = visible;
        }
    }
}

