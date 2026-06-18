using System;
using System.Collections.Generic;
using System.Linq;

/*
 * TEMPLATE ESAME C# - NEGOZIO ONLINE
 *
 * Regola scelta per il template:
 * - circa il 30% dei metodi è già implementato, soprattutto dove c'è logica delicata
 *   come validazione quantità, aggiornamento magazzino, calcolo dei totali e storico acquisti.
 * - circa il 70% dei metodi contiene TODO guidati: lo studente deve completarli senza
 *   modificare firma, nome, parametri o tipo di ritorno.
 *
 * Vincolo richiesto: tutto il codice è in un unico file .cs e senza namespace.
 */

public class Program
{
    public static void Main()
    {
        // Punto di ingresso della Console App.
        ApplicazioneNegozio applicazione = new ApplicazioneNegozio();
        // applicazione.Avvia();
        TestNegozioOnline.EseguiTuttiITest();
    }
}

public class ApplicazioneNegozio
{
    private readonly CatalogoProdotti catalogoProdotti;
    private readonly CarrelloUtente carrelloUtente;
    private readonly StoricoAcquisti storicoAcquisti;
    private readonly ServizioNegozio servizioNegozio;

    public ApplicazioneNegozio()
    {
        catalogoProdotti = new CatalogoProdotti();
        carrelloUtente = new CarrelloUtente();
        storicoAcquisti = new StoricoAcquisti();
        servizioNegozio = new ServizioNegozio(catalogoProdotti, carrelloUtente, storicoAcquisti);

        CaricaDatiIniziali();
    }

   public void Avvia()
{
    Console.Clear();

    Console.WriteLine("======================================");
    Console.WriteLine("     BENVENUTO NEL NEGOZIO ONLINE     ");
    Console.WriteLine("======================================");
    Console.WriteLine("Gestisci acquisti, catalogo e magazzino.");
    Console.WriteLine();

    bool continua = true;

    while (continua)
    {
        string ruolo = ScegliRuolo();

        switch (ruolo)
        {
            case "utente":
                GestisciMenuUtente();
                break;

            case "amministratore":
                GestisciMenuAmministratore();
                break;

            case "esci":
                continua = false;
                break;
        }
    }

    Console.WriteLine();
    Console.WriteLine("Grazie per aver utilizzato il Negozio Online.");
    Console.WriteLine("Arrivederci!");
}

