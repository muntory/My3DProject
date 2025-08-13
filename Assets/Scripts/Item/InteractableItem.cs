using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : ItemBase, IInteractable
{
    private void Awake()
    {
        Initialize();
    }

    public string GetDescription()
    {
        return itemDescription;
    }

    public string GetName()
    {
        return itemName;
    }

    public void Initialize()
    {
        id = itemData.ID;
        itemName = itemData.Name;
        itemDescription = itemData.Description;
        itemEffect = itemData.statEffect;
        
    }

    public void OnInteract(CharacterBase instigator)
    {
        CharacterStat statComponent = instigator.GetComponent<CharacterStat>();

        if (itemEffect.hasDuration)
        {
            StartCoroutine(ApplyEffectAndDestroy(statComponent));
        }
        else
        {
            UseItem(statComponent);
            Destroy(gameObject);

        }

    }
    
}
