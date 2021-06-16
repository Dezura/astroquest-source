using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class Globals : MonoBehaviour
{
    public float globalVolume;

    public TimerAndScore timerAndScore;

    public ShipMain playerShip;
    public VirtualMouse virtualMouse;
    public PlayerCamera playerCamera;

    public Transform enemySpawn;
    public Transform projectileSpawn;
    
    public GameMenu gameMenu;

    public SerializableDictionaryBase<string, LayerMask> layerMasks;

    public Assets assets;
}
