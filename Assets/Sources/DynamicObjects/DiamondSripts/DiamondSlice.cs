using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DiamondSlice : MonoBehaviour
{
    private const float Delay = 2f;
    private const float SphereRadius = 100f;

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
        _rigidbody.AddForce(Random.insideUnitSphere * SphereRadius, ForceMode.Force);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        WaitForSeconds waiter;
        Vector3 finishPoint;

        waiter = new WaitForSeconds(Delay);
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
