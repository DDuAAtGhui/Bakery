using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    //Enum 값 중 랜덤한 값 반환
    public static T RandomEnum<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));

        return (T)values.GetValue(new System.Random().Next(0, values.Length));
    }

    public static Stack ReverseStack(Stack stack)
    {
        Stack reversedStack = new Stack();

        foreach (var item in stack)
        {
            reversedStack.Push(item);
        }

        return reversedStack;
    }

    public static Stack<GameObject> ReverseStack(Stack<GameObject> originalStack)
    {
        Stack<GameObject> reversedStack = new Stack<GameObject>();

        while (originalStack.Count > 0)
        {
            GameObject item = originalStack.Pop();
            reversedStack.Push(item);
        }

        return reversedStack;
    }

}
