using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FaceController : MonoBehaviour
{
    private const string TriggerName = "ShouldBlink";

    private Animator _animator;
    private float _cooldown;
    private float _elapsedTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _cooldown = 7f;
        _elapsedTime = _cooldown;
    }

    private void Update()
    {
        if (_elapsedTime >= _cooldown)
            Blink();

        _elapsedTime += Time.deltaTime;
    }

    private void Blink()
    {
        _elapsedTime = 0;
        _animator.SetTrigger(TriggerName);
    }
}
