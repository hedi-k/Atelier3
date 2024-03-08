using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Exemplaire (exemplaire d'une revue)
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// Représente le numéro de l'exemplaire
        /// </summary>
        public int Numero { get; set; }
        /// <summary>
        /// Représente la photo de l'exemplaire (le chemin)
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Représente la date d'achat de l'exemplaire
        /// </summary>
        public DateTime DateAchat { get; set; }
        /// <summary>
        /// Représente l'id de l'état de l exemplaire
        /// </summary>
        public string IdEtat { get; set; }
        /// <summary>
        /// Représente l'id de l'exemplaire
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Objet exemplaire
        /// </summary>
        /// <param name="numero">numéro de l'exemplaire</param>
        /// <param name="dateAchat">date d'achat de l'exemplaire</param>
        /// <param name="photo">photo de l'exemplaire</param>
        /// <param name="idEtat">id de l'était de l exemplaire</param>
        /// <param name="idDocument">id du document de l exemplaire</param>
        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Id = idDocument;
        }

    }
}
