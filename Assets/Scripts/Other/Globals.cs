using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class Globals : MonoBehaviour
{
    public ShipMain playerShip;
    public VirtualMouse virtualMouse;
    public PlayerCamera playerCamera;

    public Transform projectileSpawns;

    public SerializableDictionaryBase<string, LayerMask> layerMasks;

    public Assets assets;
}
