using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGame : MonoBehaviour, IPunObservable
{
    public bool started;

    public MasterController[] playerMasters;
    // public Logic[] playerLogics;

    // Picking up positions
    public GameObject[] positions;

    // Objects
    public GameObject[] objects;

    // Both home positions
    public GameObject[] homePositions;


    public GameObject currentEndObject;
    public GameObject currentEndPosition;

    public int currentPlayer;

    //Same part definitions
    // 0 Pre Start
    // 1 Grabbing the object
    // 2 Docking in home point
    // 3 returning object -- Maybe part 4 is not necessary
    public int repetition;
    public const int REPETITIONS = 4;


    public void setPlayers()
    {
        playerMasters = new MasterController[2];
        //playerLogics = new Logic[2];
        GameObject master = GameObject.Find("Player 0");
        if (master != null)
        {
            playerMasters[0] = master.GetComponent<MasterController>();
            // playerLogics[0] = master.GetComponent<Logic>();
            master.GetComponent<Logic>().logicGame = this;
        }
        else
            GetComponent<PhotonView>().RPC("boadcastError", PhotonTargets.All, "ERROR FROM " + PhotonNetwork.player.NickName + " NOT PLAYER 0 FOUND");
        master = GameObject.Find("Player 1");
        if (master != null)
        {
            playerMasters[1] = master.GetComponent<MasterController>();
            //playerLogics[1] = master.GetComponent<Logic>();
            master.GetComponent<Logic>().logicGame = this;
        }
        else
            GetComponent<PhotonView>().RPC("boadcastError", PhotonTargets.All, "ERROR FROM " + PhotonNetwork.player.NickName + " NOT PLAYER 1 FOUND");

    }

    [PunRPC]
    public void boadcastError(string msg)
    {
        Debug.LogError(msg);
    }


    public void initialObjectPosition(int indexObj, int indexPos)
    {

        currentEndObject = objects[indexObj];
        currentEndPosition = positions[indexPos];
        currentEndObject.SetActive(true);
        currentEndObject.transform.position = currentEndPosition.transform.position;
        currentPlayer = 0;
        playerMasters[0].setNewLogic();
        //playerMasters[1].setNewLogic();


    }
    [PunRPC]
    public void nextStep()
    {
        repetition += currentPlayer;
        if(repetition == REPETITIONS)
        {
            playerMasters[0].fillPlayerInformation();
            playerMasters[1].fillPlayerInformation();
            repetition = 0;

        }
        currentPlayer = (currentPlayer + 1) % 2;
        
    }

    [PunRPC]
    public void refreshObjectAndPosition(int indexObj, int indexPos)
    {
        currentEndObject = objects[indexObj];
        currentEndPosition = positions[indexPos];
    }




    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        repetition = 0;
    }
    [PunRPC]
    public void getStared(int indexObj, int indexPos)
    {
        started = true;
        setPlayers();
        initialObjectPosition(indexObj, indexPos);
        
    }   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!started)
            {
                GetComponent<PhotonView>().RPC("getStared", PhotonTargets.All, Random.Range(0, 2), Random.Range(0, 2));
            }
        }
    }
}
