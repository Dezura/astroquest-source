using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// This script just adds special functionality to the editor inspector of CustomizableGun
[CustomEditor(typeof(CustomizableGun))]
public class CustomizableGunEditor : Editor
{
    CustomizableGun targetScript;

    string[] currentGunParams;

	override public void OnInspectorGUI()
	{
		targetScript = target as CustomizableGun;

        serializedObject.Update();

        EditorGUILayout.Separator();
        serializedObject.FindProperty("gunType").enumValueIndex = (int)(CustomizableGun.gunEnum)EditorGUILayout.EnumPopup("Gun Type", targetScript.gunType);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("shootPoints"), true);

        EditorGUILayout.Separator();

        // This is pretty unefficient, but since its an editor script I don't think it should affect the game performance?
		switch ((int) targetScript.gunType)
        {
            case 0: // Blaster
                currentGunParams = new string[] {
                    "projectileType",
                    "autofire",
                    "damage",
                    "firerate",
                    "projectileSize",
                    "projectileSpeed",
                    "projectileLifespan"
                };
                
                break;
            case 1: // Shotgun
                currentGunParams = new string[] {
                    "projectileType",
                    "autofire",
                    "damage",
                    "firerate",
                    "projectileSize",
                    "projectileSpeed",
                    "projectileLifespan",
                    "multishot"
                };

                break;
            case 2: // Laser
                currentGunParams = new string[] {
                };

                break;
            case 3: // Missile
                currentGunParams = new string[] {
                };

                break;
            default:
                break;
        }

        EditorGUILayout.LabelField("Gun Params", EditorStyles.boldLabel);
        // Draw each param from customGunParams
        foreach (string param in currentGunParams) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(param), true);
        }
		
        serializedObject.ApplyModifiedProperties();
	}
}