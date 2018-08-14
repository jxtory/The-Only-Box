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
    public bool BPsychomotor = true;
    // 眼眶
    private int eye;
    // 眼球
    private int eyeBall;
    // 鼻子
    private int nose;
    // 嘴
    private int mouth;
    private GameObject mouthGameObject;
    // 特征
    private int feature;
    // - 心情 - 
    private int mood;
    // - 视线 -
    private GameObject him;

    // - - - - - - - - - - 

    // - 相关属性 -
    public int Eye
    {
        get
        {
            return eye;
        }

        set
        {
            eye = value;
        }
    }
    public int EyeBall
    {
        get
        {
            return eyeBall;
        }
        set
        {
            eyeBall = value;
        }
    }
    public int Nose
    {
        get
        {
            return nose;
        }
        set
        {
            nose = value;
        }
    }
    public int Mouth
    {
        get
        {
            return mouth;
        }
        set
        {
            mouth = Mouth;
        }
    }
    public int Feature
    {
        get
        {
            return feature;
        }
        set
        {
            feature = value;
        }
    }

    // - - - - - - - - - - -

    // - 心理运动控制(嘴部贴图控制) -
    public void Psychomotor()
    {
        /*
            mood = 0 ~ 260
            degree = 1 ~ 13
            mood_level = 1 to 13
            生气|难过|委屈|焦虑|沉默|惊讶|惊奇|大惊|卖萌|微笑|温柔|开心
        */
        // 验证集合
        ArrayList tMouthRange = new ArrayList();
        // 嘴部贴图调用地址
        int tMouth = -1;

        // 太生气
        tMouth = psycho(0, "Angry-Con");
        tMouthRange.Add(tMouth);
        // 生气
        tMouth = psycho(1, "Angry");
        tMouthRange.Add(tMouth);
        // 难过
        tMouth = psycho(2, "Unhappy");
        tMouthRange.Add(tMouth);
        // 委屈
        tMouth = psycho(3, "Unhappy2");
        tMouthRange.Add(tMouth);
        // 焦虑
        tMouth = psycho(4, "Nervous");
        tMouthRange.Add(tMouth);
        // 沉默
        tMouth = psycho(5, "Silent");
        tMouthRange.Add(tMouth);
        // 惊讶
        tMouth = psycho(6, "Surprised1");
        tMouthRange.Add(tMouth);
        // 惊奇
        tMouth = psycho(7, "Surprised2");
        tMouthRange.Add(tMouth);
        // 大惊
        tMouth = psycho(8, "Surprised3");
        tMouthRange.Add(tMouth);
        // 卖萌
        tMouth = psycho(9, "Sweet");
        tMouthRange.Add(tMouth);
        // 微笑
        tMouth = psycho(10, "Smail-Smile");
        tMouthRange.Add(tMouth);
        // 温柔
        tMouth = psycho(11, "Tender");
        tMouthRange.Add(tMouth);
        // 开心
        tMouth = psycho(12, "Smile");
        tMouthRange.Add(tMouth);

        for(int i = 0; i < tMouthRange.Count; i++){
            //int t = (int)tMouthRange[i];
            if((int)tMouthRange[i] != -1){
                mouthGameObject.GetComponent<SpriteRenderer>().sprite = GC.GetBoxMouth((int)tMouthRange[i]);
            }
        }

    }

    // - 心理分析 -
    private int psycho(int degree, string partNames)
    {
        // 嘴部贴图调用地址
        int tMouth = -1;
        // int kind;
        // string partName = kind < 2 ? partNames[0] : partNames[Random.Range(0, kind)];
        string partName = partNames;

        if(mood >= 20 * (degree - 1) && mood < 20 * degree){
            tMouth = GC.GetMouthNumber(partName);
            return tMouth;
        }
        return tMouth;
    }

    // - 设置盒子面部 -
    public void SetBoxFace(int[] face)
    {
        // 赋予面部部位
        this.Eye = face[0];
        this.EyeBall = face[1];
        this.Nose = face[2];
        if(!GC.FaceMode){
            this.Mouth = face[3];
        }
    }

    // - 设置心情 -
    public void SetMood(int mood)
    {
        this.mood = mood;
    }

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

        // 创建面部
        GameObject Face = new GameObject();
        Face.name = "Face";
        Face.transform.parent = BoxSelf.transform;

        // 创建眼睛
        GameObject Eye = new GameObject();
        Eye.name = "Eye";
        Eye.transform.parent = Face.transform;
        Eye.AddComponent<SpriteRenderer>();
        Eye.GetComponent<SpriteRenderer>().sprite = GC.GetBoxEye(eye);
        // 显示优先级
        Eye.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // 创建眼球
        GameObject EyeBall = new GameObject();
        EyeBall.name = "EyeBall";
        EyeBall.transform.parent = Face.transform;
        EyeBall.AddComponent<SpriteRenderer>();
        EyeBall.GetComponent<SpriteRenderer>().sprite = GC.GetBoxEyeBall(eyeBall);
        // 显示优先级
        EyeBall.GetComponent<SpriteRenderer>().sortingOrder = 3;

        // 创建鼻子
        GameObject Nose = new GameObject();
        Nose.name = "EyeNose";
        Nose.transform.parent = Face.transform;
        Nose.AddComponent<SpriteRenderer>();
        Nose.GetComponent<SpriteRenderer>().sprite = GC.GetBoxNose(nose);
        // 显示优先级
        Nose.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // 创建嘴
        GameObject Mouth = new GameObject();
        mouthGameObject = Mouth;
        Mouth.name = "EyeMouth";
        Mouth.transform.parent = Face.transform;
        Mouth.AddComponent<SpriteRenderer>();
        if(!GC.FaceMode){
            Mouth.GetComponent<SpriteRenderer>().sprite = GC.GetBoxMouth(mouth);
        }
        // 显示优先级
        Mouth.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // 缩放物体
        BoxSelf.transform.localScale = scaleSize;
    }

	// Use this for initialization
	void Start () {
        // - 控制心情 开关-
        BPsychomotor = GC.FaceMode;

	}
	
	// Update is called once per frame
	void Update () {
        // - 控制心情 -
        if(BPsychomotor){Psychomotor();}
		
	}
}
