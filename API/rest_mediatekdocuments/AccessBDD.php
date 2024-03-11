<?php

include_once("ConnexionPDO.php");

/**
 * Classe de construction des requêtes SQL à envoyer à la BDD
 */
class AccessBDD {

    public $login = "hedi";
    public $mdp = "mediatek86bdd";
    public $bd = "mediatek86";
    public $serveur = "127.0.0.1";
    public $port = "3306";
    public $conn = null;

    /**
     * constructeur : demande de connexion à la BDD
     */
    public function __construct() {
        try {
            $this->conn = new ConnexionPDO($this->login, $this->mdp, $this->bd, $this->serveur, $this->port);
        } catch (Exception $e) {
            throw $e;
        }
    }

    /**
     * récupération de toutes les lignes d'une table
     * @param string $table nom de la table
     * @return lignes de la requete
     */
    public function selectAll($table) {
        if ($this->conn != null) {
            switch ($table) {
                case "livre" :
                    return $this->selectAllLivres();
                case "dvd" :
                    return $this->selectAllDvd();
                case "revue" :
                    return $this->selectAllRevues();
                case "exemplaire" :
                    return $this->selectExemplairesRevue();
                case "lesCommandeLivre":
                    return $this->selectAllCommandeLivre();
                case "lesCommandeDvds":
                    return $this->selectAllCommandeDvds();
                case "lesCommandeRevues":
                    return $this->selectAllCommandeRevues();
                case "genre" :
                case "public" :
                case "rayon" :
                case "etat" :
                case "suivi":
// select portant sur une table contenant juste id et libelle
                    return $this->selectTableSimple($table);
                default:
// select portant sur une table, sans condition
                    return $this->selectTable($table);
            }
        } else {
            return null;
        }
    }

    /**
     * récupération des lignes concernées
     * @param string $table nom de la table
     * @param array $champs nom et valeur de chaque champs de recherche
     * @return lignes répondant aux critères de recherches
     */
    public function select($table, $champs) {
        if ($this->conn != null && $champs != null) {
            switch ($table) {
                case "exemplaire" :
                    return $this->selectExemplairesRevue($champs['id']);
                default:
// cas d'un select sur une table avec recherche sur des champs
                    return $this->selectTableOnConditons($table, $champs);
            }
        } else {
            return null;
        }
    }

    /**
     * récupération de toutes les lignes d'une table simple (qui contient juste id et libelle)
     * @param string $table
     * @return lignes triées sur lebelle
     */
    public function selectTableSimple($table) {
        $req = "select * from $table order by libelle;";
        return $this->conn->query($req);
    }

    /**
     * récupération de toutes les lignes d'une table
     * @param string $table
     * @return toutes les lignes de la table
     */
    public function selectTable($table) {
        $req = "select * from $table;";
        return $this->conn->query($req);
    }

    /**
     * récupération des lignes d'une table dont les champs concernés correspondent aux valeurs
     * @param type $table
     * @param type $champs
     * @return type
     */
    public function selectTableOnConditons($table, $champs) {
// construction de la requête
        $requete = "select * from $table where ";
        foreach ($champs as $key => $value) {
            $requete .= "$key=:$key and";
        }
// (enlève le dernier and)
        $requete = substr($requete, 0, strlen($requete) - 3);
        return $this->conn->query($requete, $champs);
    }

    /**
     * récupération de toutes les lignes de la table Livre et les tables associées
     * @return lignes de la requete
     */
    public function selectAllLivres() {
        $req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, ";
        $req .= "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as lePublic, r.libelle as rayon ";
        $req .= "from livre l join document d on l.id=d.id ";
        $req .= "join genre g on g.id=d.idGenre ";
        $req .= "join public p on p.id=d.idPublic ";
        $req .= "join rayon r on r.id=d.idRayon ";
        $req .= "order by titre ";
        return $this->conn->query($req);
    }

    /**
     * récupération de toutes les lignes de la table DVD et les tables associées
     * @return lignes de la requete
     */
    public function selectAllDvd() {
        $req = "Select l.id, l.duree, l.realisateur, d.titre, d.image, l.synopsis, ";
        $req .= "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as lePublic, r.libelle as rayon ";
        $req .= "from dvd l join document d on l.id=d.id ";
        $req .= "join genre g on g.id=d.idGenre ";
        $req .= "join public p on p.id=d.idPublic ";
        $req .= "join rayon r on r.id=d.idRayon ";
        $req .= "order by titre ";
        return $this->conn->query($req);
    }

