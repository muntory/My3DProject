using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapItem : ItemBase
{
    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        id = itemData.ID;
        itemName = itemData.Name;
        itemDescription = itemData.Description;
        itemEffect = itemData.statEffect;

    }

    // 아이템 효과를 적용하는 코루틴은 캐릭터쪽에서 하는게 더 좋은 듯
    // 이렇게 하면 코루틴 실행되는 동안 아이템이 레벨에 계속 남아 있게 됨
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent<PlayerStat>(out PlayerStat playerStat))
        {
            if (itemEffect.hasDuration)
            {
                StartCoroutine(ApplyEffectAndDestroy(playerStat));
            }
            else
            {
                UseItem(playerStat);
                Destroy(gameObject);
            }
            

        }
    }

    
    
}
