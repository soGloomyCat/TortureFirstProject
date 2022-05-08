using UnityEngine;

[RequireComponent(typeof(Collector))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] private Mover _moverSystems;

    private bool _isOnGround;
    private bool _isOnBoard;
    private bool _isOnPlatform;

    protected bool IsOnGround => _isOnGround;
    protected bool IsOnBoard => _isOnBoard;
    public bool IsOnPlatform => _isOnPlatform;

    abstract protected bool CanClickAgain();

    private void OnEnable()
    {
        if (_moverSystems == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");
    }

    public void InitializeJump()
    {
        _isOnGround = false;
        _moverSystems.PrepairToJump();
    }

    public void InitializeForcedJump()
    {
        _isOnGround = false;
        _moverSystems.PrepairToForcedJump();
    }

    protected void CorrectHorizontalPosition()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Ground ground))
            _isOnGround = true;
        else if (collision.transform.TryGetComponent(out BoardMoveHandler boardMover))
            _isOnBoard = true;
        else if (collision.transform.TryGetComponent(out Platform platform))
            _isOnPlatform = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out BoardMoveHandler boardMover))
            _isOnBoard = false;
        else if (collision.transform.TryGetComponent(out Platform platform))
            _isOnPlatform = false;
    }
}