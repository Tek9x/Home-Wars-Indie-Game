using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class ATT_FlamethrowerArma : MonoBehaviour
{
	// Token: 0x06000157 RID: 343 RVA: 0x0003C12C File Offset: 0x0003A32C
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaEffettoFiamma.Contains(altri.gameObject))
		{
			this.ListaEffettoFiamma.Add(altri.gameObject);
		}
		if (altri.tag == "Nemico Testa" && !this.ListaEffettoFiamma.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoFiamma.Add(altri.transform.parent.gameObject);
		}
		if (altri.tag == "Nemico Coll Suppl" && !this.ListaEffettoFiamma.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoFiamma.Add(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0003C218 File Offset: 0x0003A418
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaEffettoFiamma.Contains(altri.gameObject))
		{
			this.ListaEffettoFiamma.Remove(altri.gameObject);
		}
		if (altri.tag == "Nemico Testa" && this.ListaEffettoFiamma.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoFiamma.Remove(altri.transform.parent.gameObject);
		}
		if (altri.tag == "Nemico Coll Suppl" && this.ListaEffettoFiamma.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoFiamma.Remove(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x04000654 RID: 1620
	public List<GameObject> ListaEffettoFiamma;
}
