using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class ColliderAereaAttNemici : MonoBehaviour
{
	// Token: 0x06000795 RID: 1941 RVA: 0x0010EEA0 File Offset: 0x0010D0A0
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Alleato" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Add(altri.gameObject);
		}
		else if (altri.tag == "Trappola" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Add(altri.gameObject);
		}
		else if (altri.name == "Avamposto Alleato(Clone)" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Add(altri.gameObject);
		}
		else if (altri.name == "Cassa Supply(Clone)" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Add(altri.gameObject);
		}
		else if (altri.name == "Camion per Convoglio(Clone)" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Add(altri.gameObject);
		}
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0010EFF0 File Offset: 0x0010D1F0
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Alleato" && this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Remove(altri.gameObject);
		}
		if (altri.tag == "Trappola" && this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Remove(altri.gameObject);
		}
		else if (altri.name == "Avamposto Alleato(Clone)" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Remove(altri.gameObject);
		}
		else if (altri.name == "Cassa Supply(Clone)" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Remove(altri.gameObject);
		}
		else if (altri.name == "Camion per Convoglio(Clone)" && !this.ListaAeraEffetto.Contains(altri.gameObject))
		{
			this.ListaAeraEffetto.Remove(altri.gameObject);
		}
	}

	// Token: 0x04001C4B RID: 7243
	public List<GameObject> ListaAeraEffetto;
}
