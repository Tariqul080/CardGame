using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace CallBreak {
    public class UIManager : MonoBehaviour {

        [Header("Card Init")]
        [SerializeField] private Card cardFeb = null;
        [SerializeField] private Transform allCardParent = null;
        [SerializeField] private GameObject playButton = null;
        [SerializeField] private GameObject allPlayerInformation = null;

        [Header("Card Pool")]
        [SerializeField] public RectTransform bottomPlayerPosition = null;
        [SerializeField] public RectTransform rightPlayerPosition = null;
        [SerializeField] public RectTransform topPlayerPosition = null;
        [SerializeField] public RectTransform leftPlayerPosition = null;
        [SerializeField] private RectTransform cardSize = null;
        [SerializeField] private RectTransform playerCardPanel = null;
        [SerializeField] private GameManager gameManager = null;

        [Header("Card On Board")]
        [SerializeField] private Transform[] cardOnBoard = new Transform[4];
        [SerializeField] private GameObject yourTurnTxt = null;
        [SerializeField] private GameObject invalidCardTxt = null;
        [SerializeField] private GameObject gameOverObj = null;


        private Card[] allCards = new Card[52];

        private int playerCardCounter = 13;

        private void Shuffle(Card[] arr) {
            System.Random rand = new System.Random();
            for (int i = arr.Length - 1; i >= 1; i--) {
                int j = rand.Next(i + 1);
                Card tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }
        
        public void ClickPlayButton() {
            allPlayerInformation.SetActive(true);
            playButton.SetActive(false);
            CardMaker();
            Shuffle(allCards);
            Invoke(nameof(DistributeCards), 1f);
        }

        public void ClickPlayAgainButton() {
            gameOverObj.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void CardMaker() {
            int cardType = 0, cardIndex = 0;
            for (int i = 0; i < 52 ; i++) {
                Card card = Instantiate(cardFeb,allCardParent);
                card.Click = ClickCard;

                card.CreateCard((CardType)cardType, (CardName)cardIndex);
                card.transform.name = ((CardType)cardType).ToString() + cardIndex;
                cardIndex++;

                if (cardIndex == 13) {
                    cardIndex = 0;
                    cardType++;
                }
                allCards[i] = card;
            }
        }

        private void DistributeCards() {
            for (int i = 0; i < 52 ; i++) {
                Card card = allCards[i];
                // store cards
                if (i < 13) { // 0-12
                    gameManager.bottomPlayerCards[i] = card;
                }
                else if (i < 26) { // 13-25
                    gameManager.rightPlayerCards[i - 13] = card;
                }
                else if (i < 39) { // 26-38
                    gameManager.topPlayerCards[i - 26] = card;
                }
                else { // 39-51
                    gameManager.leftPlayerCards[i - 39] = card;
                }
            }

            // sort all player cards
            SortCards(gameManager.bottomPlayerCards);
            SortCards(gameManager.rightPlayerCards);
            SortCards(gameManager.topPlayerCards);
            SortCards(gameManager.leftPlayerCards);

            DistributeCardToallPlayer();

            playerCardPanel.sizeDelta = new Vector2(cardSize.sizeDelta.x * 13f, playerCardPanel.sizeDelta.y);

            yourTurnTxt.SetActive(true);
        }

        private void DistributeCardToallPlayer()
        {
            for (int i = 0; i < 52; i++) {
                if (i < 13) {
                    Card card = gameManager.bottomPlayerCards[i];
                    card.transform.DOLocalMove(bottomPlayerPosition.localPosition, 1f).OnComplete(() => {
                        card.transform.parent = playerCardPanel;
                        card.FlipCard(true);
                    });
                }
                else if (i < 26)
                {
                    Card card = gameManager.rightPlayerCards[i-13];
                    card.transform.DOLocalMove(rightPlayerPosition.localPosition, 1f).OnComplete(() => {
                        card.FlipCard(false);
                        card.gameObject.SetActive(false);
                    });
                }
                else if (i < 39)
                {
                    Card card = gameManager.topPlayerCards[i-26];
                    card.transform.DOLocalMove(topPlayerPosition.localPosition, 1f).OnComplete(() => {
                        card.FlipCard(false);
                        card.gameObject.SetActive(false);
                    });
                }
                else
                {
                    Card card = gameManager.leftPlayerCards[i-39];
                    card.transform.DOLocalMove(leftPlayerPosition.localPosition, 1f).OnComplete(() => {
                        card.FlipCard(false);
                        card.gameObject.SetActive(false);
                    });
                }
            }
        }

        private void SortCards(Card[] arr) {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++) {
                for (int j = 0; j < n - i - 1; j++) {
                    if (arr[j].Value < arr[j + 1].Value) {
                        Card temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }

        private void ClickCard(Card card) {
            if (gameManager.runingPlayer == Player.Bottom) {
                if (gameManager.CheckBottonPlayerValidClick(card)) {
                    playerCardCounter--;
                    card.transform.parent = allCardParent;
                    gameManager.runingPlayer = Player.Right;
                    card.transform.DOLocalMove(cardOnBoard[0].localPosition, 0.5f).OnComplete(() => {
                        gameManager.AddPlayingCard(card, Player.Bottom);
                        playerCardPanel.sizeDelta = new Vector2(cardSize.sizeDelta.x * playerCardCounter, playerCardPanel.sizeDelta.y);
                        gameManager.PlayBotPlayer();
                        yourTurnTxt.SetActive(false);
                    });
                }
                else {
                    ShowInvalidCardTxt();
                }
            }
        }

        public void ClickCardByBot(Card card) {
            if (card == null) {
                return;
            }
            yourTurnTxt.SetActive(false);

            switch (gameManager.runingPlayer) {
                case Player.Right:
                    card.transform.DOLocalMove(cardOnBoard[1].localPosition, 0.5f).OnComplete(() => {
                        gameManager.AddPlayingCard(card, Player.Right);
                        gameManager.runingPlayer = Player.Top;
                        gameManager.PlayBotPlayer();
                    });
                break;
                case Player.Top:
                    card.transform.DOLocalMove(cardOnBoard[2].localPosition, 0.5f).OnComplete(() => {
                        gameManager.AddPlayingCard(card, Player.Top);
                        gameManager.runingPlayer = Player.Left;
                        gameManager.PlayBotPlayer();
                    });
                  
                 
                break;
                case Player.Left:
                    card.transform.DOLocalMove(cardOnBoard[3].localPosition, 0.5f).OnComplete(() => {
                        gameManager.AddPlayingCard(card, Player.Left);
                        gameManager.runingPlayer = Player.Bottom;
                        gameManager.PlayRealPlayer();
                    });
                  
                break;
            }
            card.gameObject.SetActive(true);
            card.FlipCard(true);
        }

        public void ShowYourTurnTxt(bool isShow) {
            yourTurnTxt.SetActive(isShow);
        }

        public void ShowInvalidCardTxt() {
            yourTurnTxt.SetActive(false);
            invalidCardTxt.SetActive(true);
            Invoke(nameof(HideInvalidCard), 0.7f);
        }

        private void HideInvalidCard() {
            invalidCardTxt.SetActive(false);
            yourTurnTxt.SetActive(true);
        }

        public void GameOver() {
            Invoke(nameof(ShowGameOverDelay), 1f);
        }

        private void ShowGameOverDelay() {
            gameOverObj.SetActive(true);
        }
    }
}

