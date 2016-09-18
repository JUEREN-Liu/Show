using UnityEngine;
using System.Collections;

public class CharterController : MonoBehaviour {

    //public bool GameOver = false;
    

	void Start () {
	    
	}
	

	void Update () {
	    if (this.transform.localPosition.y < -50f)
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.localPosition = new Vector3(10, 10, 10);
            this.GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Main Camera").GetComponent<GameController>().BallReset();
            GameObject.Find("Main Camera").GetComponent<GameController>().AddFallTime();
        }

        if (this.transform.localPosition.x > 20 || this.transform.localPosition.x < -1 || this.transform.localPosition.z > 20 || this.transform.localPosition.z < -1)
        {
            GameObject.Find("Main Camera").GetComponent<GameController>().BallFall();
        }

	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EndFloorPiece")
        {
            //GameOver = true;
            this.GetComponent<Rigidbody>().isKinematic = true;
            GameObject.Find("Main Camera").GetComponent<GameController>().SetGameStage(GameStage.GameOver);
        }

    }
}
