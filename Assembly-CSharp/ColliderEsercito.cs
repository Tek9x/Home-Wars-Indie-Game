using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class ColliderEsercito : MonoBehaviour
{
	// Token: 0x0600083B RID: 2107 RVA: 0x00121178 File Offset: 0x0011F378
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.transform.parent.tag == "CentroStanza" && !this.ListaPosizioneEsercito.Contains(altri.transform.parent.gameObject))
		{
			this.ListaPosizioneEsercito.Add(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x001211E0 File Offset: 0x0011F3E0
	private void OnTriggerExit(Collider altri)
	{
		if (altri.transform.parent.tag == "CentroStanza" && this.ListaPosizioneEsercito.Contains(altri.transform.parent.gameObject))
		{
			this.ListaPosizioneEsercito.Remove(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x04001EBD RID: 7869
	public List<GameObject> ListaPosizioneEsercito;
}
