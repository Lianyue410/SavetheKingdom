using UnityEngine;

public class MushroomPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Found mushroom!");

        if (GameManager.Instance != null)
            GameManager.Instance.AddMushroom();
        else
            Debug.LogWarning("There is no GameManagerTMP in the Scene.");

        Destroy(gameObject);
    }
}