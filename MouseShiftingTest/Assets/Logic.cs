using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System.Threading;
using HTC.UnityPlugin.Vive;
using Random = UnityEngine.Random;

public class Logic : MonoBehaviour
{


    public bool useFullScenarios;
   

    // I am on my turn
    public bool onTurn;

    // My Id player
    public int idPlayer;

    // Main mannager controller. 
    public LogicGame logicGame;

    // Home point
    public GameObject homePosition;
    public GameObject homePosition2;

    // Picking up positions
    public GameObject[] positions;

    // Objects
    public GameObject[] objects;

    //Current position
    public GameObject currentEndPosition;

    //Current end object
    public GameObject currentEndObject;

    public Tracker currentTracker;


    public GameObject testRedicting1;
    public GameObject testRedicting2;

    //Same part definitions
    // It is worth to think in something more collaborative
    // 0 Pre Start //-> Maybe is important to sync this. The user A can not left the object
    //                  Until user b does not place his hand on home position
    // 1 Grabbing the object
    // 2 Docking in home point
    // 3 returning object -- Maybe part 4 is not necessary
    public int stage;

    // Maybe handle thi on LogicGame
    public int repetionNumber;

    // Reference to goals. 3 props in front of the user.
    // 0. Left 1. Middle 2 Right
    public GameObject[] props;

    // Numbre of the current goal. TODO// Change the int for the object?
    public int goal; //   0 , 1,2 

    // Tells if a goal prop is currently being showed
    public bool showing;

    //It mas include a GenericHand
    public GameObject handObject;
    public IGenericHand hand;
    public HandLogic handLogic;

    // Initial point for the hand
    public GameObject initialPoint;

    //Conditions to proceed
    public bool handOnObject; // This may change
    public bool handOnInitialPosition;


    // Comunication with the master object
    private TargetedController targetedController;
    private PersistanceManager persistanceManager;
    private TrackerMannager trackerMannager;
    private MasterController masterController;
    private NotificationsMannager notificationsMannager;
    private SurveyMannager surveyMannager;
    private PropMannager propMannager;
    private PersonalNotifications personalNotifications;




    #region propEscnearioController
    // Set of orientations. 
    // 0, -90, -45, 45, 90
    private readonly Quaternion[] orientations1 = {
                                        new Quaternion(0f, 0.0f, 0.0f, 1f),//0
                                        new Quaternion(0.0f, -0.7f , 0.0f , 0.7f),//-90
                                        new Quaternion(0.0f, -0.4f , 0.0f , 0.9f),//-45
                                        new Quaternion(0.0f, 0.4f , 0.0f , 0.9f),//45
                                        new Quaternion(0.0f, 0.7f , 0.0f , 0.7f)};//90

    // Set of orientations. 
    // 0, -45, -22.5, 22.5, 45
    private readonly Quaternion[] orientations2 = {
                                        new Quaternion(0f, 0.0f, 0.0f, 1f),//0
                                        new Quaternion(0.0f, -0.4f , 0.0f , 0.9f),//-45
                                        new Quaternion(0.0f, -0.2f , 0.0f , 1f),//-22.5
                                        new Quaternion(0.0f, 0.4f , 0.0f , 0.9f),//22.4
                                        new Quaternion(0.0f, 0.2f , 0.0f , 1f) };//45


    private Queue<Quaternion> orientationsPlayer;

    // Index 1 - Object 0-1
    // Index 2 - Side 0-1
    // Index 3 orientation 0-4
    private Queue<Quaternion>[,] orientationsPlayerMatrix;


