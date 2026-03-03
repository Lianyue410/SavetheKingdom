using UnityEngine;

public class FallDetector : MonoBehaviour
{
    public float loseY = -10f; 

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsWin || GameManager.Instance.IsLose) return;

        if (transform.position.y < loseY)
        {
            GameManager.Instance.Lose("Fell Out Of World");
        }
    }
}