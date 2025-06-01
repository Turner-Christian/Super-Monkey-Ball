using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float customGravity = -35f; // Custom gravity value

    void Awake()
    {
        Physics.gravity = new Vector3(0, customGravity, 0); // Set custom gravity
    }
}
