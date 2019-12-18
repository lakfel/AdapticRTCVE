using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsMannager : MonoBehaviour
{

    public TextMesh[] steps;
    public TextMesh personalStageBar;

    public int counterGoals;
    public bool masterDecide;
    // Start is called before the first frame update
    void Start()
    {
        counterGoals = 0;
        masterDecide = false;
    }
    private bool triggerPressed;
    public void registerGoal()
    {
        counterGoals++;
        if(counterGoals == 2)
        {
            showGoalDone(true);
            counterGoals = 0;
            masterDecide = true;
        }

        


    }
    void showGoalDone(bool show)
    {
       
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void changeTitle(string ntitle)
    {
        personalStageBar.text = ntitle;
    }

    public void lightStepNotification(int step)
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].gameObject.SetActive(false);
        }
        steps[step].gameObject.SetActive(true);
    }
    /*
    public void messageToUser(string message)
    {
        StartCoroutine(showMessage(message));
    }
    /*
    IEnumerator showMessage(string message)
    {
        personalNotificationBar.gameObject.SetActive(true);
        personalNotificationBar.text = message;
        yield return new WaitForSeconds(2);
        personalNotificationBar.gameObject.SetActive(false);
    }
    */
    /*
    public void normalSettings()
    {
        counterGoals = 0;
        showGoalDone(false);
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].color = Color.white;
            steps[i].fontSize = 35;
        }
    }*/
}
