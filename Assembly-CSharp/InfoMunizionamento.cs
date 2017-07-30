using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class InfoMunizionamento : MonoBehaviour
{
	// Token: 0x0600005D RID: 93 RVA: 0x00014A70 File Offset: 0x00012C70
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00014A84 File Offset: 0x00012C84
	private void Update()
	{
		if (GestoreNeutroTattica.èBattagliaVeloce && !this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
		{
			for (int i = 0; i < this.ListaTipiMunizioniBaseTattica.Count; i++)
			{
				this.ListaTipiMunizioniBaseTattica[i].GetComponent<QuantitàMunizione>().quantità = 100000f;
			}
			this.ListaQuantitàSupportoTattica[0] = 8;
			this.ListaQuantitàSupportoTattica[1] = 8;
			this.ListaQuantitàSupportoTattica[2] = 8;
			this.ListaQuantitàSupportoTattica[3] = 8;
			this.ListaQuantitàSupportoTattica[4] = 8;
			this.ListaQuantitàSupportoTattica[5] = 8;
		}
	}

	// Token: 0x04000207 RID: 519
	public List<GameObject> ListaTipiMunizioniBaseTattica;

	// Token: 0x04000208 RID: 520
	private GameObject infoNeutreTattica;

	// Token: 0x04000209 RID: 521
	public List<int> ListaQuantitàSupportoTattica;

	// Token: 0x0400020A RID: 522
	public int tipoBattaglia;
}
