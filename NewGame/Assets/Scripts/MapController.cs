using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
    public int FloorX = 20;
    public int FloorY = 20;
    public GameObject[][] floor;
    
    public int range = 4;
    public float height = 2.5f;

    public int endPointX, endPointY;
    public GameObject EndFloor;

    void Awake()
    {
        StartCoroutine("creatMap");
    }

    IEnumerator creatMap ()
    {
        floor = new GameObject[FloorX][];

        for (int i = 0; i < FloorX; i++)
        {
            floor[i] = new GameObject[FloorY];
            for (int j = 0; j < FloorY; j++)
            {
                floor[i][j] = Instantiate(Resources.Load("Prefabs/FLoorPiece"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                floor[i][j].transform.parent = GameObject.Find("Map").transform;
                floor[i][j].transform.localPosition = new Vector3(i, (i + j) * 0.1f, j);
                floor[i][j].GetComponent<FloorPieces>().setPosition(i, j);
            }
        }
        yield return null;

    }

    void Update()
    {

    }

    public void CreatNewMap()
    {
        try
        {
            floor[endPointX][endPointY].GetComponent<FloorPieces>().UseGravity = true;
            floor[endPointX][endPointY].GetComponent<Renderer>().material = Resources.Load("Materials/Hidden") as Material;
            floor[endPointX][endPointY].transform.tag = "FloorPiece";
        }
        catch { }

        float a = Random.Range(-1f, 1f);
        if (a >= 0)
        {
            a = 1f;
        }
        else
        {
            a = -1f;
        }
        float b = Random.Range(-1f, 1f);
        if (b >= 0)
        {
            b = 1f;
        }
        else
        {
            b = -1f;
        }
        endPointX = FloorX / 2 + Random.Range(2, FloorX / 2) * (int)a ;
        endPointY = FloorY / 2 + Random.Range(2, FloorY / 2) * (int)b ;
        print(endPointX +", "+ endPointY);
        floor[endPointX][endPointY].GetComponent<FloorPieces>().UseGravity = false;
        floor[endPointX][endPointY].GetComponent<Renderer>().material = Resources.Load("Materials/BallMaterial") as Material;
        floor[endPointX][endPointY].transform.tag = "EndFloorPiece";
        EndFloor = floor[endPointX][endPointY];
    }

    public void setTopFloorPiece (int x , int y)
    {
        for (int i = -range; i < range; i++)
        {
            for (int j = -range; j < range; j++)
            {
                int newY = Mathf.Min(Mathf.Abs(i) + Mathf.Abs(j) , range);
                try
                {
                    floor[x+i][y+j].GetComponent<FloorPieces>().setGameObjectY(height - (newY*(height / range)));
                }
                catch{ }
            }
        }
    }

    public void setTopFloorPiece(int x, int y, int range , float height)
    {
        for (int i = -range; i < range; i++)
        {
            for (int j = -range; j < range; j++)
            {
                int newY = Mathf.Min(Mathf.Abs(i) + Mathf.Abs(j), range);
                try
                {
                    
                    floor[x + i][y + j].GetComponent<FloorPieces>().setGameObjectY(height - (newY * (height / range)));
                }
                catch { }
            }
        }
    }

    public void setStaticTopFloorPiece (int x, int y)
    {
        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                int newY = (Mathf.Abs(i) + Mathf.Abs(j)) > range ? (range + 1) : (Mathf.Abs(i) + Mathf.Abs(j));
                try
                {
                    floor[x + i][y + j].GetComponent<FloorPieces>().setStaticGameObjectY(height - newY * height / (range + 1));
                    floor[x + i][y + j].GetComponent<FloorPieces>().UseGravity = height - (newY * (height / (range + 1))) == 0 ? true : false;
                }
                catch { }
            }
        }
    }

    public void setStaticTopFloorPiece(int x, int y, int range, float height)
    {
        
        if (height != 0)
        {
            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    int newY = (Mathf.Abs(i) + Mathf.Abs(j)) > range ? (range + 1) : (Mathf.Abs(i) + Mathf.Abs(j));
                    try
                    {
                        floor[x + i][y + j].GetComponent<FloorPieces>().setStaticGameObjectY(height - newY * height / (range + 1));
                        floor[x + i][y + j].GetComponent<FloorPieces>().UseGravity = height - (newY * (height / (range + 1))) == 0 ? true : false;
                    }
                    catch { }
                }
            }
        }
        
    }
}
