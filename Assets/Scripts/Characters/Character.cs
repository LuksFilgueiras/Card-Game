using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType{
    none,
    spearman,
    soldier,
    cavalry,
    ranged
}

public class Character : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D characterRigidbody;
    public CharacterType characterType;
    public CharacterType characterWeakness;
    public LayerMask characterLayer;

    [Header("Stats")]
    public int damage;
    public int defense;
    public int health;
    public float moveSpeed;
    public bool isLookingLeft;
    public float attackDelay = 0.8f, attackDelayCounter;
    public float distanceToAttack = 0.8f;
    public bool isDead;

    [Header("Ranged")]
    public Projectile projectile;

    [Header("Battle Components")]
    public BattleManager battleManager;
    public Collider2D[] charactersInGame;
    public Transform target;

    [Header("Animation Resources")]
    public SpriteRenderer characterSpriteRenderer;
    public Sprite[] attackAnimationSprites;
    public Sprite[] dyingAnimationSprites;

    public void SetAttributes(Card card){
        this.damage = card.cardDamage;
        this.defense = card.cardDefense;
        this.health = card.cardHealth;
        gameObject.tag = card.userTag;
    }

    public void Start(){
        battleManager = FindObjectOfType<BattleManager>();
        battleManager.AddCharacters(gameObject.tag, this);
    }

    public void Update(){
        if(isDead){
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            return;
        }

        GetAllCharacters();
        GetToClosestEnemy();
        Flip();
    }

    public void GetAllCharacters(){
        charactersInGame = Physics2D.OverlapCircleAll(transform.position, 20f, characterLayer);
    }

    public void GetToClosestEnemy(){
        float lessDistant = 9999f;

        foreach(Collider2D col in charactersInGame){
            if(Vector2.Distance(transform.position, col.transform.position) < lessDistant){
                if(col.gameObject.tag != gameObject.tag){
                    lessDistant = Vector2.Distance(transform.position, col.transform.position);
                    target = col.transform;
                }
            }
        }

        if(target == null){
            return;
        }
       

        if(Vector2.Distance(transform.position, target.position) < distanceToAttack){
            animator.SetBool("Moving", false);

            if(characterType == CharacterType.ranged){
                FireProjectile();
            }else{
                Attack();
            }
        }else{
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            animator.SetBool("Moving", true);
        }
    }

    public void Flip(){
        if(target == null){
            return;
        }

        if(!isLookingLeft && target.position.x - transform.position.x < 0){
            isLookingLeft = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        if(isLookingLeft && target.position.x - transform.position.x > 0){
            isLookingLeft = false;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    public void Attack(){
        if(attackDelayCounter <= 0f){
            animator.SetTrigger("Attack");
            target.GetComponent<Character>().CalculateDamage(damage, characterType);
            attackDelayCounter = attackDelay;
        }else{
            attackDelayCounter -= Time.deltaTime;
        }
    }

    public void FireProjectile(){
        if(attackDelayCounter <= 0f){
            animator.SetTrigger("Attack");
            Projectile newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            newProjectile.SetProjectile(damage, gameObject.tag, characterType);
            newProjectile.transform.localScale = transform.localScale;
            newProjectile.transform.right = target.transform.position - newProjectile.transform.position;
            newProjectile.projectileRigidbody.AddForce(newProjectile.transform.right * 6f, ForceMode2D.Impulse);
            attackDelayCounter = attackDelay;
        }else{
            attackDelayCounter -= Time.deltaTime;
        }
    }


    public void CalculateDamage(int damage, CharacterType type){
        if(type == characterWeakness){
            defense = 0;
        }

        health -= damage - defense;

        if(health <= 0){
            animator.SetBool("Moving", false);
            animator.SetTrigger("Die");
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            isDead = true;
            //Destroy(this.gameObject);
        }
    }

    #region Animation Triggers
    public void SetAttackSprite(int index){
        characterSpriteRenderer.sprite = attackAnimationSprites[index];
    }

    public void SetDyingSprite(int index){
        characterSpriteRenderer.sprite = dyingAnimationSprites[index];
    }

    #endregion

}