    /**
     * récupération de toutes les lignes de la table Revue et les tables associées
     * @return lignes de la requete
     */
    public function selectAllRevues() {
        $req = "Select l.id, l.periodicite, d.titre, d.image, l.delaiMiseADispo, ";
        $req .= "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as lePublic, r.libelle as rayon ";
        $req .= "from revue l join document d on l.id=d.id ";
        $req .= "join genre g on g.id=d.idGenre ";
        $req .= "join public p on p.id=d.idPublic ";
        $req .= "join rayon r on r.id=d.idRayon ";
        $req .= "order by titre ";
        return $this->conn->query($req);
    }

    /**
     * récupération de tous les exemplaires d'une revue
     * @param string $id id de la revue
     * @return lignes de la requete
     */
    public function selectExemplairesRevue($id) {
        $param = array(
            "id" => $id
        );
        $req = "Select e.id, e.numero, e.dateAchat, e.photo, e.idEtat ";
        $req .= "from exemplaire e join document d on e.id=d.id ";
        $req .= "where e.id = :id ";
        $req .= "order by e.dateAchat DESC";
        return $this->conn->query($req, $param);
    }

    /**
     * Récupére toutes les commandes de livres
     * Le trie entre dvd et livre se fait sur idDvd
     * @return type
     */
    public function selectAllCommandeLivre() {
        $req = "SELECT a.id, a.nbExemplaire, a.idLivreDvd, a.idSuivi, ";
        $req .= "b.id,b.dateCommande, b.montant, ";
        $req .= "c.id as ids, c.libelle as suivi ";
        $req .= "FROM commandedocument a JOIN  commande b on a.id=b.id ";
        $req .= "JOIN suivi c on c.id =a.idSuivi ";
        $req .= "WHERE idLivreDvd < 19999 ";
        $req .= "ORDER by dateCommande DESC";
        return $this->conn->query($req);
    }

    /**
     * Récupère toutes les commandes de dvd
     * Le trie entre dvd et livre se fait sur idDVd
     * @return type
     */
    public function selectAllCommandeDvds() {
        $req = "SELECT a.id, a.nbExemplaire, a.idLivreDvd, a.idSuivi, ";
        $req .= "b.id,b.dateCommande, b.montant, ";
        $req .= "c.id as ids, c.libelle as suivi ";
        $req .= "FROM commandedocument a JOIN  commande b on a.id=b.id ";
        $req .= "JOIN suivi c on c.id =a.idSuivi ";
        $req .= "WHERE idLivreDvd > 19999 ";
        $req .= "ORDER by dateCommande DESC";
        return $this->conn->query($req);
    }

    public function selectAllCommandeRevues() {
        $req = "SELECT a.id, a.dateCommande, a.montant,b.id, ";
        $req .= "b.dateFinAbonnement, b.idRevue ";
        $req .= "FROM commande a JOIN abonnement b on a.id = b.id ";
        $req .= "ORDER by a.dateCommande DESC";
        return $this->conn->query($req);
    }

    /**
     * suppresion d'une ou plusieurs lignes dans une table
     * @param string $table nom de la table
     * @param array $champs nom et valeur de chaque champs
     * @return true si la suppression a fonctionné
     */
    public function delete($table, $champs) {
        if ($this->conn != null) {
// construction de la requête
            $requete = "delete from $table where ";
            foreach ($champs as $key => $value) {
                $requete .= "$key=:$key and ";
            }
// (enlève le dernier and)
            $requete = substr($requete, 0, strlen($requete) - 5);
            return $this->conn->execute($requete, $champs);
        } else {
            return null;
        }
    }

    /**
     * ajout d'une ligne dans une table
     * @param string $table nom de la table
     * @param array $champs nom et valeur de chaque champs de la ligne
     * @return true si l'ajout a fonctionné
     */
    public function insertOne($table, $champs) {
        if ($this->conn != null && $champs != null) {
// construction de la requête
            $requete = "insert into $table (";
            foreach ($champs as $key => $value) {
                $requete .= "$key,";
            }
// (enlève la dernière virgule)
            $requete = substr($requete, 0, strlen($requete) - 1);
            $requete .= ") values (";
            foreach ($champs as $key => $value) {
                $requete .= ":$key,";
            }
// (enlève la dernière virgule)
            $requete = substr($requete, 0, strlen($requete) - 1);
            $requete .= ");";
            return $this->conn->execute($requete, $champs);
        } else {
            return null;
        }
    }

