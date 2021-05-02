using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
   public void GravitySwitch(Vector3 gravityDirection,float gravityPower)
   {
      Physics.gravity = gravityDirection * gravityPower;
   }
}
