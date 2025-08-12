using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase, IStat
{
    PlayerStat playerStat;

    private void Awake()
    {
        GameManager.Instance.AddPlayer(this);

        if (playerStat == null)
        {
            playerStat = GetComponent<PlayerStat>();
        }
    }

    public CharacterStat GetStatComponent()
    {
        return playerStat;
    }
}
