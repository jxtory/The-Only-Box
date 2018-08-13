using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {
    // 物体总署
    public GameObject GameController;

    // - 定义游戏贴图和数据 - 
    // * 贴图 *
    // 盒子框
    private Sprite boxFrame;
    private Sprite[] boxEye;
    private Sprite[] boxEyeBall;
    private Sprite[] boxMouth;
    private Sprite[] boxNose;
    // * 数据 * 
    private string[] dEye;
    private string[] dEyeBall;
    private string[] dMouth;
    private string[] dNose;

	// Use this for initialization
	void Start () {
        // 初始化
        Init();
		// 边界控制
		BordersControl();
		
	}
	
	// Update is called once per frame
	void Update () {
		// 边界控制
		BordersControl();
		
	}

    // 边界控制
	void BordersControl()
	{
        GameObject.Find("Borders/Top").transform.position = new Vector3(0, (float)-Camera.main.orthographicSize, 0);
        GameObject.Find("Borders/Bottom").transform.position = new Vector3(0, (float)Camera.main.orthographicSize, 0);
        GameObject.Find("Borders/Left").transform.position = new Vector3((float)(-(Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize), 0, 0);
        GameObject.Find("Borders/Right").transform.position = new Vector3((float)((Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize), 0 ,0);
	}

	// 初始化
    void Init()
    {
    	// 载入GameController
        GameController = GameObject.Find("Factory Controller");

        // 数据定义
        // Eye = new string[]{"Block", "Round", "Triangle", "Triangle-Right"};
        dEye = new string[]{
            "Block",
            "Round",
            "Triangle",
            "Triangle-Right"
        };

        dEyeBall = new string[]{
            "Cross",
            "Disk",
            "Little"
        };

        dMouth = new string[]{
            "Angry",
            "Angry-Con",
            "Nervous",
            "Silent",
            "Small-Smile",
            "Smile",
            "Surprised1",
            "Surprised2",
            "Surprised3",
            "Sweet",
            "Tender",
            "Unhappy",
            "Unhappy2",
        };

        dNose = new string[]{
            "Chestnut",
            "Diamond",
            "Fire",
            "Little",
            "Rect"
        };

        // 资源载入
        // 盒子框
        boxFrame = Resources.Load<Sprite>("Scenes/Box_Frame");
        boxEye = new Sprite[dEye.Length];
        boxEyeBall = new Sprite[dEyeBall.Length];
        boxNose = new Sprite[dNose.Length];
        boxMouth = new Sprite[dMouth.Length];
        // 眼睛
        for(int i = 0; i < dEye.Length; i++)
        {
            string t = "Scenes/Eye/" + dEye[i];
            boxEye[i] = Resources.Load<Sprite>(t);
        }
        // 眼球
        for(int i = 0; i < dEyeBall.Length; i++)
        {
            string t = "Scenes/EyeBall/" + dEyeBall[i];
            boxEyeBall[i] = Resources.Load<Sprite>(t);
        }
        // 鼻子
        for(int i = 0; i < dNose.Length; i++)
        {
            string t = "Scenes/Nose/" + dNose[i];
            boxNose[i] = Resources.Load<Sprite>(t);
        }
        // 嘴
        for(int i = 0; i < dMouth.Length; i++)
        {
            string t = "Scenes/Mouth/" + dMouth[i];
            boxMouth[i] = Resources.Load<Sprite>(t);
        }
        
    }

    public void CreBox()
    {
    	// 创建一个新的Box GameObject
    	GameObject newBox = new GameObject();

    	// 名字确定
    	newBox.transform.name = "Box";
    	// Box归属于GameController子物体
    	newBox.transform.parent = GameController.transform;
    	// 添加精灵渲染器
        newBox.AddComponent<SpriteRenderer>();
        // 贴图
        newBox.GetComponent<SpriteRenderer>().sprite = boxFrame;
        // 添加刚体和碰撞器
        newBox.AddComponent<Rigidbody2D>();
        newBox.AddComponent<BoxCollider2D>();
        newBox.transform.position = new Vector3(0,0,0);
        newBox.GetComponent<SpriteRenderer>().sortingOrder = 1;

        // 创建面部
        GameObject Face = new GameObject();
        Face.name = "Face";
        Face.transform.parent = newBox.transform;

        // 创建眼睛
        GameObject Eye = new GameObject();
        Eye.name = "Eye";
        Eye.transform.parent = Face.transform;
        Eye.AddComponent<SpriteRenderer>();
        Eye.GetComponent<SpriteRenderer>().sprite = boxEye[Random.Range(0, dEye.Length)];
        // 显示优先级
        Eye.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // 创建眼球
        GameObject EyeBall = new GameObject();
        EyeBall.name = "EyeBall";
        EyeBall.transform.parent = Face.transform;
        EyeBall.AddComponent<SpriteRenderer>();
        EyeBall.GetComponent<SpriteRenderer>().sprite = boxEyeBall[Random.Range(0, dEyeBall.Length)];
        // 显示优先级
        EyeBall.GetComponent<SpriteRenderer>().sortingOrder = 3;

        // 创建鼻子
        GameObject Nose = new GameObject();
        Nose.name = "EyeNose";
        Nose.transform.parent = Face.transform;
        Nose.AddComponent<SpriteRenderer>();
        Nose.GetComponent<SpriteRenderer>().sprite = boxNose[Random.Range(0, dNose.Length)];
        // 显示优先级
        Nose.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // 创建鼻子
        GameObject Mouth = new GameObject();
        Mouth.name = "EyeMouth";
        Mouth.transform.parent = Face.transform;
        Mouth.AddComponent<SpriteRenderer>();
        int t = Random.Range(0, dMouth.Length);
        Mouth.GetComponent<SpriteRenderer>().sprite = boxMouth[t];
        // 显示优先级
        Mouth.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // 缩放物体
        newBox.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

    }

}
