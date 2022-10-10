
using UnityEngine;
namespace CallBreak
{
    public class PlayScript : MonoBehaviour
    {
        [SerializeField] private GameObject AllPlayerProfile = null;
        [SerializeField] private GameM GameMenegerRef = null;
        [SerializeField] private GameObject Playbutton = null;


        public void ClickedPlayButton(){
            AllPlayerProfile.SetActive(true);
            Playbutton.SetActive(false);
            GameMenegerRef.CardMaker();
        }

        
    }
}
