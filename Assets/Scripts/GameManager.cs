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

        public void PlayBotPlayer() {
            Invoke(nameof(PlayBotPlayerDelay), 1f);
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
    }
}
