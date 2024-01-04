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
            //���� �� waypoint ������ ���� currentWaypoint�� �׻� null��
            //�׶��� Waypoints�� ù��° �ڽ��� transform�� waypoint�� ����
            return transform.GetChild(0);
        }

        //���� currentWaypoint�� �ε���
        int currentIndex = currentWaypoint.GetSiblingIndex();

        //���� waypoint�� �ε����� ���� �ε����� �ʱ�ȭ
        int nextIndex = currentIndex;

        //�ݴ�� ���� ������� üũ
        switch (reverseType)
        {
            case ReverseType.Straight: //���� ����� ���
                nextIndex += 1; //�ϴ� ���� �ε����� ����

                //���� waypoint�� �ε����� �ڽ� ��(�����÷ο�)�� ��� 
                //= ���� �ε����� ������ �ε��� �϶�
                if (nextIndex == transform.childCount)
                {
                    switch (loopType)
                    {
                        case LoopType.Loop: //Loop �������� 0�� waypoint�� �ε��� ����
                            nextIndex = 0; break;


                        case LoopType.Not_Loop:    //�ƴϸ� �ε��� -1 ���༭ �����÷ο� �����ϰ�                                              
                            nextIndex -= 1; break; //���� �ε��� = ���� �� ����� ���ְ� �ϱ�

                    }
                }
                break;

            case ReverseType.Reverse: //�ݴ�� ���� ����� ���
                nextIndex -= 1; //�ϴ� �����ε����� ����

                if (nextIndex < 0) //���簡 0�� �ڽ��ϰ��
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

        //������ ������ nexIndex ��ȣ�� �ڽ��� TF ��ȯ
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
            //���� ��������Ʈ�� �� �����ֱ�
            Gizmos.DrawLine(transform.GetChild(i).position,
                transform.GetChild(i + 1).position);
        }

        if (loopType == LoopType.Loop)
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position,
                transform.GetChild(0).position);
    }
}
