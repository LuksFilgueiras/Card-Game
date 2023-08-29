using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TurnManager turnManager;

    public List<Character> playerCharactersInBattle;
    public List<Character> otherCharactersInBattle;

    public string playerTag;
    public string otherTag;

    public bool allCharactersPlaced;

    [Header("Victory Screen")]
    public GameObject victoryScreenGameObject;
    public TextMeshProUGUI victoryText;
    public float victoryScreenDuration = 5f;

    public void Start(){
        victoryScreenGameObject.SetActive(false);
    }

    public void Update(){
        if(!allCharactersPlaced){
            if(playerCharactersInBattle.Count >= 10 || otherCharactersInBattle.Count >= 10){
                allCharactersPlaced = true;
            }

            return;
        }

        if(allCharactersPlaced){
            if(CheckVitoryOver(otherCharactersInBattle)){
                WinBattle(playerTag);
            }

            if(CheckVitoryOver(playerCharactersInBattle)){
                WinBattle(otherTag);
            }
        }
    }


    public void AddCharacters(string userTag, Character character){
        if(userTag == "Player"){
            playerCharactersInBattle.Add(character);
            playerTag = userTag;
        }else{
            otherCharactersInBattle.Add(character);
            otherTag = userTag;
        }
    }

    public bool CheckVitoryOver(List<Character> characters){
        int index = 0;
        foreach(Character character in characters){
            if(character.isDead){
                index++;
            }
        }

        if(index == characters.Count){
            return true;
        }

        return false;
    }

    public void WinBattle(string userTag){
        allCharactersPlaced = false;

        StartCoroutine(VictoryScreen(userTag));
    }

    public void CleanBattleField(List<Character> characters){
        foreach(Character character in characters){
            Destroy(character.gameObject);
        }

        characters.Clear();
    }

    public IEnumerator VictoryScreen(string userTag){
        float timer = 0, duration = victoryScreenDuration;
        
        victoryScreenGameObject.SetActive(true);
        victoryText.text = userTag + " Victory";

        while(timer < duration){
            timer += Time.deltaTime;
            yield return null;
        }

        CleanBattleField(otherCharactersInBattle);
        CleanBattleField(playerCharactersInBattle);
        victoryScreenGameObject.SetActive(false);
        turnManager.state = BattleState.draw;
    }
}
