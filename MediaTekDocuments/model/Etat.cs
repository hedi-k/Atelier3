
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Etat (état d'usure d'un document)
    /// </summary>
    public class Etat
    {
        /// <summary>
        /// Représente l'id de l'état 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Représente le libelle de l'état
        /// </summary>
        public string Libelle { get; set; }
        /// <summary>
        /// Objet Etat
        /// </summary>
        /// <param name="id">id de l etat</param>
        /// <param name="libelle">nom de l etat</param>
        public Etat(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

    }
}
