using UnityEngine;

namespace CoreLib.Module
{
    // Singleton으로 사용할 MonoBehaviour
    public abstract class ModuleBase : MonoBehaviour
    {
        public abstract ModulePhase GetInitializePhase();
        public abstract void OnInitialize();
        public abstract void OnRelease();
    }

    public abstract class ModuleBase<T> : ModuleBase where T : ModuleBase<T>
    {
        public static T? Instance => ModuleManager.GetModule<T>(typeof(T));
    }
}