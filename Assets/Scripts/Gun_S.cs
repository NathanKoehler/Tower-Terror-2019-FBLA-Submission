using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun_S : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite weapon;
    public GameObject bullet;
    public RuntimeAnimatorController animation;
    public AudioClip firingSound;
    public string reloadSound;

    public float muzzleLocationX;
    public float muzzleLocationY;
    public GameObject childGameObject;

    public float reloadSpeed;
    public float fireRate;
    
    public int clipSize;
    public int bulletCount;
}
