using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugHandler : MonoBehaviour
{

    public static DebugHandler instance;

    [SerializeField] Text debugText_Cp;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitDebugInfo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InitDebugInfo();
        }
    }

    public void AddDebugInfo(object text)
    {
        debugText_Cp.text += "\r\n" + text.ToString();
    }

    void InitDebugInfo()
    {
        debugText_Cp.text = "DebugInfo:";
    }
}
