using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    public Image energy, triangle;//UGUI物件都自待底層的基本物件RectTransfrom
    public static ObjectPool instance;
    public Transform create_position;
    public int count, ball_position;
    public float speed1,speed2;
    int ball_num=4;//種類
    public GameObject[] bulletPrefab,P1,P2;
    public  List<GameObject>bulletList = new List<GameObject>() ;//列表儲存例項化的子彈
    List<GameObject> stainList = new List<GameObject>();
    private int[] nowSelectChNum_array = new int[2] { 0, 1 };//目前選擇角色編號
    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        InitPool();
        //P1[Random.Range(0, 3)].SetActive(true);
        //P2[Random.Range(0, 3)].SetActive(true);
    }


    ////設定角色
    //public void SetCharacter(int _p1, int _p2)
    //{
    //    nowSelectChNum_array[0] = _p1;
    //    nowSelectChNum_array[1] = _p2;
    //}
    private void Start()
    {
        //P1[nowSelectChNum_array[0]].SetActive(true);
        //P2[nowSelectChNum_array[1]].SetActive(true);
    }
    //初始化資源池
    private void InitPool()
    {
        for (int i = 0; i < bulletPrefab.Length - 1; i++)
            //for (int i = 0; i < MaxCreatBullet; i++)
        {
            count = i;
            for (int x = 0; x < ball_num; x++) {
                CreatBullet();
            }
        }
        for (int x = 0; x < 6; x++)
        //for (int x = 0; x < MaxCreatBullet; x++)
        {
            CreatStain();
        }
    }
    //public int MaxCreatBullet = 6;
    //public int MaxCreatStain = 11;
    //例項化一個子彈，加入到列表中，並隱藏
    private void  CreatBullet()
    { 
            GameObject go = Instantiate(bulletPrefab[count]);
            bulletList.Add(go);
            go.transform.position=create_position.transform.position;
            go.SetActive(false); 
        
    }
    //KoroshiTip-落底點圖示
    private void CreatStain()
    {
        GameObject go = Instantiate(bulletPrefab[bulletPrefab.Length-1]);
        stainList.Add(go);
        go.transform.position = new Vector3(0,0,0);
        go.SetActive(false);

    }
    //返回子彈列表中還沒有使用的子彈物件，如果沒有的話，則例項化新的子彈
    public GameObject GetBullet(int ball)
    {
        /*foreach (GameObject bullet in bulletList[ball])
        {
            if (bullet.activeInHierarchy == false)
            {
                ball_position = ball;
                bullet.SetActive(true);
                Debug.Log("Get");
                return bullet;
            }
        }*/
        //return CreatBullet();

        lock (bulletList)
        {
            int lastIndex = bulletList.Count - 1;
            if (lastIndex >= 0)
            {
                GameObject go = bulletList[ball];
                bulletList.RemoveAt(ball);
                go.SetActive(true);
                Debug.Log("出現物件:" + go.name + "\n");
                if (go.transform.position != create_position.position)
                {
                    go.transform.position=create_position.position;
                }
                return go;
            }
            else
            {
                return GetBullet(Random.Range(0, lastIndex));
                
            }
            
        }
    }

    private int LoopWarning = 0;//KoroshiTip-無限迴圈檢測
    public GameObject GetStain(Vector3 position,Quaternion rotation) {
        lock (stainList)
        {
            int lastIndex = stainList.Count - 1;
            if (lastIndex >= 0)
            {
                GameObject go = stainList[lastIndex];
                if (stainList.Count <= 0) { Debug.LogError("空的!"); return null; }
                stainList.RemoveAt(lastIndex);
                go.SetActive(true);        
                    go.transform.position = position;
                    go.transform.rotation = rotation;
                LoopWarning = 0;//跳脫迴圈
                return go;
            }
            else
            {
                if (LoopWarning > 5) { Debug.LogError("無限迴圈!");return null; }//檢測迴圈 
                LoopWarning++;
                return GetStain(position,rotation);
            }
        }
    }
    
    //回收子彈到資源池中
    //回收成功返回true，失敗返回false
    public void  PutBack(GameObject go)
    {
        /* if (bulletList[ball].Contains(go))
         {
             Debug.Log("Put");
             go.SetActive(false);
             return true;
         }
         Debug.Log("false");
         return false;*/
        lock (bulletList)
        {
            if (go.name.Contains("(Clone)") == false) { Debug.LogError("抓到!不是生成出來的物件!\n" + go.name); return; }
            bulletList.Add(go);
            //Debug.Log(bulletList.IndexOf(go));
            go.SetActive(false);
            go.transform.position = create_position.transform.position;
        }
    }

    public void PutStainBack(GameObject go, Vector3 position, Quaternion rotation) {
        //Debug.Log("Put Stain Back");
        lock (stainList)
        {
            stainList.Add(go);
            go.SetActive(false);
            go.transform.position = position;
            go.transform.rotation = rotation;
        }
    }
}