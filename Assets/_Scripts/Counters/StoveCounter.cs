using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgess
{

    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public event EventHandler<IHasProgess.OnProgressChangeEventArgs> OnProgressChange;

    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fryied,
        Burned,
    }
    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private float burningTimer;

    private State state;


    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {

        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChange?.Invoke(this, new IHasProgess.OnProgressChangeEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        fryingTimer = 0f;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burningTimer = 0f;
                        state = State.Fryied;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });


                    }

                    break;
                case State.Fryied:
                    burningTimer += Time.deltaTime;
                    OnProgressChange?.Invoke(this, new IHasProgess.OnProgressChangeEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;
                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });
                        OnProgressChange?.Invoke(this, new IHasProgess.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0
                        });

                    }
                    break;
                case State.Burned:
                    break;
                default:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!this.HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                    {
                        state = state
                    });
                    OnProgressChange?.Invoke(this, new IHasProgess.OnProgressChangeEventArgs
                    {
                        progressNormalized = fryingTimer/fryingRecipeSO.fryingTimerMax
                    });

                    fryingTimer = 0f;
                }
            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;
                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });
                        OnProgressChange?.Invoke(this, new IHasProgess.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0
                        });
                    };
                }

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                {
                    state = state
                });
                OnProgressChange?.Invoke(this, new IHasProgess.OnProgressChangeEventArgs
                {
                    progressNormalized = 0
                });
            }
        }
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {

            return fryingRecipeSO.output;
        }
        else
        {
            return null;

        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;

    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;

    }
    public bool IsFryied()
    {
        return state == State.Fryied;
    }
}
