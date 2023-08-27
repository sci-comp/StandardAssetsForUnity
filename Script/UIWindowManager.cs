using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWindowManager : MonoBehaviour
{
    private readonly static List<UIWindow> windows = new();
    private EventSystem eventSystem;

    public bool AreWindowsOpen => windows.Count > 0;
    public UIWindow TopWindow => windows.Count > 0 ? windows[^1] : null;

    private void Start()
    {
        eventSystem = EventSystem.current;
        UIWindow[] windows = FindObjectsOfType<UIWindow>(true);

        foreach (UIWindow window in windows)
        {
            window.AlreadyStarted = true;
        }
    }

    private void Update()
    {
        // If a window is open, then make sure currentSelectedGameObject is not null.
        if (eventSystem != null && AreWindowsOpen)
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                GameObject currentlySelected = eventSystem.currentSelectedGameObject;
                if (!TopWindow.SelectableGameObjects.Contains(currentlySelected))
                { 
                    // The currently selected game object is not a child of the top window.
                    TopWindow.Focus();
                }
            }
            else
            {
                // Nothing is selected, yet a panel is open
                TopWindow.Focus();
            }
        }
    }

    public void RegisterWindow(UIWindow window)
    {
        windows.Add(window);
        UpdateWindowInteractableState();
        FocusTopWindow();
    }

    public void UnregisterWindow(UIWindow window)
    {
        windows.Remove(window);
        UpdateWindowInteractableState();
        FocusTopWindow();
    }

    private void UpdateWindowInteractableState()
    {
        UIWindow topWindow = TopWindow;
        foreach (UIWindow window in windows)
        {
            window.Interactable = window == topWindow;
        }
    }

    private void FocusTopWindow()
    {
        if (windows.Count == 0)
        {
            eventSystem.SetSelectedGameObject(null);
            return;
        }

        UIWindow topWindow = TopWindow;
        if (topWindow.gameObject.activeInHierarchy)
        {
            topWindow.Focus();
            topWindow.transform.SetAsLastSibling();
        }
        else
        {
            windows.RemoveAt(windows.Count - 1);
            FocusTopWindow();
        }
    }

}

