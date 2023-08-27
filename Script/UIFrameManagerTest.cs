using UnityEngine;
using UnityEngine.UI;

public class UIFrameManagerTest : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup = null;

    [SerializeField] Button btnShowFrame01 = null;
    [SerializeField] Button btnShowFrame02 = null;
    [SerializeField] Button btnShowFrame03 = null;

    [SerializeField] UIFrame frame01 = null;
    [SerializeField] UIFrame frame02 = null;
    [SerializeField] UIFrame frame03 = null;

    private void Start()
    {
        btnShowFrame01.onClick.AddListener(ButtonShowFrame01);
        btnShowFrame02.onClick.AddListener(ButtonShowFrame02);
        btnShowFrame03.onClick.AddListener(ButtonShowFrame03);
    }

    private void ButtonShowFrame01()
    {
        SetButtonsInactive();
        frame01.gameObject.SetActive(true);
        frame01.EnableFrame();
    }

    private void ButtonShowFrame02()
    {
        SetButtonsInactive();
        frame02.gameObject.SetActive(true);
        frame02.EnableFrame();
    }

    private void ButtonShowFrame03()
    {
        SetButtonsInactive();
        frame03.gameObject.SetActive(true);
        frame03.EnableFrame();
    }

    private void SetButtonsInactive()
    {
        canvasGroup.interactable = false;
    }
}

