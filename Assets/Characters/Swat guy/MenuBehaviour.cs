using System.Collections;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    private Animator _animator;
    private int _index;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(10f);

        _index = Random.Range(0, 4);
        if(_index == 0 )
            _animator.SetTrigger("sho");

        if(_index == 1 )
            _animator.SetTrigger("rotation");

        if (_index == 2)
            _animator.SetTrigger("backflip");

        if (_index == 3)
            _animator.SetTrigger("warming");

        StartCoroutine(Animation());
    }
}