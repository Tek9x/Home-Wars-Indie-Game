using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class ScalataCollider : MonoBehaviour
{
	// Token: 0x06000505 RID: 1285 RVA: 0x000AC6C0 File Offset: 0x000AA8C0
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.transform.parent.tag == "Ambiente" && !this.ListaPerScalata.Contains(altri.transform.parent.gameObject))
		{
			this.ListaPerScalata.Add(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x000AC728 File Offset: 0x000AA928
	private void OnTriggerExit(Collider altri)
	{
		if (altri.transform.parent.tag == "Ambiente" && this.ListaPerScalata.Contains(altri.transform.parent.gameObject))
		{
			this.ListaPerScalata.Remove(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x040012FA RID: 4858
	public List<GameObject> ListaPerScalata;
}
