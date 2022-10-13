using UnityEngine;

namespace CallBreak {
    public enum Player {
        Bottom, Right, Top, Left
    }

    public class GameManager : MonoBehaviour {
        [SerializeField] private UIManager uiManager = null;
        [SerializeField] private CardSpriteRef cardSpriteRef = null;

        [HideInInspector] public Card[] bottomPlayerCards = new Card[13];
        [HideInInspector] public Card[] rightPlayerCards = new Card[13];
        [HideInInspector] public Card[] topPlayerCards = new Card[13];
        [HideInInspector] public Card[] leftPlayerCards = new Card[13];
        
        [HideInInspector] public Player runingPlayer = Player.Bottom;
        [HideInInspector] public int round = 0;

        private Card[] playingCards = new Card[4];
        private Player[] playingPlayers = new Player[4];
        private int playingCardIndex = 0;
        

        public void AddPlayingCard(Card card, Player player) {
            if (playingCardIndex < 4) {
                playingCards[playingCardIndex] = card;
                playingPlayers[playingCardIndex] = player;
                playingCardIndex++;
            }
        }

        public bool CheckBottonPlayerValidClick(Card card) {
            //if table empty Play selected card 
            if (IsBoardEmpty()) {
                return true;
            }

            // check selected type card 
            Card firstCard = playingCards[0];
            if (firstCard.type == card.type) {
                Card bigerPlayingCard = playingCards[0];
                for (int i = 1; i < 4; i++) {
                    if (playingCards[i] != null) {
                        if (playingCards[i].type == bigerPlayingCard.type) {
                            if ((int)playingCards[i].cName > (int)bigerPlayingCard.cName) {
                                bigerPlayingCard = playingCards[i];
                            }
                        }
                    }
                }
                if ((int)card.cName > (int)bigerPlayingCard.cName) {
                    return true;
                }

                for (int i = 0; i < 13; i++) {
                    if (bottomPlayerCards[i] != null && bottomPlayerCards[i].type == bigerPlayingCard.type) {
                        if ((int)bottomPlayerCards[i].cName > (int)bigerPlayingCard.cName) {
                            return false;
                        }
                    }
                }
                return true;
            }
            else {
                if (card.type == CardType.Spade) {
                    // check I have first card type or not 
                    for (int i = 0; i < 13; i++) {
                        if (bottomPlayerCards[i] != null) {
                            if (bottomPlayerCards[i].type == firstCard.type) { // I have first card type so super card can not play
                                return false;
                            }
                        }
                    }
                    // I have no first card type 
                    // Check super card on table or not
                    Card bigerSuperCard = null;
                    for (int i = 0; i < 4; i++) {
                        if (playingCards[i] != null) {
                            if (playingCards[i].type == CardType.Spade) {
                                if (bigerSuperCard == null) {
                                    bigerSuperCard = playingCards[i];
                                }
                                else {
                                    if ((int)bigerSuperCard.cName < (int)playingCards[i].cName) {
                                        bigerSuperCard = playingCards[i];
                                    }
                                }
                            }
                        }
                    }
                    // No super card found on table 
                    if (bigerSuperCard == null) {
                        return true;
                    }
                    else { // super card found on table 
                        if((int)card.cName > (int)bigerSuperCard.cName) {
                            return true;
                        }
                        // find my biger super card .
                        Card myBigerSuperCard = card;
                        for(int i = 0; i < 13; i++) {
                            if(bottomPlayerCards[i] != null && bottomPlayerCards[i].type == CardType.Spade) {
                                if((int)bottomPlayerCards[i].cName > (int)myBigerSuperCard.cName) {
                                    myBigerSuperCard = bottomPlayerCards[i];
                                }
                            }
                        }
                        // Compare biger super card 
                        if ((int)myBigerSuperCard.cName > (int)card.cName) {
                            return false;
                        }
                        return true;
                    }
                }
                else {
                    for (int i = 0; i < 13 ; i++) {
                        if (bottomPlayerCards[i] != null) {
                            if (playingCards[0].type == bottomPlayerCards[i].type) {
                                return false;
                            }
                        }
                    }

                    for (int i = 0; i < 13; i++) {
                        if (bottomPlayerCards[i] != null) {
                            if (bottomPlayerCards[i].type == CardType.Spade) {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
        }

        private void RoundComplete() {
            round++;
            Player winner = RoundWinner();
            ClearPlayingCard();
            runingPlayer = winner;

            if (round > 12) {
                uiManager.GameOver();
            }
            else {
                if (runingPlayer == Player.Bottom) {
                    PlayBotPlayer();
                }
                else {
                    PlayBotPlayer();
                }
            }
        }

        private void ClearPlayingCard() {
            for (int i = 0; i < 4; i++) {
                if (playingCards[i] != null) {
                    Destroy(playingCards[i].gameObject);
                }
            }
            playingCardIndex = 0;
        }

        public void PlayBotPlayer() {
            if (playingCardIndex == 4) {
                Invoke(nameof(RoundComplete), 1f);
            }
            else {
                Invoke(nameof(PlayBotPlayerDelay), 1f);
            }
        }

        public void PlayRealPlayer() {
            if (playingCardIndex == 4) {
                Invoke(nameof(RoundComplete), 1f);
            }
        }

        private void PlayBotPlayerDelay() {
            if (round > 12) {
                return;
            }

            switch (runingPlayer) {
                case Player.Right:
                    uiManager.ClickCardByBot(BotPlayLogicalCard(rightPlayerCards));
                break;
                case Player.Top:
                    uiManager.ClickCardByBot(BotPlayLogicalCard(topPlayerCards));
                break;
                case Player.Left:
                    uiManager.ClickCardByBot(BotPlayLogicalCard(leftPlayerCards));
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

        // Get round
        private Player RoundWinner() {
            // check super card
            bool isSuperCardFound = false;
            for (int i = 0; i < 4; i++) {
                if (playingCards[i].type == CardType.Spade) {
                    isSuperCardFound = true;
                    break;
                }
            }

            if (isSuperCardFound) {
                Card bigSuperCard = null;
                int playerIndex = 0;
                for (int i = 0; i < 4; i++) {
                    if (playingCards[i].type == CardType.Spade) {
                        if (bigSuperCard == null) {
                            bigSuperCard = playingCards[i];
                            playerIndex = i;
                        }
                        else {
                            if ((int)bigSuperCard.cName < (int)playingCards[i].cName) {
                                bigSuperCard = playingCards[i];
                                playerIndex = i;
                            }
                        }
                    }
                }
                return playingPlayers[playerIndex];
            }
            else {
                // check same type of card
                bool isSameTypeCardFound = false;
                for (int i = 1; i < 4; i++) {
                    if (playingCards[i].type == playingCards[0].type) {
                        isSameTypeCardFound = true;
                        break;
                    }
                }

                if (isSameTypeCardFound) {
                    Card bigSameCard = playingCards[0];
                    int playerIndex = 0;
                    for (int i = 1; i < 4; i++) {
                        if (playingCards[i].type == playingCards[0].type) {
                            if ((int)bigSameCard.cName < (int)playingCards[i].cName) {
                                bigSameCard = playingCards[i];
                                playerIndex = i;
                            }
                        }
                    }
                    return playingPlayers[playerIndex];
                }
                else {
                    return playingPlayers[0];
                }
            }
        }
    }
}
