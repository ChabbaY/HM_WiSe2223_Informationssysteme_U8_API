# Informationssysteme_U8_API

eine REST-Schnittstelle zum Anlegen von Lieferanten (Suppliers),
Materialien (Materials), Bestellanforderungen (Purchase Requisitions),
konkreten Anfragen (Requests) nach den Bestellanforderungen und
Angeboten (Offers) von Lieferanten. Angebote können pro Position
einen Preis über die Preisinformation (PriceInformation) definieren.

---

## Die Geschäftsprozesse im Überblick

Für eine Beschreibung der Geschäftsprozesse und für ein
Klassendiagramm siehe **U7_Linus_Englert.pdf**.

---

## Die Schnittstelle

Die REST-Schnittstelle umfasst eine Swagger-UI, diese
beinhaltet eine Übersicht über alle verfügbaren Endpunkte.

### Beschränkungen
Positionen von Bestellanforderungen (positions), sowie
Adressen (addresses) und Preisinformationen (priceinformations)
können nicht ohne referenzierendes Objekt erstellt werden.
Welche Objekte nötig sind, ist jeweils aus dem Pfad bzw.
Klassendiagramm ersichtlich.<br/>
Bestimmte Felder sind notwendig (required). Diese sind in
der Swagger-UI mit einem roten Stern markiert.<br/>
Um die Integrität der Daten zu wahren, wird auf referntielle
Integrität, sowohl beim Einfügen, als auch beim Löschen,
geprüft. Objekte, die nicht ohne referenzierendes Objekt
erstellt werden können werden kaskadierend gelöscht.
Referenzierte Objekte können nicht gelöscht werden.

### Sonstige Hinweise
Die Schnittstelle ist komplett auf Englisch und bietet auch
keine Übersetzungen an.

### Empfohlene Reihenfolge zum Testen
Getestet werden kann direkt in Swagger-UI.
Andernfalls bietet sich Postman dafür an.
Folgende Reihenfolge kann verwendet werden, dabei werden
jeweils nur vorherige Elemente referenziert:
<ol>
	<li>Supplier</li>
	<li>Address (ref. Supplier)</li>
	<li>Material</li>
	<li>PurchaseRequisition</li>
	<li>Position (ref. PurchaseRequisition, Material)</li>
	<li>Request (ref. PurchaseRequisition)</li>
	<li>Offer (ref. Supplier, Request)</li>
	<li>PriceInformation (ref. Offer, Position)</li>
</ol>
Es werden auch bereits Testdaten in Startup.cs generiert.

---

## Sonstiges
Viel Spaß beim Testen!