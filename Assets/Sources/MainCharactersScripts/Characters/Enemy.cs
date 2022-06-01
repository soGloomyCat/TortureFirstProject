using UnityEngine;

public class Enemy : Character
{
    private const int TriggerValue = 0;

    [Range(0, 100)]
    [SerializeField] private float _minCooldown;
    [Range(0, 100)]
    [SerializeField] private float _maxCooldown;

    private float _elapsedTime;
    private float _currentCooldown;

    private void Start()
    {
        _elapsedTime = 0;
    }

    private void Update()
    {
        if (IsOnGround)
            CorrectHorizontalPosition();

        if (CanClickAgain())
            InitializeJump();

        _elapsedTime -= Time.deltaTime;
    }

    protected override bool CanClickAgain()
    {
        if (_elapsedTime <= TriggerValue)
        {
            if (IsOnGround || IsOnBoard || IsOnPlatform)
            {
                _currentCooldown = IdentifyNextCooldown();
                _elapsedTime = _currentCooldown;
                return true;
            }
        }

        return false;
    }

    private float IdentifyNextCooldown()
    {
        return Random.Range(_minCooldown, _maxCooldown);
    }
}
