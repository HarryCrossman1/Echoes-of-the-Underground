using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractor : MonoBehaviour
{
    private Renderer BaseMat;
    // Start is called before the first frame update
    void Start()
    {
        BaseMat = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnterHoverColorChange()
    { 
        BaseMat.material.color = Color.white;
    }
    public void ExitHoverColorChange() 
    {
        BaseMat.material.color = Color.red;
    }
    public void ButtonPressed()
    {
        if(StoryManager.instance !=null)
        StoryManager.instance.PressedButtonName = gameObject.name;
    }
}
