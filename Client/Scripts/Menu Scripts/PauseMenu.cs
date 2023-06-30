using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameDisplay;
    public GameObject howToPlayMenu1;
    public GameObject howToPlayMenu2;
    public GameObject heartDescriptionMenu;
    public GameObject tipsMenu;
    public GameObject systemMenu;
    public GameObject mainMenu;


    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        OpenhowToPlayMenu1();
    }
    public void ClosePauseMenu()
    {
        CloseAllSubMenus();
        pauseMenu.SetActive(false);
    }

    public void CloseAllSubMenus()
    {
        howToPlayMenu1.SetActive(false);
        howToPlayMenu2.SetActive(false);
        heartDescriptionMenu.SetActive(false);
        tipsMenu.SetActive(false);
        systemMenu.SetActive(false);

    }

    public void OpenhowToPlayMenu1()
    {
        CloseAllSubMenus();
        howToPlayMenu1.SetActive(true);
    }

    public void OpenhowToPlayMenu2()
    {
        CloseAllSubMenus();
        howToPlayMenu2.SetActive(true);
    }

    public void OpenheartDescriptionMenu()
    {
        CloseAllSubMenus();
        heartDescriptionMenu.SetActive(true);
    }

    public void OpentipsMenu()
    {
        CloseAllSubMenus();
        tipsMenu.SetActive(true);
    }

    public void OpensystemMenu()
    {
        CloseAllSubMenus();
        systemMenu.SetActive(true);
    }
}