    /**
     * modification d'une ligne dans une table
     * @param string $table nom de la table
     * @param string $id id de la ligne à modifier
     * @param array $param nom et valeur de chaque champs de la ligne
     * @return true si la modification a fonctionné
     */
    public function updateOne($table, $id, $champs) {
        if ($this->conn != null && $champs != null) {
// construction de la requête
            $requete = "update $table set ";
            foreach ($champs as $key => $value) {
                $requete .= "$key=:$key,";
            }
// (enlève la dernière virgule)
            $requete = substr($requete, 0, strlen($requete) - 1);
            $champs["id"] = $id;
            $requete .= " where id=:id;";
            return $this->conn->execute($requete, $champs);
        } else {
            return null;
        }
    }

    /**
     * Ajout d'un livre
     * insertOne retourne true si cela fonctionne.
     * @param type $champs
     * @return null
     */
    public function ajoutLivre($champs) {
        $champsDocument = [
            "id" => $champs["Id"],
            "titre" => $champs["Titre"],
            "image" => $champs["Image"],
            "idRayon" => $champs["IdRayon"],
            "idPublic" => $champs["IdPublic"],
            "idGenre" => $champs["IdGenre"]];
        $champsDvdLivre = ["id" => $champs["Id"]];
        $champsLivre = [
            "id" => $champs["Id"],
            "ISBN" => $champs["Isbn"],
            "auteur" => $champs["Auteur"],
            "collection" => $champs["Collection"]];
        $result = $this->insertOne("document", $champsDocument);
        if ($result == null || $result == false) {
            return null;
        }
        $result = $this->insertOne("livres_dvd", $champsDvdLivre);
        if ($result == null || $result == false) {
            return null;
        }
        return $this->insertOne("livre", $champsLivre);
    }

    /**
     * Modifie un livre
     * @param type $id
     * @param type $champs
     * @return null
     */
    public function modifiLivre($id, $champs) {
        $champsDocument = [
            "titre" => $champs["Titre"],
            "image" => $champs["Image"],
            "idRayon" => $champs["IdRayon"],
            "idPublic" => $champs["IdPublic"],
            "idGenre" => $champs["IdGenre"]];

        $champsLivre = [//"id" => $champs["Id"],
            "ISBN" => $champs["Isbn"],
            "auteur" => $champs["Auteur"],
            "collection" => $champs["Collection"]];

        $result = $this->updateOne("document", $id, $champsDocument);
        if ($result == null || $result == false) {
            return null;
        }

        return $this->updateOne("livre", $id, $champsLivre);
    }

    /**
     * Ajoute un Dvd
     * @param type $champs
     * @return null
     */
    public function ajoutDvd($champs) {
        $champsDocument = [
            "id" => $champs["Id"],
            "titre" => $champs["Titre"],
            "image" => $champs["Image"],
            "idRayon" => $champs["IdRayon"],
            "idPublic" => $champs["IdPublic"],
            "idGenre" => $champs["IdGenre"]];
        $champsDvdLivre = ["id" => $champs["Id"]];

        $champsDvd = [
            "id" => $champs["Id"],
            "synopsis" => $champs["Synopsis"],
            "realisateur" => $champs["Realisateur"],
            "duree" => $champs["Duree"]];

        $result = $this->insertOne("document", $champsDocument);
        if ($result == null || $result == false) {
            return null;
        }

        $result = $this->insertOne("livres_dvd", $champsDvdLivre);
        if ($result == null || $result == false) {
            return null;
        }
        return $this->insertOne("dvd", $champsDvd);
    }

    /**
     * Modifie un Dvd
     * @param type $id
     * @param type $champs
     * @return null
     */
    public function modiDvd($id, $champs) {
        $champsDocument = [
            "titre" => $champs["Titre"],
            "image" => $champs["Image"],
            "idRayon" => $champs["IdRayon"],
            "idPublic" => $champs["IdPublic"],
            "idGenre" => $champs["IdGenre"]
        ];

        $champsDvd = [
            "synopsis" => $champs["Synopsis"],
            "realisateur" => $champs["Realisateur"],
            "duree" => $champs["Duree"]
        ];

        $result = $this->updateOne("document", $id, $champsDocument);
        if ($result == null || $result == false) {
            return null;
        }

        return $this->updateOne("dvd", $id, $champsDvd);
    }

    /**
     * Ajout d'une revue
     * @param type $champs
     * @return null
     */
    public function ajoutRevue($champs) {
        $champsDocument = [
            "id" => $champs["Id"],
            "titre" => $champs["Titre"],
            "image" => $champs["Image"],
            "idRayon" => $champs["IdRayon"],
            "idPublic" => $champs["IdPublic"],
            "idGenre" => $champs["IdGenre"]
        ];

        $champsRevue = [
            "id" => $champs["Id"],
            "periodicite" => $champs["Periodicite"],
            "delaiMiseADispo" => $champs["DelaiMiseADispo"]
        ];

        $result = $this->insertOne("document", $champsDocument);
        if ($result == null || $result == false) {
            return null;
        }
        return $this->insertOne("revue", $champsRevue);
    }

