using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class AreaTrappola : MonoBehaviour
{
	// Token: 0x06000804 RID: 2052 RVA: 0x0011A210 File Offset: 0x00118410
	private void Start()
	{
		this.suonoArea = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0011A220 File Offset: 0x00118420
	private void Update()
	{
		if (base.GetComponent<PresenzaTrappola>().disposta)
		{
			this.EffettoRallentamento();
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0011A238 File Offset: 0x00118438
	private void EffettoRallentamento()
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
			this.timerAutodannoTrappola += Time.deltaTime;
			if (base.GetComponent<PresenzaTrappola>().tipoTrappola == 2)
			{
				foreach (GameObject current in this.ListaAreaTrappola)
				{
					if (current != null && current.GetComponent<PresenzaNemico>().insettoVolante && this.timerAutodannoTrappola > 1.5f)
					{
						base.GetComponent<PresenzaTrappola>().vita -= base.GetComponent<PresenzaTrappola>().dannoSubitoPerAzione;
					}
				}
			}
			else
			{
				foreach (GameObject current2 in this.ListaAreaTrappola)
				{
					if (current2 != null && current2.GetComponent<NavigazioneConCamminata>())
					{
						current2.GetComponent<NavigazioneConCamminata>().rallDaTrappola = true;
						current2.GetComponent<NavigazioneConCamminata>().effettoRallDaTrappola = base.GetComponent<PresenzaTrappola>().percDiRallentamento;
						if (this.timerAutodannoTrappola > 1.5f)
						{
							base.GetComponent<PresenzaTrappola>().vita -= base.GetComponent<PresenzaTrappola>().dannoSubitoPerAzione;
						}
					}
				}
			}
			if (this.timerAutodannoTrappola > 1.5f)
			{
				this.timerAutodannoTrappola = 0f;
			}
		}
		else
		{
			this.suonoArea.Stop();
			this.partenzaSuono = false;
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0011A4D8 File Offset: 0x001186D8
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaAreaTrappola.Contains(altri.gameObject))
		{
			this.ListaAreaTrappola.Add(altri.gameObject);
		}
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x0011A524 File Offset: 0x00118724
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

	// Token: 0x04001E0D RID: 7693
	public List<GameObject> ListaAreaTrappola;

	// Token: 0x04001E0E RID: 7694
	private AudioSource suonoArea;

	// Token: 0x04001E0F RID: 7695
	private bool partenzaSuono;

	// Token: 0x04001E10 RID: 7696
	private float timerAutodannoTrappola;
}
