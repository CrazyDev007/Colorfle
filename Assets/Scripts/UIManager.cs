using System;
using System.Collections.Generic;
using UnityEngine;

namespace MB
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField] private List<UIScreenMapping> screenMappings;

        private readonly Dictionary<UIScreenType, UIScreen> _screens = new Dictionary<UIScreenType, UIScreen>();

        private UIScreen _activeScreen;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            foreach (var mapping in screenMappings)
            {
                if (mapping.screen != null && !_screens.ContainsKey(mapping.type))
                {
                    _screens.Add(mapping.type, mapping.screen);
                    if (mapping.isDefault)
                    {
                        ShowScreen(mapping.type);
                    }
                    else
                    {
                        HideScreen(mapping.type);
                    }
                }
            }
        }

        public void ShowScreen(UIScreenType screenType)
        {
            if (_screens.TryGetValue(screenType, out var screen))
            {
                screen.Show();
            }
            else
            {
                Debug.LogError($"Screen of type {screenType} not found.");
            }
        }

        public void HideScreen(UIScreenType screenType)
        {
            if (_screens.TryGetValue(screenType, out var screen))
            {
                screen.Hide();
            }
            else
            {
                Debug.LogError($"Screen of type {screenType} not found.");
            }
        }
    }

    public enum UIScreenType
    {
        Main,
        Win,
        Loose,
    }


    [Serializable]
    public struct UIScreenMapping
    {
        public UIScreenType type;
        public UIScreen screen;
        public bool isDefault;
    }
}