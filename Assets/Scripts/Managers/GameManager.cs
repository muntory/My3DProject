using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    List<PlayerCharacter> players = new List<PlayerCharacter>();

    protected override void Awake()
    {
        base.Awake();

        //test
        GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        UIManager.Instance.CreateUI<HealthBar>();

    }

    public void AddPlayer(PlayerCharacter player)
    {
        Debug.Assert(player != null);
        players.Add(player);
    }

    public PlayerCharacter GetPlayer(int id)
    {
        return players[id];
    }
}
