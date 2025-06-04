using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] protected internal string initalStateNameType = "";
    [SerializeField] public string initialStateNameTypeV2 = "";
    [SerializeField] protected bool shearchChildren;
    //[SerializeField] private TextMeshProUGUI stateText;

    // Dictionary of state behaviours
    private Dictionary<Type, EntityStateBehaviour> StateBehaviour = new Dictionary<Type, EntityStateBehaviour>();
    
    //tracks the current state running in the StateBehaviour
    private EntityStateBehaviour currentState = null;

    //initialize the state machine and sets up the inicial state
    private void Start()
    {
        if (!InitializeStates())
        {
            // stop this class from executing
            this.enabled = false;
            return;
        }

        SetupInitialState();
    }

    private bool InitializeStates()
    {
        EntityStateBehaviour[] stateBehaviourComponents = GetComponents<EntityStateBehaviour>();
        for (int i = 0; i < stateBehaviourComponents.Length; ++i)
        {
            //check if the state already exist in the scene
            EntityStateBehaviour behaviourScript = stateBehaviourComponents[i];
            StateBehaviour.Add(behaviourScript.GetType(), behaviourScript);
        }

        if (shearchChildren)
        {
            //searches for the state behaviour components in children
            EntityStateBehaviour[] stateBehaviourChildrenComponents = GetComponentsInChildren<EntityStateBehaviour>();
            for (int i = 0; i < stateBehaviourComponents.Length; ++i)
            {
                EntityStateBehaviour behaviourScript = stateBehaviourChildrenComponents[i];
                StateBehaviour.Add(behaviourScript.GetType(), behaviourScript);
            }
        }
        // Initializes all the states, if it fails then this turns off till the person configuring this fixes it
        foreach (KeyValuePair<Type, EntityStateBehaviour> KeyValueState in StateBehaviour)
        {
            EntityStateBehaviour CurrentBehaviour = KeyValueState.Value;
            if (CurrentBehaviour.Initialize())
            {
                CurrentBehaviour.AssociatedStateMachine = this;
                continue;
            }

            Debug.Log($"StateMachine On {gameObject.name} has failed to initalize the state {CurrentBehaviour.GetType().Name}!");
            return false;
        }

        return true;
    }

    //Updates the current state and checks we can transition to a new state naturally
    private void Update()
    {
        currentState.OnStateUpdate();
        Type newState = currentState.StateTransitionCondicion();
        if (IsValidNewStateIndex(newState))
        {
            //if(stateText!=null)
            //stateText.text = newState.ToString();

            currentState.enabled = false;
            currentState = StateBehaviour[newState];
            currentState.enabled = true;
        }
    }
    /*private void FixedUpdate()
    {
        currentState.OnStateFixedUpdate();
    }*/

    // Function To help See If States Are The Same, Unused at the moment
    public bool IsCurrentState(EntityStateBehaviour stateBehaviour)
    {
        return currentState == stateBehaviour;
    }
    private void SetupInitialState()
    {
        Type InitialTypeSetup = Type.GetType(initialStateNameTypeV2);

        if (IsValidNewStateIndex(InitialTypeSetup))
        {
            currentState = StateBehaviour[InitialTypeSetup];
            currentState.enabled=true;
            return;
        }
        Debug.Log($"StateMachine On {gameObject.name} is has no state behaviours associated with it!");
    }
    public void SetState(Type StateKey)
    {
        if (IsValidNewStateIndex(StateKey))
        {
            currentState.enabled = false;
            currentState = StateBehaviour[StateKey];
            currentState.enabled = true;
        }
    }
    // Verify if the Index is Valid
    private bool IsValidNewStateIndex(Type StateKey)
    {
        if (StateKey == null)
            return false;

        return StateBehaviour.ContainsKey(StateKey);
    }
    // Gets The Current Running State
    public EntityStateBehaviour GetCurrentState()
    {
        return currentState;
    }
}
