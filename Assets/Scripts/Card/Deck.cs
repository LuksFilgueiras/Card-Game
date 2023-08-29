using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public string userTag;
    public Transform spawnPosition;
    public Transform hands;
    public List<Card> cards;
    public List<Card> cardsInHand = new List<Card>();

    [Header("Flags")]
    public bool isDrawingCards;

    public void ShuffleCards(){
        List<Card> shuffledList = new List<Card>();
        int cardsAmount = cards.Count;

        for(int i = 0; i < cardsAmount; i++){
            int randomCardIndex = Random.Range(0, cards.Count);
            shuffledList.Add(cards[randomCardIndex]);
            cards.RemoveAt(randomCardIndex);
        }

        cards = shuffledList;
    }

    public void Awake(){
        userTag = gameObject.tag;
        ShuffleCards();
    }


    public IEnumerator DrawCards(){
        isDrawingCards = true;
        float drawTime = 0f;
        float drawStep = 0.4f;
        int i = cardsInHand.Count;


        while(cardsInHand.Count < 4){
            drawTime += Time.deltaTime;
            if(drawTime >= drawStep){
                Card card;
                card = Instantiate(cards[0], hands);
                cards.RemoveAt(0);
                card.charactersSpawnPosition = spawnPosition;
                card.fromDeck = this;
                cardsInHand.Add(card);
                drawTime = 0;
            }
            yield return null;
        }

        isDrawingCards = false;
    }

    public void DrawCardsAI(){
        int i = cardsInHand.Count;

        while(cardsInHand.Count < 4){
            Card card = Instantiate(cards[0], transform);
            cards.RemoveAt(0);
            card.charactersSpawnPosition = spawnPosition;
            card.fromDeck = this;
            cardsInHand.Add(card);
        }
    }

    public void UnselectCardsBy(Card cardSelected){
        foreach(Card card in cardsInHand){
            if(card != cardSelected){
                card.isSelected = false;
                card.showCard = false;
            }
        }
    }

    public void ReturnCardAtLast(Card card){
        cards.Add(Instantiate(card));
        cardsInHand.Remove(card);
        Destroy(card.gameObject);
    }
}
