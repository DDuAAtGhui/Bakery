using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] Waypoints waypoints;
    [Tooltip("�̸����� ã�����")][SerializeField] string targetWaypointsName;
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

        //�ϴ� �׽�Ʈ������ ���� ����Ʈ ��������
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);
    }

    private void Initialize()
    {
        //currentWaypoint = null�� ���¿��� �Ķ���ͷ� �����ϰ� ��ȯ�ޱ�
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        //���� �� ù��° waypoint ���� �����ϱ� ���Ѵٸ�
        transform.position = currentWaypoint.position;
    }

    private void Update()
    {
        MoveToWaypoint();
        SmoothRotate();

        //�������� ������ ��������Ʈ�� �����Ÿ��� ������ ���� ������ �޾ƿ���
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
        //�������� �ٶ󺸴� ���⺤��
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
