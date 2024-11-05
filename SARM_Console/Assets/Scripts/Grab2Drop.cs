using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grab2Drop : MonoBehaviour
{
    [SerializeField] private Toggle Garb_or_Drop;
    [SerializeField] private Text label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void toggle_hand()
    {
        if (Garb_or_Drop.isOn)
        {
            this.label.text = "Grab";
        }
        else
        {
            this.label.text = "Drop";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
