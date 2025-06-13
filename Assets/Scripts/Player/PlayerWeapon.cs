using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    private PlayerAttackingState playerRef;
    private void Start()
    {
        playerRef = GameManager.Instance.Player.GetComponent<PlayerAttackingState>();
    }
}
