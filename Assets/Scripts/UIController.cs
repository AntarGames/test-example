using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject startPanel;

    public void HideStartPanel()
    {
        startPanel.SetActive(false);
    }
    public void PlayBtnClick()
    {
        GameManager.StartGame();
        HideStartPanel();
    }
}
