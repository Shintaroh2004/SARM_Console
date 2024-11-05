using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Message
{
    public int A;
    public int B;
    public int C;
    public int D;
    public int G;
    public int M;
}

public class ArmState : MonoBehaviour
{
    [SerializeField] public float link1 = 0.0f;
    [SerializeField] public float link2 = 0.0f;
    [SerializeField] public float link3 = 50.0f;
    [SerializeField] public float link4 = 0.0f;
    [SerializeField] public int grab = 0;
    [SerializeField] public int mode = 0;

    [SerializeField] public float pre_link1 = 0.0f;
    [SerializeField] public float pre_link2 = 0.0f;
    [SerializeField] public float pre_link3 = 50.0f;
    [SerializeField] public float pre_link4 = 0.0f;

    [SerializeField] public Slider link1_slider;
    [SerializeField] public Slider link2_slider;
    [SerializeField] public Slider link3_slider;
    [SerializeField] public Slider link4_slider;
    [SerializeField] public Toggle Garb_or_Drop;
    [SerializeField] public TextMeshProUGUI statelamp;
    [SerializeField] public TextMeshProUGUI statename;

    [SerializeField] public GameObject axis_base;
    [SerializeField] public GameObject axis_1;
    [SerializeField] public GameObject axis_2;
    [SerializeField] public GameObject axis_4;
    [SerializeField] public GameObject axis_5;
    [SerializeField] public GameObject axis_6;
    [SerializeField] public GameObject axis_8;
    [SerializeField] public GameObject axis_10;
    [SerializeField] public GameObject axis_11;
    [SerializeField] public GameObject axis_end;
    [SerializeField] public GameObject end_point;

    [SerializeField] private UnitySerialPort serial;
    
    [SerializeField] private float limit_height = -0.18f;

    private string recv_str;
    private int safety_count = 0; 

    private bool height_has_change = false;

    // Start is called before the first frame update
    void Start()
    {
        //�M������M�����Ƃ��ɁA���̃��b�Z�[�W�̏������s��
        serial.OnDataReceived += OnDataReceived;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode==1)
        {
            try 
            {
                Message mes = JsonUtility.FromJson<Message>(recv_str);
                this.link1 = ac2deg(mes.A,-90.0f,90.0f);
                this.link2 = ac2deg(mes.B,0.0f,40.0f);
                this.link3 = ac2deg(mes.C,-15.0f,50.0f);
                this.link4 = ac2deg(mes.D,-90.0f,90.0f);
                this.grab = mes.G;
            }
            catch
            {
                Debug.LogWarning("Cant parse to JSON.");
            }
        }
        else
        {
            this.link1 = this.link1_slider.value;
            this.link2 = this.link2_slider.value;
            this.link3 = this.link3_slider.value;
            this.link4 = this.link4_slider.value;
            this.grab = this.Garb_or_Drop.isOn ? 1 : 0;
        }

        this.axis_base.GetComponent<Rotate>().angle = (this.link1-this.pre_link1);
        this.axis_1.GetComponent<Rotate>().angle = (this.link2-this.pre_link2);
        this.axis_2.GetComponent<Rotate>().angle = (this.link2-this.pre_link2);
        this.axis_11.GetComponent<Rotate>().angle = (this.link3-this.pre_link3);
        this.axis_10.GetComponent<Rotate>().angle = -(this.link3-this.pre_link3)+(this.link2-this.pre_link2);
        this.axis_8.GetComponent<Rotate>().angle = (this.link3-this.pre_link3);
        this.axis_4.GetComponent<Rotate>().angle = -(this.link2-this.pre_link2);
        this.axis_5.GetComponent<Rotate>().angle = (this.link3-this.pre_link3);
        this.axis_6.GetComponent<Rotate>().angle = -(this.link3-this.pre_link3);
        this.axis_end.GetComponent<Rotate>().angle = (this.link4-this.pre_link4);

        if(
            Math.Abs(this.pre_link2-this.link2)<=6.0f &&
            Math.Abs(this.pre_link3-this.link3)<=6.0f
        )
        {
            this.height_has_change = false;
        }
        else
        {
            this.height_has_change = true;
        }

        this.pre_link1 = this.link1;
        this.pre_link2 = this.link2;
        this.pre_link3 = this.link3;
        this.pre_link4 = this.link4;
    }

    void LateUpdate()
    {
        if (height_has_change)
        {
            this.safety_count=0;
        }

        if (this.end_point.transform.position.y < this.limit_height)
        {
            this.statelamp.text = "<color=red>●</color>";
            this.statename.text = "Danger";
            this.safety_count=0;
        }
        else
        {
            if (this.safety_count>2)
            {
                this.statelamp.text = "<color=green>●</color>";
                this.statename.text = "Safety";
                if(serial.isRunning_)
                {
                    string mes =
                        $"A{-this.link1:+00.00;-00.00}" + 
                        $"B{-this.link2:+00.00;-00.00}" + 
                        $"C{this.link3:+00.00;-00.00}" + //構造的に反転しない
                        $"D{-this.link4:+00.00;-00.00}" +
                        $"G{this.grab:0}" +
                        $"M{this.mode:0}" +
                        "\n";
                    serial.Write(mes);
                }
                this.safety_count=0;
            }
            else
            {
                safety_count++;
            }
        }

    }

    public void toOrigin()
    {
        this.link1_slider.value = 0.0f;
        this.link2_slider.value = 0.0f;
        this.link3_slider.value = 50.0f;
        this.link4_slider.value = 0.0f;
        this.Garb_or_Drop.isOn = false;

        this.link1 = 0.0f;
        this.link2 = 0.0f;
        this.link3 = 50.0f;
        this.link4 = 0.0f;
        this.grab = 0;
        this.mode = 0;
    }

    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\n" }, System.StringSplitOptions.None);
        try
        {
            Debug.Log(data[0]);
            recv_str = data[0];
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);//�G���[��\��
        }
    }

    public void PCMode()
    {
        this.mode=0;
    }

    public void ControllerMode()
    {
        this.mode=1;
    }

    float ac2deg(int ac,float min,float max)
    {
        float deg = (float)ac*(270.0f/4096.0f)+min;

        if(deg<min)
        {
            float min_deg=min;
            return min;
        }
        else if (deg>max)
        {
            float max_deg = max;
            return max_deg;
        }
        else
        {
            return deg;
        }
    }
}
