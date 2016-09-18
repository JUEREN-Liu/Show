using UnityEngine;
using System.Collections;
using live2d;


public enum GameStage { NewGame, Playing, GameOver }

public class GameController : MonoBehaviour {
    MapController mapController;
    GameObject ball;
    GameObject cameraPositionLook;
    Transform cameraNormal;
    Transform cameraLookFall;
    Transform cameraLookFall2;
    GameObject ui;
    UIController uiController;
    Material ballMaterial;

    public float timer;
    float speed = 1f;
    bool canTouchMap = false;
    bool cameraRotate = false;
    bool lookBall = false;
    
    private GameStage gameStage; 

    int fallTime;

    void Awake()
    {
        ui = Instantiate(Resources.Load("Prefabs/UI"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        uiController = ui.GetComponent<UIController>();
        cameraPositionLook = GameObject.Find("CameraPositionLook");
        cameraNormal = GameObject.Find("CameraNormal").transform;
        cameraLookFall = GameObject.Find("CameraLookFall").transform;
        cameraLookFall2 = GameObject.Find("CameraLookFall2").transform;
        this.gameObject.AddComponent<MapController>();
        mapController = this.GetComponent<MapController>();
        ball = Instantiate(Resources.Load("Prefabs/Ball"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        ball.transform.parent = GameObject.Find("Map").transform;
        ballMaterial = Resources.Load("Materials/BallMaterial") as Material;
    }

    void Start () {
        SetGameStage(GameStage.NewGame);
    }
	
    public GameStage GetGameStage()
    {
        return gameStage;
    }

    public void SetGameStage(GameStage stage)
    {
        gameStage = stage;
        if (stage == GameStage.NewGame)
        {
            mapController.CreatNewMap();
            ball.transform.localPosition = new Vector3(10, 10, 10);
            ball.GetComponent<Rigidbody>().isKinematic = true;
            canTouchMap = false;
            fallTime = 0;
            cameraRotate = true;
        }
        if (stage == GameStage.Playing)
        {
            timer = 0;
            ball.GetComponent<Rigidbody>().isKinematic = false;
            canTouchMap = true;
            cameraRotate = false;
            cameraPositionLook.transform.localRotation = Quaternion.identity;
        }
        if (stage == GameStage.GameOver)
        {
            canTouchMap = false;
            cameraRotate = true;
        }
        uiController.SetUIGameStage((int)gameStage);
    }

    public void BallFall ()
    {
        this.transform.parent = Vector3.Distance(ball.transform.position, cameraLookFall.position) < Vector3.Distance(ball.transform.position, cameraLookFall2.position) ? cameraLookFall : cameraLookFall2;
        this.transform.localPosition = Vector3.zero;
        lookBall = true;
    }

    public void BallReset()
    {
        lookBall = false;
        this.transform.parent = cameraNormal;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }

    public void PlayAgain()
    {
        fallTime = 0;
    }

    public void AddFallTime()
    {
        fallTime++;
        uiController.UpdateUIData();
    }

    public int GetFallTime()
    {
        return fallTime;
    }

	void Update () {
        /*
        int x = (int)(Mathf.Sin(Time.time * speed )* 10 + 10);
        int y = (int)(Mathf.Sin(Time.time * speed * 4) * 10 + 10);
        float h = (Mathf.Sin(Time.time * speed) * 2 );
        mapController.setTopFloorPiece(x,y,3, 2);
        */

        timer += Time.deltaTime;
        
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            setTouchPoint(Input.mousePosition);
            //SetUnityChanLook(Input.mousePosition);
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Vector3 tou = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);
            setTouchPoint(tou);
            //SetUnityChanLook(tou);
        }
#endif
        if (cameraRotate)
        {
            cameraPositionLook.transform.Rotate(Vector3.up * 1);
            ballMaterial.SetFloat("_Red", (Mathf.Sin(Time.time * 2) + 1) / 2);
            ballMaterial.SetFloat("_Green", (Mathf.Cos(Time.time * 2) + 1) / 2);
            ballMaterial.SetFloat("_Blue", (Mathf.Sin(Time.time * 3) + 1) / 2);
            ballMaterial.SetFloat("_Alpha", 0.7f);
        }
        else
        {
            try
            {
                Vector3 ballPos = new Vector3(ball.transform.position.x, 0f, ball.transform.position.z);
                float dis = Vector3.Distance(ballPos, mapController.EndFloor.transform.position);

                if (dis > 5f)
                {
                    ballMaterial.SetFloat("_Red", 1f);
                    ballMaterial.SetFloat("_Green", (Mathf.Cos(Time.time) + 1) / 2);
                    ballMaterial.SetFloat("_Blue", (Mathf.Sin(Time.time) + 1) / 2);
                    ballMaterial.SetFloat("_Alpha", 0.5f);
                }
                else if (dis < 5f)
                {

                    ballMaterial.SetFloat("_Red", 1f);
                    ballMaterial.SetFloat("_Green", 0 + dis / 10);
                    ballMaterial.SetFloat("_Blue", 0 + dis / 10);
                    ballMaterial.SetFloat("_Alpha", 0.7f - ((5 - dis) / 8f));
                }

            }
            catch { }
        }

        if (lookBall)
        {
            this.transform.LookAt(ball.transform);
        }
    }

    void setTouchPoint(Vector3 p)
    {
        if (!canTouchMap) return;
        Ray ray = Camera.main.ScreenPointToRay(p);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "FloorPiece")
            {
                mapController.setTopFloorPiece(hit.transform.GetComponent<FloorPieces>().GetX, hit.transform.GetComponent<FloorPieces>().GetY);
            }
        }
    }

    void SetUnityChanLook(Vector3 p)
    {
        if (gameStage == GameStage.GameOver)
        {
            float x = (p.x / Screen.width - 0.5f) * 2;
            float y = (p.y / Screen.height - 0.5f) * 2;
            uiController.unityChan.SetLook(new Vector2(x, y));
        }
    }
}
