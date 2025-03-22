using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public bool isScoreItem = false;
    public int points = 1;

    public bool isSpeedBoost = false;
    public float boostDuration = 2f; // 60 seconds
    public float boostMultiplier = 1.25f;

    public bool isKiller = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<ThirdPersonController>();

            if (isKiller)
            {
                player.isKilled = true;
            }
            else if (isSpeedBoost)
            {
                player.ActivateSpeedBoost(boostDuration, boostMultiplier);
            }
            else if (isScoreItem)
            {
                ScoreManager.instance.AddScore(points);
            }

            Destroy(gameObject);
        }
    }
}