    private readonly int[,] scenarios = { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
    private Queue<int[]> scenariosPlayer;


    private int[] currentScenario;
    public void fillPlayerInformationFull()
    {



        orientationsPlayerMatrix = new Queue<Quaternion>[2, 2];
        scenariosPlayer.Clear();

        int[] checksS0 = { 0,0,0,0  };
        bool[] checksO0 = { false, false, false, false };

        int posRnd;
        Quaternion[] orientations = orientations2; // Change this if other set of orientations is wanted

        // Scenarios Player 0
        for (int i = 0; i < 16; i++)
        {
            while (checksS0[posRnd = Random.Range(0, 4) ] < 4) ;
                checksS0[posRnd]++;
            scenariosPlayer.Enqueue(new int [] { scenarios[posRnd, 0], scenarios[posRnd, 1]});
        }


        // Scenarios Player 0
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; i++)
            {
                for (int k = 0; k < 4; k++)
                    checksO0[k] = false;
                orientationsPlayerMatrix[i,j] = new Queue<Quaternion>();
                for (int k = 0; k < 4; k++)
                { 
                    while (checksO0[posRnd = Random.Range(0, 4)]) ;
                        checksO0[posRnd] = true;
                    orientationsPlayer.Enqueue(orientations2[posRnd + 1]); ;
                }
            }
        }

    }

    public void fillPlayerInformation()
    {
        orientationsPlayer.Clear();
        scenariosPlayer.Clear();

        bool[] checksO0 = { false, false, false, false };
        bool[] checksS0 = { false, false, false, false };

        int posRnd;
        Quaternion[] orientations = orientations2; // Change this if other set of orientations is wanted

        // Orientations Player 0
        for (int i = 0; i < 4; i++)
        {
            while (checksO0[posRnd = Random.Range(0, 4)]) ;
            checksO0[posRnd] = true;
            orientationsPlayer.Enqueue(orientations1[posRnd + 1]);
        }
        // Scenarios Player 0
        for (int i = 0; i < 4; i++)
        {
            while (checksS0[posRnd = Random.Range(0, 4)]) ;
            checksS0[posRnd] = true;
            scenariosPlayer.Enqueue(new int[] { scenarios[posRnd, 0], scenarios[posRnd, 1] });
        }

    }


    #endregion

    void Start()
    {

        if(!GetComponent<PhotonView>().isMine)
        {
            this.enabled = false;
        }

        orientationsPlayer = new Queue<Quaternion>();
        scenariosPlayer = new Queue<int[]>();

        masterController = gameObject.GetComponent<MasterController>();
        targetedController = gameObject.GetComponent<TargetedController>();
        persistanceManager = gameObject.GetComponent<PersistanceManager>();
        trackerMannager = gameObject.GetComponent<TrackerMannager>();
        surveyMannager = gameObject.GetComponent<SurveyMannager>();
        hand = handObject.gameObject.GetComponent<IGenericHand>();
        propMannager = gameObject.GetComponent<PropMannager>();
        personalNotifications = gameObject.GetComponent<PersonalNotifications>();
        useFullScenarios = true;


    }

    public void setNew()
    {
        handOnObject = false;
        goal = 0;
        showing = false;
        stage = -1;
        


        if(logicGame != null)
        {
            idPlayer = Int32.Parse(gameObject.name.ToCharArray()[gameObject.name.Length - 1] + "");
            homePosition = logicGame.homePositions[idPlayer];
            homePosition2 = logicGame.homePositions2[idPlayer];
            positions = logicGame.positions;
            objects = logicGame.objects;
            onTurn = (idPlayer == logicGame.currentPlayer);
            notificationsMannager = GameObject.Find("Instructions Player " + idPlayer).GetComponent<NotificationsMannager>();
            notificationsMannager.lightStepNotification(7);
        }
        trackerMannager.setTrackers();
        fillPlayerInformation();
    }

    public void onTurnStep()
    {
        if ( idPlayer == logicGame.currentPlayer)
        {
            notificationsMannager.lightStepNotification(0);
            handLogic.allowToGrab = true;
        }
        else
        {
            handLogic.allowToGrab = false;
            notificationsMannager.lightStepNotification(7);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<PhotonView>().isMine)
            //if (!surveyMannager.isSurveyActive && !notificationsMannager.masterDecide)
                if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)
                    || ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Trigger)
                    || Input.GetKeyDown(KeyCode.Q))
                {
                    if(onTurn = (idPlayer == logicGame.currentPlayer))
                    {
                        reGoal();
                    }
                }
                else if ( Input.GetKeyDown(KeyCode.T))
                {
                  test();
                }
    }

    private IEnumerator transformProp()
    {
        if (masterController.condition == MasterController.CONDITION.SM_RT)
        {
            propMannager.adapticCommand(currentEndObject.GetComponent<PropSpecs>().type);
        }
        yield return new WaitForSeconds(1.5f);
        yield return null;
    }
    private void pairTracker(bool restartTtracker)
    {
        if(restartTtracker)
        { 
            trackerMannager.fLeftTracker.detach();
            trackerMannager.fRightTracker.detach();



            if (idPlayer == 0)
                if (currentEndPosition.GetComponent<SpotSide>().currentSide == SpotSide.SIDE.LEFT)
                    currentTracker = trackerMannager.fLeftTracker;
                else
                    currentTracker = trackerMannager.fRightTracker;
            else
                if (currentEndPosition.GetComponent<SpotSide>().currentSide == SpotSide.SIDE.LEFT)
                currentTracker = trackerMannager.fRightTracker;
            else
                currentTracker = trackerMannager.fLeftTracker;
        }
        currentTracker.detach();
        currentTracker.VirtualObject = currentEndObject;
        //currentTracker.initialReference = currentEndPosition;
        
        currentTracker.attach(!restartTtracker);
        
       
        
        currentEndObject.GetComponent<PropSpecs>().activeChildren(true);
        currentEndObject.GetComponent<PropSpecs>().ghost.SetActive(false);
        
        
    }

    // All movements are isolated from the other user. TODO review this in order to male it more collaborative
    // The movement starts with the hand on the home point. This could be sync with the realease movement of the othe user
    // Once the hand is on the homepoint and trigger is pressed, the user can grabthe object. TODO // Some visual feedback to
    //              make clear that the user can proceed to grab the object. Why? In this case the user wills e the object all the time
    //              but the begining iof the movement must to be set to apply the retargetting correctly
    // The user grab the object and press the trigger
    // The ghost appears and the users must make it match and press the triggger. In this case the trigger must be placed on te table,not elevated.
    // The user releases the object and place his hand on this right in a home point, in the mean while the object changes (or not ) its shape
    // The user grab the object again 
    // The user releases the the object and the other participant starts. (think if pressing the trigger is o r not neccesary)
    public void reGoal()
    {
        if (conditionsToProceed(stage))
        {
            //Pre There is nothing // Maybe we can skup this part. Make it sync with the other user ending
            //Post Homeposition sphere activated
            if (stage == -1)
            {
                //fillPlayerInformation(); TODO fill player information at the begining of one set of trials

                logicGame.GetComponent<PhotonView>().RPC("enableHomePoint", PhotonTargets.All, idPlayer, true, false);
                //homePosition.SetActive(true);
                currentEndObject = logicGame.currentEndObject;
                currentEndPosition = logicGame.currentEndPosition;
                stage = 0;
            }
            //Pre Hand on home position.
            //Post Home position desactivated. Object allowed to be grabbed in end position
            else if (stage == 0)
            {
                targetedController.disableRT = false;
                if (currentEndObject.GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
                {
                    currentEndObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
                }
                targetedController.starShifting(currentEndPosition.transform.position, hand.giveRealPosition());

                //currentEndObject.GetComponent<PropSpecs>().activeChildren(false);

                //targetedController.starShifting(currentEndPosition.transform.position, hand.giveRealPosition());

                currentEndObject.transform.position = currentEndPosition.transform.position;
                StartCoroutine(transformProp());
                logicGame.GetComponent<PhotonView>().RPC("enableHomePoint", PhotonTargets.All, idPlayer, false, false);
                
                persistanceManager.startDocking(stage);

                stage = 1;

            }
            // Pre the user´s hand on object
            // Post hand hiden, object green and allowed to manipulate. Ghost on home position in radom orientation
            else if (stage == 1) // Hand in object Maybe should check the coliders are overlapped. Pre No shadow in scene. Pos shadow in point Z
            {
                persistanceManager.tracker = currentTracker;
                persistanceManager.trackedObject = currentEndObject.GetComponent<PropSpecs>();
                persistanceManager.saveDocking();

                if (masterController.condition == MasterController.CONDITION.NM_RT ||
                        masterController.condition == MasterController.CONDITION.SM_RT)
                {
                    currentEndObject.transform.position = targetedController.retargetedPosition.transform.position;
                }
                else
                {
                    currentEndObject.transform.position = currentEndPosition.transform.position;
                }
                currentEndObject.GetComponent<PropSpecs>().resetProp(true);
                pairTracker(true);
                handLogic.process(); logicGame.GetComponent<PhotonView>().RPC("setActiveGhost", PhotonTargets.All, true);


                // this is normal
                Quaternion orientationPlayerC = orientationsPlayer.Dequeue();

                // this isfull orientation
                int posObj = currentEndObject.GetComponent<PropSpecs>().idProp;
                int posSpot = currentEndPosition.GetComponent<SpotSide>().idSoptSide;
                orientationPlayerC = orientationsPlayerMatrix[posObj, posSpot].Dequeue();

                logicGame.GetComponent<PhotonView>().RPC("movePropDock", PhotonTargets.All, true, homePosition.transform.rotation * orientationPlayerC);
                //movePropDock(true);// TODO maybe add some animation to make the "transformation between objects"

                // persistanceManager.saveDocking();
                // persistanceManager.startDocking(stage);

                persistanceManager.startDocking(stage);

                stage = 2;
            }
            // Pre object on ghost in a correct orientation and position
            // Pos Hand appears, Objet released, HP appears on the rigth side.
            else if (stage == 2)
            {
                persistanceManager.tracker = currentTracker;
                persistanceManager.trackedObject = currentEndObject.GetComponent<PropSpecs>();
                persistanceManager.saveDocking();
                // bool isLeft = idPlayer == 0 ;

                handLogic.process();
                targetedController.disableRT = true;
                currentEndObject.transform.rotation = currentEndObject.GetComponent<PropSpecs>().ghost.transform.rotation;
                currentEndObject.transform.position = currentEndObject.GetComponent<PropSpecs>().ghost.transform.position;
                logicGame.GetComponent<PhotonView>().RPC("setActiveGhost", PhotonTargets.All, false);
                logicGame.GetComponent<PhotonView>().RPC("relocatePropDock", PhotonTargets.All);
                handLogic.allowToGrab = false;
                logicGame.GetComponent<PhotonView>().RPC("enableHomePoint", PhotonTargets.All, idPlayer, true,true);
                //persistanceManager.saveDocking();
                //propContr.movePropDock(false);
                persistanceManager.startDocking(stage);
                stage = 3;
            }
            // Pre hand on HP2
            // Pos Object transformed, HP2 Hidden
            else if (stage == 3) // Object moved to initial position in desired orientation. Pre. Shadow on, initial status off. Pos Shadow off, initial position on
            {
                persistanceManager.tracker = currentTracker;
                persistanceManager.trackedObject = currentEndObject.GetComponent<PropSpecs>();
                persistanceManager.saveDocking();
                Quaternion lastOrientation = currentEndObject.transform.rotation;
                Debug.Log("Last end orientation " + lastOrientation);
                logicGame.GetComponent<PhotonView>().RPC("enableHomePoint", PhotonTargets.All, idPlayer, false,true);
                //TODO Animation for transform the object
                currentEndObject.SetActive(false);
                currentScenario = scenariosPlayer.Dequeue();
                currentEndObject = objects[currentScenario[0]];
                currentEndPosition = positions[currentScenario[1]];
                logicGame.GetComponent<PhotonView>().RPC("refreshObjectAndPosition", PhotonTargets.All, currentScenario[0], currentScenario[1]);
                //logicGame.GetComponent<PhotonView>().RPC("refreshOtherPlayerProp", PhotonTargets.All, currentScenario[0], currentScenario[1]);
                if (currentEndObject.GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
                {
                    currentEndObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
                }

                StartCoroutine(transformProp());
                currentEndObject.transform.position = homePosition.transform.position;
                currentEndObject.transform.rotation = lastOrientation;
                currentEndObject.GetComponent<PropSpecs>().resetProp(true);
                currentEndObject.transform.Rotate(new Vector3(0f, 180f, 0f));


                Debug.Log("Orientation before pairing " + currentEndObject.transform.rotation);
                currentEndObject.SetActive(true);
                handLogic.allowToGrab = true;


                persistanceManager.startDocking(stage);
                stage = 4;
            }
            // Pre, hand on object
            // Pos hand dissapears and object in moving mood.
            else if (stage == 4)
            {
                persistanceManager.tracker = currentTracker;
                persistanceManager.trackedObject = currentEndObject.GetComponent<PropSpecs>();
                persistanceManager.saveDocking();
                pairTracker(false);
                targetedController.disableRT = false;
                handLogic.process();
                //targetedController.starShifting(currentEndPosition.transform.position, currentEndObject.transform.position);
                targetedController.starShifting( currentEndPosition.transform.position, currentEndObject.transform.position);
                logicGame.GetComponent<PhotonView>().RPC("setActiveGhost", PhotonTargets.All, true);
                Quaternion neutral = homePosition.transform.rotation;
         
                logicGame.GetComponent<PhotonView>().RPC("movePropDock", PhotonTargets.All, false,  homePosition.transform.rotation);
                //currentTracker.objectTracked = currentEndObject;
                //currentEndObject.SetActive(true);
                //homePosition.SetActive(false);
                //Vector3 po = homePosition.transform.position;
                //homePosition.transform.position = new Vector3(po.x - 0.15f, po.y, po.z);
                persistanceManager.startDocking(stage);
                stage = 5;
            }
            // Pre object on end position
            // Pos end. hand appears
            else if (stage == 5)
            {
                persistanceManager.tracker = currentTracker;
                persistanceManager.trackedObject = currentEndObject.GetComponent<PropSpecs>();
                persistanceManager.saveDocking();
                handLogic.process();
                PropSpecs propSpecs = currentEndObject.GetComponent<PropSpecs>();
                currentEndObject.transform.rotation = propSpecs.ghost.transform.rotation;
                currentEndObject.transform.position = propSpecs.ghost.transform.position;
                propSpecs.ghost.SetActive(false);
                logicGame.GetComponent<PhotonView>().RPC("relocatePropDock", PhotonTargets.All);
                propSpecs.resetProp(false);
                onTurn = false;
                logicGame.GetComponent<PhotonView>().RPC("nextStep", PhotonTargets.All);
                stage = -1;
                targetedController.disableRT = true;
                
            }
            if(stage != -1)
                notificationsMannager.lightStepNotification(stage + 1);
        }
               
    }




    public void test()
    {
        targetedController.starShifting(testRedicting1.transform.position, testRedicting2.transform.position);
    }


    public bool conditionsToProceed(int stage)
    {
        bool answer = true;
        if (stage == 0) // The hand must be touchin point zero;
        {
            NewGoal newGoal = homePosition.GetComponent<NewGoal>();
            answer = newGoal.handOnInitialPosition;
            if(!answer)
            {
                //personalNotifications.messageToUser("Be sure your hand is on the sphere");
                personalNotifications.messageToUser("La mano en la esfera");
            }
        }
        else if (stage == 1)
        {
            answer =  (handLogic.possibleObject != null);// Implicit threshold lies in the size of both hand and objects
            if (!answer)
            {
                //personalNotifications.messageToUser("Be sure your hands is on the object. \n Do not move it");
                personalNotifications.messageToUser("La mano sobre el objeto");
            }
        }
        else if (stage == 2)
        {
            float threshold = 0.02f;
            PropSpecs propSpecs = currentEndObject.GetComponent<PropSpecs>();
            answer = propSpecs.distanceToDock() < threshold;
            if (!answer)
            {
                //notificationsMannager.messageToUser(@"It seems you are not in the final position");
                personalNotifications.messageToUser("Que coincida el objeto y el fantasma");
            }
        }
        else if (stage == 3)
        {

            NewGoal newGoal = homePosition2.GetComponent<NewGoal>();
            answer = newGoal.handOnInitialPosition;
            if (!answer)
            {
                //notificationsMannager.messageToUser(@"It seems you are not in the final position");
                personalNotifications.messageToUser("La mano sobre la esfera");
            }
        }
        else if (stage == 4)
        {
            answer = (handLogic.possibleObject != null);
            if (!answer)
            {
                //notificationsMannager.messageToUser("Be sure your hand is on the sphere");
                personalNotifications.messageToUser("La mano sobre el objeto");
            }
        }
        else if (stage == 5)
        {
            float threshold = 0.02f;
            PropSpecs propSpecs = currentEndObject.GetComponent<PropSpecs>();
            answer = propSpecs.distanceToDock() < threshold;
            if (!answer)
            {
                //notificationsMannager.messageToUser("Be sure your hand is on the sphere");
                personalNotifications.messageToUser("Que coincida el objeto y el fantasma");
            }
        }


        return answer;
    }


}
