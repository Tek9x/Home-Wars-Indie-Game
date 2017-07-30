using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class ColliderAreaTrappole : MonoBehaviour
{
	// Token: 0x0600080A RID: 2058 RVA: 0x0011A5B0 File Offset: 0x001187B0
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaAreaTrappola.Contains(altri.gameObject))
		{
			this.ListaAreaTrappola.Add(altri.gameObject);
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0011A5FC File Offset: 0x001187FC
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaAreaTrappola.Contains(altri.gameObject))
		{
			this.ListaAreaTrappola.Remove(altri.gameObject);
		}
	}

	// Token: 0x04001E11 RID: 7697
	public List<GameObject> ListaAreaTrappola;
}
