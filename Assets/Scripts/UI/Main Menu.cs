using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void ChangePanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void BackChangePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

}