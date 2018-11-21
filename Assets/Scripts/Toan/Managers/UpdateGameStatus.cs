using EnumCollection;
using Manager;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGameStatus : MonoBehaviour
{
    public static UpdateGameStatus Instance { get; private set; }
    public List<Player> Players             { get; private set; }
    public bool GameIsRunning               { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);
        GameIsRunning = true;
        Players = new List<Player>();
        InvokeRepeating("UpdatePlayerState", 30.0f, 2.0f);
    }
    public void AddPlayer(Player player)
    {
        if(!Players.Contains(player))
        {
            Players.Add(player);
        }
    }
    private void UpdatePlayerState()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if(Players[i] != null && !Players[i].IsAlive())
            {
                Players[i].Lose();
                GameIsRunning = false;                
            }
        }
    }
}
