# Informationssysteme_U4_API

eine REST-Schnittstelle zum Anlegen von Kunden,
Ansprechpartnern für Kunden, deren Beziehungen
und Anfragen von Kunden über bestimmte Materialien.

---

## Die Geschäftsprozesse im Überblick

## Schritt 1: Anlegen neuer Kunde
### Was geschieht?
Ein neuer Kunde wird mit allen notwendigen Informationen
zur Kontaktaufnahme im System erstellt.
### Welche Daten sind für die Durchführung notwendig?
Erstellt werden Kundenstammdaten wie Anrede, Name,
Adresse(n), aber auch Kommunikationsmittel wie Telefon
und E-Mail-Adresse. Es kann auch die Sprache gespeichert
werden (für internationale Kunden).
### Welche Daten werden bei der Durchführung verändert oder erstellt?
Erstellt werden Kundenstammdaten wie Anrede, Name,
Adresse(n), aber auch Kommunikationsmittel wie Telefon
und E-Mail-Adresse. Es kann auch die Sprache gespeichert
werden (für internationale Kunden).

## Schritt 2: Anlegen Ansprechpartner für Kunde
### Was geschieht?
Ein neuer Ansprechpartner wird mit allen notwendigen
Informationen im System erstellt.
### Welche Daten sind für die Durchführung notwendig?
Es gibt keine vorab benötigten Daten.
### Welche Daten werden bei der Durchführung verändert oder erstellt?
Erstellt werden Stammdaten wie Anrede und Name. Es können
auch hier Adressen und Kommunikationsmittel (Telefon,
E-Mail) gespeichert werden.

## Schritt 3: Anlegen GP-Beziehung
### Was geschieht?
Ein Ansprechpartner wird einem Kunden zugeordnet. Die
Beziehung wird im System erfasst.
### Welche Daten sind für die Durchführung notwendig?
Ein Kunde und ein Ansprechpartner.
### Welche Daten werden bei der Durchführung verändert oder erstellt?
Es wird eine neue Beziehung angelegt, die mit einem
Kommentar versehen werden kann, um beispielsweise den Typ
der Beziehung zu definieren.

## Schritt 4: Anlegen Kundenanfrage
### Was geschieht?
Ein Kunde hat eine neue Anfrage, die bestimmte Materialien
umfasst. Die Anfrage wird im System erfasst.
### Welche Daten sind für die Durchführung notwendig?
Der anfragende Kunde und das angefragte Material.
### Welche Daten werden bei der Durchführung verändert oder erstellt?
Es wird eine neue Anfrage erstellt, die dem anfragenden
Kunden zugeordnet ist. Diese referenziert angefragte
Materialien über Positionen, die die angefragte Menge
beinhalten.

---

## Die Schnittstelle

Die REST-Schnittstelle umfasst eine Swagger-UI, diese
beinhaltet eine Übersicht über alle verfügbaren Endpunkte.

### Beschränkungen
Anfragen (requests) und deren Positionen (positions), sowie
Adressen (addresses) können nicht ohne referenzierendes
Objekt erstellt werden. Welche Objekte nötig sind, ist
jeweils aus dem Pfad ersichtlich.<br/>
bestimmte Felder sind notwendig (required). Diese sind in
der Swagger-UI mit einem roten Stern markiert.<br/>
Zwecks der Beschränkung auf die unbedingt notwendigen
Funktionalitäten werden ausschließlich GET- und POST-Requests
unterstützt.

### Sonstige Hinweise
Wenn eine Referenz benötigt wird (required), wird beim
Anlegen auf referentielle Integrität geprüft, bei
optionalen Angaben ist dies nicht der Fall.<br/>
Die Schnittstelle ist komplett auf Englisch und bietet auch
keine Übersetzungen an.

### Empfohlene Reihenfolge zum Testen
Getestet werden kann direkt in Swagger-UI.
Folgende Reihenfolge kann verwendet werden, dabei werden
jeweils nur vorherige Elemente referenziert:
<ol>
	<li>AddressInformation</li>
	<li>Address (ref. AddressInformation)</li>
	<li>Customer (ref. AddressInformation)</li>
	<li>Contact (ref. AddressInformation)</li>
	<li>Relation (ref. Customer, Contact)</li>
	<li>Request (ref. Customer)</li>
	<li>Material</li>
	<li>Position (ref. Request, Material)</li>
</ol>

---

## Sonstiges
Viel Spaß beim Testen!