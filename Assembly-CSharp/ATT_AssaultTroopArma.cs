using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class ATT_AssaultTroopArma : MonoBehaviour
{
	// Token: 0x06000154 RID: 340 RVA: 0x0003BF4C File Offset: 0x0003A14C
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaEffettoShotgun.Contains(altri.gameObject))
		{
			this.ListaEffettoShotgun.Add(altri.gameObject);
		}
		if (altri.tag == "Nemico Testa" && !this.ListaEffettoShotgun.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoShotgun.Add(altri.transform.parent.gameObject);
		}
		if (altri.tag == "Nemico Coll Suppl" && !this.ListaEffettoShotgun.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoShotgun.Add(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0003C038 File Offset: 0x0003A238
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaEffettoShotgun.Contains(altri.gameObject))
		{
			this.ListaEffettoShotgun.Remove(altri.gameObject);
		}
		if (altri.tag == "Nemico Testa" && this.ListaEffettoShotgun.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoShotgun.Remove(altri.transform.parent.gameObject);
		}
		if (altri.tag == "Nemico Coll Suppl" && this.ListaEffettoShotgun.Contains(altri.transform.parent.gameObject))
		{
			this.ListaEffettoShotgun.Remove(altri.transform.parent.gameObject);
		}
	}

	// Token: 0x04000653 RID: 1619
	public List<GameObject> ListaEffettoShotgun;
}
