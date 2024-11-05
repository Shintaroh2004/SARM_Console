using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public float angle=0.0f;

    void Update()
    {
        target.transform.RotateAround(this.transform.position, this.transform.up, this.angle);
    }
}
