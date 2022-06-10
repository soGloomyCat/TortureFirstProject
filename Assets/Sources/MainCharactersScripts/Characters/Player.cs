using UnityEngine;

public class Player : Character
{
    private const int ButtonIndex = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(ButtonIndex) && CanClickAgain())
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
