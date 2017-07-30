using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class OffMeshCollegamento : MonoBehaviour
{
	// Token: 0x060006B7 RID: 1719 RVA: 0x000ED87C File Offset: 0x000EBA7C
	private void Start()
	{
		this.ListaElementi = new List<GameObject>();
		this.collegamento = this.linkPartenza.GetComponent<OffMeshLink>();
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x000ED89C File Offset: 0x000EBA9C
	private void Update()
	{
		if (this.ListaElementi.Count <= 0)
		{
			this.collegamento.activated = true;
		}
		else
		{
			this.collegamento.activated = false;
		}
		Debug.Log(this.ListaElementi.Count);
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x000ED8EC File Offset: 0x000EBAEC
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Alleato")
		{
			this.ListaElementi.Add(altri.gameObject);
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x000ED920 File Offset: 0x000EBB20
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Alleato")
		{
			this.ListaElementi.Remove(altri.gameObject);
		}
	}

	// Token: 0x040018FD RID: 6397
	public GameObject linkPartenza;

	// Token: 0x040018FE RID: 6398
	private List<GameObject> ListaElementi;

	// Token: 0x040018FF RID: 6399
	private OffMeshLink collegamento;
}
