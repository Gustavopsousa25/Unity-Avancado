#if UNITY_EDITOR

using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(StateMachine))]
public class StateMachineEditor : Editor
{
    private int Selected = 0;
    
    public override void OnInspectorGUI()
    {
        StateMachine StateMachineTargetComponent = (StateMachine) target;

        //DrawDefaultInspector();

        // Linq is the king of trash generation, so let's avoid it
        //IEnumerable<AStateBehaviour> GetAll()
        //{
        //    return AppDomain.CurrentDomain.GetAssemblies()
        //        .SelectMany(assembly => assembly.GetTypes())
        //        .Where(type => type.IsSubclassOf(typeof(AStateBehaviour)))
        //        .Select(type => Activator.CreateInstance(type) as AStateBehaviour);
        //}
        //GetAll().ToList().ForEach(x => AllTypesOf.Add(x.GetType().Name));

        EntityStateBehaviour[] Behaviours = StateMachineTargetComponent.GetComponents<EntityStateBehaviour>();
        List<string> AllTypesOf = new List<string>();
        for (int i = 0; i < Behaviours.Length; ++i)
        {
            AllTypesOf.Add(Behaviours[i].GetType().Name);

            if (Behaviours[i].GetType().Name == StateMachineTargetComponent.initialStateNameTypeV2)
                Selected = i;
        }
        
        EditorGUI.BeginChangeCheck();
       
        Selected = EditorGUILayout.Popup("Default State", Selected, AllTypesOf.ToArray());
        StateMachineTargetComponent.initialStateNameTypeV2 = AllTypesOf[Selected];

        EditorUtility.SetDirty(StateMachineTargetComponent);

        EditorGUI.EndChangeCheck();
    }
}

#endif