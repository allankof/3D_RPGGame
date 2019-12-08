using UnityEngine;
using UnityEngine.Events;

public class PropEvent : MonoBehaviour
{
    [Header("任務事件")]
    public UnityEvent OnGetProp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "零件")
        {
            Destroy(other.gameObject);
            OnGetProp.Invoke();
        }
    }
}
