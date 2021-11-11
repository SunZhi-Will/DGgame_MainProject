using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Archery_Sailor : MonoBehaviour
{
    [SerializeField]Archery_Player player;
    [SerializeField] PositionConstraint _bowPostion;
    [SerializeField] RotationConstraint _bowRotation;
    [SerializeField] ParentConstraint _straightRotation;

    int index= 0;
    [SerializeField] GameObject[] Sailors;
    [SerializeField] Transform[] RightHand;
    [SerializeField] Transform[] HeadEnd;
    ConstraintSource constraintSource;
    private void Start()
    {
        SetSource();
    }

    void SetSource()
    {
        for (int i = 0; i < Sailors.Length; i++)
        {
            if (Sailors[i].activeSelf) { index = i; break; }
        }

        constraintSource.sourceTransform = RightHand[index];
        constraintSource.weight = 1;
        _bowPostion.AddSource(constraintSource);
        _bowRotation.AddSource(constraintSource);

        constraintSource.sourceTransform = HeadEnd[index];
        constraintSource.weight = 1;
        _straightRotation.AddSource(constraintSource);

        player.animator = Sailors[index].GetComponent<Animator>();
    }
}
