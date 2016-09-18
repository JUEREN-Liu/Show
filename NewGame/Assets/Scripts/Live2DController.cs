using UnityEngine;
using System.Collections;
using live2d;


public enum UnicyChanMotion {normal, start, clear }
public class Live2DController : MonoBehaviour {
    public TextAsset mocFile;
    public Texture2D textures;
    public TextAsset[] mtnFiles;

    private Live2DModelUnity live2DModel;

    private Live2DMotion[] motion;
    private MotionQueueManager motionManager;
    private Matrix4x4 live2DCanvasPos;

    public UnicyChanMotion unityChanMotion;

    float faceParamX;
    float faceParamY;
    float EyeParamX;
    float EyeParamY;

    void Awake () {
        Live2D.init();

        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);
        live2DModel.setTexture(0, textures);
        
        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

        motion = new Live2DMotion[mtnFiles.Length];

        for (int i = 0; i < mtnFiles.Length; i ++)
        {
            motion[i] = Live2DMotion.loadMotion(mtnFiles[i].bytes);
        }

        //motion = Live2DMotion.loadMotion(mtnFiles[0].bytes);
        //motion.setLoop(true);

        motionManager = new MotionQueueManager();
        motionManager.startMotion(motion[0], false);

        //motion = Live2DMotion.loadMotion(mtnFiles[0].bytes);
    }

    public void SetUnityChanMotion(GameStage stage)
    {
        motionManager.stopAllMotions();
        if (stage == GameStage.NewGame)
        {
            SetMotion(4);
            unityChanMotion = UnicyChanMotion.start;
        }

        if (stage == GameStage.GameOver)
        {
            SetMotion(7);
            unityChanMotion = UnicyChanMotion.clear;
        }   
    }

    void SetMotion(int mot)
    {
        ResetLook();
        motionManager.stopAllMotions();
        motionManager.startMotion(motion[mot]);
    }

    public void SetLook(Vector2 pos)
    {
        motionManager.stopAllMotions();
        live2DModel.setParamFloat("PARAM_ANGLE_X",pos.x * 30);
        live2DModel.setParamFloat("PARAM_ANGLE_Y", pos.y * 30);
        live2DModel.setParamFloat("PARAM_EYE_BALL_X", pos.x);
        live2DModel.setParamFloat("PARAM_EYE_BALL_Y", pos.y);
    }

    public void ResetLook()
    {
        live2DModel.setParamFloat("PARAM_ANGLE_X", 0);
        live2DModel.setParamFloat("PARAM_ANGLE_Y", 0);
        live2DModel.setParamFloat("PARAM_EYE_BALL_X", 0);
        live2DModel.setParamFloat("PARAM_EYE_BALL_Y", 0);
    }
	
	void Update () {
        if (live2DModel == null) return;
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

        if (motionManager.isFinished())
        {
            motionManager.startMotion(motion[0]);
            unityChanMotion = UnicyChanMotion.normal;
        }
        

        motionManager.updateParam(live2DModel);

        live2DModel.update();
    }

    void OnRenderObject()
    {
        if (live2DModel == null) return;
        live2DModel.draw();
    }
}
