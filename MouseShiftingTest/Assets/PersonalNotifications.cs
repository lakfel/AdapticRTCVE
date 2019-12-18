using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalNotifications : MonoBehaviour
{
    public TextMesh userText;
    public GameObject notificationsObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator showMessage(string message)
    {
        notificationsObject.SetActive(true);
        userText.text = message;
        userText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);

        yield return null;
    }

    public void messageToUser(string message)
    {
        StartCoroutine(showMessage(message));
    }
}
