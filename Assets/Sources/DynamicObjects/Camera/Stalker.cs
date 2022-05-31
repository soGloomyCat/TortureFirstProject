using System.Collections;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    private const float _offset = 1;

    [Range(0, 100)]
    [SerializeField] private float _speed;

    private Coroutine _coroutine;

    public void OnMoveHandler()
    {
        Vector3 tempFinalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + _offset);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Move(tempFinalPosition));
    }

    private IEnumerator Move(Vector3 finalPosition)
    {
        while (transform.position != finalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPosition, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
