using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum Team {no_Team, A_Team, B_Team, C_Team}
public enum Tipe_Skeleton {Sword, Arbalest, Ax, Magician}

public class Sceleton : MonoBehaviour
{
    public GameObject view ,bone ,arrowPrefab;
    public Transform arrowSpawnPoint;

    [Header("")]
    public Tipe_Skeleton Skelet_Tipe;
    public Animator anim;
    public NavMeshAgent agent;
    public Transform points;
    public float waitTime = 4f;
    public GameObject player;

    [Header("")]
    public int health = 2;
    public float SimpleSpeed, TargetSpeed;
    public Team team;

    [Header("")]
    public bool slep;
    public bool underground;
    public bool brokenA;
    public bool brokenB;
    public bool sit;

    [HideInInspector]public bool slipStatus;
    [HideInInspector]public bool slipTarget;
    [HideInInspector]public bool slipEnd, lost = true;
    private bool iCanTakeDamage = true, spinning;
    private Coroutine patrolCoroutine;


    void Start(){
        player = GameObject.FindWithTag("Player");
        CheckStatus();
        // anim.SetBool("underground", false);
    }
    public void SpawnBone(){
        Instantiate(bone, transform.position, transform.rotation);
    }





    public void CheckStatus(){
        if(slep){
            anim.SetBool("slep", true); slipStatus = true;
        }
        if(underground){
            anim.SetBool("underground", true); slipStatus = true;
        }
        if(brokenA){
            anim.SetBool("brokenA", true); slipStatus = true;
        }
        if(brokenB){
            anim.SetBool("brokenB", true); slipStatus = true;
        }

        if(slipStatus){
            Destroy(view);
        }
        if(sit){
            anim.SetBool("sit", true); slipStatus = true;
        }
        slipEnd = slipStatus;
        slipTarget = slipStatus;

        patrolCoroutine = StartCoroutine(MoveTowardsWaypoint());
    }
    IEnumerator MoveTowardsWaypoint(){
        if (slipEnd || Skelet_Tipe == Tipe_Skeleton.Arbalest){
            yield break;
        }
        agent.speed = SimpleSpeed;

        while (true)
        {
            float NewWaitTime = Random.Range(waitTime * 0.5f ,waitTime);
            int TargetPoint = Random.Range(0 ,points.childCount);

            anim.SetBool("isPatrolling", true);
            agent.SetDestination(points.GetChild(TargetPoint).position);

            while (Vector3.Distance(transform.position, points.GetChild(TargetPoint).position) > 0.2f){
                yield return new WaitForSeconds(1f);
            }

            anim.SetBool("isPatrolling", false);
            yield return new WaitForSeconds(NewWaitTime);
        }
    }
    IEnumerator MoveToPlayer(){
        agent.speed = TargetSpeed;
        anim.SetBool("isPatrolling", true);
        anim.SetBool("TargrtPlayer", true);
        
        while (!lost)
        {
            agent.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void TargetPlayer(bool _First = false){
        if(!lost){
            return;
        }
        lost = false;

        if(slipStatus){
            if(_First && !sit){
                return;
            }
            slipEnd = false;

            anim.SetBool("underground", false);
            anim.SetBool("brokenA", false);
            anim.SetBool("brokenB", false);
            anim.SetBool("slep", false);
            anim.SetBool("sit", false);
            if(slipTarget){
                if(!sit){
                    return;
                }
            }
        }

        if (_First && team != Team.no_Team)
        {
            Transform AllEnemy = transform.parent.parent.transform;

            foreach (Transform _Enemy in AllEnemy)
            {
                Sceleton sceleton = _Enemy.GetChild(0).GetComponent<Sceleton>();
                if(team == sceleton.team){
                    sceleton.TargetPlayer();
                }
            }
        }

        if(patrolCoroutine != null){
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
        
        if(Skelet_Tipe == Tipe_Skeleton.Arbalest){
            patrolCoroutine = StartCoroutine(Arbalest_Player());
        }else{
            patrolCoroutine = StartCoroutine(MoveToPlayer());
        }
    }
    public void AttackPlayer(){
        if(Skelet_Tipe == Tipe_Skeleton.Ax){
            if(spinning){
                return;
            }
            if(Random.value > 0.65){
                StartCoroutine(AxColdoun());
            }
        }

        anim.SetTrigger("Attack");
    }
    IEnumerator AxColdoun(){
        spinning = true;
        agent.speed = 1.5f;
        anim.SetBool("Ulta", true);

        yield return new WaitForSeconds(5f);

        spinning = false;
        agent.speed = TargetSpeed;
        anim.SetBool("Ulta", false);
    }

    IEnumerator Arbalest_Player()
    {
        Vector3 Direction = player.transform.position - transform.position;
        transform.eulerAngles = new Vector3(0f, Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg, 0f);

        StartCoroutine(Arbalest_Target_Player());

        while (!lost) // поворот
        {
            Vector3 direction = player.transform.position - transform.position;
            transform.eulerAngles = new Vector3(0f, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0f);

            yield return null;
        }
    }
    IEnumerator Arbalest_Target_Player(){
        while (!lost)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Ray ray = new Ray(arrowSpawnPoint.position, directionToPlayer);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(1.4f);
                GameObject Arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
                Arrow.transform.rotation = Quaternion.LookRotation(directionToPlayer);


                StartCoroutine(Arbalest_Arrow(Arrow, directionToPlayer));
            }

            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator Arbalest_Arrow(GameObject _Arrow, Vector3 _Direction){
        StartCoroutine(End_Arrow(_Arrow));

        while (_Arrow != null)
        {
            _Arrow.transform.position += _Direction * 4f * Time.deltaTime;

            yield return null;
        }
        
        yield break;
    }IEnumerator End_Arrow(GameObject _Arrow){
        yield return new WaitForSeconds(5f);

        Destroy(_Arrow);
        yield break;
    }


    public void LostPlayer(){
        lost = true;
        anim.SetBool("TargrtPlayer", false);
        
        if(patrolCoroutine != null){
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }

        if(Skelet_Tipe == Tipe_Skeleton.Arbalest){
            StopCoroutine("Arbalest_Player");
            StopCoroutine("Arbalest_Target_Player");
        }
        
        patrolCoroutine = StartCoroutine(MoveTowardsWaypoint());
    }

    public void GetDamage(){
        if(!iCanTakeDamage){
            return;
        }

        health--;

        if(health <= 0){
            SceletonLose();
        }else{
            if(lost){
                TargetPlayer();
            }
            StartCoroutine(Sceletonimunity());
        }
    }
    IEnumerator Sceletonimunity(){
        iCanTakeDamage = false;

        yield return new WaitForSeconds(1f);

        iCanTakeDamage = true;
    }
    public void SceletonLose(){
        team = Team.no_Team;
        slipTarget = true;
        lost = true;
        if(patrolCoroutine != null){StopCoroutine(patrolCoroutine);}
        patrolCoroutine = null;

        anim.SetTrigger("Lose");
    }
    public void Destroy(){
        Destroy(transform.gameObject);
    }
    public void WakeUpAfter(float _Time){
        Invoke("GetUp", _Time);
    }
    void GetUp(){
        anim.SetBool("underground", false);
    }
}
