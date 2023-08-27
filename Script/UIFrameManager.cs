using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFrameManager : MonoBehaviour
{
    private readonly static List<UIFrame> frames = new();
    private EventSystem eventSystem;

    public bool AreFramesOpen => frames.Count > 0;
    public UIFrame TopFrame => frames.Count > 0 ? frames[^1] : null;

    private void Start()
    {
        eventSystem = EventSystem.current;
        UIFrame[] frames = FindObjectsOfType<UIFrame>(true);

        foreach (UIFrame frame in frames)
        {
            frame.AlreadyStarted = true;
        }
    }

    private void Update()
    {
        // If a frame is open, then make sure currentSelectedGameObject is not null.
        if (eventSystem != null && AreFramesOpen)
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                GameObject currentlySelected = eventSystem.currentSelectedGameObject;
                if (!TopFrame.SelectableGameObjects.Contains(currentlySelected))
                { 
                    // The currently selected game object is not a child of the top frame.
                    TopFrame.Focus();
                }
            }
            else
            {
                // Nothing is selected, yet a panel is open
                TopFrame.Focus();
            }
        }
    }

    public void RegisterFrame(UIFrame frame)
    {
        frames.Add(frame);
        UpdateFrameInteractableState();
        FocusTopFrame();
    }

    public void UnregisterFrame(UIFrame frame)
    {
        frames.Remove(frame);
        UpdateFrameInteractableState();
        FocusTopFrame();
    }

    private void UpdateFrameInteractableState()
    {
        UIFrame topFrame = TopFrame;
        foreach (UIFrame frame in frames)
        {
            frame.Interactable = frame == topFrame;
        }
    }

    private void FocusTopFrame()
    {
        if (frames.Count == 0)
        {
            eventSystem.SetSelectedGameObject(null);
            return;
        }

        UIFrame topFrame = TopFrame;
        if (topFrame.gameObject.activeInHierarchy)
        {
            topFrame.Focus();
            topFrame.transform.SetAsLastSibling();
        }
        else
        {
            frames.RemoveAt(frames.Count - 1);
            FocusTopFrame();
        }
    }
}

