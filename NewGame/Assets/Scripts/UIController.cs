using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIController : MonoBehaviour {
    GameObject talkUI;
    GameObject talkCotent;
    GameObject talkPerson;
    GameObject talkTouch;
    GameObject gameOverUI;

    GameObject pointUI;
    GameObject fallTime;

    public Live2DController unityChan;

    public enum UIStage { newGame, Playing, GameOver }
    public UIStage uiStage;

    void Awake()
    {
        talkUI = GameObject.Find("TalkUI");
        talkCotent = GameObject.Find("TalkCotent");
        talkPerson = GameObject.Find("TalkPerson");
        talkTouch = GameObject.Find("TalkTouch"); 
        gameOverUI = GameObject.Find("GameOverUI");
        pointUI = GameObject.Find("PointUI");
        fallTime = GameObject.Find("FallTime");
        unityChan = GameObject.Find("Live2D").GetComponent<Live2DController>();
        talkUI.SetActive(false);
        gameOverUI.SetActive(false);
        pointUI.SetActive(false);
        unityChan.gameObject.SetActive(false);
    }

    void Start() {
        UpdateUIData();
    }


    void Update() {

    }

    public void UpdateUIData()
    {
        fallTime.GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<GameController>().GetFallTime().ToString();
    }

    public void UITakeTouch()
    {
        GameObject.Find("Main Camera").GetComponent<GameController>().SetGameStage(GameStage.Playing);
    }

    public void UIAgain()
    {
        GameObject.Find("Main Camera").GetComponent<GameController>().SetGameStage(GameStage.NewGame);
    }

    public void SetUIGameStage(int stage)
    {
        if (stage == 0)
        {
            SetNewGame();
        }
        if (stage == 1)
        {
            SetPlaying();
        }
        if (stage == 2)
        {
            SetGameOver();
        }
    }

    void SetNewGame()
    {
        talkUI.SetActive(true);
        talkTouch.SetActive(true);
        gameOverUI.SetActive(false);
        pointUI.SetActive(false);
        unityChan.gameObject.SetActive(true);
        talkCotent.GetComponent<Text>().text = "遊戲開始後點擊地板將畫面中的球推到地圖上唯一個顏色較特別的長方形上。";
        unityChan.SetUnityChanMotion(GameStage.NewGame);
        UpdateUIData();
    }

    void SetPlaying()
    {
        talkUI.SetActive(false);
        gameOverUI.SetActive(false);
        pointUI.SetActive(true);
        unityChan.gameObject.SetActive(false);
    }

    void SetGameOver()
    {
        talkUI.SetActive(true);
        talkTouch.SetActive(false);
        gameOverUI.SetActive(true);
        pointUI.SetActive(true);
        unityChan.gameObject.SetActive(true);
        string a = "恭喜成功!!\n";
        string b = "球總共掉出去 " + GameObject.Find("Main Camera").GetComponent<GameController>().GetFallTime().ToString() + " 次";
        string c = "，共花了 "+ (int)GameObject.Find("Main Camera").GetComponent<GameController>().timer +" 秒。";
        talkCotent.GetComponent<Text>().text = a + b + c;
        unityChan.SetUnityChanMotion(GameStage.GameOver);
    }
}
