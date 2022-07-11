using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheCutter : MonoBehaviour
{
    [SerializeField]
    private List<Collider> colliders;

    private bool canCut;

    public bool CanCut 
    {
        get
        {
            return canCut;
        }
        set
        {
            if (value != canCut)
            {
                foreach (var c in colliders)
                {
                    c.enabled = value;
                }
                canCut = value;
            }
        }
    }
}
