    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBox : MonoBehaviour {

	// 盒子数据以及元素设置
    [Header("主要设置")]
    // 主控制器
    public MainControllers GC;
    // 定位自己
    public GameObject BoxSelf;
    [Header("盒子属性")]
    // 名字
    [Header("盒子名字")]
    public string MyName;
    // 颜色	默认为白色
    private Color color = Color.white;
    // 大小	
    private Vector3 scaleSize = new Vector3(1.0f, 1.0f, 1.0f);
    // - 面部细节 -
    // 面部活跃模式 true 为 活跃 false 为死板 默认为活跃
    private bool faceMode = true;
    // 眼眶
    private int eye;
    // 眼球
    private int eyeBall;
    // 鼻子
    private int nose;
    // 嘴
    private int mouth;
    // 特征
    private int feature;
    // - 心情 - 
    private int mood;
    // - 视线 -
    private GameObject him;

    // - 设置盒子 -
    public void SetBoxSelf(GameObject gameObject)
    {
        // 绑定
        this.BoxSelf = gameObject;
    }

    // - 设置缩放比例 -
    public void SetScaleSize(Vector3 f_ScaleSize)
    {
        scaleSize = f_ScaleSize;
    }

    // - 生成盒子(设置物体元素信息) -
    public void SetGameObject()
    {
        // 给盒子命名
        BoxSelf.transform.name = "Box";
        // 盒子归属于GameController子物体
        BoxSelf.transform.parent = GC.transform;
        // 添加精灵渲染器
        BoxSelf.AddComponent<SpriteRenderer>();
        // 贴图
        BoxSelf.GetComponent<SpriteRenderer>().sprite = GC.GetBoxFrame();
        // 添加刚体和碰撞器
        BoxSelf.AddComponent<Rigidbody2D>();
        BoxSelf.AddComponent<BoxCollider2D>();
        BoxSelf.GetComponent<SpriteRenderer>().sortingOrder = 1;        
        // 添加主体颜色
        BoxSelf.GetComponent<SpriteRenderer>().color = color;
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
