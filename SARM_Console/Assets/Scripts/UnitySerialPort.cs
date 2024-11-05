using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class UnitySerialPort : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    //�|�[�g��
    //��
    //Linux�ł�/dev/ttyUSB0
    //windows�ł�COM1
    //Mac�ł�/dev/tty.usbmodem1421�Ȃ�
    public string portName = "COM6";
    public int baudRate = 115200;

    private SerialPort serialPort_;
    private Thread thread_;
    public bool isRunning_ = false;

    private string message_;
    private bool isNewMessageReceived_ = false;

    //void Awake()
    //{
    //    Open();
    //}

    void Update()
    {
        if (isNewMessageReceived_)
        {
            OnDataReceived(message_);
        }
        isNewMessageReceived_ = false;
    }

    void OnDestroy()
    {
        Close();
    }

    private void Open()
    {
        serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        serialPort_.NewLine = "\n";
        //�܂���
        //serialPort_ = new SerialPort(portName, baudRate);
        serialPort_.Open();

        isRunning_ = true;

        thread_ = new Thread(Read);
        thread_.Start();
    }

    public void Close()
    {
        isNewMessageReceived_ = false;
        isRunning_ = false;

        if (thread_ != null && thread_.IsAlive)
        {
            thread_.Join();
        }

        if (serialPort_ != null && serialPort_.IsOpen)
        {
            serialPort_.Close();
            serialPort_.Dispose();
        }
    }

    private void Read()
    {
        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                message_ = serialPort_.ReadLine();
                isNewMessageReceived_ = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    public void Write(string message)
    {
        try
        {
            serialPort_.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public void Connect()
    {
        if(isRunning_)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
