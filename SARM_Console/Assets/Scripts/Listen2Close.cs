using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Listen2Close : MonoBehaviour
{
    [SerializeField] private string default_string;
    [SerializeField] private UnitySerialPort serial;
    [SerializeField] private TextMeshProUGUI TMtext;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (serial.isRunning_)
        {
            this.TMtext.text = "Disconnect";
        }
        else
        {
            this.TMtext.text = default_string;
        }
    }
}
