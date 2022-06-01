using System.Collections;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    private const float RotateAngle = 1;
    private const float DestroyDelay = 0.7f;

    [SerializeField] private DiamondSlice _slicePrefab;
    [Range(0, 12)]
    [SerializeField] private int _slicesCount;
    [SerializeField] ParticleSystem _particleSystem;

    private Transform _slicesPool;
    private Coroutine _coroutine;

    public void Split()
    {
        GenerateSlices();
        PrepairPickUp();
    }

    public void SetPool(Transform pool)
    {
        _slicesPool = pool;
    }

    public void PrepairPickUp()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(PickUp());
    }

    private IEnumerator PickUp()
    {
        WaitForSeconds waiter;

        waiter = new WaitForSeconds(DestroyDelay);
        _particleSystem.Play();

        yield return waiter;

        Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (_slicePrefab == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");
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

    private void FixedUpdate() => Rotate();

    private void Rotate() => transform.Rotate(Vector3.forward, RotateAngle);
}
