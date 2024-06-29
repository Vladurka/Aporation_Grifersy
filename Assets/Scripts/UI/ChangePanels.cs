using UnityEngine;

public class ChangePanels : MonoBehaviour
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