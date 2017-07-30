using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class ColliderObbiettivo : MonoBehaviour
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x0011876C File Offset: 0x0011696C
	private void Start()
	{
		this.ListaTempObb = new List<GameObject>();
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0011877C File Offset: 0x0011697C
	private void Update()
	{
		for (int i = 0; i < this.ListaUnitàInObbiettivo.Count; i++)
		{
			if (this.ListaUnitàInObbiettivo[i] == null)
			{
				for (int j = i; j < this.ListaUnitàInObbiettivo.Count; j++)
				{
					if (this.ListaUnitàInObbiettivo[j] != null)
					{
						this.ListaUnitàInObbiettivo[i] = this.ListaUnitàInObbiettivo[j];
						this.ListaUnitàInObbiettivo[j] = null;
					}
				}
			}
		}
		for (int k = 0; k < this.ListaUnitàInObbiettivo.Count; k++)
		{
			if (this.ListaUnitàInObbiettivo[k] == null)
			{
				for (int l = this.ListaUnitàInObbiettivo.Count - 1; l >= k; l--)
				{
					this.ListaUnitàInObbiettivo.Remove(this.ListaUnitàInObbiettivo[l]);
				}
			}
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00118880 File Offset: 0x00116A80
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaUnitàInObbiettivo.Contains(altri.gameObject))
		{
			this.ListaUnitàInObbiettivo.Add(altri.gameObject);
		}
		if (altri.tag == "Alleato" && !this.ListaUnitàInObbiettivo.Contains(altri.gameObject))
		{
			this.ListaUnitàInObbiettivo.Add(altri.gameObject);
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00118908 File Offset: 0x00116B08
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaUnitàInObbiettivo.Contains(altri.gameObject))
		{
			this.ListaUnitàInObbiettivo.Remove(altri.gameObject);
		}
		if (altri.tag == "Alleato" && this.ListaUnitàInObbiettivo.Contains(altri.gameObject))
		{
			this.ListaUnitàInObbiettivo.Remove(altri.gameObject);
		}
	}

	// Token: 0x04001DD2 RID: 7634
	private List<GameObject> ListaTempObb;

	// Token: 0x04001DD3 RID: 7635
	public List<GameObject> ListaUnitàInObbiettivo;
}
