using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ISET2020_SystemeExpert_Console
{
	class Program : IHM
	{
		static void Main(string[] args)
		{
			Program p = new Program();
			p.Executer();
		}

		public void Executer()
		{
			Console.WriteLine("** Création du moteur **");
			Moteur m = new Moteur(this);
			Console.WriteLine("** Ajout des règles **");
			StreamReader sr = new StreamReader("Regles.txt");

			string sLigne;
			while ((sLigne = sr.ReadLine()) != null)
				m.AjouterRegle(sLigne);

			while (true)
			{
				Console.WriteLine("\n** Résolution **");
				m.Resoudre();
			}
		}

		public int QuestionEntier(string sQuestion)
		{
			Console.WriteLine(sQuestion);
			try
			{
				return int.Parse(Console.ReadLine());
			}
			catch
			{
				return 0;
			}
		}

		public bool QuestionLogique(string sQuestion)
		{
			Console.WriteLine(sQuestion);
			string sRep = Console.ReadLine();
			if (sRep.ToUpper() == "O")
				return true;
			else
				return false;
		}
		public void AfficherFaits(List<IFait> fFaits)
		{
			string sTmp = "Solution(s) trouvée(s) :\n";
			sTmp += string.Join("\n", fFaits.Where(x => x.Niveau() > 0).OrderByDescending(x => x.Niveau()));
			Console.WriteLine(sTmp);
		}
		public void AfficherRegles(List<Regle> Regles_)
		{
			string sTmp = string.Join("\n", Regles_);
			Console.WriteLine(sTmp);
		}
	}
}
