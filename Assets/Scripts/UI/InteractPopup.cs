using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPopup : UIBase
{
    private IInteractable interactable;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
        nameText.text = interactable.GetName();
        descriptionText.text = interactable.GetDescription();
    }
}
