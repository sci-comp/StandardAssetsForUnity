using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class UIWindow : MonoBehaviour
{
    [Title("UI Window", "Base class", TitleAlignments.Centered)]
    [SerializeField] string windowName = "Temporary Window Name";
    [SerializeField] bool startOpen = false;
    [SerializeField] bool hasACloseButton = true;
    [ShowIf("hasACloseButton"), SerializeField] Button closeButton = null;
    [SerializeField] Selectable firstSelected;
    [SerializeField] UIWindowManager windowManager;

    [Header("Animator")]
    [SerializeField] float fadeDuration = 0.25f;
    [SerializeField] string animatorTriggerOpen = "Open";
    [SerializeField] string animatorTriggerClose = "Close";

    private int enableAnimatorHash;
    private int disableAnimatorHash;
    private Animator animator;
    private CanvasGroup canvasGroup;
    private EventSystem eventSystem;

    public bool Interactable
    {
        get { return canvasGroup.interactable; }
        set { canvasGroup.interactable = value; }
    }

    public bool AlreadyStarted { get; set; } = false;
    public string WindowName => windowName;
    public List<GameObject> SelectableGameObjects { get; set; } = new();
    public List<Selectable> Selectables { get; set; } = new();
    public Selectable LastSelected { get; set; }

    private void Awake()
    {
        enableAnimatorHash = Animator.StringToHash(animatorTriggerOpen);
        disableAnimatorHash = Animator.StringToHash(animatorTriggerClose);

        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (hasACloseButton && closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
    }

    virtual protected void Start()
    {
        eventSystem = EventSystem.current;

        RefreshSelectables();

        LastSelected = firstSelected != null ? firstSelected : Selectables[0];

        if (startOpen)
        {
            EnableWindow();
        }
        else if (!startOpen && !AlreadyStarted)
        {
            animator.SetTrigger(disableAnimatorHash);
            canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }
    }

    public void Focus()
    {
        if (LastSelected != null)
        {
            eventSystem.SetSelectedGameObject(LastSelected.gameObject);
        }
        else if (firstSelected != null)
        {
            eventSystem.SetSelectedGameObject(firstSelected.gameObject);
        }
        else
        {
            RefreshSelectables();

            if (Selectables.Count > 0)
            {
                eventSystem.SetSelectedGameObject(Selectables[0].gameObject);
            }
            else
            {
                // Nothing can be selected
                eventSystem.SetSelectedGameObject(null);
            }
        }
    }

    public void RefreshSelectables()
    {
        Selectables.Clear();
        SelectableGameObjects.Clear();

        Selectable[] allSelectables = GetComponentsInChildren<Selectable>();

        foreach (Selectable selectable in allSelectables)
        {
            if (selectable.interactable)
            {
                Selectables.Add(selectable);
                SelectableGameObjects.Add(selectable.gameObject);
            }
        }
    }

    #region Enable/Disable

    public void EnableWindow()
    {
        StartCoroutine(EnableAfterFadeIn());
    }

    public void DisableWindow()
    {
        StartCoroutine(DisableAfterFadeOut());
    }

    private IEnumerator EnableAfterFadeIn()
    {
        animator.SetTrigger(enableAnimatorHash);
        yield return new WaitForSeconds(fadeDuration);
        windowManager.RegisterWindow(this);
    }

    private IEnumerator DisableAfterFadeOut()
    {
        animator.SetTrigger(disableAnimatorHash);
        canvasGroup.interactable = false;
        yield return new WaitForSeconds(fadeDuration);
        windowManager.UnregisterWindow(this);
        gameObject.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        DisableWindow();
    }

    #endregion

}

