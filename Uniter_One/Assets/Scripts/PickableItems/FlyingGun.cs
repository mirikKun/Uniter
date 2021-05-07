using UnityEngine;

public class FlyingGun : MonoBehaviour
{
    [SerializeField] private GameObject gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AddGun>().AddingGun(gun);
            Destroy(gameObject);
        }
    }
}