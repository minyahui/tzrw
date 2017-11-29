//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;
//
//
//
////行为状态
//public enum FSMActionID
//{
//    Patroling,  //巡逻
//    Chasing,    //追赶
//    Attacking,  //攻击
//    Gethit,     //受击
//    Escape,     //逃命
//    Dead        //死亡
//}
//
////条件状态
//public enum Transition
//{
//    SawPlayer,      //发现敌人
//    ReachPlayer,    //靠近敌人-攻击范围
//    LostPlayer,     //没发现敌人
//    Gethit,
//    HPWarn,         //血量警告
//    HPZero          //血量为0
//}
//
//
//
//public class NPCContorl2 : NetworkBehaviour
//{
//
//    public UnityEngine.UI.Text testTXT;
//
//    [SyncVar]
//    public FSMActionID npcActionID;
//
//    [SyncVar]
//    public bool isRed;
//    public Factions npcFaction; //阵营
//
//    [SyncVar]
//    private int     npcHP;
//    public  int      makHP = 200;
//    private bool    isDeadPlay;  //是否播放过死亡动画
//    
//
//
//    public Transform gunPoint;
//	private Transform pTransform;
//    public GameObject bulletPerfab;
//
//
//
//    private float pSpeed = 1.5f;   //走的速度
//    private float eSpeed = 3;      //跑的速度
//
//
//    private int chasingDis = 30;
//    private Transform target;
//
//    
//
//
//    private  float shootRate = 2f;  //射击速率
//    //[SyncVar]
//    private float shootTime = 0;     //射击时间
//    public  float gethitRate = 0.5f; //受伤恢复时间
//    //[SyncVar]
//    private float gethitTime = 0;    //受伤时间
//    private float CTime = 0; //追击时间
//
//
//
//    private PlayerAnimation npcAnimation;
//    private UnityEngine.AI.NavMeshAgent npcAgent;
//
//    private  Transform[] patrolingpoints;  //巡逻点
//    private  Transform   escapetPosint;    //逃跑点
//   
//	public GameObject redPerson;
//	public GameObject bluePerson;
//
//
//    void Start () {
//
//        npcAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//        npcActionID = FSMActionID.Patroling;
//        patrolingpoints = new Transform[]
//        {
//             GameObject.Find("PatrolingPosintion").transform.FindChild("Point1"),
//             GameObject.Find("PatrolingPosintion").transform.FindChild("Point2"),
//             GameObject.Find("PatrolingPosintion").transform.FindChild("Point3"),
//             GameObject.Find("PatrolingPosintion").transform.FindChild("Point4")
//        };
//        target = patrolingpoints[0];
//        escapetPosint = GameObject.Find("EscapePosint").transform.FindChild("Point1");
//
//
//        if (!isServer)
//        {
//            npcAgent.enabled = false;
//            GetComponent<CharacterController>().enabled = false;
//            testTXT.text = ("NPC");
//            
//        }
//        else
//        {
//            CmdSetHP(makHP);
//            testTXT.text = ("ServerNPC");
//        }
//    }
//
//
//	void UpdatePerson(){
//		if (isRed) {
//			redPerson.SetActive (true);
//			bluePerson.SetActive (false);
//			if(npcAnimation==null){
//				npcAnimation = redPerson.GetComponent<PlayerAnimation>();
//			}
//			if (pTransform == null) {
//				pTransform = redPerson.transform;
//			}
//
//		} else {
//			redPerson.SetActive (false);
//			bluePerson.SetActive (true);
//			if(npcAnimation==null){
//				npcAnimation = bluePerson.GetComponent<PlayerAnimation>();
//			}
//			if (pTransform == null) {
//				pTransform = bluePerson.transform;
//			}
//
//		}
//
//
//	}
//
//	void Update () {
//        GetComponent<InfoConterl>().currentHP = npcHP;
//		UpdatePerson ();
//        if (isServer)
//        {
//            Reason();
//        }
//        testTXT.text = npcActionID.ToString();
//
//        ActionRun();
//	}
//
//
//    //条件转换
//    private void Reason()
//    {
//        switch (npcActionID)
//        {
//            case FSMActionID.Attacking:
//                {
//                    if (!CheckTarget())
//                    {
//                        Debug.Log(" 射击时目标死亡,改巡");
//                        //目标死亡
//                        target = null;
//                        npcAgent.enabled = true;
//                        npcAgent.speed = pSpeed;
//                        CTime = 0;
//                        CmdSetActionID(FSMActionID.Patroling);
//                        return;
//                    }
//
//                    float dis = Vector3.Distance(transform.position, target.position);
//                    Ray ray = new Ray( gunPoint.position , gunPoint.forward*dis+Vector3.up);
//                    Debug.DrawRay(gunPoint.position, gunPoint.forward * dis + Vector3.up, Color.red);
//
//
//
//                    RaycastHit hit;
//                        if (Physics.Raycast(ray, out hit, dis + 0.5f))
//                        {
//                        Debug.Log(" 射线对象: " + hit.collider.gameObject + "目标: " + target.name);
//
//                        if (hit.collider.gameObject != target.gameObject)
//                        {
//                            Debug.Log(" 射击时射中其它,改追击");
//                            pTransform.localRotation = Quaternion.identity;
//                            CmdSetActionID(FSMActionID.Chasing);
//                            npcAgent.enabled = true;
//                        }
//                    }
//                    else
//                    {
//                        Debug.Log(" 射击时射线空,改追击");
//                        pTransform.localRotation = Quaternion.identity;
//                        CmdSetActionID(FSMActionID.Chasing);
//                        npcAgent.enabled = true;
//                    }
//             
//                }
//                break;
//
//            case FSMActionID.Chasing:
//                {
//
//                    if (!CheckTarget())
//                    {
//                        //目标死亡
//                        target = null;
//                        npcAgent.enabled = true;
//                        npcAgent.speed = pSpeed;
//                        CTime = 0;
//                        CmdSetActionID(FSMActionID.Patroling);
//                        return;
//                    }
//
//                    float length = Vector3.Distance(transform.position, target.position);
//                    Debug.DrawRay(gunPoint.position + Vector3.up * 0, gunPoint.forward * length + Vector3.up, Color.green);
//                    if (length < chasingDis)
//                    {
//                        gunPoint.LookAt(target);
//                        Ray ray = new Ray(gunPoint.position, gunPoint.forward * length + Vector3.up);
//                        RaycastHit hit;
//                        if (Physics.Raycast(ray, out hit, length + 0.1f))
//                        {
//                            Debug.Log("追击敌人" + " 射线对象: " + hit.collider.name + "目标: " + target.name);
//                            if (hit.collider.gameObject == target.gameObject)
//                            {
//                                Debug.Log("靠近并看到敌人, 准备射击: " + target.name);
//                                //看到敌人, 切换射击状态
//                                npcAgent.enabled = false;
//                                CTime = 0;
//                                CmdSetActionID(FSMActionID.Attacking);
//                            }
//                        }
//                        else
//                        {
//                            Debug.Log("射线放空继续追");
//                        }
//                    }else
//                    {
//                        Debug.Log("跑远了,巡逻!");
//                        target = null;
//                        npcAgent.speed = pSpeed;
//                        CTime = 0;
//                        CmdSetActionID(FSMActionID.Patroling);
//                    }
//                }
//
//                break;
//
//            case FSMActionID.Escape:
//                {
//                    float dis = Vector3.Distance(transform.position, target.position);
//                    if (dis < 2.5)
//                    {
//                        target = null ;
//                        npcAgent.speed = pSpeed;
//                        CmdSetActionID(FSMActionID.Patroling);
//                    }
//                }
//
//                break;
//            case FSMActionID.Gethit:
//                {
//                    gethitTime += Time.deltaTime;
//                    if (gethitTime > gethitRate)
//                    {
//                        gethitTime = 0;
//                        
//                        if (npcHP < makHP * 0.4f)
//                        {
//                            if (npcHP <= 0)
//                            {
//                                CmdSetActionID(FSMActionID.Dead);
//                                return;
//                            }
//                            else
//                            {
//                                target = escapetPosint;
//                                npcAgent.speed = eSpeed;
//                                CmdSetActionID(FSMActionID.Escape);
//                            }
//                               
//                        }else
//                        {
//                            target = null;
//                            npcAgent.speed = pSpeed;
//                            CmdSetActionID(FSMActionID.Patroling);
//                        }
//                        npcAgent.enabled = true;
//                    }
//                }
//
//                break;
//
//            case FSMActionID.Patroling:
//
//                {
//                    if (npcHP <= 0)
//                    {
//                        CmdSetActionID(FSMActionID.Dead);
//                        GetComponent<CharacterController>().enabled = false;
//                        this.enabled = false;
//                        return;
//                    }
//
//                    if (target == null) target = patrolingpoints[Random.Range(0, patrolingpoints.Length)];
//                    float length = Vector3.Distance(target.position, transform.position);
//                    if (length < 1)
//                    {
//                        Debug.Log("改变目标");
//                        target = patrolingpoints[Random.Range(0, patrolingpoints.Length)];
//                    }
//
//
//                    Transform targetObj = null;
//                    if (isRed)
//                    {
//                        targetObj = CheckPlayerDis(GamePlayerManager.BlueList);
//                    }
//                    else
//                    {
//                        targetObj = CheckPlayerDis(GamePlayerManager.RedList);
//                    }
//                    //CmdSetPatrolingTarge(chasingObj.transform);
//                    if (targetObj != null)
//                    {
//                        target = targetObj;
//                        npcAgent.speed = eSpeed;
//                        CmdSetActionID(FSMActionID.Chasing);
//                    }
//                }
//
//                break;
//            case FSMActionID.Dead:
//
//                break;
//        }
//    }
//
//
//    private void ActionRun()
//    {
//		if (npcAnimation == null)
//			return;
//        switch (npcActionID)
//        {
//            case FSMActionID.Attacking:
//                {
//                    if (isServer)
//                    {
//                       
//                        if (shootTime <= 0)
//                        {
//                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 5);
//                            shootTime = shootRate;
//                            CmdCreatBullet();
//                        }
//                        shootTime -= Time.deltaTime;
//                    }
//				npcAnimation.Play (PlayerAnimation.PlayerAnimationStatus.attack_rifle);
//                }
//                break;
//
//            case FSMActionID.Chasing:
//                {
//                    if (isServer)
//                    {
//                        npcAgent.SetDestination(target.position);
//                    }
//					npcAnimation.Play (PlayerAnimation.PlayerAnimationStatus.run_rifle);
//                }
//
//                break;
//
//            case FSMActionID.Escape:
//                if (isServer)
//                {
//                    npcAgent.SetDestination(target.position);
//                }
//				npcAnimation.Play (PlayerAnimation.PlayerAnimationStatus.run_rifle);
//
//                break;
//            case FSMActionID.Gethit:
//				npcAnimation.Play (PlayerAnimation.PlayerAnimationStatus.underattack_rifle);
//
//                break;
//
//            case FSMActionID.Patroling:
//
//                if (isServer)
//                {
//                    if (target == null) target = patrolingpoints[Random.Range(0, 4)];
//                    npcAgent.SetDestination(target.position);
//                }
//				npcAnimation.Play (PlayerAnimation.PlayerAnimationStatus.walk_rifle);
//
//                break;
//
//            case FSMActionID.Dead:
//                if (!isDeadPlay)
//                {
//                    GamePlayerManager.gameManager.CheckGameOver(isRed ? Factions.Red : Factions.Blue);
//                    npcAnimation.Play (PlayerAnimation.PlayerAnimationStatus.dead_01_rifle);
//                    isDeadPlay = true;
//                    GetComponent<CharacterController>().enabled = false;
//                    this.enabled = false;
//                }
//                break;
//        }
//    }
//
//
//    private bool CheckTarget()
//    {
//        if (target == null) return false;
//
//        if (target.GetComponent<InfoConterl>() == null) return false;
//
//        if (target.GetComponent<InfoConterl>().currentHP > 0)
//            return true;
//        else
//            return false;
//    }
//
//
//
//
//    #region 服务器处理
//
//    [Command]
//    void CmdSetActionID(FSMActionID id)
//    {
//        npcActionID = id;
//    }
//
//
//
//
//
//
//
//    [Command]  //受伤分发
//    public void CmdGethitDistribution(int value)
//    {
//        Debug.Log(">>>>>>>>>>>>>>>>>>: 受伤分发");
//        target = null;
//        gethitTime = 0;
//
//        npcHP -= value;
//        if (npcHP <= 0) npcHP = 0;
//        npcActionID = FSMActionID.Gethit;
//        if (npcAgent.enabled) npcAgent.enabled = false;
//    }
//
//    [Command] //受击血量计算
//    void CmdSetHP(int hp)
//    {
//        npcHP = hp;
//    }
//
//
//
//
//    [Command]
//    void CmdCreatBullet()
//    {
//        GameObject bullet = GameObject.Instantiate(bulletPerfab, gunPoint.position, gunPoint.rotation);
//        bullet.transform.LookAt(target.position+Vector3.up);
//        Destroy(bullet, 2);
//        NetworkServer.Spawn(bullet);
//
//    }
//
//    [Command]
//    void CmdSetGethitTime(int value)
//    {
//        gethitTime = value;
//    }
//
//
//    #endregion
//    //巡逻状态找目标
//    Transform CheckPlayerDis(List<GameObject> ps)
//    {
//        foreach (GameObject p in ps)
//        {
//            if (p.GetComponent<InfoConterl>().currentHP <= 0) continue;
//            if (Vector3.Distance(transform.position, p.transform.position) < chasingDis)
//            {
//                return p.transform;
//            }
//        }
//
//        return null;
//    }
//
//
//
//
//
//
//
//
//}
