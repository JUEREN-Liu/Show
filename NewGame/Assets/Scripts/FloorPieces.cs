using UnityEngine;
using System.Collections;

public class FloorPieces : MonoBehaviour {

    private int x;
    private int y;

    public int GetX { get { return x; } }
    public int GetY { get { return y; } }

    private float gameObjectY = 0f;
    private bool useGravity = true;

    private Material normalMaterial;
    private Material hiddenMaterial;
    private Material settingMaterial;
    public Material myMaterial;

    public bool UseGravity { get { return useGravity; } set { useGravity = value; } }

    public void setPosition (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void setGameObjectY(float y)
    {
        if (useGravity && ((y > this.transform.localPosition.y && y > 0) || (y < this.transform.localPosition.y && y < 0)))
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, y, this.transform.localPosition.z);
        }
    }

    public void setStaticGameObjectY(float y)
    {
        try
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, y, this.transform.localPosition.z);
            useGravity = false;
            gameObjectY = y;
        }
        catch { }
    }



    void Awake ()
    {
        normalMaterial = Resources.Load("Materials/Normal") as Material;
        hiddenMaterial = Resources.Load("Materials/Hidden") as Material;
        settingMaterial = Resources.Load("Materials/Setting") as Material;
        myMaterial = this.GetComponent<Renderer>().material;
        myMaterial = normalMaterial;
    }

    void Update ()
    {
        if (this.transform.localPosition.y != gameObjectY)
        {
            this.transform.Translate(Vector3.down * Mathf.SmoothStep(this.transform.localPosition.y, gameObjectY, 0.9f));
        }
    }

}
