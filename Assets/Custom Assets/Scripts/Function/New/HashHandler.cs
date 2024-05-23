using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Hash handler
/// </summary>
//////////////////////////////////////////////////////////////////////
#region Hash handler

//--------------------------------------------------
public class HashHandler : MonoBehaviour
{
    public static HashHandler instance;

    [ReadOnly]
    public List<Hash128> hashes = new List<Hash128>();

    private void Awake()
    {
        instance = this;
    }

    public static Hash128 RegRandHash()
    {
        Hash128 result = new Hash128();

        do
        {
            result = Hash128.Compute(Time.time.ToString() + UnityEngine.Random.value.ToString());
        }
        while (instance.hashes.Contains(result));

        instance.hashes.Add(result);

        return result;
    }

    public static void RemoveHash(params Hash128[] hashes_pr)
    {
        foreach (Hash128 hash_tp in hashes_pr)
        {
            if (instance.hashes.Contains(hash_tp))
            {
                instance.hashes.Remove(hash_tp);
            }
        }
    }

    public static bool ContainsHash(Hash128 hash_pr)
    {
        return instance.hashes.Contains(hash_pr);
    }
}

#endregion
