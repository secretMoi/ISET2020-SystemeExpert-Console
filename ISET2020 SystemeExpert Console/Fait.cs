using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISET2020_SystemeExpert_Console
{
	public interface IFait
	{
		string Libelle();
		object Valeur();
		int Niveau();
		string Question();
		void DefinirNiveau(int n);
	}

	internal class FaitEntier : IFait
	{
		protected string _Libelle;

		public string Libelle()
		{
			return _Libelle;
		}
		protected int _Valeur;

		public object Valeur()
		{
			return _Valeur;
		}
		protected int _Niveau;

		public int Niveau()
		{
			return _Niveau;
		}

		public void DefinirNiveau(int n)
		{
			_Niveau = n;
		}
		protected string _Question = null;

		public string Question()
		{
			return _Question;
		}

		public FaitEntier(string Libelle_, int Valeur_, string Question_ = null, int Niveau_ = 0)
		{
			_Libelle = Libelle_;
			_Valeur = Valeur_;
			_Question = Question_;
			_Niveau = Niveau_;
		}

		public override string ToString()
		{
			return _Libelle + "=" + _Valeur.ToString() + " (" + _Niveau.ToString() + ")";
		}
	}
	internal class FaitLogique : IFait
	{
		protected string _Libelle;

		public string Libelle()
		{
			return _Libelle;
		}
		protected bool _Valeur;

		public object Valeur()
		{
			return _Valeur;
		}
		protected int _Niveau;

		public int Niveau()
		{
			return _Niveau;
		}

		public void DefinirNiveau(int n)
		{
			_Niveau = n;
		}
		protected string _Question = null;

		public string Question()
		{
			return _Question;
		}

		public FaitLogique(string Libelle_, bool Valeur_, string Question_ = null, int Niveau_ = 0)
		{
			_Libelle = Libelle_;
			_Valeur = Valeur_;
			_Question = Question_;
			_Niveau = Niveau_;
		}

		public override string ToString()
		{
			string res = "";
			if (!_Valeur)
				res += "!";
			res += _Libelle + " (" + _Niveau + ")";
			return res;
		}
	}
	internal class BaseFaits
	{
		protected List<IFait> _Faits;

		public List<IFait> Faits => _Faits;

		public BaseFaits()
		{
			_Faits = new List<IFait>();
		}

		public void RaZ()
		{
			_Faits.Clear();
		}

		public void Ajouter(IFait f)
		{
			_Faits.Add(f);
		}

		public IFait Chercher(string Libelle_)
		{
			return Faits.FirstOrDefault(x => x.Libelle().Equals(Libelle_));
		}

		public object Valeur(string Libelle_)
		{
			IFait f = Faits.FirstOrDefault(x => x.Libelle().Equals(Libelle_));

			if (f != null)
				return f.Valeur();
			else
				return null;
		}
	}
	internal static class CalculFait
	{
		internal static IFait Determiner(IFait fFait, Moteur mMoteur)
		{
			if (fFait.GetType() == typeof(FaitEntier))
			{
				int iTmp = mMoteur.QuestionEntier(fFait.Question());
				return new FaitEntier(fFait.Libelle(), iTmp, null, 0);
			}
			else
			{
				bool bTmp = mMoteur.QuestionLogique(fFait.Question());
				return new FaitLogique(fFait.Libelle(), bTmp, null, 0);
			}
		}

		internal static IFait Determiner(string sFait)
		{
			sFait = sFait.Trim();
			string sQuestion = null;
			if (sFait.Contains("="))
			{
				string[] sTmp = sFait.Split(new[] { "=", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
				if (sTmp.Length >= 2)
				{
					if (sTmp.Length == 3)
						sQuestion = sTmp[2].Trim();
					return new FaitEntier(sTmp[0].Trim(), int.Parse(sTmp[1].Trim()), sQuestion);
				}
				else
					return null;
			}
			else
			{
				bool bValeur = true;
				if (sFait.StartsWith("!"))
				{
					bValeur = false;
					sFait = sFait.Substring(1).Trim(); // On enlève le ! du nom
				}
				string[] sTmp = sFait.Split(new[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
				if (sTmp.Length == 2)
					sQuestion = sTmp[1].Trim();
				return new FaitLogique(sTmp[0].Trim(), bValeur, sQuestion);
			}
		}
	}
}
