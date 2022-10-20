using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Room : ScriptableObject
{
    public Vector2Int[] openings = new Vector2Int[4];
    public GameObject roomPrefab;
}
