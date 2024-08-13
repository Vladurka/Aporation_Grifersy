using UnityEngine;

public class Door : MonoBehaviour, IDoor
{
    [SerializeField] private Animator _animator;

    private bool _open = true;

    public void Open()
    {
        if (_open)
        {
            _animator.SetBool("Open", false);
            _open = false;
        }

        if (!_open)
        {
            _animator.SetBool("Open", true);
            _open = true;
        }
    }
}
