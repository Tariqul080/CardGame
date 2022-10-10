using UnityEngine;
namespace CallBreak {
    public class UIManager : MonoBehaviour {
        [SerializeField] private Card cardFeb = null;
        [SerializeField] private Transform allCardParent = null;
        [SerializeField] private GameObject playButton = null;
        [SerializeField] private GameObject allPlayerInformation = null; 

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
             
            }
        }

        public void ClickPlayButton() {
            allPlayerInformation.SetActive(true);
            playButton.SetActive(false);
            CardMaker();
        }
       
    }
}

