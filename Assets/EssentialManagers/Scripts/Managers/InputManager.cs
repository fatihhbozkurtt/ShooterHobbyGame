using UnityEngine;

namespace EssentialManagers.Scripts.Managers
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public bool IsFiring { get; private set; }

        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            IsFiring = Input.GetMouseButton(0);
#elif UNITY_ANDROID || UNITY_IOS
            IsFiring = UnityEngine.Input.touchCount > 0;
#endif
        }
    }
}