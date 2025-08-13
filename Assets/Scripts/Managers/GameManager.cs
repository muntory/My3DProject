using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    List<PlayerCharacter> players = new List<PlayerCharacter>();

    protected override void Awake()
    {
        base.Awake();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject go = Resources.Load<GameObject>("Prefabs/Player");
        PlayerCharacter player = Instantiate(go).GetComponent<PlayerCharacter>();
        
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
