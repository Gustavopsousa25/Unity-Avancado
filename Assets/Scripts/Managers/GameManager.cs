using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int soulsShards = 0;
    private GameObject player;
    private UIManager uiManager;
    private RythmManager rythmManager;

    public GameObject Player { get => player; set => player = value; }

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        uiManager = UIManager.Instance;
        rythmManager = RythmManager.Instance;
        Player = FindObjectOfType<PlayerMovingState>().gameObject;

    }
    private void PlayerDied()
    {
        PlayerHealthComponent playerHp = Player.GetComponent<PlayerHealthComponent>();

    }
    private int SpendCurrency(int amount)
    {
        int total = soulsShards - amount;
        soulsShards = total;
        return soulsShards;
    }
}
