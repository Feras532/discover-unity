using UnityEngine;

public class CollectibleApple : MonoBehaviour
{
    public AudioClip biteSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(biteSound, transform.position);
            Destroy(gameObject);
        }
    }
}
