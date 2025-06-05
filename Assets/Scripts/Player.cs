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
        // Debug.Log("MPH: " + _ballSpeed);
        animator.SetFloat("Speed", _ballSpeed); // _ballSpeed should be in the same units you mapped
    }

    void LateUpdate()
    {
        _targetPosition = target.transform.position;
        transform.position = new Vector3(_targetPosition.x, _targetPosition.y -1.5f, _targetPosition.z);
    }
}
