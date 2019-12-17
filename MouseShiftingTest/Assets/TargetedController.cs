using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is in charge tof the retargeing if applies.
public class TargetedController : MonoBehaviour
{

    public bool disableRT;

    public float compensation;
    public int newGoal;

    /**
     * This factor helps to adjust the movement of the hand in the scene.
    */
    private float movementFactor = 1f;

    /**
     *  Initial position of the hand at the moment to star the retargettin
     */
    private Vector3 startPosition;

    /**
     *  Goal position. 
     */
    private Vector3 endPosition;

    /**
     * Retargetered position in real world
     */
    public GameObject retargetedPosition;

    /*
     * Master contoller of the player
     */
    public MasterController masterController;

    /*
     * Level controller 
     */
    public LevelController levelController;

    private void Start()
    {

        if (!GetComponent<PhotonView>().isMine)
        {
            this.enabled = false;
        }
        disableRT = false;
        retargetedPosition = GameObject.Find("MiddlePropPosition");
        masterController = gameObject.GetComponent<MasterController>();
        GameObject objLevelController = GameObject.Find("LevelMannager");
        levelController = objLevelController.GetComponent<LevelController>();

        // TODO delete this. Only test purpouses
       // GameObject testRetargeting = GameObject.Find("RightPropPosition");
       // GameObject initialP = GameObject.Find("HandStartPoint");
        //starShifting(testRetargeting.transform.position, initialP.transform.position);
        // Test ends here
    }

    /**
     *  Compensation factors
     */
    public float compensationFactorX;
    public float compensationFactorZ;


    public bool shifting;

    public void starShifting(Vector3 newEndPosition, Vector3 newStartPosition)
    {
        //startPosition = gameObject.transform.position;
        startPosition = newStartPosition;
        endPosition = newEndPosition;

        Vector3 retPosition = retargetedPosition.transform.position;

        float xCenter = retPosition.x - startPosition.x;
        float zCenter = retPosition.z - startPosition.z;

        float xGoal = endPosition.x - startPosition.x;
        float zGoal = endPosition.z - startPosition.z;

        compensationFactorX = (xGoal - xCenter) / zCenter;
        // compensationFactorZ = (zGoal) / zCenter; It is actually this but that must be one cuz they are at the same position in z
        // It is not working good since the position in Z is dispalced -3 mts and something. But, since the cocients should be close to 1, we force the value.
        compensationFactorZ = 1;

        shifting = true;

       // Debug.Log("Compensation factor X :" + compensationFactorX);
       // Debug.Log("Compensation factor Z :" + compensationFactorZ);
       // Debug.Log("StarZ :" + startPosition.z + " --- EndZ : " + endPosition.z +  " ---  CenterZ  " + retPosition.z);

    }


    public Vector3 giveRetargetedPosition(Vector3 realPosition)
    {
        Vector3 rePosition = new Vector3();
        rePosition = realPosition;
        
        if (!disableRT)
            if (shifting && masterController != null && levelController != null )//&& levelController.currenStage != LevelController.STAGE.TUTORIAL)
            {
                if (masterController.condition == MasterController.CONDITION.NM_RT || masterController.condition == MasterController.CONDITION.SM_RT)
                {
                    float xAnchor = (realPosition.x - startPosition.x) * movementFactor;
                    float yAnchor = (realPosition.y - startPosition.y) * movementFactor;
                    float zAnchor = (realPosition.z - startPosition.z) * movementFactor;

                    rePosition = new Vector3(xAnchor + zAnchor * compensationFactorX + startPosition.x, yAnchor + startPosition.y, zAnchor * compensationFactorZ + startPosition.z);
                    //Debug.Log("RETARGETTING ");
                }
            }

        
        return rePosition;
    }


}
