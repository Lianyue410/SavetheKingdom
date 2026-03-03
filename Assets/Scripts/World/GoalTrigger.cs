using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance == null) return;

        if (GameManager.Instance.HasEnoughMushrooms)
        {
            GameManager.Instance.Win();
        }
        else
        {
            Debug.Log("You haven't collected enough mushrooms. You can't pass the level!");
        }
    }
}