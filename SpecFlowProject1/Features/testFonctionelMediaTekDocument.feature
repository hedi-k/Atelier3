Feature: MediaTekDocument
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](SpecFlowProject1/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@mytag
Scenario: recherche par titre
	Given je saisie "La planète des singes"
	Then Il doit apparaître dans le titre des infos détaillé "La planète des singes"

Scenario: recherche par id
	Given je saisie l'id "00024"
	When je clic sur le bouton recherche
	Then il doit me trouver le titre "Pavillon noir"

Scenario: selection comboBox Genre
	When je saisie un genre "Roman"
	Then Le premier titre trouvé sur ce Genre est "Dans les coulisses du musée"

Scenario:  selection comboBox Public
	When je saisie un public "Ados"
	Then  Le premier titre trouvé sur ce Public est "Catastrophes au Brésil"

Scenario: selection comboBox Rayon
	When je saisie un rayon "BD Adultes"
	Then  Le premier titre trouvé sur ce rayon "L'archipel du danger"