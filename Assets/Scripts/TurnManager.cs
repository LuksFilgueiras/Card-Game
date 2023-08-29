using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState{
    draw,
    selection,
    battle,
}
public class TurnManager : MonoBehaviour
{
    public BattleState state;
    public Deck playerDeck;
    public Button passTurnButton;
    public TextMeshProUGUI turnPhrase;

    [Header("End Turn")]
    public bool finishingTurn;
    public float finishCountDown = 3f;
    public TextMeshProUGUI finishCountDownText;


    [Header("Battle Turn")]
    public Card cardSelected;

    public void Start(){
        state = BattleState.draw;
        passTurnButton.gameObject.SetActive(false);
    }

    public void Update(){
        if(state == BattleState.draw){
            if(!playerDeck.isDrawingCards){
                DrawTurn();
            }

            if(playerDeck.isDrawingCards){
                LockAllCards();
            }else{
                UnlockAllCards();
                state = BattleState.selection;
            }
        }

        if(HasCardSelected()){
            finishCountDownText.text = "";
            passTurnButton.gameObject.SetActive(true);
        }else{
            passTurnButton.gameObject.SetActive(false);
        }

        if(finishingTurn){
            finishCountDown -= Time.deltaTime;
            turnPhrase.text = "Cancelar";
            finishCountDownText.text = Mathf.Ceil(finishCountDown).ToString() + " s";
            if(finishCountDown <= 0f){
                state = BattleState.battle;
                Card cardPlaced = Instantiate(cardSelected);
                cardPlaced.userTag = playerDeck.gameObject.tag;
                cardPlaced.ActivateCard();
                playerDeck.ReturnCardAtLast(cardSelected);

                finishCountDown = 3f;
                finishingTurn = false;
            }
        }else{
            turnPhrase.text = "Finalizar";
        }
    }

    public void FinishTurn(){
        if(!finishingTurn){
            finishingTurn = true;
            LockAllCards();
        }else{
            finishingTurn = false;
            UnlockAllCards();
            finishCountDown = 3f;
        }
    }

    public void LockAllCards(){
        foreach(Card card in playerDeck.cardsInHand){
            card.cardBackground.raycastTarget = false;
            card.lockCard = true;
        }
    }

    public void UnlockAllCards(){
        foreach(Card card in playerDeck.cardsInHand){
            card.cardBackground.raycastTarget = true;
            card.lockCard = false;
        }
    }

    public bool HasCardSelected(){
        foreach(Card card in playerDeck.cardsInHand){
            if(card.isSelected){
                cardSelected = card;
                return true;
            }
        }

        return false;
    }

    public void DrawTurn(){
        playerDeck.StartCoroutine(playerDeck.DrawCards());
    }
}

