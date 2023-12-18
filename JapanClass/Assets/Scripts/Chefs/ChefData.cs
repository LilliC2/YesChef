using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChefData : GameBehaviour
{

    public ChefClass chefData;
    bool foundFood;
    GameObject currentFood;
    float elapsed = 1;
    [SerializeField]
    LayerMask rawFood;

    [Header("Audio")]
    [SerializeField]
    AudioSource cuttingAudio;
    AudioSource cookingAudio;
    AudioSource mixingAudio;
    AudioSource kneadingAudio;

    public GameObject rangeIndicator;

    public Animator anim;

    public bool validPos;

    [SerializeField]
    Collider[] rawFoodInRange;
    public bool placed;

    // Start is called before the first frame update
    void Start()
    {
        foundFood = false;
        //anim = GetComponentInChildren<Animator>();
        print(anim);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Physics.CheckSphere(transform.position, chefData.range, )

        //check if any raw food are in range
        rawFoodInRange = Physics.OverlapSphere(transform.position, chefData.range, rawFood);

        //check if placed
        if (placed)
        {
            //check if chef has compatible skill
            if (!foundFood)
            {
                anim.SetBool("Cooking", false);


                for (int i = 0; i < rawFoodInRange.Length; i++)
                {
                    currentFood = rawFoodInRange[i].gameObject;
                    if (chefData.kneedSkill && currentFood.GetComponent<FoodData>().foodData.needsKneading)
                    {
                        //print("I can kneed it");

                        foundFood = true;
                    }
                    else if (chefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
                    {
                        //print("I can cut it");

                        foundFood = true;
                    }
                    else if (chefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
                    {
                        //print("I can cook it");
                        foundFood = true;
                    }
                    else if (chefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
                    {
                        //print("I can mix it");

                        foundFood = true;
                    }
                    else
                    {
                        foundFood = false;
                    }

                }
            }
            //when food is found
            else
            {

                //look at food
                if (currentFood != null) transform.LookAt(currentFood.transform.position);
                //print("Found food i can cook");
                //every second, add skillPrepPoints to food skillPrepPoints

                if (currentFood != null && (rawFoodInRange.Contains(currentFood.gameObject.GetComponent<Collider>()) && currentFood.GetComponent<FoodData>().foodData.isCooked != true))
                {
                    //print("Cooking");
                    anim.SetBool("Cooking", true);

                    elapsed += Time.deltaTime;
                    if (elapsed >= 0.2f)
                    {

                        elapsed = elapsed % 0.2f;
                        //add prep points
                        //kneeding
                        if (chefData.kneedSkill)
                        {
                            if (kneadingAudio != null) kneadingAudio.Play();
                            currentFood.GetComponent<FoodData>().foodData.kneedPrepPoints += chefData.kneedEffectivness;
                        }
                        //cutting
                        if (chefData.cutSkill)
                        {
                            if (cuttingAudio != null) cuttingAudio.Play();
                            currentFood.GetComponent<FoodData>().foodData.cutPrepPoints += chefData.cutEffectivness;
                        }
                        //mixing
                        if (chefData.mixSkill)
                        {
                            if (mixingAudio != null) mixingAudio.Play();
                            currentFood.GetComponent<FoodData>().foodData.mixPrepPoints += chefData.mixEffectivness;
                        }

                        //cooking
                        if (chefData.cookSkill)
                        {
                            if (cookingAudio != null) cookingAudio.Play();
                            currentFood.GetComponent<FoodData>().foodData.cookPrepPoints += chefData.cookEffectivness;
                        }
                    }
                }
                else
                {
                    if (cookingAudio != null) cookingAudio.Stop();
                    if (kneadingAudio != null) kneadingAudio.Stop();
                    if (mixingAudio != null) mixingAudio.Stop();
                    if (cuttingAudio != null) cuttingAudio.Stop();

                    foundFood = false;
                    currentFood = null;
                }
            }
        }

        

    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, chefData.range);
    }

    private void OnMouseDown()
    {
        if(placed) _UI.OpenChefPopUp(this.gameObject);

    }
}
