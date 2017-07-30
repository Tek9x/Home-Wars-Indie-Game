using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class ColliderCentroStanza : MonoBehaviour
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x000ED764 File Offset: 0x000EB964
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaEsercitiInStanza.Contains(altri.gameObject))
		{
			this.ListaEsercitiInStanza.Add(altri.gameObject);
		}
		if (altri.tag == "Alleato" && !this.ListaEsercitiInStanza.Contains(altri.gameObject))
		{
			this.ListaEsercitiInStanza.Add(altri.gameObject);
		}
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x000ED7EC File Offset: 0x000EB9EC
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaEsercitiInStanza.Contains(altri.gameObject))
		{
			this.ListaEsercitiInStanza.Remove(altri.gameObject);
		}
		if (altri.tag == "Alleato" && this.ListaEsercitiInStanza.Contains(altri.gameObject))
		{
			this.ListaEsercitiInStanza.Remove(altri.gameObject);
		}
	}

	// Token: 0x040018FC RID: 6396
	public List<GameObject> ListaEsercitiInStanza;
}
