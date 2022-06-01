using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SleepersCell : MonoBehaviour
{
    private const int InviolableObjectsCount = 1;
    private const float Denominator = 2f;
    private const float Multiplier = 0.27f;

    [SerializeField] private ParticleSystem _particleSystem;

    private BoxCollider _collider;

    public int SleepersCount => transform.childCount - InviolableObjectsCount;
    public float SidePosition => transform.position.x;
    public float VerticalPosition => transform.position.y;
    public float HorizontalPosition => transform.position.z;

    private void OnEnable()
    {
        if (_particleSystem == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");

        _collider = GetComponent<BoxCollider>();
    }

    public void StartDestroy()
    {
        _particleSystem.Play();

        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SetColliderParameters()
    {
        float tempVerticalSize = SleepersCount / Denominator;
        float tempVerticalCenter = SleepersCount * Multiplier;

        _collider.size = new Vector3(_collider.size.x, tempVerticalSize, _collider.size.z);
        _collider.center = new Vector3(_collider.center.x, tempVerticalCenter, _collider.center.z);
    }
}
