using UnityEngine;

public class SleepersCell : MonoBehaviour
{
    private const int _inviolableObjectsCount = 1;

    [SerializeField] private ParticleSystem _particleSystem;

    public int SleepersCount => transform.childCount - _inviolableObjectsCount;
    public float SidePosition => transform.position.x;
    public float VerticalPosition => transform.position.y;
    public float HorizontalPosition => transform.position.z;

    private void OnEnable()
    {
        if (_particleSystem == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");
    }

    public void DestroyAvailableSleepers()
    {
        _particleSystem.Play();

        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
