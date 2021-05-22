using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 target;
    public List<SimulationInfo> simsInfo;
    private int viewN;


    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3(0, 0, 0);
        viewN = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchTarget();
    }

    void LateUpdate()
    {
        transform.position = new Vector3(this.transform.position.x, 
            this.transform.position.y, 
            target.z);
    }

    void SwitchTarget()
    {
        // If we detect input in the form of a left-mouse click from the player, keep going
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (simsInfo!=null && simsInfo.Count >0) {
                this.target = simsInfo[viewN].sim.transform.position;
                Debug.Log("You are now viewing: Blue-Indiv-" + simsInfo[viewN].individualIndexBlue + " vs Red-indiv-" + simsInfo[viewN].individualIndexRed + "at:" + this.target);
                viewN++;
                if (viewN == simsInfo.Count)
                {
                    viewN = 0;
                }
     
            }
        }
    }
}
