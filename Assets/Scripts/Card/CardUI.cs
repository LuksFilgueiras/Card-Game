using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image characterPhoto;
    public TextMeshProUGUI characterName, characterInfo, damage, defense, health;

    public void SetCardUI(Sprite photo, string name, string info, int damage, int defense, int health){
        characterPhoto.sprite = photo;
        characterName.text = name;
        characterInfo.text = info;
        this.damage.text = damage.ToString();
        this.defense.text = defense.ToString();
        this.health.text = health.ToString();
    }   
}
