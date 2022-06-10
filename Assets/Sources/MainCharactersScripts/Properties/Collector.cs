using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private Player _player;

    private int _countCubesCollected;
    private float _offset;
    private Rigidbody _rigidbody;
    private List<AwakenedCharacter> _awakenedList;
    private Coroutine _coroutine;

    public event UnityAction<Transform> AddedNewAwakend;
    public event UnityAction<Transform> RemovedLastAwakend;
    public event UnityAction<Transform> RemovedPlayer;

    public void OnTrapHandler()
    {
        if (_countCubesCollected > 0)
        {
            if (_awakenedList.Count >= 2)
                RemovedLastAwakend?.Invoke(_awakenedList[_awakenedList.Count - OffsetCount * 2].transform);
            else
                RemovedLastAwakend?.Invoke(transform);

            Destroy(_awakenedList[_awakenedList.Count - OffsetCount].gameObject);
            _awakenedList.RemoveAt(_awakenedList.Count - OffsetCount);
            _countCubesCollected--;
        }
        else
        {
            RemovedPlayer?.Invoke(transform);
            Destroy(gameObject);
        }
    }

    public bool CheckSameStatus(AwakenedCharacter awaker)
    {
        if (awaker.IsLinkedToPlayer && _awakenedList[0].IsLinkedToPlayer == false || awaker.IsLinkedToPlayer == false && _awakenedList[0].IsLinkedToPlayer)
            return true;

        return false;
    }

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
        _awakenedList = new List<AwakenedCharacter>();
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
            tempCharacter.SetAttacher(_player);
            tempOffset = _offset * _countCubesCollected;

            tempCharacter.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            tempCharacter.transform.position = new Vector3(transform.position.x, transform.position.y + tempOffset, transform.position.z);

            if (_awakenedList.Count > 0)
                tempCharacter.transform.localRotation = _awakenedList[_awakenedList.Count - OffsetCount].transform.localRotation;

            if (_awakenedPool.childCount > MinValueForChangeSpawnType)
                tempCharacter.InizializeParameters(_currentMaterial, _awakenedList[_awakenedList.Count - OffsetCount].Rigidbody);
            else
                tempCharacter.InizializeParameters(_currentMaterial, _rigidbody);

            _awakenedList.Add(tempCharacter);
            AddedNewAwakend?.Invoke(tempCharacter.transform);

            yield return null;
        }
    }
}
