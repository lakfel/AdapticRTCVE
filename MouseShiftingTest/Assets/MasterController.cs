using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// In Adaptic + retargetin project this script manages the whole logic.
// In second project this mannages only the user logic, locally and in network
public class MasterController : MonoBehaviour, IPunObservable
{
    // Networking mannagement here//
    #region NetworkinMannagement
    // Current player ID
    public int userId;

    //Local player instance. Used to verufy if the local player is represented in the scene.
    // JFGA TODO. Verify if this is needed or not
    public static GameObject LocalPlayerInstance;
    #endregion

    public enum CONDITION
    {
        SM_RT,
        SM_OO,
        NM_RT,
        NM_OO
    }

    // Script for manage tracking. 
    // TODO review if needed it. Now we will have only on condition.
    private TrackerMannager trackerMannager;

    // Information for the user. No sync here
    private NotificationsMannager notificationsMannager;

    // Survey to be performed in each case.
    private SurveyMannager surveyMannager;

    //TODO Need to have stages. And mannage this in the data output.
    private Logic logic;

    // Base condition
    public CONDITION condition;

    public bool started;

    
    public LevelController levelController; 
   

    public int stageCounter;

    public bool surveyActivated;


    private void Awake()
    {
        this.gameObject.name = GetComponent<PhotonView>().owner.NickName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        trackerMannager = gameObject.GetComponent<TrackerMannager>();
        notificationsMannager = gameObject.GetComponent<NotificationsMannager>();
        surveyMannager = gameObject.GetComponent<SurveyMannager>();
        logic = gameObject.GetComponent<Logic>();
        if (!GetComponent<PhotonView>().isMine)
        {
            //trackerMannager.enabled = false;
            //logic.enabled = false;
        }
            GameObject objLevelController = GameObject.Find("LevelMannager");
        if (objLevelController != null)
            levelController = objLevelController.GetComponent<LevelController>();
        else
            Debug.Log("JFGA -- No Level controller FOUND!");

        surveyActivated = false;
        
        // TODO the steps should be shared. Notification Mannager to be changes drasticly
        //notificationsMannager.lightStepNotification(1);

        started = true;

         
         
    }

    public void setNew()
    {
        /*
        started = false;
        if (notificacionTextObject != null)
            notificacionTextObject.SetActive(true);

        stageCounter = 0;
        stagesDone = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            stagesDone[i] = false;
        }

        stages = new EXP_STAGE[]
        {
            EXP_STAGE.TUTORIAL,
            EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING,
            EXP_STAGE.PROP_MATCHING_NO_RETARGETING,
            EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING,
            EXP_STAGE.PROP_NOT_MATCHING_NO_RETARGETING
        };
        persistanceManager.recording = false;
        persistanceManager.userId = System.DateTime.Now.ToString("yyMMddHHmmss");
        TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
        text.text = "Welcome";*/
    }

    // Update is called once per frame
    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
           /* trackerMannager.setTrackers();

            if (!started)
            {
                started = true;

                currentStage = EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING;
                stagesDone[0] = true;

                notificationsMannager.lightStepNotification(1);

                if (notificacionTextObject != null)
                {
                    TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
                    text.text = "TUTORIAL";
                }

                if (persistanceManager != null)
                    persistanceManager.saveGeneral();
                else
                    Debug.LogError("PersistanceMannager missing!!! No results reported");
                //Call persistance to update
            }
            else
                nextStage();

            trackerMannager.setTrackers();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            setNew();
            notificationsMannager.normalSettings();
            persistanceManager.recording = false;

            GameObject leftProp = GameObject.Find("LeftProp");
            if (leftProp != null)
                leftProp.GetComponent<PropController>().angleNumber = 0;

            GameObject rightProp = GameObject.Find("RightProp");
            if (rightProp != null)
                rightProp.GetComponent<PropController>().angleNumber = 0;*/

        }
    }

    public void nextStage()
    {
      /*  persistanceManager.recording = true;
        if(stageCounter < 4)
        {
            logic.stage = -1;
            stageCounter++;
            int newStage = ordersStages[subjectOrder, stageCounter-1];
            stagesDone[newStage] = true; 
            currentStage = stages[newStage];
            if (notificacionTextObject != null)
            { 
                TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
                text.text = "Stage number : " + stageCounter;  
            }

        }
        if (stageCounter >= 2 && stageCounter <= 5)
        {
            surveyMannager.startSurvey(stageCounter, currentStage.ToString("G"));
        }*/
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
