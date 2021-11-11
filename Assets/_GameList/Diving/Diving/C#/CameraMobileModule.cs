using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMobileModule : MonoBehaviour
{
    // Start is called before the first frame update
    //紀錄手指觸碰位置
	public GameObject m_Camera;

	void Start ()
	{
	}
	
	
	void Update ()
	{
		Awake();
	}

    public float baseWidth = 1024;
    public float baseHeight = 768;
    public float baseOrthographicSize = 5;

    void Awake(){
        m_Camera.GetComponent<Camera>().aspect = baseWidth / baseHeight ;
    }
}
