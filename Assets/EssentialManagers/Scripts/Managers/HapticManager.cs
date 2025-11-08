// using MoreMountains.NiceVibrations;

namespace EssentialManagers.Scripts.Managers
{
    public class HapticManager : MonoSingleton<HapticManager>
    {
        public bool HapticsEnabled { get; private set; } = true;
        
        // public void PlayHaptic(HapticTypes type)
        // {
        //     if (!HapticsEnabled) return;
        //
        //     MMVibrationManager.Haptic(type);
        // }

        public void SetHapticsStatus(bool isEnable)
        {
            HapticsEnabled = isEnable;
        }
    }
}