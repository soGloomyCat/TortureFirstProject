using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DiamondSlice : MonoBehaviour
{
    private const float _delay = 1f;
    private const float _minSideOffset = -0.8f;
    private const float _maxSideOffset = 0.8f;
    private const float _minVerticalOffset = 0;
    private const float _maxVerticalOffset = 1f;
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
        _rigidbody.AddForce(new Vector3(Random.Range(_minSideOffset, _maxSideOffset),
                                        Random.Range(_minVerticalOffset, _maxVerticalOffset),
                                        Random.Range(_minHorizontalOffset, _maxHorizontalOffset)), ForceMode.Force);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float timer;
        Vector3 finishPoint;

        timer = 0;
        finishPoint = new Vector3(10, 10, 0);


        while (timer <= _delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _rigidbody.isKinematic = true;

        while (transform.position != finishPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, finishPoint, _speed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}
