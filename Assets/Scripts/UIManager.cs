using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image healhBar;

    public void UpdateHpBar(float amount, float maxAmount)
    {
        healhBar.fillAmount = amount/maxAmount ;
    }
}
