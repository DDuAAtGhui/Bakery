using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadStack : MonoBehaviour
{
    public Transform stackTF;
    public Vector3 stackInterval = new Vector3(0, 0.5f, 0);

    //���� ����ִ� �� ����
    //���� �������� ���ξֵ���� ������ �ϴϱ� ���� ���
    public Stack<GameObject> breadStack = new Stack<GameObject>();
}
