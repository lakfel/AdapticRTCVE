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
    private TrackerMannager trackerMannager;

    // Information for the user. No sync here
    private NotificationsMannager notificationsMannager;

    // Survey to be performed in each case.
    private SurveyMannager surveyMannager;

    private Logic logic;

    // Base condition
    public CONDITION condition;
  
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
     

        surveyActivated = false;
        
        // TODO the steps should be shared. Notification Mannager to be changes drasticly
        //notificationsMannager.lightStepNotification(1);

         
         
    }

    public void setNew()
    {
        
       /*
        if (notificacionTextObject != null)
            notificacionTextObject.SetActive(true);
            */
        
        //persistanceManager.recording = false;
        //persistanceManager.userId = System.DateTime.Now.ToString("yyMMddHHmmss");
        //TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
        //text.text = "Welcome";*/
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

    public void setNewLogic()
    {
        if(GetComponent<PhotonView>().isMine)
        { 
            logic.setNew();
        }
    }

    [PunRPC]
    public void fillPlayerInformation()
    {
        if(GetComponent<PhotonView>().isMine)
        {
            logic.fillPlayerInformation();
        }
    }

    public void nextStage()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            logic.onTurnStep();
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
