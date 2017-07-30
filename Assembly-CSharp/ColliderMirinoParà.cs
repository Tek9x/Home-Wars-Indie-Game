using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class ColliderMirinoParà : MonoBehaviour
{
	// Token: 0x0600015D RID: 349 RVA: 0x0003C4EC File Offset: 0x0003A6EC
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Ambiente" && !this.ListaAmbienteToccato.Contains(altri.gameObject))
		{
			this.ListaAmbienteToccato.Add(altri.gameObject);
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0003C538 File Offset: 0x0003A738
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Ambiente" && this.ListaAmbienteToccato.Contains(altri.gameObject))
		{
			this.ListaAmbienteToccato.Remove(altri.gameObject);
		}
	}

	// Token: 0x04000656 RID: 1622
	public List<GameObject> ListaAmbienteToccato;
}
