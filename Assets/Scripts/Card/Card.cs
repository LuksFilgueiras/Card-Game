using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardUI cardUI;
    public Deck fromDeck;
    public string userTag;

    [Header("Animator")]
    public Animator animator;
    public bool showCard;
    public bool lockCard;
    public bool isSelected;

    [Header("Card Components")]
    public Image cardBackground;
    public string cardName;
    public Sprite cardPhoto;

    [TextArea(5, 10)]
    public string cardInfo;
    public Character cardCharacterPrefab;
    public int characterAmount;

    [Header("Card Status")]
    public int cardDamage;
    public int cardDefense;
    public int cardHealth;

    [Header("Battle Status")]
    public BattleManager battleManager;
    public Transform charactersSpawnPosition;


    public void Awake(){
        cardUI.SetCardUI(cardPhoto, cardName, cardInfo, cardDamage, cardDefense, cardHealth);
        battleManager = FindObjectOfType<BattleManager>();
    }

    public void Update(){
        ControlAnimations();
    }

    public void SelectCard(){
        if(!isSelected){
            isSelected = true;
            fromDeck.UnselectCardsBy(this);
        }else{
            isSelected = false;
        }
    }

    public void ControlAnimations(){
        animator.SetBool("CardSelected", isSelected);
        animator.SetBool("ShowCard", showCard);
        animator.SetBool("LockCard", lockCard);
    }

    public void ActivateCard(){
        StartCoroutine(InstantiateCharacters());
    }

    IEnumerator InstantiateCharacters(){
        int amount = 0;
        float spawnTime = 0f; 
        float spawnTimeStep = 0.15f;

        while(amount < characterAmount){
            spawnTime += Time.deltaTime;

            if(spawnTime >= spawnTimeStep){
                float x = Random.Range(-1.5f, 1.5f);
                float y = Random.Range(-1.5f, 1.5f);
                Character character = Instantiate(cardCharacterPrefab, charactersSpawnPosition.position + new Vector3(x, y, 0), Quaternion.identity);
                character.SetAttributes(this);
                amount++;
                spawnTime = 0;
            }

            yield return null;
        }
        
        Destroy(this.gameObject);
    }


    #region POINTER FUNCTIONS
    public void OnPointerDown(PointerEventData eventData)
    {
        SelectCard();   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isSelected){
            showCard = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isSelected){
            showCard = false;
        }
    }

    #endregion
}
