using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    ObjectPool OP;
    ObjectController OC;
    ObjectCreate O_Create;
    public float food_AddLess=0.05f;
    public Animator anim_boss;
    public Animation _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        OP = ObjectPool.instance;
        OC = ObjectController.instance;
        O_Create = ObjectCreate.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (O_Create.game_time == 0) {
            gameObject.GetComponent<Collider>().enabled = false;
        }
        
    }
    public void OnTriggerEnter(Collider other)
    {
        switch (gameObject.name) {
            case "Boss":
                Debug.Log("Eat");
                if (other.gameObject.tag == "Food")
                {
                    //Debug.Log("Food");
                    OP.energy.fillAmount += food_AddLess;
                    OP.triangle.rectTransform.anchoredPosition = new Vector2(-710 + (OP.energy.fillAmount / 0.1f * 142.8f), 33);
                    OP.PutBack(other.gameObject);
                }
                else if (other.gameObject.tag == "NotFood")
                {
                    //Debug.Log("NotFood");
                    OP.energy.fillAmount -= food_AddLess;
                    OP.triangle.rectTransform.anchoredPosition = new Vector2(-710 + (OP.energy.fillAmount / 0.1f * 142.8f), 33);
                    OP.PutBack(other.gameObject);
                }
                break;
            case "trash can":
                OP.PutBack(other.gameObject);
                break;
            case "Eat":
                if (other.gameObject.tag == "Food" || other.gameObject.tag == "NotFood") {
                    anim_boss.SetTrigger("Eat");
                    //Debug.Log("Eat");
                }
                break;

        }
        
    }

    public void toChange() {
        O_Create.isChange = true;
        _camera.Play();

    }
    public void AudioEat() {
        O_Create.A_Eat.Play();
    }
    
}
