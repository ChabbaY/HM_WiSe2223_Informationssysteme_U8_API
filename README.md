# Informationssysteme_U8_API

eine REST-Schnittstelle zum Anlegen von Lieferanten (Suppliers),
Materialien (Materials), Bestellanforderungen (Purchase Requisitions),
konkreten Anfragen (Requests) nach den Bestellanforderungen und
Angeboten (Offers) von Lieferanten. Angebote k�nnen pro Position
einen Preis �ber die Preisinformation (PriceInformation) definieren.

---

## Die Gesch�ftsprozesse im �berblick

F�r eine Beschreibung der Gesch�ftsprozesse und f�r ein
Klassendiagramm siehe **U7_Linus_Englert.pdf**.

---

## Die Schnittstelle

Die REST-Schnittstelle umfasst eine Swagger-UI, diese
beinhaltet eine �bersicht �ber alle verf�gbaren Endpunkte.

### Beschr�nkungen
Positionen von Bestellanforderungen (positions), sowie
Adressen (addresses) und Preisinformationen (priceinformations)
k�nnen nicht ohne referenzierendes Objekt erstellt werden.
Welche Objekte n�tig sind, ist jeweils aus dem Pfad bzw.
Klassendiagramm ersichtlich.<br/>
Bestimmte Felder sind notwendig (required). Diese sind in
der Swagger-UI mit einem roten Stern markiert.<br/>
Um die Integrit�t der Daten zu wahren, wird auf referntielle
Integrit�t, sowohl beim Einf�gen, als auch beim L�schen,
gepr�ft. Objekte, die nicht ohne referenzierendes Objekt
erstellt werden k�nnen werden kaskadierend gel�scht.
Referenzierte Objekte k�nnen nicht gel�scht werden.

### Sonstige Hinweise
Die Schnittstelle ist komplett auf Englisch und bietet auch
keine �bersetzungen an.

### Empfohlene Reihenfolge zum Testen
Getestet werden kann direkt in Swagger-UI.
Andernfalls bietet sich Postman daf�r an.
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
Viel Spa� beim Testen!