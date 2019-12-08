using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("基本屬性")]
    public float atk = 20f;
    public float hp = 250f;
    [Range(0f, 100f)]
    public float speed = 10f;
    [Range(0f, 100f), Tooltip("攻擊距離")]
    public float distanceATK = 1.5f;
    [Range(0f, 100f), Tooltip("追蹤距離")]
    public float distanceTrack = 12f;

    /// <summary>
    /// 敵人行動範圍
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);                // 圖示顏色
        Gizmos.DrawSphere(transform.position, distanceATK);     // 圓形範圍

        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceTrack);
    }
}
