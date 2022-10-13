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

        private Card[] playingCards = new Card[4];
        private int playingCardIndex = 0;

        public void AddPlayingCard(Card card) {
            if (playingCardIndex < 4) {
                playingCards[playingCardIndex] = card;
                playingCardIndex++;
            }
        }

        public void ClearPlayingCard() {
            for (int i = 0; i < 4; i++) {
                if (playingCards[i] != null) {
                    Destroy(playingCards[i].gameObject);
                }
            }
            playingCardIndex = 0;
        }

        public void PlayBotPlayer() {
            Invoke(nameof(PlayBotPlayerDelay), 1f);
        }

        private void PlayBotPlayerDelay() {
            if (round > 12) {
                return;
            }

            switch (runingPlayer) {
                case Player.Right:
                    uIManager.ClickCardByBot(BotPlayLogicalCard(rightPlayerCards));
                break;
                case Player.Top:
                    uIManager.ClickCardByBot(BotPlayLogicalCard(topPlayerCards));
                break;
                case Player.Left:
                    uIManager.ClickCardByBot(BotPlayLogicalCard(leftPlayerCards));
                    round++;
                break;
            }
        }

        // Bot Thinking Area
        private bool IsBoardEmpty() {
            for (int i = 0; i < 4; i++) {
                if (playingCards[i] != null) {
                    return false;
                }
            }
            return true;
        }

        private Card GetFirstValidCard(Card[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                if (arr[i] != null) {
                    return arr[i];
                }
            }
            return null;
        }

        private Card BotPlayLogicalCard(Card[] arr) {
            if (IsBoardEmpty()) {
                return GetFirstValidCard(arr);
            }

            // already card has on board
            CardType validCard = playingCards[0].type;
            int cardPower = (int)playingCards[0].cName;

            // Try move same type of card
            for (int i = 0; i < 13; i++) {
                if (arr[i] != null) {
                    if (arr[i].type == validCard) {
                        return arr[i];
                    }
                }
            }

            // Try move Super card
            for (int i = 0; i < 13; i++) {
                if (arr[i] != null) {
                    if (arr[i].type == CardType.Spade) {
                        return arr[i];
                    }
                }
            }

            // move any card
            for (int i = 0; i < 13; i++) {
                if (arr[i] != null) {
                    return arr[i];
                }
            }

            return null;
        }
    }
}
