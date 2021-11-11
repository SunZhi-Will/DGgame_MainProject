using UnityEngine;

public class BullteControll : MonoBehaviour
{
    BaseBallManage m_manage;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Batter")
        {
            collision.gameObject.GetComponent<BatterControll>().OnDrop();
        }
    }
}
