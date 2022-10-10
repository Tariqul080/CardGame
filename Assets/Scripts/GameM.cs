using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CallBreak
{
    public class GameM : MonoBehaviour
    {
        [SerializeField] private Card cardFeb = null;
        [SerializeField] private CardSpriteRef cardSR = null;
        [SerializeField] private GameObject allcard = null;

        public void CardMaker(){
            int cardtype = 0;
            for(int i = 0; i < 52; i++ )
            {
                Card cardPreFeb = Instantiate(cardFeb, allcard.transform);
                if(i<-1 && i < 13)
                {
                    

                    
                }
            }
            
        }

        private  void Awake() {
           
        }
    
    }
}
