using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class AreaColla : MonoBehaviour
{
	// Token: 0x060007F8 RID: 2040 RVA: 0x00119BB0 File Offset: 0x00117DB0
	private void Start()
	{
		this.suonoArea = base.GetComponent<AudioSource>();
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00119BC0 File Offset: 0x00117DC0
	private void Update()
	{
		if (base.GetComponent<PresenzaTrappola>().disposta)
		{
			this.EffettoColla();
		}
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00119BD8 File Offset: 0x00117DD8
	private void EffettoColla()
	{
		if (this.ListaAreaTrappola.Count > 0)
		{
			for (int i = 0; i < this.ListaAreaTrappola.Count; i++)
			{
				if (this.ListaAreaTrappola[i] == null && i < this.ListaAreaTrappola.Count - 1)
				{
					this.ListaAreaTrappola[i] = this.ListaAreaTrappola[i + 1];
					this.ListaAreaTrappola[i + 1] = null;
				}
			}
			for (int j = 0; j < this.ListaAreaTrappola.Count; j++)
			{
				if (this.ListaAreaTrappola[j] == null)
				{
					this.ListaAreaTrappola.RemoveAt(j);
				}
			}
			if (!this.partenzaSuono)
			{
				this.suonoArea.Play();
				this.partenzaSuono = true;
			}
			this.timerAutodannoColla += Time.deltaTime;
			foreach (GameObject current in this.ListaAreaTrappola)
			{
				if (current != null && current.GetComponent<NavigazioneConCamminata>())
				{
					current.GetComponent<NavigazioneConCamminata>().rallDaTrappola = true;
					current.GetComponent<NavigazioneConCamminata>().effettoRallDaTrappola = base.GetComponent<PresenzaTrappola>().percDiRallentamento;
					if (this.timerAutodannoColla > 1.5f)
					{
						base.GetComponent<PresenzaTrappola>().vita -= base.GetComponent<PresenzaTrappola>().dannoSubitoPerAzione;
					}
				}
			}
			if (this.timerAutodannoColla > 1.5f)
			{
				this.timerAutodannoColla = 0f;
			}
		}
		else
		{
			this.suonoArea.Stop();
			this.partenzaSuono = false;
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00119DC4 File Offset: 0x00117FC4
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaAreaTrappola.Contains(altri.gameObject))
		{
			this.ListaAreaTrappola.Add(altri.gameObject);
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00119E10 File Offset: 0x00118010
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaAreaTrappola.Contains(altri.gameObject))
		{
			this.ListaAreaTrappola.Remove(altri.gameObject);
			if (altri.gameObject != null && altri.gameObject.GetComponent<NavigazioneConCamminata>())
			{
				altri.gameObject.GetComponent<NavigazioneConCamminata>().rallDaTrappola = false;
			}
		}
	}

	// Token: 0x04001E04 RID: 7684
	public List<GameObject> ListaAreaTrappola;

	// Token: 0x04001E05 RID: 7685
	private AudioSource suonoArea;

	// Token: 0x04001E06 RID: 7686
	private bool partenzaSuono;

	// Token: 0x04001E07 RID: 7687
	private float timerAutodannoColla;
}
