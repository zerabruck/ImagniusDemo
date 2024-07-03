using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Animator anim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private float attackCoolDown;
    private float currentCoolDown = Mathf.Infinity;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }
    private void Attack(){
            fireballs[0].transform.position = attackPoint.position;
            fireballs[0].GetComponent<FireballProjectile>().SetDirection(transform.localScale.x);
            currentCoolDown = 0;
        
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F) && currentCoolDown >= attackCoolDown && playerMovement.canAttack()){
            Debug.Log("Attack is clicked" + Time.deltaTime);
            anim.SetTrigger("singleAttack");
        }
        currentCoolDown += Time.deltaTime;
    }
}
