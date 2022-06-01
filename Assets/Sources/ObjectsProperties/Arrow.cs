using UnityEngine;

public class Arrow : MonoBehaviour
{
    private const float Offset = 1.5f;

    public void Shift(Transform newParent)
    {
        transform.parent = newParent;
        transform.position = new Vector3(newParent.position.x, newParent.position.y + Offset, newParent.position.z);
    }
}
