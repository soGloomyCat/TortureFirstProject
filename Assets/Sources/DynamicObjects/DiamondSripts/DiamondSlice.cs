using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DiamondSlice : MonoBehaviour
{
    private const float _delay = 2f;
    private const float _minSideOffset = -1.5f;
    private const float _maxSideOffset = 1.5f;
    private const float _minVerticalOffset = 0;
    private const float _maxVerticalOffset = 0.5f;
    private const float _minHorizontalOffset = -1.5f;
    private const float _maxHorizontalOffset = 1.5f;

    [Range(0, 100)]
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Dump()
    {
        _rigidbody.AddForce(Random.insideUnitSphere * 50, ForceMode.Force);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        WaitForSeconds waiter;
        float timer;
        Vector3 finishPoint;

        waiter = new WaitForSeconds(_delay);
        timer = 0;
        finishPoint = new Vector3(10, 10, 0);

        yield return waiter;

        _rigidbody.isKinematic = true;

        while (transform.position != finishPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, finishPoint, _speed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}
