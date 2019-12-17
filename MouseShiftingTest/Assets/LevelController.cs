using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Master conttroller of level logic.
public class LevelController : MonoBehaviour, IPunObservable
{
    // Script to export logs
    private PersistanceManager persistanceManager;

    public enum STAGE
    {
        // Bases A and B
        // Base A: SM_RT 
        // Base B: NM_RT or NM_RT -> TODO
        TUTORIAL, // TODO what kind of props are gonna use here
        FIRST, // Base A User 1, Base B User 2
        SECOND // Base A User 2, Base B User 2 
    }

    public STAGE currenStage;
    public int numStage;
    public readonly STAGE[] stages = new STAGE[] { STAGE.TUTORIAL, STAGE.FIRST, STAGE.SECOND };

    public bool started;

    public bool surveyActivated;

    // Object to mannage all the text instructions to the users
    // TODO attach in VO editor
    public GameObject notificacionTextObject;

    // Start is called before the first frame update
    void Start()
    {
        persistanceManager = gameObject.GetComponent<PersistanceManager>();
        if (persistanceManager != null)
        {
            persistanceManager.storeLocal = false;
        }

    }

    [PunRPC]
    public void pressNextsStage()
    {
        if (!started)
        {
            started = true;
            currenStage = STAGE.TUTORIAL;
            numStage = 0;

            //TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
            //text.text = "TUTORIAL";

            persistanceManager.saveGeneral();

            //TODO What is needed for having the instructinos and UI guide work

        }
        else
        {

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<PhotonView>().RPC("pressNextsStage", PhotonTargets.All);
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
