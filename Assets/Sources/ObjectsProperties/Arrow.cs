using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Arrow : MonoBehaviour
{
    private const float Offset = 1.5f;

    private Animator _animator;
    private Coroutine _coroutine;

    public void Shift(Transform newParent)
    {
        transform.parent = newParent;
        transform.position = new Vector3(newParent.position.x, newParent.position.y + Offset, newParent.position.z);
    }

    public void UnParent(Transform player)
    {
        _animator.enabled = false;
        transform.parent = null;
        transform.position = new Vector3(player.position.x, player.position.y + .4f, player.position.z);
        transform.rotation = new Quaternion(-90, 0, 0, 90);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Move());
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator Move()
    {
        float maxvertical = transform.position.y + 0.3f;
        float minvertical = transform.position.y;
        Vector3 targetPosition = new Vector3(transform.position.x, maxvertical, transform.position.z);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.5f * Time.deltaTime);

            if (transform.position.y == maxvertical)
                targetPosition = new Vector3(transform.position.x, minvertical, transform.position.z);
            else if (transform.position.y == minvertical)
                targetPosition = new Vector3(transform.position.x, maxvertical, transform.position.z);

            yield return null;
        }
    }
}
