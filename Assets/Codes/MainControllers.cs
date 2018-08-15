﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllers : MonoBehaviour {
	/* - - - - - - - - - - 
		游戏名称： 
			英：The Only Box
			中：找到那方块

		Code说明：方块、盒子、立方体、Box	以下均简称盒子

		游戏模式
			1 趣味模式	2 超强模式

		游戏元素
			欢乐球	道具	变脸
	*/

	// - 物体总署 -
	[Header("游戏总署")]
	public GameObject GameController;

	// - 控制中心 -
	// 边界控制开关
	[Header("边界控制")]
	public bool BorderControl = false;
	private bool borderTopC = true;
	private bool borderBottomC = true;
	private bool borderLeftC = true;
	private bool borderRightC = true;
	// 边界控制内容
	private GameObject borderTop;
	private GameObject borderBottom;
	private GameObject borderLeft;
	private GameObject borderRight;

	// 面部活跃模式 true 为 活跃 false 为死板 默认为活跃
	[Header("面部活跃")]
	public bool FaceMode = true;

	// - 游戏贴图和数据 -
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

	// - 游戏元素及计算数据 -
	// 用于存储盒子
	private ArrayList Boxs;
	private Vector3 boxSize = new Vector3(1, 1, 1);

	// - - - - - - - - - - -

	// - 边界控制 -
	void BordersControl()
	{
		// 允许控制
		if(BorderControl){
			// 上控制
			if(borderTopC){
				GameObject.Find("Borders/Top").transform.position = new Vector3(0, (float)Camera.main.orthographicSize, 0);
			}
			// 下控制
			if(borderBottomC){
				GameObject.Find("Borders/Bottom").transform.position = new Vector3(0, (float)-Camera.main.orthographicSize, 0);
			}
			// 左控制
			if(borderLeftC){
				GameObject.Find("Borders/Left").transform.position = new Vector3((float)(-(Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize), 0, 0);
			}
			// 右控制
			if(borderRightC){
				GameObject.Find("Borders/Right").transform.position = new Vector3((float)((Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize), 0 ,0);
			}
		}
	}

	// - 创建盒子 -
	public void CreateBox()
	{
    	// 创建一个新的盒子(躯体)
    	GameObject newBox = new GameObject();
    	// 给盒子添加MiniBox控制器(灵魂)
    	newBox.AddComponent<MiniBox>();
    	// 控制MiniBox(附身)
    	MiniBox box = newBox.GetComponent<MiniBox>();
    	// 绑定盒子与MiniBox(捆绑)
    	box.SetBoxSelf(newBox);
    	// 给盒子指定从属的控制器(寄宿)
    	box.GC = this.GetComponent<MainControllers>();
    	// 给盒子命名(名字)
    	box.MyName = "Box";
    	// 设置缩放比例(体形)
    	boxSize = new Vector3(0.7f, 0.7f, 0.7f);
    	box.SetScaleSize(boxSize);
        // 决定心灵控制
        if (!FaceMode){box.BPsychomotor = false;}
        // 设置盒子面部
        int[] face = new int[4];
    	face[0] = Random.Range(0, dEye.Length);
    	face[1] = Random.Range(0, dEyeBall.Length);
    	face[2] = Random.Range(0, dNose.Length);
    	face[3] = Random.Range(0, dMouth.Length);
    	box.SetBoxFace(face);
    	// 生成盒子(出生)
    	box.SetGameObject();

	    // 从出生点出生
	    newBox.transform.position = new Vector3();

	}

	// - 找寻GameObject -
	GameObject FindIt(string him)
	{
		return GameObject.Find(him);
	}

	// - 获取盒子框 -
	public Sprite GetBoxFrame()
	{
		return boxFrame;
	}

	// - 获取眼眶 -
	public Sprite GetBoxEye(int eye)
	{
		return boxEye[eye];
	}

	// - 获取眼球 -
	public Sprite GetBoxEyeBall(int eyeball)
	{
		return boxEyeBall[eyeball];
	}
	
	// - 获取鼻子 -
	public Sprite GetBoxNose(int nose)
	{
		return boxNose[nose];
	}

	// - 获取嘴 -
	public Sprite GetBoxMouth(int mouth)
	{
		return boxMouth[mouth];
	}

	// - 找寻嘴的地址 -
	public int GetMouthNumber(string him)
	{
		int tMouth = -1;

		for(int i = 0; i < dMouth.Length; i++){
			if(dMouth[i] == him){
				tMouth = i;
				return tMouth;
			}
		}
		return tMouth;
	}

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

	// - 初始化 -
	void Init()
	{
    	// 载入GameController
        //GameController = GameObject.Find("GameController");
        GameController = FindIt("GameController");

        // 载入边框
        borderTop = FindIt("Top");
        borderBottom = FindIt("Bottom");
        borderLeft = FindIt("Left");
        borderRight = FindIt("Right");

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

        // - 资源载入 -
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
}
