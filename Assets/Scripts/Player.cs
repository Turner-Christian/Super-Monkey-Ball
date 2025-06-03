using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject target;
    public BallController Ball;
    public Animator animator;
    private float _ballSpeed;
    private Vector3 _targetPosition;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Player object.");
        }
    }

    void Update()
    {
        _ballSpeed = Ball.mph; // Assuming Ball has a public static property mph
        Debug.Log("MPH: " + _ballSpeed);
        animator.SetFloat("Speed", _ballSpeed); // _ballSpeed should be in the same units you mapped
        // if (_ballSpeed == 0f)
        // {
        //     animator.SetBool("isWalking", false);
        //     animator.SetBool("isRunning", false);
        // }
        // else if (_ballSpeed > 5f)
        // {
        //     animator.SetBool("isWalking", true);
        //     animator.SetBool("isRunning", false);
        // }
        // else if (_ballSpeed > 30f)
        // {
        //     animator.SetBool("isWalking", false);
        //     animator.SetBool("isRunning", true);
        // }
        // Debug.Log("Ball Speed: " + _ballSpeed);
    }

    void LateUpdate()
    {
        _targetPosition = target.transform.position;
        transform.position = new Vector3(_targetPosition.x, _targetPosition.y -1.5f, _targetPosition.z);
    }
}
