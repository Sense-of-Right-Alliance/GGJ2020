using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Resource : MonoBehaviour
{
    [SerializeField] int value = 1;

    public int Value { get { return value; } }
}
