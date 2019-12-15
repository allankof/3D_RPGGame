using UnityEngine;
using UnityEngine.UI;
using Invector.CharacterController;

public class PlayerController : MonoBehaviour
{
    [Header("生命值")]
    public int hp = 100;
    public Slider slider;
    
    [Header("攻擊距離"), Range(0f, 5f)]
    public float rangeAttack = 3f;
    [Header("傷害間隔"), Range(0f, 5f)]
    public float delayAttack = 1f;

    public int attack = 20;

    private Animator ani, aniRoot;
    private vThirdPersonController tpc;
    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        aniRoot = transform.root.GetComponent<Animator>();    // 根物件
        tpc = transform.root.GetComponent<vThirdPersonController>();
        rig = transform.root.GetComponent<Rigidbody>();

        slider.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetBool("死亡開關")) return;
        PlayerMove();
        PlayerJump();
        PlayerAttack();
    }

    /// <summary>
    /// 敵人行動範圍
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * rangeAttack);    // forward : 前方 Z、right : 右方 X、up : 上方 Y
    }

    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");    // A,D或左右鍵
        float v = Input.GetAxisRaw("Vertical");      // W,S或上下

        ani.SetBool("走路開關", h != 0 || v != 0);

        //print("左右 :" + h);
        //print("上下 :" + v);

        ani.SetBool("跑步開關", Input.GetKey(KeyCode.LeftShift));
    }

    private void PlayerJump()
    {
        ani.SetBool("跳躍開關", !aniRoot.GetBool("IsGrounded"));
    }

    private void PlayerAttack()
    {
        // 如果 按下左鍵 並且 不是攻擊動畫 並且 不是在轉換線
        if (Input.GetKeyDown(KeyCode.Mouse0) && !ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊") && ani.IsInTransition(0))
        {
            ani.SetTrigger("攻擊觸發");
            Invoke("DelayAttack", delayAttack);
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("受傷"))
        {
            CancelInvoke("DelayAttack");
        }
        //      取得目前動畫狀態(圖層).是否(指定的動畫)動畫名稱
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊") || ani.GetCurrentAnimatorStateInfo(0).IsName("受傷"))
        {
            tpc.enabled = false;
            tpc.lockMovement = true;
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            tpc.enabled = true;
            tpc.lockMovement = false;
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void DelayAttack()
    {
        RaycastHit hit;    // 射線碰撞資訊
        // 物理.射線碰撞(起點，方向，射線碰撞資訊，長度)
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rangeAttack))
        {
            if (hit.collider.gameObject.name == "敵人")
            {
                hit.collider.GetComponent<Enemy>().Hit(attack);
            }
            //print(hit.collider.gameObject);
        }
    }

    public void Hit(int damage)
    {
        hp -= damage;
        slider.value = hp;
        ani.SetTrigger("受傷觸發");
        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        ani.SetBool("死亡開關", true);
        tpc.enabled = false;
        rig.constraints = RigidbodyConstraints.FreezeAll;
    }
}
