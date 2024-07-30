using UnityEngine;
using System;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Net.Http;

public class dadwasd : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Transform _prefab;
    private float _speed = 35f;
    private float _duration = 2f;

    void Start()
    {
        _button.onClick.AddListener(Function);
    }

    private async void Function()
    {
        _button.interactable = false;

        float timer = 0f;

        while(timer <  _duration)
        {
            _prefab.Rotate(Vector3.forward, _speed * Time.deltaTime);
            timer  += Time.deltaTime;   

            await Task.Yield();
        }

        _button.interactable = true;
    }

    HttpClient _client = new HttpClient();

    private async Task<int> Add(string url)
    {
        string resualy = await _client.GetStringAsync(url);

        int r = resualy.Length;

        return r;
    }
}
