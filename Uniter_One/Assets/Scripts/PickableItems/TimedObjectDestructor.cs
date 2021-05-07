using UnityEngine;


public class TimedObjectDestructor : MonoBehaviour
{
    [SerializeField] private float timeOut = 3.0f;

    private void Awake()
    {
        Destroy(gameObject, timeOut);
    }
}