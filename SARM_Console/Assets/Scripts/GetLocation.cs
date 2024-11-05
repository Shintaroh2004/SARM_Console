using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLocation : MonoBehaviour
{
    public float pos_y = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.pos_y=transform.position.y;
    }
}
