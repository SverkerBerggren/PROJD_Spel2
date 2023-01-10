using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Dialogs;

public class controllerTutorial : MonoBehaviour
{
      
    
    public void StepTwo(){
        
        uDialog.NewDialog()
                .SetDimensions(900,400)
                .SetThemeImageSet(eThemeImageSet.Fantasy)
                .SetColorScheme("Personalized")
                .SetTitleText("Starting the Game")
                .SetShowTitleCloseButton(true)
                .SetContentText("<b>You begin with choosing which cards(a so called Mulligan) and champion you want to start with</b>")
                .SetCloseWhenAnyButtonClicked(false)
                .SetAllowDraggingViaTitle(true)
                .SetAllowDraggingViaDialog(true)
                .AddButton("CONTINUE", (dialog) =>{StepThree(dialog);})
                .AddButton("CLOSE",(dialog)=>{dialog.Close();});

        
    }

    void StepThree(uDialog dialog)
    {
        dialog.Close();
        uDialog.NewDialog()
                .SetDimensions(900,400)
                .SetThemeImageSet(eThemeImageSet.Fantasy)
                .SetColorScheme("Personalized")
                .SetTitleText("Deck")
                .SetShowTitleCloseButton(true)
                .SetContentText("<b>Your deck contains a total of 40 cards. You will start with 5 cards on your hand and at the start of every turn you will get one more. Your hand can only contain a maximum of 10 cards. You can look at your Deck and the amount of cards that are left by hovering your mouse over it. The cards that you or your opponent have played will end up in the Graveyard which you can look at hovering over it.</b>")
                .SetCloseWhenAnyButtonClicked(false)
                .SetAllowDraggingViaTitle(true)
                .SetAllowDraggingViaDialog(true)  
                .AddButton("BACK",(dialog)=>{StepTwo();})
                .AddButton("CONTINUE", (dialog) =>{StepFour(dialog);})
                .AddButton("CLOSE",(dialog)=>{dialog.Close();});
    }
    
    void StepFour(uDialog dialog)
    {
        dialog.Close();
        uDialog.NewDialog()
                .SetDimensions(900,400)
                .SetThemeImageSet(eThemeImageSet.Fantasy)
                .SetColorScheme("Personalized")
                .SetTitleText("Mana")
                .SetShowTitleCloseButton(true)
                .SetContentText("<b>This is your Mana, it is the cost  to play your cards. Mana refills and increases by one after every round (up to a maximum of 10). Your mana on your cards is on the upper left corner of your cards which indicates how much mana you will need to spend on this card</b>")
                .SetCloseWhenAnyButtonClicked(false)
                .SetAllowDraggingViaTitle(true)
                .SetAllowDraggingViaDialog(true)  
                .AddButton("BACK",(dialog)=>{StepThree(dialog);})
                .AddButton("CONTINUE", (dialog) =>{StepFive(dialog);})
                .AddButton("CLOSE",(dialog)=>{dialog.Close();});
    }
    
    void StepFive(uDialog dialog)
    {
        dialog.Close();
        uDialog.NewDialog()
                .SetDimensions(900,400)
                .SetThemeImageSet(eThemeImageSet.Fantasy)
                .SetColorScheme("Personalized")
                .SetTitleText("Champions")
                .SetShowTitleCloseButton(true)
                .SetContentText("<b>You start with three champions but only one champion is active at a time. You can see your active champion on the bottom right of your screen. You can also se your opponents active champion on the upper left corner.  Your passive champions will appear above your active one. </b>")
                .SetCloseWhenAnyButtonClicked(false)
                .SetAllowDraggingViaTitle(true)
                .SetAllowDraggingViaDialog(true)  
                .AddButton("BACK",(dialog)=>{StepFour(dialog);})
                .AddButton("CONTINUE", (dialog) =>{StepSix(dialog);})
                .AddButton("CLOSE",(dialog)=>{dialog.Close();});
    }

    void StepSix(uDialog dialog)
    {
        dialog.Close();
        uDialog.NewDialog()
                .SetDimensions(900,400)
                .SetThemeImageSet(eThemeImageSet.Fantasy)
                .SetColorScheme("Personalized")
                .SetTitleText("Health")
                .SetShowTitleCloseButton(true)
                .SetContentText("<b>All of your champions have a health bar beside their character image. When the health bar reaches zero, your champion will be defeated. You can choose your next active champion, when all champions are defeated, you lose the match.</b>")
                .SetCloseWhenAnyButtonClicked(false)
                .SetAllowDraggingViaTitle(true)
                .SetAllowDraggingViaDialog(true)  
                .AddButton("BACK",(dialog)=>{StepFive(dialog);})
                .AddButton("CONTINUE", (dialog) =>{StepSeven(dialog);})
                .AddButton("CLOSE",(dialog)=>{dialog.Close();});
    }

    void StepSeven(uDialog dialog)
    {
        dialog.Close();
        uDialog.NewDialog()
                .SetDimensions(900,400)
                .SetThemeImageSet(eThemeImageSet.Fantasy)
                .SetColorScheme("Personalized")
                .SetTitleText("Landmarks")
                .SetShowTitleCloseButton(true)
                .SetContentText("<b>There are cards called Landmarks, when used they will be put out on their placements in the arena. You can have up to four landmarks in play at the same time.</b>")
                .SetCloseWhenAnyButtonClicked(false)
                .SetAllowDraggingViaTitle(true)
                .SetAllowDraggingViaDialog(true)  
                .AddButton("BACK",(dialog)=>{StepSix(dialog);})
                .AddButton("CONTINUE", (dialog) =>{StepEigth(dialog);})
                .AddButton("CLOSE",(dialog)=>{dialog.Close();});
    }

    void StepEigth(uDialog dialog)
    {
        dialog.Close();
        uDialog.NewDialog()
                .SetDimensions(900,400)
                .SetThemeImageSet(eThemeImageSet.Fantasy)
                .SetColorScheme("Personalized")
                .SetTitleText("Icons")
                .SetShowTitleCloseButton(true)
                .SetContentText("<b>There is a Round counter to keep track of which round you are on. After you are done with your turn you click on the End Turn button. You will find the Settings menu on the cogwheel icon.</b>")
                .SetCloseWhenAnyButtonClicked(false)
                .SetAllowDraggingViaTitle(true)
                .SetAllowDraggingViaDialog(true)  
                .AddButton("BACK",(dialog)=>{StepSeven(dialog);})
                .AddButton("END TUTORIAL",(dialog)=>{dialog.Close();});
    }
}
