using UnityEngine;

public class GoalCamera : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject

    void Update()
    {
        if (GameManager.goalScored && player != null)
        {
            // Look at the player
            transform.LookAt(player.transform);
        }
    }
}
