using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBox : MonoBehaviour {

	// 盒子数据以及元素设置
    [Header("主要设置")]
    // 主控制器
    public MainControllers GC;
    // 定位自己
    public GameObject Me;
    [Header("盒子属性")]
    // 名字
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

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
