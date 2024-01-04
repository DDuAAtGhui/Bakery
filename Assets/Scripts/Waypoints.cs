using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GizmoType
{
    Wire, Solid
}
public enum LoopType
{
    Not_Loop, Loop
}
public enum ReverseType
{
    Straight, Reverse
}
public class Waypoints : MonoBehaviour
{
    [SerializeField] float SphereRadius = 1f;
    [SerializeField] GizmoType gizmoType = GizmoType.Wire;

    [Header("Path")]
    [SerializeField] LoopType loopType = LoopType.Not_Loop;
    [SerializeField] ReverseType reverseType = ReverseType.Straight;

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            //이제 막 waypoint 진입할 때는 currentWaypoint는 항상 null임
            //그때는 Waypoints의 첫번째 자식의 transform을 waypoint로 설정
            return transform.GetChild(0);
        }

        //현재 currentWaypoint의 인덱스
        int currentIndex = currentWaypoint.GetSiblingIndex();

        //다음 waypoint의 인덱스를 현재 인덱스로 초기화
        int nextIndex = currentIndex;

        //반대로 가는 경로인지 체크
        switch (reverseType)
        {
            case ReverseType.Straight: //정상 경로일 경우
                nextIndex += 1; //일단 다음 인덱스로 설정

                //다음 waypoint의 인덱스가 자식 수(오버플로우)의 경우 
                //= 현재 인덱스가 마지막 인덱스 일때
                if (nextIndex == transform.childCount)
                {
                    switch (loopType)
                    {
                        case LoopType.Loop: //Loop 켜있으면 0번 waypoint로 인덱스 설정
                            nextIndex = 0; break;


                        case LoopType.Not_Loop:    //아니면 인덱스 -1 해줘서 오버플로우 방지하고                                              
                            nextIndex -= 1; break; //다음 인덱스 = 현재 로 만들어 서있게 하기

                    }
                }
                break;

            case ReverseType.Reverse: //반대로 가는 경로일 경우
                nextIndex -= 1; //일단 다음인덱스로 설정

                if (nextIndex < 0) //현재가 0번 자식일경우
                {
                    switch (loopType)
                    {
                        case LoopType.Loop:
                            nextIndex = transform.childCount - 1; break;

                        case LoopType.Not_Loop:
                            nextIndex += 1; break;
                    }
                }
                break;
        }

        //위에서 설정한 nexIndex 번호의 자식의 TF 반환
        return transform.GetChild(nextIndex);
    }

    private void OnDrawGizmos()
    {
        foreach (Transform tf in transform)
        {
            Gizmos.color = Color.blue;

            switch (gizmoType)
            {
                case GizmoType.Wire:
                    Gizmos.DrawWireSphere(tf.position, SphereRadius);
                    break;

                case GizmoType.Solid:
                    Gizmos.DrawSphere(tf.position, SphereRadius);
                    break;
            }
        }

        Gizmos.color = Color.red;

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            //다음 웨이포인트로 선 보여주기
            Gizmos.DrawLine(transform.GetChild(i).position,
                transform.GetChild(i + 1).position);
        }

        if (loopType == LoopType.Loop)
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position,
                transform.GetChild(0).position);
    }
}
