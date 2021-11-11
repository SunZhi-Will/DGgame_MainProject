using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushRecord : MonoBehaviour
{
    public GameObject g_Brush;
    public Quaternion g_Rotation;
    public void SetBrush(GameObject _go){
        Destroy(g_Brush);

        g_Brush = Instantiate(_go, transform.position, g_Rotation);
        g_Brush.transform.parent = gameObject.transform;
        

        if(_go.name.IndexOf("Box") > -1){
            ObjectEditing.g_PlaceObjects = null;
        }else{
            ObjectEditing.g_PlaceObjects = g_Brush;
        }
        
        
    }
    public void SetRotations(float _rf){
        transform.Rotate(new Vector3 (0f, 0f, _rf));
        g_Rotation = transform.rotation;
        GameObject _go = Instantiate(g_Brush, transform.position, g_Rotation);
        Destroy(g_Brush);
        g_Brush = _go;
        g_Brush.transform.parent = gameObject.transform;
    }
}
