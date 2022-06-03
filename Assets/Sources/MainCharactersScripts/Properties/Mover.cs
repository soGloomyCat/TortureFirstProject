using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    private const float NormalForceBooster = 0.2f;
    private const float MaxForceBooster = 0.4f;
    private const int MediumCellValue = 2;
    private const int LargeCellValue = 3;

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
    private Vector3 _baseDirection;

    public event UnityAction Moved;
    public event UnityAction Traped;
    public event UnityAction Encountered;
    public event UnityAction Finished;

    private void OnEnable()
    {
        if (_platform == null || _platformsPool == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");

        _rigidbody = GetComponent<Rigidbody>();
        _isDefined = false;
        _onTraped = false;
        _onBoard = false;
        _baseDirection = new Vector3(0, _verticalDirection, _horizontalDirection);
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
        _rigidbody.AddForce(new Vector3(_baseDirection.x, _baseDirection.y * DetermineJumpForce(), _baseDirection.z), ForceMode.Impulse);
        Moved?.Invoke();
        _isDefined = false;
    }

    public void PrepairToForcedJump()
    {
        Encountered?.Invoke();
        _rigidbody.AddForce(_baseDirection, ForceMode.Impulse);
        Moved?.Invoke();
    }
    private float DetermineJumpForce()
    {
        Ray tempRay;
        RaycastHit tempHit;
        LayerMask ignoreLayerMask;
        float forceMultiplier;
        float rayLengt;

        ignoreLayerMask = ~LayerMask.GetMask("DiamondSlice");
        tempRay = new Ray(transform.position, Vector3.forward);
        forceMultiplier = 1;
        rayLengt = 1.5f;

        if (Physics.Raycast(tempRay, out tempHit, rayLengt, ignoreLayerMask))
            if (tempHit.transform.TryGetComponent(out SleepersCell largeSleepersCell) && largeSleepersCell.SleepersCount >= LargeCellValue)
                return forceMultiplier + MaxForceBooster;
            else if (tempHit.transform.TryGetComponent(out SleepersCell mediumSleepersCell) && mediumSleepersCell.SleepersCount >= MediumCellValue)
                return forceMultiplier + NormalForceBooster;

        return forceMultiplier;
    }

    private void DeterminePresenceOpponent()
    {
        Ray tempRay;
        RaycastHit tempHit;
        float rayLength;

        tempRay = new Ray(transform.position, new Vector3(0, -0.5f, 0.1f));
        rayLength = 2f;

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
            float dist = finish.transform.position.x + transform.position.x;
            _baseDirection = new Vector3(-dist, _baseDirection.y, _baseDirection.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out Finish finish))
        {
            _baseDirection = new Vector3(0, _baseDirection.y, _baseDirection.z);
            Finished?.Invoke();
        }
    }
}
