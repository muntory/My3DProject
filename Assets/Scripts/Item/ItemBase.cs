using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public SO_ItemData itemData;
    protected int id;
    protected string itemName;
    protected string itemDescription;
    protected StatEffect itemEffect;

    protected void UseItem(CharacterStat stat)
    {
        float result = 0f;

        switch (itemEffect.statType)
        {
            case StatType.Health:
                result = stat.Health;
                break;
            case StatType.WalkSpeed:
                result = stat.WalkSpeed;
                break;
        }

        if (itemEffect.isPercent)
        {
            result *= itemEffect.value;
        }
        else
        {
            result += itemEffect.value;
        }

        switch (itemEffect.statType)
        {
            case StatType.Health:
                stat.Health = result;
                break;
            case StatType.WalkSpeed:
                stat.WalkSpeed = result;
                break;
        }
    }

    protected IEnumerator ApplyEffectAndDestroy(CharacterStat stat)
    {
        yield return StartCoroutine(ApplyEffectForDuration(stat));
        Destroy(gameObject);
    }

    protected IEnumerator ApplyEffectForDuration(CharacterStat stat)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        float baseValue = stat.GetStat(itemEffect.statType).Value;
        Debug.Log(itemEffect.duration);

        while (elapsedTime < itemEffect.duration)
        {
            elapsedTime = Time.time - startTime;

            float multiplier = Mathf.Clamp01((itemEffect.duration - elapsedTime) / itemEffect.duration);
            float finalValue = itemEffect.value * multiplier;

            stat.SetStat(itemEffect.statType, baseValue + finalValue);

            yield return null;
        }

        stat.SetStat(itemEffect.statType, baseValue);

    }

}
