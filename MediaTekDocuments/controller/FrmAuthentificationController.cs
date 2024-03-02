using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    class FrmAuthentificationController
    {
        private readonly Access access;

        
        public FrmAuthentificationController()
        {
            access = Access.GetInstance();
        }
        //reçoit un utilisateur de la vue et l'envoi au DAL.
        public Utilisateur Authentification(Utilisateur utilisateur)
        {
            return access.Authentification(utilisateur);
        }
    }
}
