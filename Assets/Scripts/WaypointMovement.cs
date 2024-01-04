using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] Waypoints waypoints;
    [Tooltip("이름으로 찾을경우")][SerializeField] string targetWaypointsName;
    Transform currentWaypoint;

    [Header("Movement")]
    [SerializeField] float speed = 6f;
    [SerializeField] float distanceThreshold = 0.1f;

    [Header("Rotate")]
    [SerializeField] float rotateSpeed = 10f;
    Quaternion rotationTarget;
    Vector3 dirToWaypoint;
    private void Awake()
    {
        if (waypoints == null)
        {
            waypoints = GameObject.Find(targetWaypointsName).GetComponent<Waypoints>();
        }
    }
    private void Start()
    {
        Initialize();

        //일단 테스트용으로 다음 포인트 가져오기
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);
    }

    private void Initialize()
    {
        //currentWaypoint = null인 상태에서 파라미터로 전달하고 반환받기
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        //시작 시 첫번째 waypoint 으로 워프하길 원한다면
        transform.position = currentWaypoint.position;
    }

    private void Update()
    {
        MoveToWaypoint();
        SmoothRotate();

        //목적지로 설정된 웨이포인트에 일정거리로 들어오면 다음 목적지 받아오기
        if (Vector3.Distance(transform.position, currentWaypoint.position)
            <= distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            // transform.LookAt(currentWaypoint);          
        }

    }

    private void MoveToWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            currentWaypoint.position, speed * Time.deltaTime);
    }

    void SmoothRotate()
    {
        //목적지를 바라보는 방향벡터
        dirToWaypoint = (currentWaypoint.position - transform.position).normalized;
        rotationTarget = Quaternion.LookRotation(dirToWaypoint);

        transform.rotation = Quaternion.Slerp(transform.rotation,
            rotationTarget, rotateSpeed * Time.deltaTime);
    }

    public void ChangeWaypoints(string wayPointsName)
    {
        if (waypoints.gameObject.name != wayPointsName)
        {
            waypoints = GameObject.Find(wayPointsName).GetComponent<Waypoints>();
        }
    }
    public void ChangeWaypoints(GameObject waypoints)
    {
        if (this.waypoints.gameObject != waypoints)
        {
            this.waypoints = waypoints.GetComponent<Waypoints>();
        }
    }
}
