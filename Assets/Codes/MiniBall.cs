using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBall : MonoBehaviour {

	// 球数据以及元素设置
	[Header("主要设置")]
	// 主控制器
	public MainControllers GC;
	// 定位自己
	public GameObject BallSelf;
	[Header("球名字")]
	// 名字
	public string MyName;
	private Vector3 mySpawnPoint;
	// 颜色	默认为白色
	[Header("球颜色")]
	public Color color = Color.white;
	public Color bodyColor = Color.red;
	// 大小	
	private Vector3 scaleSize = new Vector3(1.0f, 1.0f, 1.0f);
	// 在岗
	[Header("是否工作")]
	[SerializeField]
	private bool working;
	// 安静状态
	private bool sleepState;
	// 失踪
	private bool missing;
	private float toMissTime;
    // 坠落速度
    [Header("坠落速度")]
    [SerializeField]
    private float magnitude;
	// - 拖动监测 -
	private bool isTouchDown = false;
	private Vector3 lastTouchPosition = Vector3.zero;

	// - - - - - - - - - -
    // 获取速度值
    public float Magnitude
    {
        get
        {
            return magnitude;
        }
    }

	// 设置出生地
	public Vector3 MySpawnPoint
	{
	    set
	    {
	        mySpawnPoint = value;
	    }
	}

    // 获取在岗值
    public bool Working
    {
        get
        {
            return working;
        }
    }

	// - - - - - - - - - -

	// - 工作检测 -
	void checkWorking()
	{
	    // 工作范围
	    if (BallSelf.transform.position.y < GC.BorderTop.transform.position.y && BallSelf.transform.position.y > GC.BorderBottom.transform.position.y && BallSelf.transform.position.x > GC.BorderLeft.transform.position.x && BallSelf.transform.position.x < GC.BorderRight.transform.position.x)
	    {
	        working = true;
	    }

	    // 检测出生点
	    GC.GetSpawnPoint(BallSelf, 1);

	    // 超出上边界
	    if (BallSelf.transform.position.y > GC.BorderTop.transform.position.y){
	        working = false;
	        if(GC.EntranceDir.name != "Top"){
	            // 重生
	            BallSelf.transform.position = mySpawnPoint;
	            // 刚体动态
	            BallSelf.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	        }
	    }

	    // 超出下边界
	    if (BallSelf.transform.position.y < GC.BorderTop.transform.position.y && BallSelf.transform.position.y < GC.BorderBottom.transform.position.y){
	        working = false;
	        if(GC.EntranceDir.name != "Bottom"){
	            // 重生
	            BallSelf.transform.position = mySpawnPoint;
	            // 刚体动态
	            BallSelf.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	        }
	    }

	    // 超出左边界
	    if (BallSelf.transform.position.y < GC.BorderTop.transform.position.y && BallSelf.transform.position.y > GC.BorderBottom.transform.position.y && BallSelf.transform.position.x < GC.BorderLeft.transform.position.x){
	        working = false;
	        if(GC.EntranceDir.name != "Left"){
	            // 重生
	            BallSelf.transform.position = mySpawnPoint;
	            // 刚体动态
	            BallSelf.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	        }
	    }

	    // 超出右边界
	    if (BallSelf.transform.position.y < GC.BorderTop.transform.position.y && BallSelf.transform.position.y > GC.BorderBottom.transform.position.y && BallSelf.transform.position.x > GC.BorderRight.transform.position.x){
	        working = false;
	        if(GC.EntranceDir.name != "Right"){
	            // 重生
	            BallSelf.transform.position = mySpawnPoint;
	            // 刚体动态
	            BallSelf.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	        }
	    }

	    // - 检测静态化 -
	    // 获取动态
	    this.sleepState = BallSelf.GetComponent<Rigidbody2D>().IsSleeping();
	    this.magnitude = BallSelf.GetComponent<Rigidbody2D>().velocity.magnitude;
	    if(sleepState || (!sleepState && magnitude < 0.5f))
	    {
	    	// 在岗检测
	    	if(!working)
	    	{
	    		// 失踪判定
	    		missing = true;
	    	} else {
	    		missing = false;
	    	}

	    } else {
	    	// 捕获动作认为其没有失踪
	    	missing = false;
	    	toMissTime = 0;
	    }

	    // 失踪检测
	    if(missing)
	    {
	    	// 记录失踪时间
		    toMissTime += Time.deltaTime;
		    if(toMissTime > 3)
		    {
		    	// 重生
		    	BallSelf.transform.position = mySpawnPoint;
		    	// 刚体动态
		    	BallSelf.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		    }
	    } else {
	    	// 失踪时间归零
	    	toMissTime = 0;
	    }

	}

	// - 设置球 -
	public void SetBallSelf(GameObject gameObject)
	{
	    // 绑定
	    this.BallSelf = gameObject;
	}

	// - 设置缩放比例 -
	public void SetScaleSize(Vector3 f_ScaleSize)
	{
	    scaleSize = f_ScaleSize;
	}

	// - 生产球(设置物体元素信息) -
	public void SetGameObject()
	{
		// 给球命名
		BallSelf.transform.name = "JoyBall";
		// 球归属于GameController子物体
		BallSelf.transform.parent = GC.transform;
		// 添加精灵渲染器
		BallSelf.AddComponent<SpriteRenderer>();
		// 贴图
		BallSelf.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/JoyBall/JoyBall");
		// 添加刚体和碰撞器
		BallSelf.AddComponent<Rigidbody2D>();
		BallSelf.AddComponent<CircleCollider2D>();
		BallSelf.GetComponent<SpriteRenderer>().sortingOrder = 1;
		// 添加主体颜色
		BallSelf.GetComponent<SpriteRenderer>().color = color;

		// 创建圆身
		GameObject JoyBallBody = new GameObject();
		JoyBallBody.name = "JoyBallBody";
		JoyBallBody.transform.parent = BallSelf.transform;
		JoyBallBody.AddComponent<SpriteRenderer>();
		JoyBallBody.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/JoyBall/JoyBall");
		// 自缩放
		JoyBallBody.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		// 添加身体颜色
		bodyColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		JoyBallBody.GetComponent<SpriteRenderer>().color = bodyColor;
		// 显示优先级
		JoyBallBody.GetComponent<SpriteRenderer>().sortingOrder = 1;


		// 缩放物体
		BallSelf.transform.localScale = scaleSize;

	}

	// - 拖动检测 -
	void TouchMove()
	{        
	    if(isTouchDown){
	    	// 设置选中对象和时间
	    	GC.SetSelectObject(BallSelf);

	        // 刚体静态
	        BallSelf.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
	        BallSelf.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

	        // 拖动移动
	        if(lastTouchPosition != Vector3.zero)
	        {
	            Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastTouchPosition;
	            BallSelf.transform.position += offset;
	        }
	        lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    } else {
	        // 刚体动态
	        BallSelf.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	    }

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// 查岗		
		checkWorking();

		// - 拖动检测 -
		TouchMove();
	}

	// Touch检测
	void OnMouseDown()
	{
	    // 按下
	    isTouchDown = true;

	}

	// 松开检查
	void OnMouseUp()
	{
	    // 拿起
	    isTouchDown = false;
	    lastTouchPosition = Vector3.zero;
	}

}
