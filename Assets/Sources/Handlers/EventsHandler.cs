using UnityEngine;

public class EventsHandler : MonoBehaviour
{
    [SerializeField] private Mover[] _movers;
    [SerializeField] private Collector[] _collectors;
    [SerializeField] private Stalker[] _stalkers;
    [SerializeField] private Arrow[] _arrows;
    [SerializeField] private TimeHandler _timeHandler;

    private void OnEnable()
    {
        if (_movers == null || _collectors == null || _stalkers == null || _timeHandler == null || _arrows == null)
            throw new System.ArgumentNullException("Отсутствует один из обязательных параметров. Проверьте редактор.");

        AddEvents();
    }

    private void OnDisable()
    {
        RemoveEvents();
    }

    private void AddEvents()
    {
        for (int i = 0; i < _movers.Length; i++)
        {
            _movers[i].Traped += _collectors[i].OnTrapHandler;
        }

        for (int i = 0; i < _stalkers.Length; i++)
        {
            _movers[i].Moved += _stalkers[i].OnMoveHandler;
            _movers[i].Finished += _timeHandler.ChangeScale;
        }

        for (int i = 0; i < _arrows.Length; i++)
        {
            _collectors[i].AddedNewAwakend += _arrows[i].Shift;
            _collectors[i].RemovedLastAwakend += _arrows[i].Shift;
            _collectors[i].RemovedPlayer += _arrows[i].UnParent;
        }
    }

    private void RemoveEvents()
    {
        for (int i = 0; i < _movers.Length; i++)
        {
            _movers[i].Traped -= _collectors[i].OnTrapHandler;
        }

        for (int i = 0; i < _stalkers.Length; i++)
        {
            _movers[i].Moved -= _stalkers[i].OnMoveHandler;
            _movers[i].Finished -= _timeHandler.ChangeScale;
        }

        for (int i = 0; i < _arrows.Length; i++)
        {
            _collectors[i].AddedNewAwakend -= _arrows[i].Shift;
            _collectors[i].RemovedLastAwakend -= _arrows[i].Shift;
            _collectors[i].RemovedPlayer += _arrows[i].UnParent;
        }
    }
}
