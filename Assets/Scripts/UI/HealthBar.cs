using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : UIBase
{
    [SerializeField]
    Image fillImage;

    IStat stat;

    private void Awake()
    {
        if (stat == null)
        {
            stat = GameManager.Instance.GetPlayer(0);
        }
    }

    private void OnEnable()
    {
        stat.GetStatComponent().onHealthChange += UpdateUI;
    }

    private void OnDisable()
    {
        stat.GetStatComponent().onHealthChange -= UpdateUI;
    }

    public void UpdateUI(float value)
    {
        CharacterStat characterStat = stat.GetStatComponent();
        Debug.Assert(characterStat != null);

        float percent = value / characterStat.MaxHealth;
        SetPercent(percent);
    }

    public void SetPercent(float percent)
    {
        percent = Mathf.Clamp01(percent);
        fillImage.fillAmount = percent;
    }
}