    /**
     * Modifie une Revue
     * @param type $id
     * @param type $champs
     * @return null
     */
    public function modifiRevue($id, $champs) {
        $champsDocument = [
            "titre" => $champs["Titre"],
            "image" => $champs["Image"],
            "idRayon" => $champs["IdRayon"],
            "idPublic" => $champs["IdPublic"],
            "idGenre" => $champs["IdGenre"]
        ];

        $champsRevue = [
            "periodicite" => $champs["Periodicite"],
            "delaiMiseADispo" => $champs["DelaiMiseADispo"]
        ];
        $result = $this->updateOne("document", $id, $champsDocument);
        if ($result == null || $result == false) {
            return null;
        }
        return $this->updateOne("revue", $id, $champsRevue);
    }

    /**
     * Ajoute une commande
     * @param type $champs
     * @return null
     */
    public function ajoutCommandeLivreOuDvd($champs) {
        $champsCommande = [
            "id" => $champs["Id"],
            "dateCommande" => $champs["DateCommande"],
            "montant" => $champs["Montant"]
        ];

        $champsCommandeDocument = [
            "id" => $champs["Id"],
            "nbExemplaire" => $champs["NbExemplaire"],
            "idLivreDvd" => $champs["IdLivreDvd"],
            "idSuivi" => $champs["IdSuivi"]
        ];

        $result = $this->insertOne("commande", $champsCommande);
        if ($result == null || $result == false) {
            return null;
        }
        return $this->insertOne("commandedocument", $champsCommandeDocument);
    }

    /**
     * Modifie une commande de livre ou de dvd
     * @param type $id
     * @param type $champs
     * @return null
     */
    public function modifiCommandeLivreOuDvd($id, $champs) {
        $champsCommande = [
            "dateCommande" => $champs["DateCommande"],
            "montant" => $champs["Montant"]
        ];

        $champsCommandeDocument = [
            "nbExemplaire" => $champs["NbExemplaire"],
            "idLivreDvd" => $champs["IdLivreDvd"],
            "idSuivi" => $champs["IdSuivi"]
        ];

        $result = $this->updateOne("commande", $id, $champsCommande);
        if ($result == null || $result == false) {
            return null;
        }

        return $this->updateOne("commandedocument", $id, $champsCommandeDocument);
    }

    /**
     * Ajoute une commande d abonnement
     * @param type $champs
     * @return null
     */
    public function ajoutCommandeAbonnementRevue($champs) {
        $champsCommande = [
            "id" => $champs["Id"],
            "dateCommande" => $champs["DateCommande"],
            "montant" => $champs["Montant"]
        ];

        $champsAbonnement = [
            "id" => $champs["Id"],
            "dateFinAbonnement" => $champs["DateFinAbonnement"],
            "idRevue" => $champs["IdRevue"]
        ];

        $result = $this->insertOne("commande", $champsCommande);
        if ($result == null || $result == false) {
            return null;
        }
        return $this->insertOne("abonnement", $champsAbonnement);
    }

    /**
     * Fonction qui modifie l'etat d'un exemplaire
     * @param type $id
     * @param type $champs
     * @return type
     */
    public function modifiExempaire($id, $champs) {
        $champsModif = [
            "idEtat" => $champs["IdEtat"]
        ];
        return $this->updateOneBis("exemplaire", $id, $champsModif);
    }

    /**
     * modifie l etat d'un exemplaire est appelé par modifiExemplaire
     * @param type $table
     * @param type $id
     * @param type $champs
     * @return null
     */
    public function updateOneBis($table, $id, $champs) {
        if ($this->conn != null && $champs != null) {
// construction de la requête
            $requete = "update $table set ";
            foreach ($champs as $key => $value) {
                $requete .= "$key=:$key,";
            }
// (enlève la dernière virgule)
            $requete = substr($requete, 0, strlen($requete) - 1);
            $champs["numero"] = $id;
            $requete .= " where numero=:numero;";
            return $this->conn->execute($requete, $champs);
        } else {
            return null;
        }
    }
/**
 * Créer la requete SQL qui cherche l'utilisateur et le mdp
 * @param type $champs
 * @return type
 */
public function authentification($champs) {
    $nom = $champs["Nom"];
    $pwd = hash('sha256',$champs["Pwd"]);
    
    $req = "SELECT * FROM utilisateur WHERE nom = '$nom' AND pwd = '$pwd';";
    return $this->conn->query($req);

}

}
