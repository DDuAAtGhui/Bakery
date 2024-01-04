using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadStack : MonoBehaviour
{
    public Transform stackTF;
    public Vector3 stackInterval = new Vector3(0, 0.5f, 0);

    //현재 들고있는 빵 개수
    //가장 마지막에 쌓인애들부터 꺼내야 하니까 스택 사용
    public Stack<GameObject> breadStack = new Stack<GameObject>();
}
