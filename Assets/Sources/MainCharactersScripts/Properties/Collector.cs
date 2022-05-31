using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Collector : MonoBehaviour
{
    private const int _inviolableObjectsCount = 1;
    private const int _offsetCount = 1;
    private const int _minValueForChangeSpawnType = 1;
    private const int _triggerValue = 0;

    [SerializeField] private AwakenedCharacter _prefab;
    [SerializeField] private Transform _awakenedPool;
    [SerializeField] private Material _currentMaterial;

    private int _countCubesCollected;
    private float _offset;
    private Rigidbody _rigidbody;
    private List<AwakenedCharacter> _awakenedArray;
    private Coroutine _coroutine;

    public event Action<Transform> AddedNewAwakend;
    public event Action<Transform> RemovedLastAwakend;

    private void OnEnable()
    {
        if (_prefab == null || _awakenedPool == null || _currentMaterial == null)
            throw new ArgumentNullException("Отсутствует один из обязательных параметров. Проверьте редактор");
    }

    private void Start()
    {
        _countCubesCollected = 0;
        _offset = transform.localScale.y;
        _rigidbody = GetComponent<Rigidbody>();
        _awakenedArray = new List<AwakenedCharacter>();
    }

    public void OnTrapHandler()
    {
        if (_countCubesCollected > 0)
        {
            if (_awakenedArray.Count >= 2)
                RemovedLastAwakend?.Invoke(_awakenedArray[_awakenedArray.Count - _offsetCount * 2].transform);
            else
                RemovedLastAwakend?.Invoke(transform);

            Destroy(_awakenedArray[_awakenedArray.Count - _offsetCount].gameObject);
            _awakenedArray.RemoveAt(_awakenedArray.Count - _offsetCount);
            _countCubesCollected--;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out SleepersCell sleepersCell) && sleepersCell.SleepersCount > _triggerValue)
        {
            sleepersCell.DestroyAvailableSleepers();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(WakeUpSleepers(sleepersCell.transform.childCount - _inviolableObjectsCount));
        }
        else if (other.transform.TryGetComponent(out Diamond diamond))
        {
            if (transform.TryGetComponent(out Player player))
                diamond.Split();
            else
                diamond.PickUp();
        }
    }

    private IEnumerator WakeUpSleepers(int sleepersCount)
    {
        float tempOffset;
        WaitForSeconds delay;
        AwakenedCharacter tempCharacter;

        delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < sleepersCount; i++)
        {
            _countCubesCollected++;
            tempCharacter = Instantiate(_prefab, _awakenedPool);
            tempOffset = _offset * _countCubesCollected;
            tempCharacter.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            tempCharacter.transform.position = new Vector3(transform.position.x, transform.position.y + tempOffset, transform.position.z);

            if (_awakenedPool.childCount > _minValueForChangeSpawnType)
                tempCharacter.InizializeParameters(_currentMaterial, _awakenedArray[_awakenedArray.Count - _offsetCount].Rigidbody);
            else
                tempCharacter.InizializeParameters(_currentMaterial, _rigidbody);

            _awakenedArray.Add(tempCharacter);
            AddedNewAwakend?.Invoke(tempCharacter.transform);

            yield return delay;
        }
    }
}
