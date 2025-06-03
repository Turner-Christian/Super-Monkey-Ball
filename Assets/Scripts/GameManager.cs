using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance for easy access
    [SerializeField] private float _customGravity = -60f; // Custom gravity value

    void Awake()
    {
        Instance = this; // Set the singleton instance
        Physics.gravity = new Vector3(0, _customGravity, 0); // Set custom gravity
    }
}
