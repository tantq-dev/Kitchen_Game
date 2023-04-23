using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successRecipeAmount = 0;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There one more instance of this component");

        }
        else
        {
            Instance= this;
        }
        waitingRecipeSOList= new List<RecipeSO>();
    }
    private void Start()
    {
        spawnRecipeTimer = spawnRecipeTimerMax;

    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer<=0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (GameManager.Instance.IsGamePlaying()&&waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.name);
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }
    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach(RecipeSO waitingRecipeSO in waitingRecipeSOList)
        {
            if(waitingRecipeSO.kitchenObjectList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatcheRecipe = true;

                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectList){
                    bool ingredientFound = false;

                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound=true;

                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatcheRecipe = false;

                    }
                }
                if(plateContentsMatcheRecipe)
                {

                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    successRecipeAmount += 1;
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetSuccesRecipeAmount()
    {
        return successRecipeAmount;
    }

}
