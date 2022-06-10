using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    private void OnEnable() => Time.timeScale = 2f;

    public void ChangeScale() => Time.timeScale = 3f;
}
