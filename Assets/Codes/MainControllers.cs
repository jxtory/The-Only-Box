using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllers : MonoBehaviour {
	// - 物体总署 -
	[Header("游戏总署")]
	public GameObject GameController;
	// - 区域空间 -
	[Header("区域空间")]
	public GameObject AreaSpace;
	// - 辅助相机 -
	[SerializeField]
	[Header("辅助相机")]
	private GameObject auxCamera = null;

	// - 控制中心 -
	// 边界控制开关
	[Header("边界控制")]
	public bool BorderControl = true;
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
	// 面部数据
	private string[] dEye;
	private string[] dEyeBall;
	private string[] dMouth;
	private string[] dNose;
	// 区域数据
	private string[] dAreaCenter_DeliVery;

	// - 游戏元素及计算数据 -
	// 用于存储盒子
	private ArrayList Areas;
	private ArrayList Boxs;
	private Vector3 boxSize = new Vector3(1, 1, 1);
	// 出生点
	private Vector3 spawnPoint;
	private Vector3 joyBallSpawnPoint;
	// 入口方向
	private GameObject entranceDir;

	[Header("跟随镜头(场景)")]
	// 场景是否跟随主镜头
	public bool FollowCamera = false;
	// 辅助相机目标位置	和 显示效率
	private ArrayList auxCameraTarget;
	private float auxCameraShowTimer = 5f;
	private int auxCameraShowTime = 5;
    private Vector3 auxCameraVelocity = Vector3.zero;

	// - - - - - - - - - - -
	// - 属性控制 -
	// 获取上边框
	public GameObject BorderTop
	{
		get
		{
			return borderTop;
		}
	}

	// 获取下边框
	public GameObject BorderBottom
	{
		get
		{
			return borderBottom;
		}
	}

	// 获取左边框
	public GameObject BorderLeft
	{
		get
		{
			return borderLeft;
		}
	}

	// 获取右边框
	public GameObject BorderRight
	{
		get
		{
			return borderRight;
		}
	}

	// - - - - - - - - - - -

	// - 辅助相机控制 -
	void auxCameraControl()
	{
		// 查看辅助相机和任务 如果存在立即投入工作
		if(auxCamera != null && auxCameraTarget != null && auxCameraTarget.Count > 0){
            Vector3 tg = (Vector3)auxCameraTarget[0];
			Vector3 tp = auxCamera.transform.position;
            if (Vector3.Distance(tp, tg) > 0.5f){
				auxCamera.transform.position = Vector3.SmoothDamp(tp, tg, ref auxCameraVelocity, auxCameraShowTimer / auxCameraShowTime);
			} else{
				auxCamera.transform.position = tg;
                auxCameraTarget.Remove(0);
                auxCameraVelocity = Vector3.zero;
			}
		}
	}

	// - 边界控制 -
	void BordersControl()
	{
        float cmx = FollowCamera ? Camera.main.transform.position.x : 0; 
        float cmy = FollowCamera ? Camera.main.transform.position.y : 0;
		// 允许控制
		if(BorderControl){
			// 上控制
			if(borderTopC){
				borderTop.transform.position = new Vector3(cmx, (float)Camera.main.orthographicSize + cmy, 0);
			}
			// 下控制
			if(borderBottomC){
				borderBottom.transform.position = new Vector3(cmx, (float)-Camera.main.orthographicSize + cmy, 0);
			}
			// 左控制
			if(borderLeftC){
				borderLeft.transform.position = new Vector3((float)(-(Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize) + cmx, cmy, 0);
			}
			// 右控制
			if(borderRightC){
				borderRight.transform.position = new Vector3((float)((Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize) + cmx, cmy, 0);
			}
		}
	}

	// - 清除子物体 -
	void ClearThem(GameObject him)
	{
		// 遍历所有这个子物体
		for(int i = 0; i < him.transform.childCount; i++)
		{
			// 谢谢Object 对游戏做的贡献！
			GameObject thankObject = him.transform.GetChild(i).gameObject;
			// 再见！
			Destroy(thankObject);
		}
	}

	// - 创建区域 -
	public void CreateArea(int type, string[] team)
	{
		/*
			type 
				1为产区    2为区域中心
		*/
		if(type == 1){
			ClearThem(AreaSpace);
			Areas = new ArrayList();
			// 随机抽取区域
			string areaname = (string)team[Random.Range(0, team.Length)];
			areaname = "Scenes/AreaCenter/" + areaname;
			// 实例化区域
			GameObject area = (GameObject)Instantiate(Resources.Load(areaname));
			// 交给区域空间控制器
			area.transform.parent = AreaSpace.transform;
			Areas.Add(area);
			// - 初始化出生点 -
			spawnPoint = FindIt("BoxSpawnPoint") != null ? FindIt("BoxSpawnPoint").transform.position : new Vector3();
			// - 初始化入口位置 -
			entranceDir = FindIt("Borders/" + area.GetComponent<AreaCenter>().EntranceDir);
		}

	}

	// - 创建辅助相机 -
	void createAuxCamera(Vector3 pos, Vector3[] target, float f_time = 5f)
	{
		// 新建相机
		auxCamera = new GameObject();
		auxCamera.AddComponent<Camera>();
		auxCamera.transform.position = pos;
		auxCamera.GetComponent<Camera>().backgroundColor = new Color(25, 25, 25, 255);
		auxCamera.name = "auxCamera";

		// 设置任务轨迹
		auxCameraTarget = new ArrayList();
		for(int i = 0; i < target.Length; i++){
			auxCameraTarget.Add(target[i]);
		}

		// 设置任务时间
		auxCameraShowTimer = f_time;

		// 设置任务次数
		auxCameraShowTime = target.Length;

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
	    if(spawnPoint == new Vector3(0, 0, 0)){
	    	box.MySpawnPoint = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), 0);
	    	newBox.transform.position = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), 0);
	    } else {
		    box.MySpawnPoint = spawnPoint;
		    newBox.transform.position = spawnPoint;
	    }

	    // 将盒子加入到大集合
	    Boxs.Add(newBox);

	}

	// - 入口控制 -
	private bool entranceControl()
	{
		for(int i = 0; i < Boxs.Count; i++){
			GameObject box = Boxs[i] as GameObject;
			if(!box.GetComponent<MiniBox>().Working){
				return true;
			}

		}
		return false;
	}

	// - 入口门禁 -
	private void entranceGuard()
	{
		// 原始点
		Vector3 pos_s = getEntranceTargetPos(entranceDir);
		// 开启点
		Vector3 pos_o = getEntranceTargetPos(entranceDir, 1);
		if(entranceControl() == true){
            SetBorderControlPart(entranceDir, false);
            entranceDir.transform.position = Vector3.Lerp(entranceDir.transform.position, pos_o, 0.025f);

		} else {
            if(Vector3.Distance(entranceDir.transform.position, pos_s) < 0.50f){
            	SetBorderControlPart(entranceDir);
            } else {
            	entranceDir.transform.position = Vector3.Lerp(entranceDir.transform.position, pos_s, 0.025f);

            }
		}
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

	// - 获取门禁目标位置 -
	private Vector3 getEntranceTargetPos(GameObject him, int kind = 0)
	{
		float cmx = FollowCamera ? Camera.main.transform.position.x : 0; 
		float cmy = FollowCamera ? Camera.main.transform.position.y : 0;

		Vector3 pos = new Vector3();

		if(kind == 0){
			if(him == borderTop){
				pos = new Vector3(cmx, (float)Camera.main.orthographicSize + cmy, 0);
			}

			if(him == borderBottom){
				pos = new Vector3(cmx, (float)-Camera.main.orthographicSize + cmy, 0);
			}

			if(him == borderLeft){
				pos = new Vector3((float)(-(Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize) + cmx, cmy, 0);
			}

			if(him == borderRight){
				pos = new Vector3((float)((Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize) + cmx, cmy, 0);
			}

		} else {
			if(him == borderTop){
				pos = new Vector3(cmx + entranceDir.transform.localScale.y, (float)Camera.main.orthographicSize + cmy, 0);
			}

			if(him == borderBottom){
				pos = new Vector3(cmx + entranceDir.transform.localScale.y, (float)-Camera.main.orthographicSize + cmy, 0);
			}

			if(him == borderLeft){
				pos = new Vector3((float)(-(Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize) + cmx, cmy - entranceDir.transform.localScale.y, 0);
			}

			if(him == borderRight){
				pos = new Vector3((float)((Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize) + cmx, cmy - entranceDir.transform.localScale.y, 0);
			}

		}

		return pos;

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

	// - 设置边缘部分控制权 -
	private bool SetBorderControlPart(GameObject him, bool b = true)
	{
		if(him == borderTop){
			borderTopC = b;
		}

		if(him == borderBottom){
			borderBottomC = b;
		}

		if(him == borderLeft){
			borderLeftC = b;
		}

		if(him == borderRight){
			borderRightC = b;
		}

		return b;
	}

	// * - - - - - - - - - - *

	// - 初始化 -
	void Init()
	{
    	// 载入GameController
        //GameController = GameObject.Find("GameController");
        GameController = FindIt("GameController");

        // 创建区域空间
        GameObject areaSpace = new GameObject();
        AreaSpace = areaSpace;
        AreaSpace.name = "AreaSpace";
        AreaSpace.transform.position = new Vector3(0, 0, 0);

        // 载入边框
        GameObject Borders = new GameObject();
        Borders.name = "Borders";
        Borders.transform.position = new Vector3(0, 0, 0);

        // 上边框
        borderTop = new GameObject();
        borderTop.name = "Top";
        borderTop.transform.parent = Borders.transform;
        borderTop.transform.localScale = new Vector3(1, 26, 1);
        borderTop.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        borderTop.AddComponent<SpriteRenderer>();
        borderTop.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderTop.AddComponent<BoxCollider2D>();
        borderTop.GetComponent<SpriteRenderer>().sortingOrder = 5;

        // 下边框
        borderBottom = new GameObject();
        borderBottom.name = "Bottom";
        borderBottom.transform.parent = Borders.transform;
        borderBottom.transform.localScale = new Vector3(1, 26, 1);
        borderBottom.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        borderBottom.AddComponent<SpriteRenderer>();
        borderBottom.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderBottom.AddComponent<BoxCollider2D>();
        borderBottom.GetComponent<SpriteRenderer>().sortingOrder = 5;

        // 左边框
        borderLeft = new GameObject();
        borderLeft.name = "Left";
        borderLeft.transform.parent = Borders.transform;
        borderLeft.transform.localScale = new Vector3(1, 12, 1);
        borderLeft.AddComponent<SpriteRenderer>();
        borderLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderLeft.AddComponent<BoxCollider2D>();
        borderLeft.GetComponent<SpriteRenderer>().sortingOrder = 5;

        // 右边框
        borderRight = new GameObject();
        borderRight.name = "Right";
        borderRight.transform.parent = Borders.transform;
        borderRight.transform.localScale = new Vector3(1, 12, 1);
        borderRight.AddComponent<SpriteRenderer>();
        borderRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderRight.AddComponent<BoxCollider2D>();
        borderRight.GetComponent<SpriteRenderer>().sortingOrder = 5;

        /*
	        borderTop = FindIt("Borders/Top");
	        borderBottom = FindIt("Borders/Bottom");
	        borderLeft = FindIt("Borders/Left");
	        borderRight = FindIt("Borders/Right");
        */

        // 数据定义
        // Eye = new string[]{"Block", "Round", "Triangle", "Triangle-Right"};
        // 面部数据
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

        // 区域数据
        dAreaCenter_DeliVery = new string[]{
        	"DeliveryArea"
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

        // 给特征预留

        // - - - - - - - - - - - 
        // 创建产区
        CreateArea(1, dAreaCenter_DeliVery);

        // - 初始化盒子 -
        Boxs = new ArrayList();
        Areas = new ArrayList();
	}

	// - 初始化 -
	void Awake()
	{
		// 初始化
		Init();
	}

	// Use this for initialization
	void Start () {
		// 出生点特写
		if(spawnPoint != new Vector3()){
			createAuxCamera(spawnPoint, new Vector3[]{Camera.main.transform.position});

		}

		// 边界控制
		BordersControl();
	}
	
	// Update is called once per frame
	void Update () {
		// 边界控制
		BordersControl();

		// 入口控制
		entranceGuard();

		// 辅助相机控制
		auxCameraControl();

	}

// End
}
