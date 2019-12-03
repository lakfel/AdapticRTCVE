using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINetwork : MonoBehaviour
{
    public TextMesh txtSummary;
    public TextMesh txtStatusUser1;
    public TextMesh txtStatusUser2;
    //TODO Maybe customize with names
    public void refreshStatus(bool[] activeUsers)
    {
        int count = 0;
        string text1 = "Usuario 1: Offline";
        string text2 = "Usuario 2: Offline";
        if (activeUsers[0])
        {
            text1 = "Usuario 1: Online";
            count++;
        }
        if (activeUsers[1])
        {
            text2 = "Usuario 2: Online";
            count++;
        }
        txtStatusUser1.text = text1;
        txtStatusUser2.text = text2;
        txtSummary.text = "Usuarios conectados: " + count;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
