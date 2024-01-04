using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static Vector2 inputDir = Vector2.zero;
    NavMeshAgent navAgent;
    Animator anim;

    PlayerTouchMovement touchMovement;
    PlayerBreadStack stacker;
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        touchMovement = GetComponent<PlayerTouchMovement>();
        stacker = GetComponent<PlayerBreadStack>();
    }
    private void Start()
    {
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }
    private void Update()
    {
        SetAnimatorParam();
    }

    private void SetAnimatorParam()
    {
        anim.SetBool(GameManager.instance.isMove,
            touchMovement.MovementAmount != Vector2.zero);

        anim.SetBool(GameManager.instance.isStack,
            stacker.breadStack.Count > 0);
    }

    void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();

        Debug.Log("ภิทย บคลอ : " + inputDir);
    }
}
