using UnityEngine;

namespace CallBreak {
    public class UIManager : MonoBehaviour {

        [Header("Card Init")]
        [SerializeField] private Card cardFeb = null;
        [SerializeField] private Transform allCardParent = null;
        [SerializeField] private GameObject playButton = null;
        [SerializeField] private GameObject allPlayerInformation = null; 
        
        [Header("Card Pool")] 
        [SerializeField] private RectTransform cardSize = null;
        [SerializeField] private RectTransform playerCardPanel = null;

        private Card[] allCards = new Card[52];
        
        public void ClickPlayButton() {
            allPlayerInformation.SetActive(true);
            playButton.SetActive(false);
            CardMaker();
            Invoke(nameof(ShortPlayerCards), 1f);
        }

        private void CardMaker() {
            int cardType = 0, cardIndex = 0;
            for(int i = 0; i < 52 ; i++) {
                Card card = Instantiate(cardFeb,allCardParent);

                card.CreateCard((CardType)cardType, (CardName)cardIndex, true);
                card.transform.name = ((CardType)cardType).ToString() + cardIndex;
                cardIndex++;

                if (cardIndex == 13) {
                    cardIndex = 0;
                    cardType++;
                }
                allCards[i] = card;
            }
        }

        private void ShortPlayerCards() {
            for(int i = 0; i < 13 ; i++) {
                Card card = allCards[i];
                card.transform.parent = playerCardPanel;
            }
            playerCardPanel.sizeDelta = new Vector2(cardSize.sizeDelta.x * 13f, playerCardPanel.sizeDelta.y);
        }
    }
}

