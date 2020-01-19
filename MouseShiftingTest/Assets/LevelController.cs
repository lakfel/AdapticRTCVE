using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Master conttroller of level logic.
public class LevelController : MonoBehaviour, IPunObservable
{
    // Script to export logs
    private PersistanceManager persistanceManager;

  

    public bool started;

    public bool surveyActivated;

    // Object to mannage all the text instructions to the users
    // TODO attach in VO editor
    public GameObject notificacionTextObject;

    // Start is called before the first frame update
    void Start()
    {
      

    }

    [PunRPC]
    public void pressNextsStage()
    {
        if (!started)
        {
            started = true;

            //TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
            //text.text = "TUTORIAL";



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
