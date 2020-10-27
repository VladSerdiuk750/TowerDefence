using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPoints : MonoBehaviour
{
    [SerializeField] public static Transform[] Points;

    [SerializeField] private Transform[] view;

    private void Awake() 
    {
        Points = new Transform[transform.childCount];
        for(int i = 0; i < Points.Length; i++)
        {
            Points[i] = transform.GetChild(i);
        }

        view = Points;
    }
}
