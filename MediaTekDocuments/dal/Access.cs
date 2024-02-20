using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";
        /// <summary>
        /// méthode HTTP pour update
        private const string PUT = "PUT";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire/" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        //**********************************************  GESTION DES LIVRES  **********************************************
        //Envoi l'objet livre à l'api pour la BDD.
        public bool CreerLivre(Livre unLivre)
        {
            String jsonCreerLivre = JsonConvert.SerializeObject(unLivre);
            //Les espaces ne sont pas pris en charge.
            jsonCreerLivre = jsonCreerLivre.Replace(" ", "-"); 
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Object> liste = TraitementRecup<Object>(POST, "livre/" + jsonCreerLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        //Supprime le livre dont l'id est envoyé à l'api.
        public bool SupprimerLivre(Livre unLivre)
        {
            String jsonSupprimerLivre = JsonConvert.SerializeObject(unLivre.Id);
            try
            {
                List<Object> liste = TraitementRecup<Object>(DELETE, "livre/{\"Id\":" + jsonSupprimerLivre + "}");
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //Modifie le livre
        public bool ModifierLivre(Livre unLivre)
        {
            String jsonModifierLivre = JsonConvert.SerializeObject(unLivre);
            try
            {
                List<Object> liste = TraitementRecup<Object>(PUT, "livre/" + unLivre.Id + "/" + jsonModifierLivre.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //**********************************************  GESTION DES DVDs  **********************************************
        //Envoi l'objet DVD à l'api pour la BDD.
        public bool CreerDvd(Dvd unDvd)
        {
            String jsonCreerDvd = JsonConvert.SerializeObject(unDvd);
            try
            {
                List<Object> liste = TraitementRecup<Object>(POST, "dvd/" + jsonCreerDvd.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        //Supprime le dvd dont l'id est envoyé à l'api.
        public bool SupprimerDvd(Dvd unDvd)
        {
            String jsonSupprimerDvd = JsonConvert.SerializeObject(unDvd.Id);
            try
            {
                List<Object> liste = TraitementRecup<Object>(DELETE, "dvd/{\"Id\":" + jsonSupprimerDvd + "}");
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //Modifie le dvd 
        public bool ModifierDvd(Dvd unDvd)
        {
            String jsonModifierDvd = JsonConvert.SerializeObject(unDvd);
            try
            {
                List<Object> liste = TraitementRecup<Object>(PUT, "dvd/" + unDvd.Id + "/" + jsonModifierDvd.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //**********************************************  GESTION DES REVUES  **********************************************
        //Envoi l'objet Revue à l'api pour la BDD.
        public bool CreerRevue(Revue uneRevue)
        {
            String jsonCreerRevue = JsonConvert.SerializeObject(uneRevue);
            try
            {
                List<Object> liste = TraitementRecup<Object>(POST, "revue/" + jsonCreerRevue.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //Supprime la revue dont l'id est envoyé à l'api.
        public bool SupprimerRevue(Revue uneRevue)
        {
            String jsonSupprimerRevue = JsonConvert.SerializeObject(uneRevue.Id);
            try
            {
                List<Object> liste = TraitementRecup<Object>(DELETE, "revue/{\"Id\":" + jsonSupprimerRevue + "}");
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //Modifie la revue.
        public bool ModifierRevue(Revue uneRevue) 
        {
            String jsonModifiRevue = JsonConvert.SerializeObject(uneRevue);
            try
            {
                List<Object> liste = TraitementRecup<Object>(PUT, "revue/" + uneRevue.Id + "/" + jsonModifiRevue.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //**********************************************  GESTION DES COMMANDES DE LIVRES  **********************************************
        //retourne les Suivi pour la cbxSuivi
        public List<Categorie> GetSuivi()
        {
            IEnumerable<Suivi> lesSuivi = TraitementRecup<Suivi>(GET, "suivi");
            return new List<Categorie>(lesSuivi);
        }
        //retourne la liste de commande de livre
        public List<CommandeDocument> GetAllCommandeLivre()
        {
            List<CommandeDocument> lesCommandeLivre = TraitementRecup<CommandeDocument>(GET, "lesCommandeLivre");
            return lesCommandeLivre;
        }
        //Envoi une commande de Livre
        public bool EnvoiCmd(CommandeDocument uneCommande)
        {
            String jsonCreerCommandeLivre = JsonConvert.SerializeObject(uneCommande, new CustomDateTimeConverter());
            try
            {
                List<Object> liste = TraitementRecup<Object>(POST, "commandeLivre/" + jsonCreerCommandeLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //Supprime une commande de Livre
        public bool SupprimerCmdLivre(CommandeDocument uneCommande)
        {
            String jsonSupprimerCmdLivre = JsonConvert.SerializeObject(uneCommande.Id);
            try
            {
                List<Object> liste = TraitementRecup<Object>(DELETE, "commandedocument/{\"Id\":" + jsonSupprimerCmdLivre + "}");
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        //Modifie une commande de livre
        public bool ModifCmdLivre(CommandeDocument uneCommande)
        {
            String jsonModificmdLivre = JsonConvert.SerializeObject(uneCommande);
            try
            {
                List<Object> liste = TraitementRecup<Object>(PUT, "commandeLivre/" + uneCommande.Id + "/" + jsonModificmdLivre.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message)
        {
            Console.WriteLine("METHODE = " + methode + " MESSAGE = " + message);// affiche le traitement (pour postMAN)******************** test à effacer
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}
