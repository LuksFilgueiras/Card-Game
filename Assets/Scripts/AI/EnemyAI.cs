using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public TurnManager turnManager;
    public Deck deck;
    public Card cardSelected;

    public Transform spawnPosition;
    

    public void Awake(){
        ShuffleCards();
    }

    public void Update(){
        if(turnManager.state == BattleState.selection){
            SelectionTurn();
        }

        if(turnManager.state == BattleState.battle){
            BattleTurn();
        }
    }

    #region Deck Methods
    public void ShuffleCards(){
        deck.ShuffleCards();
    }

    public void DrawCards(){
        deck.DrawCardsAI();
    }

    public void ReturnCardAtLast(Card card){
        deck.ReturnCardAtLast(card);
    }

    #endregion

    #region AI
    public void SelectionTurn(){
        if(cardSelected == null){
            DrawCards();
            int cardIndex = Random.Range(0, deck.cardsInHand.Count);
            cardSelected = deck.cardsInHand[cardIndex];
        }
    }

    public void BattleTurn(){
        if(cardSelected != null){
            Card newCard = Instantiate(cardSelected);
            newCard.userTag = gameObject.tag;
            newCard.ActivateCard();
            ReturnCardAtLast(cardSelected);
        }
    }

    #endregion
}
