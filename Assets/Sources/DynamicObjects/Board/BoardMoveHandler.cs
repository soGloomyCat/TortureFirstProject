using System.Collections;
using UnityEngine;

public class BoardMoveHandler : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _points;

    private int _pointIndex;
    private Coroutine _moveCoroutine;

    private void OnEnable()
    {
        if (_points.Length == 0)
            throw new System.ArgumentNullException("Массив точек пуст. Проверьте редактор.");
    }

    private void Start()
    {
        _pointIndex = 0;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(Move());
    }

    private int GetCurrent(int index)
    {
        index++;

        if (index == _points.Length)
            return index = 0;

        return index;
    }

    private IEnumerator Move()
    {
        while (transform.position.x != _points[_pointIndex].transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, _points[_pointIndex].transform.position, _speed * Time.deltaTime);

            if (transform.position == _points[_pointIndex].transform.position)
                _pointIndex = GetCurrent(_pointIndex);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _pointIndex = GetCurrent(_pointIndex);
    }
}
