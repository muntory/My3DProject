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

    // 코루틴 실행하고 Destroy하면 실행중인 코루틴도 사라져서 효과 적용 코루틴 끝날때까지 기다렸다가 destroy 
    // 아이템에서는 해당 아이템 데이터를 사용하는 캐릭터에게 전달만 해주고 캐릭터에서 적용시키는게 좋은 구조 같다.
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
