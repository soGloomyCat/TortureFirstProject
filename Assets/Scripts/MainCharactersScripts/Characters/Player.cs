using UnityEngine;

public class Player : Character
{
    private const int _buttonIndex = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_buttonIndex))
            Time.timeScale = 2;

        if (Input.GetMouseButtonDown(_buttonIndex) && CanClickAgain())
            InitializeJump();

        if (IsOnGround)
            CorrectHorizontalPosition();
    }

    protected override bool CanClickAgain()
    {
        if (IsOnGround || IsOnBoard || IsOnPlatform)
            return true;

        return false;
    }
}
