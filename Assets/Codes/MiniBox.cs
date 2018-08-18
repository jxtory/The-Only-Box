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
    [Header("盒子名字")]
    // 名字
    public string MyName;
    private Vector3 mySpawnPoint;
    // 颜色	默认为白色
    private Color color = Color.white;
    // 大小	
    private Vector3 scaleSize = new Vector3(1.0f, 1.0f, 1.0f);
    // 在岗
    [Header("是否工作")]
    [SerializeField]
    private bool working;
    // 是否跳跃
    private bool isJumping;
    // 坠落速度
    [Header("坠落速度")]
    [SerializeField]
    private float magnitude;
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
    [Header("心情值")]
    [SerializeField]
    private int mood = 6 * 20;
    private float moodSelfHealing = 0;
    private int moodState = 0;
    // - 视线 -
    private GameObject him;

    // - - - - - - - - - - 

    // - 相关属性 -
    // 设置眼眶
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

    // 设置眼球
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

    // 设置鼻子
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

    // 设置嘴
    public int Mouth
    {
        get
        {
            return mouth;
        }
        set
        {
            mouth = value;
        }
    }

    // 设置特征
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

    // - - - - - - - - - - -

    // * - - - - - - - - - - *

    // - 人工智能控制器 -
    public void AIController()
    {
        // 坠落中
        AICToFalling();

        // 情绪控制
        AICMoodControl();

        // 站立控制
        AICToStand();


    }

    // - 人工智能 坠落 -
    private void AICToFalling()
    {
        // 绑定刚体
        Rigidbody2D him = BoxSelf.GetComponent<Rigidbody2D>();
        this.magnitude = him.velocity.magnitude;
        // 分析速度
        if(magnitude > 3 && !isJumping){
            // 焦虑
            SetMood(4 * 20 - 10);
        }

        if(magnitude > 5 && !isJumping){
            // 不高兴
            SetMood(3 * 20 - 10);
        }

        if(magnitude > 15 && !isJumping){
            // 难过
            SetMood(2 * 20 - 10);
        }

    }

    // - 人工智能 情绪控制 -
    private void AICMoodControl()
    {
        // 漫长积累
        moodSelfHealing += 1f + 1f * Time.deltaTime;

        // 心情限制
        //int limit;

        switch(moodState){
            // 安心   < 100 - 1
            case 0:
                if(mood < 100 - 1){
                    // 自愈
                    if(moodSelfHealing > 5){
                        SetMood(mood + 1);
                        moodSelfHealing = 0;
                    }
                } 

                break;

            // 开心   < 260
            case 1:
                if(mood < 260){
                    // 自愈
                    if(moodSelfHealing > 5){
                        SetMood(mood + 1);
                        moodSelfHealing = 0;
                    }
                }

                break;

            // 灰心
            case 2:
                SetMood(mood);

                break;

            // 伤心
            case 3:
                if(mood > -40){
                    // 自残
                    if(moodSelfHealing > 2){
                        SetMood(mood - 1);
                        moodSelfHealing = 0;
                    }
                }
                break;

            // 掌控
            default:
                break;
        }

    }

    // - 人工智能 伤心中 -

    // - 人工智能 站立 -
    private void AICToStand()
    {
        // 旋转角度
        Vector3 rn = BoxSelf.transform.localEulerAngles;
        // 是否停止运动
        bool issp = BoxSelf.GetComponent<Rigidbody2D>().IsSleeping();
        // 绑定刚体
        Rigidbody2D him = BoxSelf.GetComponent<Rigidbody2D>();

        // 获取欧拉角-角度 还需再行研究
        float tf = 360 - (rn.z % 360) > 180 ? 360 - (360 - (rn.z % 360)) : 360 - (360 - (rn.z % 360)) - 360;

        // 站立运算
        if ((tf > 44 || tf < -44) && issp && !isJumping){
            him.velocity = new Vector2(0, 7f);
            isJumping = true;
        }

        // 跳起检测
        // if(Mathf.Abs(tf) < 45 || him.velocity.magnitude < 0.5f){
        if(him.velocity.magnitude < 0.5f){
            isJumping = false;
        }

        // 跳起中调整位置
        if (isJumping) {
            // if (tf > 44 && tf <= 180)
            if (tf > 0)
            {
                Quaternion ros = Quaternion.Euler(0, 0, 25f);
                BoxSelf.transform.rotation = Quaternion.Slerp(BoxSelf.transform.rotation, ros, 5f * Time.deltaTime);
                if (Quaternion.Angle(ros, BoxSelf.transform.rotation) < 3)
                {
                    BoxSelf.transform.rotation = ros;
                }
            }

            //if (tf >= -180 && tf < -44)
            if (tf < 0)
            {
                Quaternion ros = Quaternion.Euler(0, 0, -25f);
                BoxSelf.transform.rotation = Quaternion.Slerp(BoxSelf.transform.rotation, ros, 5f * Time.deltaTime);
                if (Quaternion.Angle(ros, BoxSelf.transform.rotation) < 3)
                {
                    BoxSelf.transform.rotation = ros;
                }
            }
        }
    }

    // - 工作检测 -
    void checkWorking()
    {
        // 工作范围
        if (BoxSelf.transform.position.y < GC.BorderTop.transform.position.y && BoxSelf.transform.position.y > GC.BorderBottom.transform.position.y && BoxSelf.transform.position.x > GC.BorderLeft.transform.position.x && BoxSelf.transform.position.x < GC.BorderRight.transform.position.x)
        {
            working = true;
        }

        // 超出上边界
        if (BoxSelf.transform.position.y > GC.BorderTop.transform.position.y){
            working = false;
        }

        // 超出下边界
        if (BoxSelf.transform.position.y < GC.BorderTop.transform.position.y && BoxSelf.transform.position.y < GC.BorderBottom.transform.position.y){
            working = false;
            BoxSelf.transform.position = mySpawnPoint;
        }

        // 超出左边界
        if (BoxSelf.transform.position.y < GC.BorderTop.transform.position.y && BoxSelf.transform.position.y > GC.BorderBottom.transform.position.y && BoxSelf.transform.position.x < GC.BorderLeft.transform.position.x){
            working = false;
            BoxSelf.transform.position = mySpawnPoint;
        }

        // 超出右边界
        if (BoxSelf.transform.position.y < GC.BorderTop.transform.position.y && BoxSelf.transform.position.y > GC.BorderBottom.transform.position.y && BoxSelf.transform.position.x > GC.BorderRight.transform.position.x){
            working = false;
            BoxSelf.transform.position = mySpawnPoint;
        }

    }

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
        this.Mouth = face[3];
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
        Mouth.GetComponent<SpriteRenderer>().sprite = GC.GetBoxMouth(this.Mouth);
        // 显示优先级
        Mouth.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // 缩放物体
        BoxSelf.transform.localScale = scaleSize;
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        // - 控制心情 -
        if(BPsychomotor){Psychomotor();}

        // - 查岗 -
        checkWorking();

        // - 人工智能控制 -
        AIController();

	}

    /*
        void OnBecameVisible(){
            Debug.Log("In camera");
        }

        void OnBecameInvisible(){
            Debug.Log("not in camera");
        }
    */

}
