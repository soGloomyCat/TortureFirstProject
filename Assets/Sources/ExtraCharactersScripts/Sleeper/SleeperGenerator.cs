using UnityEngine;

public class SleeperGenerator : MonoBehaviour
{
    [SerializeField] private Sleeper _sleeper;
    [SerializeField] private SleepersCell[] _sleepersCells;
    [Range(0, 100)]
    [SerializeField] private int _amountObjects;

    private float _startOffset;
    private float _offset;

    private void OnEnable()
    {
        if (_sleeper == null || _sleepersCells == null)
            throw new System.ArgumentNullException("Отсутствует один из обязательных параметров. Проверьте редактор.");
    }

    private void Start()
    {
        _startOffset = _sleeper.transform.localScale.y / 2;
        _offset = _sleeper.transform.localScale.y;
        Spawn();
    }

    private void Spawn()
    {
        Sleeper tempObject;
        int currentIteration;
        int currentIndexCell;
        float tempOffset;

        currentIteration = 1;

        while (currentIteration <= _amountObjects)
        {
            currentIndexCell = Random.Range(0, _sleepersCells.Length);

            while (_sleepersCells[currentIndexCell].SleepersCount >= _sleepersCells[currentIndexCell].Count)
                currentIndexCell = Random.Range(0, _sleepersCells.Length);

            tempObject = Instantiate(_sleeper, _sleepersCells[currentIndexCell].transform);

            if (_sleepersCells[currentIndexCell].transform.childCount > 2)
            {
                tempOffset = GetCurrentOffset(_sleepersCells[currentIndexCell].SleepersCount);
                tempObject.transform.position = new Vector3(tempObject.transform.position.x,
                                                            tempObject.transform.position.y + tempOffset,
                                                            tempObject.transform.position.z);
            }
            else
            {
                tempObject.transform.position = new Vector3(_sleepersCells[currentIndexCell].SidePosition,
                                                            _sleepersCells[currentIndexCell].VerticalPosition + _startOffset,
                                                            _sleepersCells[currentIndexCell].HorizontalPosition);
            }

            currentIteration++;
            _sleepersCells[currentIndexCell].SetColliderParameters();
        }
    }

    private float GetCurrentOffset(int countObjectsCreated)
    {
        float tempOffset;

        tempOffset = _offset * countObjectsCreated - _startOffset;

        return tempOffset;
    }
}
