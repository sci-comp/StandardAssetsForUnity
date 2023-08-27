using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class UIFrame : MonoBehaviour
{
    [SerializeField] bool startOpen = false;
    [SerializeField] bool hasACloseButton = true;
    [ShowIf("hasACloseButton"), SerializeField] Button closeButton = null;
    [SerializeField] float fadeDuration = 0.25f;
    [SerializeField] string panelName = "Temporary Panel Name";
    [SerializeField] string animatorTriggerOpen = "Open";
    [SerializeField] string animatorTriggerClose = "Close";
    [SerializeField] Selectable firstSelected;
    [SerializeField] UIFrameManager canvasManager;
    
    private int enableAnimatorHash;
    private int disableAnimatorHash;
    private Animator animator;
    private CanvasGroup canvasGroup;
    private EventSystem eventSystem;
    private Selectable lastSelected;

    public bool AlreadyStarted { get; set; } = false;
    public string PanelName => panelName;
    public List<GameObject> SelectableGameObjects { get; set; } = new();
    public List<Selectable> Selectables { get; set; } = new();

    public bool Interactable
    {
        get { return canvasGroup.interactable; }
        set { canvasGroup.interactable = value; }
    }

    private void Awake()
    {
        enableAnimatorHash = Animator.StringToHash(animatorTriggerOpen);
        disableAnimatorHash = Animator.StringToHash(animatorTriggerClose);

        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        eventSystem = EventSystem.current;

        if (hasACloseButton && closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        RefreshSelectables();

        lastSelected = firstSelected != null ? firstSelected : Selectables[0];

        if (startOpen)
        {
            EnableFrame();
        }
        else if (!startOpen && !AlreadyStarted)
        {
            Debug.Log("Start disable");
            animator.SetTrigger(disableAnimatorHash);
            canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            lastSelected = eventSystem.currentSelectedGameObject.GetComponent<Selectable>();
        }
    }

    private void OnCloseButtonClicked()
    {
        DisableFrame();
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

    public void EnableFrame()
    {
        StartCoroutine(EnableAfterFadeIn());
    }

    public void DisableFrame()
    {
        StartCoroutine(DisableAfterFadeOut());
    }

    private IEnumerator EnableAfterFadeIn()
    {
        animator.SetTrigger(enableAnimatorHash);
        yield return new WaitForSeconds(fadeDuration);
        canvasManager.RegisterFrame(this);
    }

    private IEnumerator DisableAfterFadeOut()
    {
        animator.SetTrigger(disableAnimatorHash);
        canvasGroup.interactable = false;
        yield return new WaitForSeconds(fadeDuration);
        canvasManager.UnregisterFrame(this);
        gameObject.SetActive(false);
    }

    public void Focus()
    {
        if (lastSelected != null)
        {
            eventSystem.SetSelectedGameObject(lastSelected.gameObject);
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
}

