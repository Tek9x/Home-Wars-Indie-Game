using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class ATT_ToxicCloudArma : MonoBehaviour
{
	// Token: 0x0600015A RID: 346 RVA: 0x0003C30C File Offset: 0x0003A50C
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaEffettoNube.Contains(altri.gameObject))
		{
			this.ListaEffettoNube.Add(altri.gameObject);
		}
		if (altri.tag == "Nemico Testa" && !this.ListaEffettoNube.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoNube.Add(altri.transform.parent.gameObject);
		}
		if (altri.tag == "Nemico Coll Suppl" && !this.ListaEffettoNube.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoNube.Add(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0003C3F8 File Offset: 0x0003A5F8
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaEffettoNube.Contains(altri.gameObject))
		{
			this.ListaEffettoNube.Remove(altri.gameObject);
		}
		if (altri.tag == "Nemico Testa" && this.ListaEffettoNube.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoNube.Add(altri.transform.parent.gameObject);
		}
		if (altri.tag == "Nemico Coll Suppl" && this.ListaEffettoNube.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoNube.Add(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x04000655 RID: 1621
	public List<GameObject> ListaEffettoNube;
}
