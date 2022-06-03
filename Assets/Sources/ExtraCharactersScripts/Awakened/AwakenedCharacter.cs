using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CharacterJoint))]
[RequireComponent(typeof(Rigidbody))]
public class AwakenedCharacter : MonoBehaviour
{
    private const float Offset = 2;

    private Renderer _renderer;
    private CharacterJoint _characterJoint;
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _characterJoint = GetComponent<CharacterJoint>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void InizializeParameters(Material material, Rigidbody rigidbody)
    {
        float verticalPosition = transform.localScale.y * Offset;

        _renderer.material = material;
        _characterJoint.connectedBody = rigidbody;
        _characterJoint.anchor = new Vector3(0, -verticalPosition, 0);
        _characterJoint.connectedAnchor = new Vector3(0, -(--verticalPosition), 0);
    }
}
