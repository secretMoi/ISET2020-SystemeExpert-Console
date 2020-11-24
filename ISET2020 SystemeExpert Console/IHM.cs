using System.Collections.Generic;

namespace ISET2020_SystemeExpert_Console
{
	public interface IHM
	{
		int QuestionEntier(string sQuestion);
		bool QuestionLogique(string sQuestion);
		void AfficherFaits(List<IFait> fFaits);
		void AfficherRegles(List<Regle> rRegles);
	}
}
