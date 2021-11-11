using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//物件重力系統
public class ObjectGravity : MonoBehaviour
{
    public enum ModeState
    {
        Rigidbody,
        Transform
    }
    public ModeState modeState;
    private Rigidbody nowRigidbody;
    [System.Serializable]
    public class RigidbodySetup
    {
        public float Power = 1;
    }
    public RigidbodySetup rigidbodySetup;

    void Start()
    {
        switch (modeState)
        {
            case ModeState.Rigidbody:
                nowRigidbody = GetComponent<Rigidbody>();
                if (nowRigidbody == null) { Debug.LogError("Rigidbody不存在"); }
                break;
        }
    }

    void FixedUpdate()
    {
        switch (modeState)
        {
            case ModeState.Rigidbody:
                RigidbodyGravity();
                break;
        }
    }


    void RigidbodyGravity()
    {
        Vector3 _velocity = nowRigidbody.velocity;
        nowRigidbody.velocity = new Vector3(_velocity.x, _velocity.y - rigidbodySetup.Power, _velocity.z);
    }
}
