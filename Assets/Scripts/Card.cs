using System.Diagnostics;
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
        private int cardValue; // 0 - 52

        public int Value {
            get {return cardValue;}
        }

        private void SetValue() {
            int typeValue;
            switch (type) {
                case CardType.Spade:
                    typeValue = 39;
                break;
                case CardType.Diamond:
                    typeValue = 26;
                break;
                case CardType.Clover:
                    typeValue = 13;
                break;
                default:
                    typeValue = 0;
                break;
            }

            cardValue = typeValue + (int)cName;
        }

        public void CreateCard(CardType _type, CardName _cName, bool isOpenFront = false) {
            type = _type;
            cName = _cName;
            cardImg.sprite = isOpenFront ? cardRef.GetCard(type, cName) : cardRef.GetBackground();
            SetValue();
        }

        public void FlipCard(bool isFront) {
            cardImg.sprite = isFront ? cardRef.GetCard(type, cName) : cardRef.GetBackground();
        }

        public void ClickCard() {
            Click?.Invoke(this);
        }
    }
}
