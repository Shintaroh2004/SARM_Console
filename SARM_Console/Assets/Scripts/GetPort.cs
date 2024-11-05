using System.Collections;
using System.IO.Ports;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetPort : MonoBehaviour
{

    [SerializeField] private UnitySerialPort serial;
    TMP_Dropdown dropdown;
    private List<string> ports_list;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        string[] ports = SerialPort.GetPortNames();
        ports_list = new List<string>(ports);
        dropdown.ClearOptions();
        dropdown.AddOptions(ports_list);

        dropdown.onValueChanged.AddListener((value) => OnValueChanged(value));
        serial.portName = dropdown.captionText.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged(int value)
    {
        serial.portName = dropdown.captionText.text;
    }
}
