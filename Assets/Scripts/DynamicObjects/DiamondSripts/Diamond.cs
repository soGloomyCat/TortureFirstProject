using UnityEngine;

public class Diamond : MonoBehaviour
{
    private const float _rotateAngle = 1;

    [SerializeField] private DiamondSlice _slicePrefab;
    [Range(0, 12)]
    [SerializeField] private int _slicesCount;

    private Transform _slicesPool;

    private void OnEnable()
    {
        if (_slicePrefab == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    public void SetPool(Transform pool)
    {
        _slicesPool = pool;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }

    public void Split()
    {
        GenerateSlices();
        PickUp();
    }

    private void GenerateSlices()
    {
        DiamondSlice tempSlice;

        for (int i = 0; i < _slicesCount; i++)
        {
            tempSlice = Instantiate(_slicePrefab, _slicesPool);
            tempSlice.transform.position = transform.position;
            tempSlice.Dump();
        }
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward, _rotateAngle);
    }
}
