using UnityEngine;
using UnityEngine.UI;
using System;

namespace CallBreak {
    public class Card : MonoBehaviour {
        [SerializeField] private CardSpriteRef cardRef = null;
        [SerializeField] private Image cardImg = null;

        public Action<Card> Click = null;

        public CardType type;
        public CardName cName;
        private int power;

        public void CreateCard(CardType _type, CardName _cName, bool isOpenFront = false) {
            type = _type;
            cName = _cName;
            cardImg.sprite = isOpenFront ? cardRef.GetCard(type, cName) : cardRef.GetBackground();
        }

        public void FlipCard(bool isFront) {
            cardImg.sprite = isFront ? cardRef.GetCard(type, cName) : cardRef.GetBackground();
        }

        public void ClickCard() {
            Click?.Invoke(this);
        }
    }
}
