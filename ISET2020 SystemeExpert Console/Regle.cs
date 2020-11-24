using System.Collections.Generic;

namespace ISET2020_SystemeExpert_Console
{
	public class Regle
	{
		public List<IFait> Hypot { get; set; }
		public IFait These { get; set; }
		public string Nom { get; set; }
		public Regle(string nom, List<IFait> hypot, IFait these)
		{
			Nom = nom;
			Hypot = hypot;
			These = these;
		}

		public override string ToString()
		{
			return Nom + " : SI(" + string.Join(" ET ", Hypot) + ") ALORS " + These;
		}
	}
	internal class BaseRegles
	{
		protected List<Regle> _Regles;

		public List<Regle> Regles
		{
			get => _Regles;
			set => _Regles = value;
		}

		public BaseRegles()
		{
			_Regles = new List<Regle>();
		}

		public void RaZ()
		{
			_Regles.Clear();
		}

		public void Ajouter(Regle r)
		{
			_Regles.Add(r);
		}

		public void Supprimer(Regle r)
		{
			_Regles.Remove(r);
		}
	}
}
