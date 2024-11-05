using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    static private int INPUT_SIZE = 256;
    static private int FPS = 30;

    // UI
    RawImage rawImage;
    WebCamTexture webCamTexture;

    // �X�^�[�g���ɌĂ΂��
    void Start()
    {
        // Web�J�����̊J�n
        WebCamDevice[] devices = WebCamTexture.devices;
        this.rawImage = GetComponent<RawImage>();
        foreach(var cam in devices)
        {
            Debug.Log(cam.name);
        }
        this.webCamTexture = new WebCamTexture("Logi C615 HD WebCam",INPUT_SIZE, INPUT_SIZE, FPS);
        this.rawImage.texture = this.webCamTexture;
        this.webCamTexture.Play();
    }
}
