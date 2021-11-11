using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Player", menuName = "KandoPYControl/KandoPYControl")]
public class KandoPYControl : ScriptableObject
{
    [Header("Input")]
    public string go = "space";

   
    public string left = "A";
    public string right = "D";
    /*
    public string options = "space";
    */
    public int characterIndex = 0;
}
