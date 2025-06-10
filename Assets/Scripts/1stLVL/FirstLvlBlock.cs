using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FirstLvlBlock : MonoBehaviour
{
    public float speed = 15f; // Speed of the block
    public float maxDistance = 7f; // Maximum distance to move before changing direction
    private bool _movingRight = true; // Flag to track the direction of movement
    void Update()
    {
        if (transform.position.x >= maxDistance)
        {
            _movingRight = false; // Change direction to left
        }
        else if (transform.position.x <= -maxDistance)
        {
            _movingRight = true; // Change direction to right
        }
        if (_movingRight)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed); // Move right
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed); // Move left
        }
    }
}
