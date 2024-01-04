using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBreadData",
    menuName = "Create New BreadData/NewBreadData")]
public class BreadData : ScriptableObject
{
    public int value = 1;
    public int tableBonus = 2;
}
