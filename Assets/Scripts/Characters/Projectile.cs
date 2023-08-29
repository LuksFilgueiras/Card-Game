using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D projectileRigidbody;
    public int damage;
    public CharacterType characterType;

    public void SetProjectile(int damage, string userTag, CharacterType characterType){
        this.damage = damage;
        this.gameObject.tag = userTag;
        this.characterType = characterType;
        Destroy(this.gameObject, 2f);
    }

    public void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag != gameObject.tag && col.gameObject.tag != "Scenario" && col.gameObject.layer != 7){
            col.GetComponent<Character>().CalculateDamage(damage, characterType);
            Destroy(this.gameObject);
        }
    }
}
