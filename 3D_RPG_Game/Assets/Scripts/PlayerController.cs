using UnityEngine;
using Invector.CharacterController;

public class PlayerController : MonoBehaviour
{
    [Header("生命值")]
    public int hp = 100;

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
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerJump();
        PlayerAttack();
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("攻擊觸發");
        }
        //      取得目前動畫狀態(圖層).是否(指定的動畫)動畫名稱
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊"))
        {
            tpc.enabled = false;
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            tpc.enabled = true;
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void Hit(int damage)
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");
        print(hp);
    }
}
