using UnityEngine;

public class StartLastMission : MonoBehaviour
{
    [SerializeField] private GameObject _text;

    private PapichMovement _papich;
    private void Start()
    {
        _papich = ServiceLocator.Current.Get<PapichMovement>();

        _papich.CurrentState = PapichMovement.State.Hide;

        _text.SetActive(true);
    }
}
