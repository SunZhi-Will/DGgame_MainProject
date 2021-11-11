using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//模型材質球切換
public class ModeSetupChange : MonoBehaviour
{
    public PlayerNumber playerNumber;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer_scr;//更換對象
    [SerializeField]
    private Material[] material_array;//欲更換材質球

    public Material[] Test_material_array;//欲更換材質球

    public bool Skip = false;
    //顏色切換
    //public IEnumerator Start()
    public void Start()
    {
        SetColorChange(Skip);
    }
    public void SetColorChange(bool _skip = false)
    {
        if (_skip == false) 
        {
            if (material_array == null) { Debug.LogError("material_lsit 沒有設定!"); }
            if (GameMaster_koroshi.s_GameMaster == null) { return; }//確認目前非測試模式
            if (GameMaster_koroshi.s_GameMaster.PlayerModelColorChange(playerNumber) == false) { return; }
            //StartCoroutine(WaitChangeColor());
        }
        skinnedMeshRenderer_scr.materials = new Material[material_array.Length];
        for (int i = 0; i < skinnedMeshRenderer_scr.materials.Length; i++)
        {
            skinnedMeshRenderer_scr.materials[i].mainTexture = material_array[i].mainTexture;
        }
        Debug.Log(gameObject.name + "\n" + "更換材質球" + " " + "數量:" + material_array.Length);

    }
    //IEnumerator WaitChangeColor()
    //{
    //    yield return new WaitForSeconds(3);

    //}
}