    private void CaricaDatiIniziali()
    {
        // Metodo già implementato: fornisce prodotti di partenza per testare subito il sistema.
        catalogoProdotti.AggiungiProdotto(new Prodotto("P001", "Tastiera meccanica", 79.90m, 10));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P002", "Mouse wireless", 24.50m, 25));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P003", "Monitor 24 pollici", 149.99m, 7));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P004", "Cavo USB-C", 9.99m, 40));
    }

   private string ScegliRuolo()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Seleziona ruolo:");
        Console.WriteLine("utente");
        Console.WriteLine("amministratore");
        Console.WriteLine("esci");
        Console.Write("> ");

        string ruolo = (Console.ReadLine() ?? "")
            .Trim()
            .ToLower();

        if (ruolo == "utente" ||
            ruolo == "amministratore" ||
            ruolo == "esci")
        {
            return ruolo;
        }

        Console.WriteLine("Scelta non valida.");
    }
}

   private void GestisciMenuUtente()
{
    bool continua = true;

    while (continua)
    {
        Console.WriteLine();
        Console.WriteLine("===== MENU UTENTE =====");
        Console.WriteLine("1 - Visualizza catalogo");
        Console.WriteLine("2 - Aggiungi prodotto al carrello");
        Console.WriteLine("3 - Visualizza carrello");
        Console.WriteLine("4 - Modifica quantità nel carrello");
        Console.WriteLine("5 - Rimuovi prodotto dal carrello");
        Console.WriteLine("6 - Svuota carrello");
        Console.WriteLine("7 - Conferma acquisto");
        Console.WriteLine("8 - Visualizza storico acquisti");
        Console.WriteLine("0 - Indietro");

        string scelta = Console.ReadLine() ?? "";

        switch (scelta)
        {
            case "1":
                MostraCatalogo();
                break;

            case "2":
                MostraCatalogo();

                Console.Write("Codice prodotto: ");
                string codice = Console.ReadLine() ?? "";

                int quantita =
                    LeggiInteroPositivo("Quantità: ");

                if (servizioNegozio.AggiungiProdottoAlCarrello(codice, quantita))
                {
                    Console.WriteLine("Prodotto aggiunto.");
                }
                else
                {
                    Console.WriteLine("Operazione non riuscita.");
                }
                break;

            case "3":
                MostraCarrello();
                break;

            case "4":
                MostraCarrello();

                Console.Write("Codice prodotto: ");
                string codiceModifica =
                    Console.ReadLine() ?? "";

                int nuovaQuantita =
                    LeggiInteroPositivo("Nuova quantità: ");

                if (carrelloUtente.ModificaQuantitaNelCarrello(
                    codiceModifica,
                    nuovaQuantita))
                {
                    Console.WriteLine("Quantità aggiornata.");
                }
                else
                {
                    Console.WriteLine("Prodotto non trovato o quantità non valida.");
                }
                break;

            case "5":
                MostraCarrello();

                Console.Write("Codice prodotto: ");
                string codiceRimozione =
                    Console.ReadLine() ?? "";

                if (carrelloUtente.RimuoviDalCarrello(codiceRimozione))
                {
                    Console.WriteLine("Prodotto rimosso.");
                }
                else
                {
                    Console.WriteLine("Prodotto non presente nel carrello.");
                }
                break;

            case "6":
                carrelloUtente.SvuotaCarrello();
                Console.WriteLine("Carrello svuotato.");
                break;

            case "7":
                try
                {
                    Console.Write("Nome utente: ");
                    string nomeUtente =
                        Console.ReadLine() ?? "";

                    Acquisto acquisto =
                        servizioNegozio.ConfermaAcquisto(nomeUtente);

                    servizioNegozio.StampaAcquisto(acquisto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;

            case "8":
                MostraStoricoUtente();
                break;

            case "0":
                continua = false;
                break;

            default:
                Console.WriteLine("Scelta non valida.");
                break;
        }
    }
}

   private void GestisciMenuAmministratore()
{
    bool continua = true;

    while (continua)
    {
        Console.WriteLine();
        Console.WriteLine("===== MENU AMMINISTRATORE =====");
        Console.WriteLine("1 - Visualizza catalogo");
        Console.WriteLine("2 - Aggiungi prodotto");
        Console.WriteLine("3 - Elimina prodotto");
        Console.WriteLine("4 - Modifica prezzo");
        Console.WriteLine("5 - Modifica quantità");
        Console.WriteLine("6 - Visualizza acquisti");
        Console.WriteLine("7 - Report prodotti");
        Console.WriteLine("0 - Indietro");

        string scelta = Console.ReadLine() ?? "";

        switch (scelta)
        {
            case "1":
                MostraCatalogo();
                break;

            case "2":
                try
                {
                    Console.Write("Codice: ");
                    string codice = Console.ReadLine() ?? "";

                    Console.Write("Nome: ");
                    string nome = Console.ReadLine() ?? "";

                    decimal prezzo =
                        LeggiPrezzoPositivo("Prezzo: ");

                    int quantita =
                        LeggiInteroPositivo("Quantità: ");

                    catalogoProdotti.AggiungiProdotto(
                        new Prodotto(
                            codice,
                            nome,
                            prezzo,
                            quantita));

                    Console.WriteLine("Prodotto aggiunto.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;

            case "3":
                Console.Write("Codice prodotto: ");
                string codiceElimina =
                    Console.ReadLine() ?? "";

                if (catalogoProdotti.EliminaProdotto(codiceElimina))
                {
                    Console.WriteLine("Prodotto eliminato.");
                }
                else
                {
                    Console.WriteLine("Prodotto non trovato.");
                }
                break;

            case "4":
                Console.Write("Codice prodotto: ");
                string codicePrezzo =
                    Console.ReadLine() ?? "";

                decimal nuovoPrezzo =
                    LeggiPrezzoPositivo("Nuovo prezzo: ");

                if (catalogoProdotti.ModificaPrezzoProdotto(
                    codicePrezzo,
                    nuovoPrezzo))
                {
                    Console.WriteLine("Prezzo aggiornato.");
                }
                else
                {
                    Console.WriteLine("Prodotto non trovato.");
                }
                break;

            case "5":
                Console.Write("Codice prodotto: ");
                string codiceQuantita =
                    Console.ReadLine() ?? "";

                Console.Write("Variazione quantità (+/-): ");

                if (int.TryParse(
                    Console.ReadLine(),
                    out int variazione))
                {
                    try
                    {
                        if (catalogoProdotti.ModificaQuantitaProdotto(
                            codiceQuantita,
                            variazione))
                        {
                            Console.WriteLine("Quantità aggiornata.");
                        }
                        else
                        {
                            Console.WriteLine("Prodotto non trovato.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Valore non valido.");
                }
                break;

            case "6":
                List<Acquisto> acquisti =
                    storicoAcquisti.OttieniTuttiGliAcquisti();

                if (acquisti.Count == 0)
                {
                    Console.WriteLine("Nessun acquisto registrato.");
                }
                else
                {
                    foreach (Acquisto acquisto in acquisti)
                    {
                        servizioNegozio.StampaAcquisto(acquisto);
                    }
                }
                break;

            case "7":
                servizioNegozio.StampaReportProdotti();
                break;

            case "0":
                continua = false;
                break;

            default:
                Console.WriteLine("Scelta non valida.");
                break;
        }
    }
}

   private void MostraCatalogo()
{
    List<Prodotto> prodotti = catalogoProdotti.OttieniTuttiIProdotti();

    if (prodotti.Count == 0)
    {
        Console.WriteLine("Nessun prodotto presente nel catalogo.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== CATALOGO =====");

    foreach (Prodotto prodotto in prodotti)
    {
        Console.WriteLine(
            $"{prodotto.CodiceProdotto} - " +
            $"{prodotto.Nome} | " +
            $"Prezzo: {prodotto.Prezzo:C} | " +
            $"Disponibili: {prodotto.QuantitaDisponibile}");
    }

    Console.WriteLine();
}

    private void MostraCarrello()
{
    List<ElementoCarrello> elementi = carrelloUtente.OttieniElementi();

    if (elementi.Count == 0)
    {
        Console.WriteLine("Il carrello è vuoto.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== CARRELLO =====");

    foreach (ElementoCarrello elemento in elementi)
    {
        Console.WriteLine(
            $"{elemento.ProdottoSelezionato.CodiceProdotto} - " +
            $"{elemento.ProdottoSelezionato.Nome} | " +
            $"Quantità: {elemento.QuantitaScelta} | " +
            $"Prezzo unitario: {elemento.PrezzoUnitario:C} | " +
            $"Totale: {elemento.CalcolaTotaleParziale():C}");
    }

    Console.WriteLine();
    Console.WriteLine($"Totale carrello: {carrelloUtente.CalcolaTotale():C}");
    Console.WriteLine();
}

    private void MostraStoricoUtente()
{
    Console.Write("Nome utente: ");
    string nomeUtente = Console.ReadLine() ?? string.Empty;

    List<Acquisto> acquisti =
        storicoAcquisti.OttieniAcquistiPerUtente(nomeUtente);

    if (acquisti.Count == 0)
    {
        Console.WriteLine("Nessun acquisto trovato.");
        return;
    }

    foreach (Acquisto acquisto in acquisti)
    {
        servizioNegozio.StampaAcquisto(acquisto);
    }
}
   private int LeggiInteroPositivo(string messaggio)
{
    int valore;

    while (true)
    {
        Console.Write(messaggio);

        string? input = Console.ReadLine();

        if (int.TryParse(input, out valore) && valore > 0)
        {
            return valore;
        }

        Console.WriteLine("Inserire un numero intero maggiore di zero.");
    }
}

    private decimal LeggiPrezzoPositivo(string messaggio)
{
    decimal prezzo;

    while (true)
    {
        Console.Write(messaggio);

        string? input = Console.ReadLine();

        if (decimal.TryParse(input, out prezzo) && prezzo > 0)
        {
            return prezzo;
        }

        Console.WriteLine("Inserire un prezzo maggiore di zero.");
    }
}
}

public interface IGestioneCatalogo
{
    void AggiungiProdotto(Prodotto prodotto);
    bool EliminaProdotto(string codiceProdotto);
    Prodotto? CercaProdottoPerCodice(string codiceProdotto);
    List<Prodotto> OttieniTuttiIProdotti();
    bool ModificaPrezzoProdotto(string codiceProdotto, decimal nuovoPrezzo);
    bool ModificaQuantitaProdotto(string codiceProdotto, int variazioneQuantita);
}

public interface IGestioneCarrello
{
    bool AggiungiAlCarrello(Prodotto prodotto, int quantita);
    bool ModificaQuantitaNelCarrello(string codiceProdotto, int nuovaQuantita);
    bool RimuoviDalCarrello(string codiceProdotto);
    void SvuotaCarrello();
    decimal CalcolaTotale();
    List<ElementoCarrello> OttieniElementi();
}

public interface IGestioneAcquisti
{
    void RegistraAcquisto(Acquisto acquisto);
    List<Acquisto> OttieniTuttiGliAcquisti();
    List<Acquisto> OttieniAcquistiPerUtente(string nomeUtente);
}

public class Prodotto
{
    public string CodiceProdotto { get; private set; }
    public string Nome { get; private set; }
    public decimal Prezzo { get; private set; }
    public int QuantitaDisponibile { get; private set; }
    public int QuantitaIniziale { get; private set; }

    public Prodotto(string codiceProdotto, string nome, decimal prezzo, int quantitaDisponibile)
    {
        CodiceProdotto = codiceProdotto;
        Nome = nome;
        Prezzo = prezzo;
        QuantitaDisponibile = quantitaDisponibile;
        QuantitaIniziale = quantitaDisponibile;
    }

    public void CambiaPrezzo(decimal nuovoPrezzo)
    {
        // Metodo già implementato: centralizza la validazione del prezzo.
        if (nuovoPrezzo <= 0)
        {
            throw new ArgumentException("Il prezzo deve essere maggiore di zero.");
        }

        Prezzo = nuovoPrezzo;
    }

    public void CambiaQuantita(int variazioneQuantita)
    {
        // Metodo già implementato: impedisce di portare il magazzino sotto zero.
        int nuovaQuantita = QuantitaDisponibile + variazioneQuantita;

        if (nuovaQuantita < 0)
        {
            throw new InvalidOperationException("La quantità disponibile non può diventare negativa.");
        }

        QuantitaDisponibile = nuovaQuantita;
    }

    public int CalcolaQuantitaVenduta()
    {
        // Metodo già implementato: serve per il report amministratore.
        return QuantitaIniziale - QuantitaDisponibile;
    }
}

public class ElementoCarrello
{
    public Prodotto ProdottoSelezionato { get; private set; }
    public int QuantitaScelta { get; private set; }
    public decimal PrezzoUnitario { get; private set; }

    public ElementoCarrello(Prodotto prodottoSelezionato, int quantitaScelta)
    {
        ProdottoSelezionato = prodottoSelezionato;
        QuantitaScelta = quantitaScelta;
        PrezzoUnitario = prodottoSelezionato.Prezzo;
    }

    public decimal CalcolaTotaleParziale()
    {
        // Metodo già implementato: evita di duplicare il calcolo del parziale.
        return PrezzoUnitario * QuantitaScelta;
    }

    public void CambiaQuantitaScelta(int nuovaQuantita)
{
    if (nuovaQuantita <= 0)
    {
        throw new ArgumentException(
            "La quantità deve essere maggiore di zero.");
    }

    QuantitaScelta = nuovaQuantita;
}
}

public class Acquisto
{
    public string NomeUtente { get; private set; }
    public List<ElementoAcquistato> ProdottiAcquistati { get; private set; }
    public decimal TotaleOrdine { get; private set; }
    public DateTime DataAcquisto { get; private set; }

    public Acquisto(string nomeUtente, List<ElementoAcquistato> prodottiAcquistati)
    {
        NomeUtente = nomeUtente;
        ProdottiAcquistati = prodottiAcquistati;
        DataAcquisto = DateTime.Now;
        TotaleOrdine = CalcolaTotaleOrdine();
    }

    private decimal CalcolaTotaleOrdine()
    {
        // Metodo già implementato: somma tutti i parziali dei prodotti acquistati.
        return ProdottiAcquistati.Sum(prodotto => prodotto.TotaleParziale);
    }
}

public class ElementoAcquistato
{
    public string CodiceProdotto { get; private set; }
    public string NomeProdotto { get; private set; }
    public int QuantitaAcquistata { get; private set; }
    public decimal PrezzoUnitario { get; private set; }
    public decimal TotaleParziale { get; private set; }

    public ElementoAcquistato(string codiceProdotto, string nomeProdotto, int quantitaAcquistata, decimal prezzoUnitario)
    {
        CodiceProdotto = codiceProdotto;
        NomeProdotto = nomeProdotto;
        QuantitaAcquistata = quantitaAcquistata;
        PrezzoUnitario = prezzoUnitario;
        TotaleParziale = prezzoUnitario * quantitaAcquistata;
    }
}

public class CatalogoProdotti : IGestioneCatalogo
{
    private readonly List<Prodotto> prodotti;

    public CatalogoProdotti()
    {
        prodotti = new List<Prodotto>();
    }

    public void AggiungiProdotto(Prodotto prodotto)
    {
        // Metodo già implementato: evita codici duplicati nel catalogo.
        bool codiceGiaPresente = prodotti.Any(p => p.CodiceProdotto == prodotto.CodiceProdotto);

        if (codiceGiaPresente)
        {
            throw new InvalidOperationException("Esiste già un prodotto con lo stesso codice.");
        }

        prodotti.Add(prodotto);
    }

    public bool EliminaProdotto(string codiceProdotto)
{
    Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

    if (prodotto == null)
    {
        return false;
    }

    prodotti.Remove(prodotto);
    return true;
}

    public Prodotto? CercaProdottoPerCodice(string codiceProdotto)
    {
        // Metodo già implementato: ricerca case-insensitive per rendere più comodo l'input da console.
        return prodotti.FirstOrDefault(prodotto =>
            prodotto.CodiceProdotto.Equals(codiceProdotto, StringComparison.OrdinalIgnoreCase));
    }

    public List<Prodotto> OttieniTuttiIProdotti()
    {
        // Metodo già implementato: restituisce una copia per proteggere la lista interna.
        return new List<Prodotto>(prodotti);
    }

    public bool ModificaPrezzoProdotto(string codiceProdotto, decimal nuovoPrezzo)
{
    Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

    if (prodotto == null)
    {
        return false;
    }

    prodotto.CambiaPrezzo(nuovoPrezzo);
    return true;
}

   public bool ModificaQuantitaProdotto(string codiceProdotto, int variazioneQuantita)
{
    Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

    if (prodotto == null)
    {
        return false;
    }

    prodotto.CambiaQuantita(variazioneQuantita);
    return true;
}
}

public class CarrelloUtente : IGestioneCarrello
{
    private readonly List<ElementoCarrello> elementiCarrello;

    public CarrelloUtente()
    {
        elementiCarrello = new List<ElementoCarrello>();
    }

    public bool AggiungiAlCarrello(Prodotto prodotto, int quantita)
{
    if (quantita <= 0)
    {
        return false;
    }

    if (quantita > prodotto.QuantitaDisponibile)
    {
        return false;
    }

    ElementoCarrello? elementoEsistente = elementiCarrello.FirstOrDefault(
        elemento => elemento.ProdottoSelezionato.CodiceProdotto.Equals(
            prodotto.CodiceProdotto,
            StringComparison.OrdinalIgnoreCase));

    if (elementoEsistente != null)
    {
        int nuovaQuantita = elementoEsistente.QuantitaScelta + quantita;

        if (nuovaQuantita > prodotto.QuantitaDisponibile)
        {
            return false;
        }

        elementoEsistente.CambiaQuantitaScelta(nuovaQuantita);
        return true;
    }

    elementiCarrello.Add(new ElementoCarrello(prodotto, quantita));
    return true;
}

   public bool ModificaQuantitaNelCarrello(string codiceProdotto, int nuovaQuantita)
{
    ElementoCarrello? elemento = elementiCarrello.FirstOrDefault(
        elementoCarrello => elementoCarrello.ProdottoSelezionato.CodiceProdotto.Equals(
            codiceProdotto,
            StringComparison.OrdinalIgnoreCase));

    if (elemento == null)
    {
        return false;
    }

    if (nuovaQuantita <= 0)
    {
        return false;
    }

    if (nuovaQuantita > elemento.ProdottoSelezionato.QuantitaDisponibile)
    {
        return false;
    }

    elemento.CambiaQuantitaScelta(nuovaQuantita);
    return true;
}

   public bool RimuoviDalCarrello(string codiceProdotto)
{
    ElementoCarrello? elemento = elementiCarrello.FirstOrDefault(
        elementoCarrello => elementoCarrello.ProdottoSelezionato.CodiceProdotto.Equals(
            codiceProdotto,
            StringComparison.OrdinalIgnoreCase));

    if (elemento == null)
    {
        return false;
    }

    elementiCarrello.Remove(elemento);
    return true;
}

    public void SvuotaCarrello()
    {
        // Metodo già implementato: cancella tutti gli elementi del carrello.
        elementiCarrello.Clear();
    }

    public decimal CalcolaTotale()
    {
        // Metodo già implementato: ricalcola sempre il totale dai parziali correnti.
        return elementiCarrello.Sum(elemento => elemento.CalcolaTotaleParziale());
    }

    public List<ElementoCarrello> OttieniElementi()
    {
        // Metodo già implementato: restituisce una copia per evitare modifiche esterne dirette.
        return new List<ElementoCarrello>(elementiCarrello);
    }
}

public class StoricoAcquisti : IGestioneAcquisti
{
    private readonly List<Acquisto> acquisti;

    public StoricoAcquisti()
    {
        acquisti = new List<Acquisto>();
    }

    public void RegistraAcquisto(Acquisto acquisto)
    {
        // Metodo già implementato: conserva l'acquisto in memoria durante l'esecuzione.
        acquisti.Add(acquisto);
    }

    public List<Acquisto> OttieniTuttiGliAcquisti()
    {
        // Metodo già implementato: restituisce una copia dello storico.
        return new List<Acquisto>(acquisti);
    }

   public List<Acquisto> OttieniAcquistiPerUtente(string nomeUtente)
{
    return acquisti
        .Where(acquisto =>
            acquisto.NomeUtente.Equals(
                nomeUtente,
                StringComparison.OrdinalIgnoreCase))
        .ToList();
}
}

public class ServizioNegozio
{
    private readonly CatalogoProdotti catalogoProdotti;
    private readonly CarrelloUtente carrelloUtente;
    private readonly StoricoAcquisti storicoAcquisti;

    public ServizioNegozio(CatalogoProdotti catalogoProdotti, CarrelloUtente carrelloUtente, StoricoAcquisti storicoAcquisti)
    {
        this.catalogoProdotti = catalogoProdotti;
        this.carrelloUtente = carrelloUtente;
        this.storicoAcquisti = storicoAcquisti;
    }

    public bool AggiungiProdottoAlCarrello(string codiceProdotto, int quantita)
{
    Prodotto? prodotto = catalogoProdotti.CercaProdottoPerCodice(codiceProdotto);

    if (prodotto == null)
    {
        return false;
    }

    return carrelloUtente.AggiungiAlCarrello(prodotto, quantita);
}

    public Acquisto ConfermaAcquisto(string nomeUtente)
    {
        // Metodo già implementato: è una delle logiche più importanti della traccia.
        // 1. impedisce acquisti con carrello vuoto;
        // 2. ricontrolla la disponibilità prima di scalare il magazzino;
        // 3. crea una copia dei dati acquistati;
        // 4. aggiorna il magazzino;
        // 5. registra l'acquisto nello storico;
        // 6. svuota il carrello.
        List<ElementoCarrello> elementi = carrelloUtente.OttieniElementi();

        if (elementi.Count == 0)
        {
            throw new InvalidOperationException("Non è possibile confermare un acquisto con carrello vuoto.");
        }

        foreach (ElementoCarrello elemento in elementi)
        {
            if (elemento.QuantitaScelta <= 0)
            {
                throw new InvalidOperationException("Nel carrello è presente una quantità non valida.");
            }

            if (elemento.QuantitaScelta > elemento.ProdottoSelezionato.QuantitaDisponibile)
            {
                throw new InvalidOperationException("La quantità richiesta supera la disponibilità di magazzino.");
            }
        }

        List<ElementoAcquistato> prodottiAcquistati = elementi
            .Select(elemento => new ElementoAcquistato(
                elemento.ProdottoSelezionato.CodiceProdotto,
                elemento.ProdottoSelezionato.Nome,
                elemento.QuantitaScelta,
                elemento.PrezzoUnitario))
            .ToList();

        foreach (ElementoCarrello elemento in elementi)
        {
            elemento.ProdottoSelezionato.CambiaQuantita(-elemento.QuantitaScelta);
        }

        Acquisto acquisto = new Acquisto(nomeUtente, prodottiAcquistati);
        storicoAcquisti.RegistraAcquisto(acquisto);
        carrelloUtente.SvuotaCarrello();

        return acquisto;
    }

    public List<ReportProdotto> CreaReportProdotti()
    {
        // Metodo già implementato: prepara il report richiesto per l'amministratore.
        return catalogoProdotti.OttieniTuttiIProdotti()
            .Select(prodotto => new ReportProdotto(
                prodotto.CodiceProdotto,
                prodotto.Nome,
                prodotto.QuantitaIniziale,
                prodotto.CalcolaQuantitaVenduta(),
                prodotto.QuantitaDisponibile))
            .ToList();
    }

   public void StampaAcquisto(Acquisto acquisto)
{
    Console.WriteLine("=================================");
    Console.WriteLine($"Utente: {acquisto.NomeUtente}");
    Console.WriteLine($"Data: {acquisto.DataAcquisto}");
    Console.WriteLine();

    foreach (ElementoAcquistato prodotto in acquisto.ProdottiAcquistati)
    {
        Console.WriteLine(
            $"{prodotto.CodiceProdotto} - " +
            $"{prodotto.NomeProdotto} | " +
            $"Qta: {prodotto.QuantitaAcquistata} | " +
            $"Prezzo: {prodotto.PrezzoUnitario:C} | " +
            $"Totale: {prodotto.TotaleParziale:C}");
    }

    Console.WriteLine();
    Console.WriteLine($"Totale ordine: {acquisto.TotaleOrdine:C}");
    Console.WriteLine("=================================");
}

    public void StampaReportProdotti()
{
    List<ReportProdotto> report = CreaReportProdotti();

    foreach (ReportProdotto prodotto in report)
    {
        Console.WriteLine(
            $"{prodotto.CodiceProdotto} - " +
            $"{prodotto.NomeProdotto} | " +
            $"Iniziale: {prodotto.QuantitaIniziale} | " +
            $"Venduta: {prodotto.QuantitaVenduta} | " +
            $"Disponibile: {prodotto.QuantitaDisponibile}");
    }
}
}

public class ReportProdotto
{
    public string CodiceProdotto { get; private set; }
    public string NomeProdotto { get; private set; }
    public int QuantitaIniziale { get; private set; }
    public int QuantitaVenduta { get; private set; }
    public int QuantitaDisponibile { get; private set; }

    public ReportProdotto(string codiceProdotto, string nomeProdotto, int quantitaIniziale, int quantitaVenduta, int quantitaDisponibile)
    {
        CodiceProdotto = codiceProdotto;
        NomeProdotto = nomeProdotto;
        QuantitaIniziale = quantitaIniziale;
        QuantitaVenduta = quantitaVenduta;
        QuantitaDisponibile = quantitaDisponibile;
    }
}
