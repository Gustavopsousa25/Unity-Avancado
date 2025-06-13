using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image healhBar;
    [SerializeField] private GameObject deathScreenUI, pauseScreenUI, currencyUI;

    protected override void Awake()
    {
        base.Awake();
        deathScreenUI.SetActive(false);
    }
    private void Start()
    {
        HideHpbar();
        HidePauseMenu();
        HideDeathScreen();
        HideCurrency();
    }
    public void UpdateHpBar(float amount, float maxAmount)
    {
        healhBar.fillAmount = amount/maxAmount ;
    }
    public void ShowDeathScreen()
    {
        deathScreenUI.SetActive(true);
    }
    public void HideDeathScreen()
    {
        deathScreenUI.SetActive(false);
    }
    public void ShowPauseMenu()
    {

    }
    public void HidePauseMenu()
    {

    }
    public void ShowHpBar()
    {
        healhBar.gameObject.SetActive(true);
    }
    public void HideHpbar()
    {
        healhBar.gameObject.SetActive(false);
    }
    public void ShowCurrency()
    {

    }
    public void HideCurrency()
    {

    }
}
