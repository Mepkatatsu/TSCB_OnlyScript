using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CoreLib
{
    public static class UnityHelper
    {
        public static void AddOnClickListener(this GameObject gameObject, UnityAction action)
        {
            if (!gameObject)
                return;

            if (!gameObject.TryGetComponent<Button>(out var button))
            {
                button = gameObject.AddComponent<Button>();
            }
            
            button.AddOnClickListener(action);
        }
        
        public static void AddOnClickListener(this Button button, UnityAction action)
        {
            if (!button)
                return;
            
            button.onClick.AddListener(action);
        }
        
        public static void AddOnValueChangedListener(this Slider slider, UnityAction<float> action)
        {
            if (!slider)
                return;
            
            slider.onValueChanged.AddListener(action);
        }

        public static void SetActiveSafe(this MonoBehaviour monoBehaviour, bool value)
        {
            if (!monoBehaviour)
                return;
            
            SetActiveSafe(monoBehaviour.gameObject, value);
        }
        
        public static void SetActiveSafe(this GameObject gameObject, bool value)
        {
            if (!gameObject)
                return;
            
            if (gameObject.activeSelf != value)
                gameObject.SetActive(value);
        }
    }
}
