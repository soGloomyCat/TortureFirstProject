using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    private const float _normalForceBooster = 0.2f;
    private const float _maxForceBooster = 0.4f;
    private const int _mediumCellValue = 2;
    private const int _largeCellValue = 3;

    [Range(0, 10)]
    [SerializeField] private float _verticalDirection;
    [Range(0, 10)]
    [SerializeField] private float _horizontalDirection;
    [SerializeField] private Platform _platform;
    [SerializeField] private Transform _platformsPool;

    private Rigidbody _rigidbody;
    private bool _isDefined;
    private bool _onTraped;
    private bool _onBoard;
    private Vector3 _finishPosition;

    public event UnityAction Moved;
    public event UnityAction Traped;
    public event UnityAction Encountered;

    private void OnEnable()
    {
        Time.timeScale = 2;
        if (_platform == null || _platformsPool == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isDefined = false;
        _onTraped = false;
        _onBoard = false;

    }

    private void Update()
    {
        if (_isDefined == false)
            DeterminePresenceOpponent();

        if (_onTraped == false)
            DetermineTrap();

        if (_onBoard == false)
            DetermineBoard();

    }

    public void PrepairToJump()
    {
        _rigidbody.AddForce(new Vector3(0, _verticalDirection * DetermineJumpForce(), _horizontalDirection / DetermineJumpForce()), ForceMode.Impulse);
        Moved?.Invoke();
        _isDefined = false;
    }

    public void PrepairToForcedJump()
    {
        Encountered?.Invoke();
        _rigidbody.AddForce(new Vector3(0, _verticalDirection, _horizontalDirection), ForceMode.Impulse);
        Moved?.Invoke();
    }
    private float DetermineJumpForce()
    {
        Ray tempRay;
        RaycastHit tempHit;
        float forceMultiplier;

        tempRay = new Ray(transform.position, Vector3.forward);
        forceMultiplier = 1;

        if (Physics.Raycast(tempRay, out tempHit, 1.5f))
            if (tempHit.transform.TryGetComponent(out SleepersCell largeSleepersCell) && largeSleepersCell.SleepersCount >= _largeCellValue)
                return forceMultiplier + _maxForceBooster;
            else if (tempHit.transform.TryGetComponent(out SleepersCell mediumSleepersCell) && mediumSleepersCell.SleepersCount >= _mediumCellValue)
                return forceMultiplier + _normalForceBooster;

        return forceMultiplier;
    }

    private void DeterminePresenceOpponent()
    {
        Ray tempRay;
        RaycastHit tempHit;
        float rayLength;

        tempRay = new Ray(transform.position, new Vector3(0, -0.5f, 0.1f));
        rayLength = 1.5f;

        if (Physics.Raycast(tempRay, out tempHit, rayLength))
            if (tempHit.collider.transform.TryGetComponent(out Character character))
            {
                _isDefined = true;
                character.InitializeForcedJump();
            }
    }

    private void DetermineTrap()
    {
        Ray tempRay;
        RaycastHit tempHit;
        float rayLength;

        tempRay = new Ray(transform.position, Vector3.down);
        rayLength = 0.3f;

        if (Physics.Raycast(tempRay, out tempHit, rayLength))
        {
            if (tempHit.collider.transform.TryGetComponent(out Trap trap))
            {
                _onTraped = true;
                Platform tempPlatform = Instantiate(_platform, _platformsPool);
                tempPlatform.transform.position = new Vector3(transform.position.x, tempHit.point.y, transform.position.z);
                Traped?.Invoke();
            }
        }
    }

    private void DetermineBoard()
    {
        Ray tempRay;
        RaycastHit tempHit;
        float rayLength;

        tempRay = new Ray(transform.position, Vector3.down);
        rayLength = 0.3f;

        if (Physics.Raycast(tempRay, out tempHit, rayLength))
        {
            if (tempHit.collider.transform.TryGetComponent(out BoardMoveHandler board))
            {
                _onBoard = true;
                transform.position = new Vector3(board.transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out BoardMoveHandler boardMover))
            transform.parent = boardMover.transform;

        if (collision.transform.TryGetComponent(out Platform platform))
            _onTraped = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out BoardMoveHandler boardMover))
        {
            _onBoard = false;
            transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Finish finish))
        {
            _finishPosition = new Vector3(finish.transform.position.x, transform.position.y, transform.position.z);
            transform.position = _finishPosition;
        }
    }
}
