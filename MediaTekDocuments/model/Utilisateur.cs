using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Utilisateur
    {
        public string Id { get; }
        public string Nom { get; }
        public string Pwd { get; }
        public string IdService { get; }

        public Utilisateur (string id, string nom, string pwd, string idService)
        {
            Id = id;
            Nom = nom;
            Pwd = pwd;
            IdService = idService;
        }
    }
}
