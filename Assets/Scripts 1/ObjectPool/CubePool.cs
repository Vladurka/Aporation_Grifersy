using UnityEngine;

public class CubePool : MonoBehaviour
{
    [SerializeField] private int _poolAmount = 3;
    [SerializeField] private bool _autoExpand = true;
    [SerializeField] private CubePrefab _cubePrefab;
    [SerializeField] private Transform[] _spawnPositions;

    private int _index = 0;

    private ObjectPool<CubePrefab> _pool;

    private void Awake()
    {
       _pool = new ObjectPool<CubePrefab>(_cubePrefab, _poolAmount, transform);
       _pool.AutoExpand = _autoExpand;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            CreateCube();
    }

    private void CreateCube()
    {
        var cube = _pool.GetFreeElement();
        cube.transform.position = _spawnPositions[_index].position;
    }
}
