using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngicatePosition : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject end_point;
    [SerializeField] private TextMeshProUGUI position;

    private Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rePos = end_point.transform.position-root.transform.position;
        this.initPos = rePos;

        this.position.text = "Position\n" + 
                        $"\n" + 
                        $"x: {300.0f*rePos.x/initPos.x:+000.00;-000.00} [mm]\n" + 
                        $"y: {300.0f*rePos.y/initPos.y:+000.00;-000.00} [mm]\n" + 
                        $"z: {300.0f*rePos.z/initPos.x:+000.00;-000.00} [mm]\n";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rePos = end_point.transform.position-root.transform.position;

        this.position.text = "Position\n" + 
                $"\n" + 
                $"x: {300.0f*rePos.x/initPos.x:+000.00;-000.00} [mm]\n" + 
                $"y: {300.0f*rePos.y/initPos.y:+000.00;-000.00} [mm]\n" + 
                $"z: {300.0f*rePos.z/initPos.x:+000.00;-000.00} [mm]\n";
    }


}
