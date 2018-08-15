using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllers : MonoBehaviour {
	/* - - - - - - - - - - 
		��Ϸ���ƣ� 
			Ӣ��The Only Box
			�У��ҵ��Ƿ���

		Code˵�������顢���ӡ������塢Box	���¾���ƺ���

		��Ϸģʽ
			1 Ȥζģʽ	2 ��ǿģʽ

		��ϷԪ��
			������	����	����

		��ϷȤζ
			������ͷ��д
			��������
			
	*/

	// - �������� -
	[Header("��Ϸ����")]
	public GameObject GameController;
	// ������
	private Vector3 spawnPoint;
	private Vector3 joyBallSpawnPoint;

	// - �������� -
	// �߽���ƿ���
	[Header("�߽����")]
	public bool BorderControl = true;
	private bool borderTopC = true;
	private bool borderBottomC = true;
	private bool borderLeftC = true;
	private bool borderRightC = true;
	// �߽��������
	private GameObject borderTop;
	private GameObject borderBottom;
	private GameObject borderLeft;
	private GameObject borderRight;

	// �沿��Ծģʽ true Ϊ ��Ծ false Ϊ���� Ĭ��Ϊ��Ծ
	[Header("�沿��Ծ")]
	public bool FaceMode = true;

	// - ��Ϸ��ͼ������ -
	// * ��ͼ *
	// ���ӿ�
	private Sprite boxFrame;
	private Sprite[] boxEye;
	private Sprite[] boxEyeBall;
	private Sprite[] boxMouth;
	private Sprite[] boxNose;
	// * ���� * 
	private string[] dEye;
	private string[] dEyeBall;
	private string[] dMouth;
	private string[] dNose;

	// - ��ϷԪ�ؼ��������� -
	// ���ڴ洢����
	private ArrayList Boxs;
	private Vector3 boxSize = new Vector3(1, 1, 1);

	// - - - - - - - - - - -
	// - ���Կ��� -
	// ��ȡ�ϱ߿�
	public GameObject BorderTop
	{
		get
		{
			return borderTop;
		}
	}

	// ��ȡ�±߿�
	public GameObject BorderBottom
	{
		get
		{
			return borderBottom;
		}
	}

	// ��ȡ��߿�
	public GameObject BorderLeft
	{
		get
		{
			return borderLeft;
		}
	}

	// ��ȡ�ұ߿�
	public GameObject BorderRight
	{
		get
		{
			return borderRight;
		}
	}

	// - - - - - - - - - - -

	// - �߽���� -
	void BordersControl()
	{
		// �������
		if(BorderControl){
			// �Ͽ���
			if(borderTopC){
				borderTop.transform.position = new Vector3(0, (float)Camera.main.orthographicSize, 0);
			}
			// �¿���
			if(borderBottomC){
				borderBottom.transform.position = new Vector3(0, (float)-Camera.main.orthographicSize, 0);
			}
			// �����
			if(borderLeftC){
				borderLeft.transform.position = new Vector3((float)(-(Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize), 0, 0);
			}
			// �ҿ���
			if(borderRightC){
				borderRight.transform.position = new Vector3((float)((Screen.width * 1.0f / Screen.height) * Camera.main.orthographicSize), 0 ,0);
			}
		}
	}

	// - �������� -
	public void CreateBox()
	{
    	// ����һ���µĺ���(����)
    	GameObject newBox = new GameObject();
    	// ���������MiniBox������(���)
    	newBox.AddComponent<MiniBox>();
    	// ����MiniBox(����)
    	MiniBox box = newBox.GetComponent<MiniBox>();
    	// �󶨺�����MiniBox(����)
    	box.SetBoxSelf(newBox);
    	// ������ָ�������Ŀ�����(����)
    	box.GC = this.GetComponent<MainControllers>();
    	// ����������(����)
    	box.MyName = "Box";
    	// �������ű���(����)
    	boxSize = new Vector3(0.7f, 0.7f, 0.7f);
    	box.SetScaleSize(boxSize);
        // �����������
        if (!FaceMode){box.BPsychomotor = false;}
        // ���ú����沿
        int[] face = new int[4];
    	face[0] = Random.Range(0, dEye.Length);
    	face[1] = Random.Range(0, dEyeBall.Length);
    	face[2] = Random.Range(0, dNose.Length);
    	face[3] = Random.Range(0, dMouth.Length);
    	box.SetBoxFace(face);
    	// ���ɺ���(����)
    	box.SetGameObject();

	    // �ӳ��������
	    box.MySpawnPoint = spawnPoint;
	    newBox.transform.position = spawnPoint;

	    // �����Ӽ��뵽�󼯺�
	    Boxs.Add(newBox);

	}

	// - ��ѰGameObject -
	GameObject FindIt(string him)
	{
		return GameObject.Find(him);
	}

	// - ��ȡ���ӿ� -
	public Sprite GetBoxFrame()
	{
		return boxFrame;
	}

	// - ��ȡ�ۿ� -
	public Sprite GetBoxEye(int eye)
	{
		return boxEye[eye];
	}

	// - ��ȡ���� -
	public Sprite GetBoxEyeBall(int eyeball)
	{
		return boxEyeBall[eyeball];
	}
	
	// - ��ȡ���� -
	public Sprite GetBoxNose(int nose)
	{
		return boxNose[nose];
	}

	// - ��ȡ�� -
	public Sprite GetBoxMouth(int mouth)
	{
		return boxMouth[mouth];
	}

	// - ��Ѱ��ĵ�ַ -
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
		// ��ʼ��
		Init();
		// �߽����
		BordersControl();
	}
	
	// Update is called once per frame
	void Update () {
		// �߽����
		BordersControl();

	}

	// - ��ʼ�� -
	void Init()
	{
    	// ����GameController
        //GameController = GameObject.Find("GameController");
        GameController = FindIt("GameController");

        // ����߿�
        GameObject Borders = new GameObject();
        Borders.name = "Borders";
        Borders.transform.position = new Vector3(0, 0, 0);

        // �ϱ߿�
        borderTop = new GameObject();
        borderTop.name = "Top";
        borderTop.transform.parent = Borders.transform;
        borderTop.transform.localScale = new Vector3(1, 26, 1);
        borderTop.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        borderTop.AddComponent<SpriteRenderer>();
        borderTop.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderTop.AddComponent<BoxCollider2D>();

        // �±߿�
        borderBottom = new GameObject();
        borderBottom.name = "Bottom";
        borderBottom.transform.parent = Borders.transform;
        borderBottom.transform.localScale = new Vector3(1, 26, 1);
        borderBottom.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        borderBottom.AddComponent<SpriteRenderer>();
        borderBottom.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderBottom.AddComponent<BoxCollider2D>();

        // ��߿�
        borderLeft = new GameObject();
        borderLeft.name = "Left";
        borderLeft.transform.parent = Borders.transform;
        borderLeft.transform.localScale = new Vector3(1, 12, 1);
        borderLeft.AddComponent<SpriteRenderer>();
        borderLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderLeft.AddComponent<BoxCollider2D>();

        // �ұ߿�
        borderRight = new GameObject();
        borderRight.name = "Right";
        borderRight.transform.parent = Borders.transform;
        borderRight.transform.localScale = new Vector3(1, 12, 1);
        borderRight.AddComponent<SpriteRenderer>();
        borderRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Scenes/Border");
        borderRight.AddComponent<BoxCollider2D>();

        /*
	        borderTop = FindIt("Borders/Top");
	        borderBottom = FindIt("Borders/Bottom");
	        borderLeft = FindIt("Borders/Left");
	        borderRight = FindIt("Borders/Right");
        */

        // ���ݶ���
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

        // - ��Դ���� -
        // ���ӿ�
        boxFrame = Resources.Load<Sprite>("Scenes/Box_Frame");
        boxEye = new Sprite[dEye.Length];
        boxEyeBall = new Sprite[dEyeBall.Length];
        boxNose = new Sprite[dNose.Length];
        boxMouth = new Sprite[dMouth.Length];
        // �۾�
        for(int i = 0; i < dEye.Length; i++)
        {
            string t = "Scenes/Eye/" + dEye[i];
            boxEye[i] = Resources.Load<Sprite>(t);
        }
        // ����
        for(int i = 0; i < dEyeBall.Length; i++)
        {
            string t = "Scenes/EyeBall/" + dEyeBall[i];
            boxEyeBall[i] = Resources.Load<Sprite>(t);
        }
        // ����
        for(int i = 0; i < dNose.Length; i++)
        {
            string t = "Scenes/Nose/" + dNose[i];
            boxNose[i] = Resources.Load<Sprite>(t);
        }
        // ��
        for(int i = 0; i < dMouth.Length; i++)
        {
            string t = "Scenes/Mouth/" + dMouth[i];
            boxMouth[i] = Resources.Load<Sprite>(t);
        }

        // - - - - - - - - - - - 
        // - ��ʼ�������� -
        spawnPoint = FindIt("BoxSpawnPoint") != null ? FindIt("BoxSpawnPoint").transform.position : new Vector3();

        // - ��ʼ������ -
        Boxs = new ArrayList();
	}
}
