using UnityEngine;
using UnityEngine.UI;

namespace CallBreak {
    public class Card : MonoBehaviour {
        [SerializeField] private CardSpriteRef cardRef = null;
        [SerializeField] private Image cardImg = null;

        public CardType type;
        public CardName cName;
        private int power;

        public void CreateCard(CardType _type, CardName _cName, bool isOpenFront = false) {
            type = _type;
            cName = _cName;
            cardImg.sprite = isOpenFront ? cardRef.GetCard(type, cName) : cardRef.GetBackground();
        }
    }
}
