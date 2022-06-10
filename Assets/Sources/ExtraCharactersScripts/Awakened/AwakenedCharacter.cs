using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CharacterJoint))]
[RequireComponent(typeof(Rigidbody))]
public class AwakenedCharacter : MonoBehaviour
{
    private const float Offset = 2;

    [SerializeField] private SpriteRenderer _eyes;
    [SerializeField] private SpriteRenderer _mouth;
    [SerializeField] private BlinkHandler _blinkHandler;

    private Renderer _renderer;
    private CharacterJoint _characterJoint;
    private Rigidbody _rigidbody;
    private bool _isLinkedToPlayer;

    public Rigidbody Rigidbody => _rigidbody;
    public bool IsLinkedToPlayer => _isLinkedToPlayer;

    public void SetAttacher(Player player = null)
    {
        if (player != null)
            _isLinkedToPlayer = true;
        else
            _isLinkedToPlayer = false;
    }

    public void InizializeParameters(Material material, Rigidbody rigidbody)
    {
        float verticalPosition = transform.localScale.y * Offset;

        _renderer.material = material;
        _characterJoint.connectedBody = rigidbody;
        _characterJoint.anchor = new Vector3(0, -verticalPosition, 0);
        _characterJoint.connectedAnchor = new Vector3(0, -(--verticalPosition), 0);
    }

    public void PrepairToChangeColor()
    {
        _blinkHandler.PrepairToBlink(_renderer.material, _eyes, _mouth);
    }

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _characterJoint = GetComponent<CharacterJoint>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out AwakenedCharacter awaker))
            if (awaker.IsLinkedToPlayer && _isLinkedToPlayer == false || awaker.IsLinkedToPlayer == false && _isLinkedToPlayer)
                awaker.PrepairToChangeColor();
    }
}
