using UnityEngine;

public class CollectibleApple : MonoBehaviour
{
    public AudioClip biteSound;
    public int scoreValue = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (biteSound)
                AudioSource.PlayClipAtPoint(biteSound, transform.position);

            ScoreManager.instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
