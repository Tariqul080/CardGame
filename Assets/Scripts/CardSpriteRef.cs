using UnityEngine;

namespace CallBreak {
    public enum CardType {
        Spade, Heart, Clover, Diamond 
    }

    public enum CardName{
       Card2 = 0, Card3, Card4, Card5, Card6, Card7, Card8, Card9, Card10, Jack, Queen, King , CardA
    }

    public class CardSpriteRef : MonoBehaviour {
        [SerializeField] private Sprite cardBack = null;

        [Header("All Cards. Note: Must according to CardName")]
        [SerializeField] private Sprite[] Spades = new Sprite[13];
        [SerializeField] private Sprite[] Hearts = new Sprite[13];
        [SerializeField] private Sprite[] Clovers = new Sprite[13];
        [SerializeField] private Sprite[] Diamonds = new Sprite[13];

        public Sprite GetCard(CardType type, CardName cName) {
            switch (type) {
                case CardType.Spade:
                    return Spades[(int)cName]; // cast enum to int
                case CardType.Heart:
                    return Hearts[(int)cName];
                case CardType.Clover:
                    return Clovers[(int)cName];
                default: // Diamond
                    return Diamonds[(int)cName];
            }
        }

        public Sprite GetBackground() {
            return cardBack;
        }
    }
}
