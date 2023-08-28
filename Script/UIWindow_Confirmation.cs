using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Sirenix.OdinInspector;

public class UIWindow_Confirmation : UIWindow
{
    [Title("UI Window - Confirmation", "Derived class", TitleAlignments.Centered)]
    [SerializeField] TMP_Text messageText;
    [SerializeField] Button btnConfirm;

    public Action OnConfirmClicked = null;

    protected override void Start()
    {
        base.Start();
        btnConfirm.onClick.AddListener(ConfirmButtonClicked);
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }

    private void ConfirmButtonClicked()
    {
        OnConfirmClicked?.Invoke();
    }

}

