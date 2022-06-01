using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Collector : MonoBehaviour
{
    private const int InviolableObjectsCount = 1;
    private const int OffsetCount = 1;
    private const int MinValueForChangeSpawnType = 1;
    private const int TriggerValue = 0;
    private const int IndexLowerSleepers = 1;

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
                RemovedLastAwakend?.Invoke(_awakenedArray[_awakenedArray.Count - OffsetCount * 2].transform);
            else
                RemovedLastAwakend?.Invoke(transform);

            Destroy(_awakenedArray[_awakenedArray.Count - OffsetCount].gameObject);
            _awakenedArray.RemoveAt(_awakenedArray.Count - OffsetCount);
            _countCubesCollected--;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out SleepersCell sleepersCell) && sleepersCell.SleepersCount > TriggerValue)
        {
            transform.position = sleepersCell.transform.GetChild(IndexLowerSleepers).position;
            sleepersCell.StartDestroy();
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(WakeUpSleepers(sleepersCell.transform.childCount - InviolableObjectsCount));
        }
        else if (other.transform.TryGetComponent(out Diamond diamond))
        {
            if (transform.TryGetComponent(out Player player))
                diamond.Split();
            else
                diamond.PrepairPickUp();
        }
    }

    private IEnumerator WakeUpSleepers(int sleepersCount)
    {
        float tempOffset;
        AwakenedCharacter tempCharacter;

        for (int i = 0; i < sleepersCount; i++)
        {
            _countCubesCollected++;
            tempCharacter = Instantiate(_prefab, _awakenedPool);
            tempOffset = _offset * _countCubesCollected;
            tempCharacter.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            tempCharacter.transform.position = new Vector3(transform.position.x, transform.position.y + tempOffset, transform.position.z);

            if (_awakenedPool.childCount > MinValueForChangeSpawnType)
                tempCharacter.InizializeParameters(_currentMaterial, _awakenedArray[_awakenedArray.Count - OffsetCount].Rigidbody);
            else
                tempCharacter.InizializeParameters(_currentMaterial, _rigidbody);

            _awakenedArray.Add(tempCharacter);
            AddedNewAwakend?.Invoke(tempCharacter.transform);

            yield return null;
        }
    }
}
