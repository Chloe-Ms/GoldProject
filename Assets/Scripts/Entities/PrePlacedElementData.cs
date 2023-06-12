using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrePlacedElement
{
    [SerializeField] private List<Vector2Int> _preplacedObstacle = new List<Vector2Int>();
    [SerializeField] private Vector2Int _preplacedBoss = new Vector2Int(0, 0);

    public List<Vector2Int> PreplacedObstacle
    {
        get { return _preplacedObstacle; }
    }

    public Vector2Int PreplacedBoss
    {
        get { return _preplacedBoss; }
    }
}
