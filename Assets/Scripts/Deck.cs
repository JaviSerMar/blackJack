using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text PointsText;
    
    public Text DealerPointsText;
    public Dropdown Apuesta;
    
    public Text CreditsText;
    public float Credits = 100;
    public float dinerillo;

     public float totalcartas;
    public float cartasm21;
    public float cartas1721;
    public float dmp;


    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

  
    private void Start()
    {
        ShuffleCards();
        hitButton.interactable = false;
        stickButton.interactable = false;   
        CreditsText.text = "Credits: " + Credits;
    }

    private void InitCardValues()
    {
         
        for(int i=0; i<faces.Length;i++)
        {
            int InitCardValues = (i%13)+1;
            if(InitCardValues>10)
            {
                InitCardValues = 10;

            }
            if(InitCardValues == 1)
            {
                InitCardValues = 11;

            }
            values[i] = InitCardValues;
        }

    }

    private void ShuffleCards()
    {
        
        for(int i=0; i<faces.Length;i++)
        {
            int j = Random.Range(0,faces.Length-1);
            Sprite tempFace = faces[i];
            faces[i] = faces[j];
            faces[j] = tempFace;

            int tempValue = values[i];
            values[i] = values[j];
            values[j] = tempValue;
        }


    }
    
    void StartGame()
    {
        Bet();
        CreditsText.text = "Credits: " + Credits;
        playAgainButton.interactable = false;
        Apuesta.interactable = false;
        DealerPointsText.text = "Points:";
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
             if(player.GetComponent<CardHand>().points == 21)
             {
                finalMessage.text = "VAMOOOOS HAS GANADO";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Points: " +dealer.GetComponent<CardHand>().points; 
                Credits = Credits + (dinerillo*2);
                CreditsText.text = "Credits: " + Credits;
                hitButton.interactable = false;
                stickButton.interactable = false;
                playAgainButton.interactable = true;
                Apuesta.interactable = true;
             }
             if(dealer.GetComponent<CardHand>().points == 21)
             {
                finalMessage.text = "NOOOOOO HAS PERDIDO!!!!";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Points: " +dealer.GetComponent<CardHand>().points;  
                hitButton.interactable = false;
                stickButton.interactable = false;
                playAgainButton.interactable = true;
                Apuesta.interactable = true;
             }
             if(dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points == 21)
             {
                finalMessage.text = "EMPATE TÉCNICO";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Points: " +dealer.GetComponent<CardHand>().points;  
                 Credits = Credits + dinerillo;
                CreditsText.text = "Credits: " + Credits;
                hitButton.interactable = false;
                stickButton.interactable = false;
                playAgainButton.interactable = true;
                Apuesta.interactable = true;
             }
        }
        PointsText.text = "Points: " +player.GetComponent<CardHand>().points; 
    }

    private void CalculateProbabilities()
    {
       
         totalcartas=0;
         float puntoplayer = player.GetComponent<CardHand>().points;
            cartasm21=0;
            cartas1721=0;
            dmp=0;

    for(int i = cardIndex+1; i < faces.Length; i++)
    {
        totalcartas++;

         if(values[3] + values[i] > puntoplayer)
        {
            dmp++;
        }


        if( values[i] == 11 && values[i] + puntoplayer > 21)
        {
            if(values[i] == 11)
            {
                values[i] = 1;
            }
        }



        if( values[i] + puntoplayer >= 17 && values[i] + puntoplayer <= 21 )
        {
            cartas1721++;  
        }


        if( values[i] + puntoplayer > 21)
        {
            cartasm21++;  
        }
    }
    
        probMessage.text =
        "Deal > Play: " + dmp/totalcartas + "\n" +
        "17 <= X <= 21: " + cartas1721/totalcartas + "\n" +
        "21 > X: " + cartasm21/totalcartas;

    }

    void PushDealer()
    {
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        PushPlayer();
        PointsText.text = "Ponits: " +player.GetComponent<CardHand>().points;     
        if(player.GetComponent<CardHand>().points>21)
        {
            finalMessage.text = "NOOOOOO HAS PERDIDO!!!!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
            DealerPointsText.text = "Ponits: " +dealer.GetComponent<CardHand>().points;  
        }
        if(player.GetComponent<CardHand>().points == 21)
        {
           finalMessage.text = "OLEEE AHIIIIII, HE GANAO";
           hitButton.interactable = false;
           stickButton.interactable = false;
           playAgainButton.interactable = true;
           Apuesta.interactable = true;
           DealerPointsText.text = "Ponits: " +dealer.GetComponent<CardHand>().points;  
            Credits = Credits + (dinerillo*2);
            CreditsText.text = "Credits: " + Credits;
        }
    }
    

    public void Stand()
    {
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        hitButton.interactable = false;
        stickButton.interactable = false;

        if(dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }


        DealerPointsText.text = "Ponits: " +dealer.GetComponent<CardHand>().points;  
        int PlayerPoints = player.GetComponent<CardHand>().points;
        int DealerPoints = dealer.GetComponent<CardHand>().points;
        

        if(DealerPoints == 21)
        {
            finalMessage.text = "NOOOOOO HAS PERDIDO!!!!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
        }
        if(DealerPoints > PlayerPoints)
        {
            finalMessage.text = "NOOOOOO HAS PERDIDO!!!!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
            
        }
         if(PlayerPoints > DealerPoints)
        {
            finalMessage.text = "VAMOOOOS HAS GANADO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            Apuesta.interactable = true;
            playAgainButton.interactable = true;
            Credits = Credits + (dinerillo*2);
            CreditsText.text = "Credits: " + Credits;
             
        }
         if(DealerPoints == PlayerPoints)
        {
            finalMessage.text = "EMPATE TÉCNICO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
            Credits = Credits + dinerillo;
            CreditsText.text = "Credits: " + Credits;
        
        }
        if(DealerPoints > 21)
        {
            finalMessage.text = "VAMOOOOS HAS GANADO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
            Credits = Credits + (dinerillo*2);
            CreditsText.text = "Credits: " + Credits;
        }            
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    public void Bet()
    {
       switch(Apuesta.value) 
       {
        case 0:
            Credits=Credits-10;
            CreditsText.text = "Credits: " + Credits;
            dinerillo = 10;
            break;
       
        case 1:
            Credits=Credits-20;
            CreditsText.text = "Credits: " + Credits;
            dinerillo = 20;
            break;
       
        case 2:
            Credits=Credits-50;
            CreditsText.text = "Credits: " + Credits;
            dinerillo = 50;
            break;
       
        case 3:
            Credits=Credits-100;
            dinerillo = 100;
            CreditsText.text = "Credits: " + Credits;
            break;
       }

    }

    public void ONOFF()
    {
        switch(Apuesta.value)
        {
            case 0:
                if(Credits < 10)
                {
                    playAgainButton.interactable = false;
                }
                else
                {
                    playAgainButton.interactable = true;
                }
            break;
             case 1:
                if(Credits < 20)
                {
                    playAgainButton.interactable = false;
                }
                 else
                {
                    playAgainButton.interactable = true;
                }
            break;
             case 2:
                if(Credits < 50)
                {
                    playAgainButton.interactable = false;
                }
                 else
                {
                    playAgainButton.interactable = true;
                }
            break;
             case 3:
                if(Credits < 100)
                {
                    playAgainButton.interactable = false;
                }
                 else
                {
                    playAgainButton.interactable = true;
                }
            break;
        }
    }
    
}
