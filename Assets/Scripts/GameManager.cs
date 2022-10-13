using System.Collections.Generic;
using UnityEngine;

namespace CallBreak {
    public enum Player {
        Bottom, Right, Top, Left
    }

    public class GameManager : MonoBehaviour {
        [SerializeField] private UIManager uIManager = null;
        [SerializeField] private CardSpriteRef cardSpriteRef = null;

        [HideInInspector] public Card[] bottomPlayerCards = new Card[13];
        [HideInInspector] public Card[] rightPlayerCards = new Card[13];
        [HideInInspector] public Card[] topPlayerCards = new Card[13];
        [HideInInspector] public Card[] leftPlayerCards = new Card[13];
        
        [HideInInspector] public Player runingPlayer = Player.Bottom;
        [HideInInspector] public int round = 0;

        [HideInInspector] public List<Card> Spade = new List<Card>();
        [HideInInspector] public List<Card> Heart = new List<Card>();
        [HideInInspector] public List<Card> Clover = new List<Card>();
        [HideInInspector] public List<Card> Diamond = new List<Card>();
        [HideInInspector] public List<List<Card>> botttomPlyerCardsList = new List<List<Card>>();
        [HideInInspector] public List<List<Card>> rightPlayerCardsList = new List<List<Card>>();
        [HideInInspector] public List<List<Card>> topPlyerCardsList = new List<List<Card>>();
        [HideInInspector] public List<List<Card>> leftotttomPlyerList = new List<List<Card>>();
        public void PlayBotPlayer() {
            Invoke(nameof(PlayBotPlayerDelay), 1f);
        }
        public void StoreCardByGroup( List<List<Card>> cardList , Card[] cards ) {
            cardList.Add(Spade);
            cardList.Add(Heart);
            cardList.Add(Clover);
            cardList.Add(Diamond);
            for (int i = 0; i < 13; i++) {
                Card card = cards[i];
                switch(card.type){
                    case CardType.Spade:
                    cardList[0].Add(card);
                    break;
                    case CardType.Heart:
                    cardList[1].Add(card);
                    break;
                    case CardType.Clover:
                    cardList[2].Add(card);
                    break;
                    case CardType.Diamond:
                    cardList[3].Add(card);
                    break;
                }
            }
           

        }

        private void PlayBotPlayerDelay() {
            if (round > 12) {
                return;
            }

            switch (runingPlayer) {
                case Player.Right:
                    uIManager.ClickCardByBot(rightPlayerCards[round]);
                break;
                case Player.Top:
                    uIManager.ClickCardByBot(topPlayerCards[round]);
                break;
                case Player.Left:
                    uIManager.ClickCardByBot(leftPlayerCards[round]);
                    round++;
                break;
            }
        }

        private void GameRules(Player turnPlayer, Card[] playingCard) {
            int playerBotton, playerRight, PlayerTop, Playerleft;
            Card card = playingCard[0];
            int cardType = (int)card.type;
            // Counting who many cards each player has to play.
            playerBotton = botttomPlyerCardsList[cardType].Count;
            playerRight = rightPlayerCardsList[cardType].Count;
            PlayerTop = topPlyerCardsList[cardType].Count;
            Playerleft = leftotttomPlyerList[cardType].Count;
            if (round == 0) {
                if (playerRight != 0) {
                    for (int i = 0; i < 4; i++) {
                        //if()

                    }
                }
            }
          


            
            



        }
    }
}
