using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class ColliderAreaEffetto : MonoBehaviour
{
	// Token: 0x06000571 RID: 1393 RVA: 0x000B2A30 File Offset: 0x000B0C30
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Add(altri.gameObject);
		}
		else if (altri.tag == "ObbiettivoTattico" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Add(altri.gameObject);
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x000B2ABC File Offset: 0x000B0CBC
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Remove(altri.gameObject);
		}
		if (altri.tag == "ObbiettivoTattico" && this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Remove(altri.gameObject);
		}
	}

	// Token: 0x04001490 RID: 5264
	public List<GameObject> ListaAeraEffetto;
}
