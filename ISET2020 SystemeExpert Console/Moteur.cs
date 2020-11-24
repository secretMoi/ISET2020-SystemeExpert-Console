using System;
using System.Collections.Generic;

namespace ISET2020_SystemeExpert_Console
{
	public class Moteur
	{
		private readonly BaseFaits _bdFaits;
		private readonly BaseRegles _bdRegles;
		private readonly IHM _ihm;

		public Moteur(IHM ihm)
		{
			_ihm = ihm;
			_bdFaits = new BaseFaits();
			_bdRegles = new BaseRegles();
		}

		public void AjouterRegle(string sRegle)
		{
			string[] aTmp = sRegle.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
			if (aTmp.Length == 2)
			{
				string[] aHypotThese = aTmp[1].Split(new[] { "SI", " ALORS " }, StringSplitOptions.RemoveEmptyEntries);
				if (aHypotThese.Length == 2)
				{
					List<IFait> lHypot = new List<IFait>();
					string[] aHypot = aHypotThese[0].Split(new[] { " ET " }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string sHypot in aHypot)
						lHypot.Add(CalculFait.Determiner(sHypot));
					IFait these = CalculFait.Determiner(aHypotThese[1].Trim());
					_bdRegles.Ajouter(new Regle(aTmp[0], lHypot, these));
				}
			}
		}

		private int Applicable(Regle r)
		{
			int iNiveauMax = -1;
			foreach (IFait f in r.Hypot)
			{
				IFait fTrouve = _bdFaits.Chercher(f.Libelle());
				if (fTrouve == null)
				{
					if (f.Question() != null)
					{
						fTrouve = CalculFait.Determiner(f, this);
						_bdFaits.Ajouter(fTrouve);
						iNiveauMax = Math.Max(iNiveauMax, 0);
					}
					else
						return -1;
				}
				if (!fTrouve.Valeur().Equals(f.Valeur()))
					return -1;
				else
					iNiveauMax = Math.Max(iNiveauMax, fTrouve.Niveau());
			}
			return iNiveauMax;
		}

		private Tuple<Regle, int> TrouverDispo(BaseRegles rDisp)
		{
			foreach (Regle r in rDisp.Regles)
			{
				int iTmp = Applicable(r);
				if (iTmp != -1)
					return Tuple.Create(r, iTmp);
			}
			return null;
		}

		internal int QuestionEntier(string sQuestion)
		{
			return _ihm.QuestionEntier(sQuestion);
		}

		internal bool QuestionLogique(string sQuestion)
		{
			return _ihm.QuestionLogique(sQuestion);
		}

		public void Resoudre()
		{
			_ihm.AfficherRegles(_bdRegles.Regles);
			bool resteRegles = true;
			BaseRegles reglesDispo = new BaseRegles
			{
				Regles = new List<Regle>(_bdRegles.Regles)
			};
			_bdFaits.RaZ();
			while (resteRegles)
			{
				Tuple<Regle, int> regleIns = TrouverDispo(reglesDispo);
				if (regleIns != null)
				{
					IFait faitIns = regleIns.Item1.These;
					faitIns.DefinirNiveau(regleIns.Item2 + 1);
					_bdFaits.Ajouter(faitIns);
					reglesDispo.Supprimer(regleIns.Item1);
				}
				else
					resteRegles = false;
			}
			_ihm.AfficherFaits(_bdFaits.Faits);
		}

		public int CombienRegles()
		{
			return _bdRegles.Regles.Count;
		}
	}
}