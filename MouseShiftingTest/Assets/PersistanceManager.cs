using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class PersistanceManager : MonoBehaviour
{
    public bool storeLocal;

    //Master
    private LevelController levelController;

    // Right hand of leapmotion. need to know position to log
    // Maybe is better to ask for the tracker;
    public GenericHand trackedHand;
    
    // Reference to the actual object to pick
    // TODO mannagement to switch between both users
    public PropController trackedObject;


    //TODO this must be assigned on runtime
    //Camera Reference User 1
    private GameObject camera1;

    //Camera Reference User 2
    private GameObject camera2;

    // Id of the current trial
    public string trialId;

    // Id of movement
    public string movementId;

    // Stage. ith or without proxy's match, with or without retargetting or tutorial
    public string currentStage;

    // This must be defined 
    // TODO define parts of the task
    public int typeMovement;

    // Differences with currentStage? 
    public string stage;

    public string condition1;

    public string condition2;

    // Counter of movements in this experiment
    public int counterMovements;
    
    // Stopwatch to measure times
    private Stopwatch sw;

    public string PATH_LOCAL = @"/Logout/";
    public const string NAME_GENERAL = "general.csv";
    public const string NAME_DETAIL_DOCK = "detail_dock.csv";
    public const string NAME_RUN_TIME = "runtime.csv";
    public const string NAME_HEAD_POSE = "headpose.csv";
    public const string NAME_DIF_SURVEY = "difsurvey.csv";


    // General Fields 
    public const string HEADINGS_GEN_ID = "ID_USER";
    public const string HEADINGS_GEN_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_GEN_ORDER_STAGES = "ORDER_STAGES";


    // Detail Dock Fields
    public const string HEADINGS_DET_ID_TRIAL = "ID_TRIAL ";
    public const string HEADINGS_DET_ID_MOVEMENT = "ID_MOVEMENT ";
    public const string HEADINGS_DET_STAGE = "STAGE ";
    public const string HEADINGS_DET_TIME_STAMP = "TIME_STAMP ";
    public const string HEADINGS_DET_TIME = "TIME ";
    public const string HEADINGS_DET_TYPE_OF_MOVEMENT = "TYPE_OF_MOVEMENT ";
    public const string HEADINGS_DET_GOAL_POS_X = "GOAL_POS_X ";
    public const string HEADINGS_DET_GOAL_POS_Y = "GOAL_POS_Y ";
    public const string HEADINGS_DET_FINAL_POS_X = "FINAL_POS_X ";
    public const string HEADINGS_DET_FINAL_POS_Y = "FINAL_POS_Y ";
    public const string HEADINGS_DET_GOAL_POS_Z = "GOAL_POS_Z ";
    public const string HEADINGS_DET_FINAL_POS_Z = "FINAL_POS_Z ";
    public const string HEADINGS_DET_GOAL_ROT_QX = "GOAL_ROT_QX ";
    public const string HEADINGS_DET_GOAL_ROT_QY = "GOAL_ROT_QY ";
    public const string HEADINGS_DET_GOAL_ROT_QZ = "GOAL_ROT_QZ ";
    public const string HEADINGS_DET_GOAL_ROT_QW = "GOAL_ROT_QW ";
    public const string HEADINGS_DET_FINAL_ROT_QX = "FINAL_ROT_QX ";
    public const string HEADINGS_DET_FINAL_ROT_QY = "FINAL_ROT_QY ";
    public const string HEADINGS_DET_FINAL_ROT_QZ = "FINAL_ROT_QZ ";
    public const string HEADINGS_DET_FINAL_ROT_QW = "FINAL_ROT_QW ";


    // Run time fields. Hand and object positions
    public const string HEADINGS_RT_ID_TRIAL = "ID_TRIAL";
    public const string HEADINGS_RT_ID_MOVEMENT = "ID_MOVEMENT";
    public const string HEADINGS_RT_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_RT_REAL_POS_X = "REAL_POS_X";
    public const string HEADINGS_RT_REAL_POS_Y = "REAL_POS_Y";
    public const string HEADINGS_RT_REAL_POS_Z = "REAL_POS_Z";
    public const string HEADINGS_RT_RET_POS_X = "RET_POS_X";
    public const string HEADINGS_RT_RET_POS_Y = "RET_POS_Y";
    public const string HEADINGS_RT_RET_POS_Z = "RET_POS_Z";
    public const string HEADINGS_RT_TRACK_REAL_POS_X = "TRACK_REAL_POS_X";
    public const string HEADINGS_RT_TRACK_REAL_POS_Y = "TRACK_REAL_POS_Y";
    public const string HEADINGS_RT_TRACK_REAL_POS_Z = "TRACK_REAL_POS_Z";
    public const string HEADINGS_RT_TRACK_RET_POS_X = "TRACK_RET_POS_X";
    public const string HEADINGS_RT_TRACK_RET_POS_Y = "TRACK_RET_POS_Y";
    public const string HEADINGS_RT_TRACK_RET_POS_Z = "TRACK_RET_POS_Z";
    public const string HEADINGS_RT_TRACK_ROT_QUA_X = "TRACK_ROT_QUA_X";
    public const string HEADINGS_RT_TRACK_ROT_QUA_Y = "TRACK_ROT_QUA_Y";
    public const string HEADINGS_RT_TRACK_ROT_QUA_Z = "TRACK_ROT_QUA_Z";
    public const string HEADINGS_RT_TRACK_ROT_QUA_W = "TRACK_ROT_QUA_W";
    public const string HEADINGS_RT_GRASPING = "GRASPING";
    public const string HEADINGS_RT_HAND_DETECTED = "HAND_DETECTED";
    public const string HEADINGS_RT_CURR_STAGE = "CURR_STAGE";

    // Head pose pos and rot;
    public const string HEADINGS_HP_ID_TRIAL = "ID_TRIAL";
    public const string HEADINGS_HP_ID_MOVEMENT = "ID_MOVEMENT";
    public const string HEADINGS_HP_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_HP_POS_X = "POS_X";
    public const string HEADINGS_HP_POS_Y = "POS_Y";
    public const string HEADINGS_HP_POS_Z = "POS_Z";
    public const string HEADINGS_HP_ROT_QUA_X = "ROT_QUA_X";
    public const string HEADINGS_HP_ROT_QUA_Y = "ROT_QUA_Y";
    public const string HEADINGS_HP_ROT_QUA_Z = "ROT_QUA_Z";
    public const string HEADINGS_HP_ROT_QUA_W = "ROT_QUA_W";
    public const string HEADINGS_HP_CURR_STAGE = "CURR_STAGE";

    //Dificulty survey
    public const string HEADINGS_DS_ID_TRIAL = "ID_TRIAL";
    public const string HEADINGS_DS_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_DS_PREV_STAGE = "PREV_STAGE";
    public const string HEADINGS_DS_CURR_STAGE = "CURR_STAGE";
    public const string HEADINGS_DS_QUESTION = "QUESTION";
    public const string HEADINGS_DS_ANSWER = "ANSWER";
    public bool recording;


    private IEnumerator saveLocal(string path, string[] entries, string[] values)
    {
        StreamWriter sw = null;
        if (!File.Exists(path))
        {
            sw = new StreamWriter(path, true);
            sw.WriteLine(string.Join(";", entries));
        }
        sw = sw ?? new StreamWriter(path, true);

        sw.WriteLine(string.Join(";", values));
        sw.Close();
        yield return null;
    }
    private IEnumerator saveRecords()
    {
        while(true)
        {
          /*  if (levelController.started && recording)
            {
                saveRunTime();
                saveHeadPose();
            }*/
            yield return new WaitForSeconds(0.2f);
        }
    }

    

    private void Update()
    {
       
        
    }

    private void Start()
    {
        recording = false;
        // TODO assign cameras objects
        camera1 = GameObject.Find("CenterEyeAnchor");
        counterMovements = 1;

        if(PhotonNetwork.isMasterClient)
            GetComponent<PhotonView>().RPC("setIdTrial", PhotonTargets.All, System.DateTime.Now.ToString("yyMMddHHmmss"));
        levelController = gameObject.GetComponent<LevelController>();
        StartCoroutine(saveRecords());
        //TODO 
    }

    [PunRPC]
    public void setIdTrial(string nTrialId)
    {
        trialId = nTrialId;
    }

    public void saveGeneral( )
    {
        //TODO How preset the order of stages
        //UnityEngine.Debug.Log("-----------General record ---------");
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
        
        string order ="";

        string[] values = { trialId, timeStamp, order };
        string pathLocal = PATH_LOCAL + NAME_GENERAL;
        string[] entries2 = { HEADINGS_GEN_ID, HEADINGS_GEN_TIME_STAMP, HEADINGS_GEN_ORDER_STAGES };

        if(storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
        //UnityEngine.Debug.Log("-----------General record Done---------");
    }

    public void startDocking(int partMovement)
    {
        sw = Stopwatch.StartNew();
        counterMovements++;
        movementId = System.DateTime.Now.ToString("HHmmss") +"N" + counterMovements;
        currentStage = levelController.currenStage.ToString("G");
    }

    public void saveDocking()
    {
        // textId
        Vector3 goalPosition = Vector3.zero;
        Quaternion goalRotation = Quaternion.identity;
        Vector3 finalPosition = Vector3.zero;
        Quaternion finalRotation = Quaternion.identity;

        if (trackedObject != null)
        {
            finalPosition = trackedObject.gameObject.transform.position;
            finalRotation = trackedObject.gameObject.transform.rotation;
            GameObject goalObj = trackedObject.dockProp;
            if(goalObj != null && (typeMovement == 1 || typeMovement == 2))
            {
                goalPosition = goalObj.transform.position;
                goalRotation = goalObj.transform.rotation;
            }
        }

        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");


        string[] values = { trialId, movementId, currentStage, timeStamp, sw.ElapsedMilliseconds.ToString(), typeMovement.ToString(),
                            goalPosition.x.ToString(), goalPosition.y.ToString() , goalPosition.z.ToString() , finalPosition.x.ToString(), finalPosition.y.ToString() , finalPosition.z.ToString(),
                            goalRotation.x.ToString(), goalRotation.y.ToString() , goalRotation.z.ToString() , goalRotation.w.ToString(), finalRotation.x.ToString(), finalRotation.y.ToString() , finalRotation.z.ToString() , finalRotation.w.ToString() };
        sw.Stop();


        string pathLocal = PATH_LOCAL + NAME_DETAIL_DOCK;
        string[] entries2 = { HEADINGS_DET_ID_TRIAL, HEADINGS_DET_ID_MOVEMENT, HEADINGS_DET_STAGE, HEADINGS_DET_TIME_STAMP, HEADINGS_DET_TIME, HEADINGS_DET_TYPE_OF_MOVEMENT,
                            HEADINGS_DET_GOAL_POS_X, HEADINGS_DET_GOAL_POS_Y, HEADINGS_DET_GOAL_POS_Z, HEADINGS_DET_FINAL_POS_X, HEADINGS_DET_FINAL_POS_Y, HEADINGS_DET_FINAL_POS_Z,
                            HEADINGS_DET_GOAL_ROT_QX, HEADINGS_DET_GOAL_ROT_QZ, HEADINGS_DET_FINAL_ROT_QX, HEADINGS_DET_FINAL_ROT_QY, HEADINGS_DET_FINAL_ROT_QZ, HEADINGS_DET_FINAL_ROT_QW};

        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));

    }

    public void saveRunTime()
    {

        Vector3 handPos = Vector3.zero;
        Vector3 handRetPos = Vector3.zero;
        Vector3 trackPos = Vector3.zero;
        Vector3 trackPosRet = Vector3.zero;
        Quaternion trackRot = Quaternion.identity;
        bool grasping = false;
        bool handDetected = false;

        if(trackedHand != null)
        {   
            //TODO Maybe in this case we can storw data of tracker anyway;
            if(trackedHand.canDraw())
            {
                handDetected = true;
                handPos = trackedHand.giveRealPosition();
                //TODO design about tracking position here - Maybe that should be mannaged by the prop controller? the tracker?
                //handRetPos = trackedHand.PositionPalmUpdatedRet;
            }
        }
        if(trackedObject != null)
        {
            trackPos = trackedObject.RealPosition;
            trackPosRet = trackedObject.transform.position;
            trackRot = trackedObject.transform.rotation;
        }


        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");


        string[] values = { trialId, movementId, timeStamp,
                            handPos.x.ToString(),handPos.y.ToString(),handPos.z.ToString(), handRetPos.x.ToString(),handRetPos.y.ToString(),handRetPos.z.ToString(),
                            trackPos.x.ToString(),trackPos.y.ToString(),trackPos.z.ToString(), trackPosRet.x.ToString(), trackPosRet.y.ToString(), trackPosRet.z.ToString(),
                            trackRot.x.ToString(),trackRot.y.ToString(),trackRot.z.ToString(),trackRot.w.ToString(), grasping.ToString(), handDetected.ToString(), currentStage
                            };

        string pathLocal = PATH_LOCAL + NAME_RUN_TIME;
        string[] entries2 = { HEADINGS_RT_ID_TRIAL, HEADINGS_RT_ID_MOVEMENT, HEADINGS_RT_TIME_STAMP,
                             HEADINGS_RT_REAL_POS_X, HEADINGS_RT_REAL_POS_Y, HEADINGS_RT_REAL_POS_Z, HEADINGS_RT_RET_POS_X, HEADINGS_RT_RET_POS_Y, HEADINGS_RT_RET_POS_Z
                            , HEADINGS_RT_TRACK_REAL_POS_X , HEADINGS_RT_TRACK_REAL_POS_Y, HEADINGS_RT_TRACK_REAL_POS_Z, HEADINGS_RT_TRACK_RET_POS_X, HEADINGS_RT_TRACK_RET_POS_Y, HEADINGS_RT_TRACK_RET_POS_Z
                            , HEADINGS_RT_TRACK_ROT_QUA_X, HEADINGS_RT_TRACK_ROT_QUA_Y, HEADINGS_RT_TRACK_ROT_QUA_Z, HEADINGS_RT_TRACK_ROT_QUA_W, HEADINGS_RT_GRASPING, HEADINGS_RT_HAND_DETECTED, HEADINGS_RT_CURR_STAGE};

        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
    }

    public void saveHeadPose()
    {
        
        Vector3 hpPos = camera1.transform.position;
        Quaternion hpRot = camera1.transform.rotation;
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");


        string[] values = { trialId, movementId, timeStamp,
                            hpPos.x.ToString(),hpPos.y.ToString(),hpPos.z.ToString(),hpRot.x.ToString(),hpRot.y.ToString(),hpRot.z.ToString(),hpRot.w.ToString(), currentStage };



        string pathLocal = PATH_LOCAL + NAME_HEAD_POSE;
        string[] entries2 = { HEADINGS_HP_ID_TRIAL, HEADINGS_HP_ID_MOVEMENT, HEADINGS_HP_TIME_STAMP,
                             HEADINGS_HP_POS_X, HEADINGS_HP_POS_Y, HEADINGS_HP_POS_Z, HEADINGS_HP_ROT_QUA_X, HEADINGS_HP_ROT_QUA_Y, HEADINGS_HP_ROT_QUA_Z, HEADINGS_HP_ROT_QUA_W, HEADINGS_HP_CURR_STAGE};

        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
    }

    public void saveSurveyResponse(string prevStage, string currStage, string question, string answer)
    {

        
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");


        string[] values = { trialId, timeStamp,
                            prevStage, currStage, question , answer };


        string pathLocal = PATH_LOCAL + NAME_DIF_SURVEY;
        string[] entries2 = { HEADINGS_DS_ID_TRIAL, HEADINGS_DS_TIME_STAMP, HEADINGS_DS_PREV_STAGE,
                             HEADINGS_DS_CURR_STAGE, HEADINGS_DS_QUESTION, HEADINGS_DS_ANSWER};

        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
    }


}
