namespace TestEnterpriseServiceBus.Adapters.RPO.Http.Responses;

public class Subject
{
	// Summary:
	//     Jednoznacny identifikator subjektu v ramci registra pravnickych osob. Atribut
	//     potrebny pre GET REQUEST detailu.
	public int? IdSubjectRpo { get; set; }

	//
	// Summary:
	//     Datum vzniku pravnickej osoby
	public DateTime? Establishment { get; set; }

	//
	// Summary:
	//     Datum zaniku pravnickej osoby
	public DateTime? Termination { get; set; }

	//
	// Summary:
	//     Dátum poslednej aktualizácie údajov o PO v databáze, t.j. kedy začala byť aktuálna
	//     verzia záznamu dostupná.
	public DateTime? DbModificationDate { get; set; }

	//
	// Summary:
	//     Zoznam identifikacnych cisel organizacie (vratane historickych, uz neplatnych
	//     zaznamov).
	public IEnumerable<IValidable>? Identifiers { get; set; }

	//
	// Summary:
	//     Sucasne (aktualne platne) ICO pravnickej osoby
	public string? IdentifierValid { get; set; }

	//
	// Summary:
	//     Zoznam nazov pravnickej osoby (vratane historickych, uz neplatnych zaznamov).
	public IEnumerable<IValidable>? FullNames { get; set; }

	//
	// Summary:
	//     Sucasny (aktualne platny) nazov pravnickej osoby
	public string? FullNameValid { get; set; }

	//
	// Summary:
	//     Zoznam vsetkych adries pravnickej osoby (vratane historickych, uz neplatnych
	//     zaznamov).
	public IEnumerable<Address>? Addresses { get; set; }

	//
	// Summary:
	//     Sucasny (aktualne platny) zoznam adries pravnickej osoby.
	public IEnumerable<Address>? AddressesValid { get; set; }

	//
	// Summary:
	//     Kod registra, ktory obsahuje referencne udaje daneho subjektu.
	public int? SourceRegisterCode { get; set; }

	//
	// Summary:
	//     Nazov registra, ktory obsahuje referencne udaje daneho subjektu.
	public string? SourceRegister { get; set; }

	//
	// Summary:
	//     Enum hodnota registra, ktory obsahuje referencne udaje daneho subjektu.
	public StaticSources.SourceRegisterEnum SourceRegisterEnum { get; set; }

	//
	// Summary:
	//     Zoznam opravneni konat v mene pravnickej osoby (vratane historickych, uz neplatnych
	//     zaznamov).
	public IEnumerable<IValidable>? Authorizations { get; set; }

	//
	// Summary:
	//     Sucasne (aktualne platne) opravnenie konat v mene pravnickej osoby.
	public string? AuthorizationValid { get; set; }

	//
	// Summary:
	//     Satutarny organ pravnickej osoby. Obsahuje zoznam statutarov pravnickej osoby
	//     (vratane historickych, uz neplatnych zaznamov).
	public IEnumerable<StatutoryBody>? StatutoryBody { get; set; }

	//
	// Summary:
	//     Sucasny satutarny organ pravnickej osoby. Obsahuje zoznam sucasnych statutarov
	//     pravnickej osoby.
	public IEnumerable<StatutoryBody>? StatutoryBodyValid { get; set; }

	//
	// Summary:
	//     Zoznam zainteresovanych osob pravnickej osoby (vratane historickych udajov).
	public IEnumerable<Stakeholder>? Stakeholders { get; set; }

	//
	// Summary:
	//     Sucasny (aktualne platny) zoznam zainteresovanych osob pravnickej osoby.
	public IEnumerable<Stakeholder>? StakeholdersValid { get; set; }

	//
	// Summary:
	//     Spisová značka právnickej osoby.
	public string? RegistrationNumber { get; set; }
}

//
// Summary:
//     Zainteresovana osoba subjektu, resp. pravnickej osoby napr. clen dozornej rady.
//     Moze byt fyzicka aj pravnicka osoba.
public class Stakeholder : ValidableValue
{
	//
	// Summary:
	//     Krstne meno ak sa jedna o fyzicku osobu
	public string? FirstName { get; set; }

	//
	// Summary:
	//     Priezvisko ak sa jedna o fyzicku osobu
	public string? LastName { get; set; }

	//
	// Summary:
	//     Rodne priezvisko ak sa jedna o fyzicku osobu
	public string? LastNameBirth { get; set; }

	//
	// Summary:
	//     Nazov zainteresovanej osoby ak sa nejedna o fyzicku osobu.
	public string? FullName { get; set; }

	//
	// Summary:
	//     Idnetifikator zainteresovanej osoby ak sa nejedna o fyzicku osobu (napr. ICO).
	public string? Identifier { get; set; }

	//
	// Summary:
	//     Kod ciselnikovej hodnoty ciselnika CL010109. Jedna sa o typ/funkciu zainteresovanej
	//     osoby v ramci subjektu.
	public string? StakeholderTypeCode { get; set; }

	//
	// Summary:
	//     Nazov typu/funkcie zainteresovanej osoby v ramci subjektu. Jedna sa o ciselnikovu
	//     hodnotu ciselnika CL010109.
	public string? StakeholderType { get; set; }

	//
	// Summary:
	//     ENUM typu zaintersovanej osoby
	public StaticSources.StakeholderTypeEnum? StakeholderTypeEnum { get; set; }

	//
	// Summary:
	//     Adresa zainteresovanej osoby. Vzdy len jedna a moze byt neplatna, tzn. validTo
	//     != null.
	public Address? Address { get; set; }
}

public interface IValidable
{
	DateTime? ValidFrom { get; set; }

	DateTime? ValidTo { get; set; }

	string? Value { get; set; }
}

public class ValidableValue : IValidable
{
	public string? Value { get; set; }

	public DateTime? ValidFrom { get; set; }

	public DateTime? ValidTo { get; set; }
}


public class StatutoryBody : ValidableValue
{
	//
	// Summary:
	//     Krstne meno clena statutarneho organu, ak sa jedna o fyzicku osobu
	public string? FirstName { get; set; }

	//
	// Summary:
	//     Priezvisko clena statutarneho organu, ak sa jedna o fyzicku osobu
	public string? LastName { get; set; }

	//
	// Summary:
	//     Rodne priezvisko clena statutarneho organu, ak sa jedna o fyzicku osobu
	public string? LastNameBirth { get; set; }

	//
	// Summary:
	//     Nazov clena statutarneho organu, ak sa nejedna o fyzicku osobu.
	public string? FullName { get; set; }

	//
	// Summary:
	//     Idnetifikator clena statutarneho organu, ak sa nejedna o fyzicku osobu (napr.
	//     ICO).
	public string? Identifier { get; set; }

	//
	// Summary:
	//     Kod funkcie člena kolektívneho štatutárneho orgánu (číselník CL010470) POZOR
	//     - TATO HODNOTA NIE JE ZO STRANY RPO ZATIAL NAPLNANA.
	public string? StatutoryBodyMemberCode { get; set; }

	//
	// Summary:
	//     Nazov funkcie člena kolektívneho štatutárneho orgánu (číselník CL010470).
	public string? StatutoryBodyMember { get; set; }

	//
	// Summary:
	//     ENUM funkcie člena kolektívneho štatutárneho orgánu (číselník CL010470) POZOR
	//     - Z DOVODU NENAPLNANIA StatutoryBodyMemberCode ZO STRANY RPO JE TATO HODNOTA
	//     VZDY StatutoryMemberTypeEnum.Neurcene
	public StaticSources.StatutoryMemberTypeEnum StatutoryBodyMemberEnum { get; set; }

	//
	// Summary:
	//     Kod typu štatutárneho orgánu (číselník CL010113)
	public string? StatutoryBodyTypeCode { get; set; }

	//
	// Summary:
	//     Nazov typu štatutárneho orgánu (číselník CL010113)
	public string? StatutoryBodyType { get; set; }

	//
	// Summary:
	//     ENUM typu štatutárneho orgánu (číselník CL010113)
	public StaticSources.StatutoryBodyTypeEnum StatutoryBodyTypeEnum { get; set; }

	//
	// Summary:
	//     Adresa zainteresovanej osoby. Vzdy len jedna a moze byt neplatna, tzn. validTo
	//     != null
	public Address? Address { get; set; }
}

public class Address : ValidableValue
{
	public string? FullAddress { get; set; }

	public string? StreetName { get; set; }

	public string? StreetNumber { get; set; }

	public string? PostalCodes { get; set; }

	public string? City { get; set; }

	public string? Country { get; set; }

	public string? CountryCode { get; set; }
}


public class StaticSources
{
	//
	// Summary:
	//     Funkcia člena kolektívneho štatutárneho orgánu (číselník CL010470)
	public enum StatutoryMemberTypeEnum
	{
		Neurcene,
		PredsedaPredstavenstva,
		PodpredsedaPredstavenstva,
		ClenPredstavenstva,
		PredsedaSpravnejRady,
		PodpredsedaSpravnejRady,
		ClenSpravnejRady,
		ClenDruzstvaPoverenyClenskouSchodzou
	}

	//
	// Summary:
	//     Typ zainteresovanej osoby (číselník CL010109)
	public enum StakeholderTypeEnum
	{
		Neurcene = 0,
		Spolocnik = 1,
		Komplementar = 2,
		Komanditista = 3,
		Konatel = 4,
		PredsedaPredstavenstva = 5,
		PodpredsedaPredstavenstva = 6,
		ClenPredstavenstva = 7,
		ClenDozornejRady = 8,
		GeneralnyRiaditel = 9,
		Riaditel = 10,
		ZastupcaRiaditela = 11,
		VykonnyRiaditel = 12,
		Rektor = 13,
		Guverner = 14,
		VeduciOrganizacnejZlozky = 15,
		Spravca = 16,
		ClenVyboru = 17,
		Predseda = 18,
		Starosta = 19,
		Primator = 20,
		Minister = 21,
		VeduciUradu = 22,
		VeduciKancelarie = 23,
		Prokurista = 24,
		PredsedaSpravnejRady = 25,
		PodpredsedaSpravnejRady = 26,
		ClenSpravnejRady = 27,
		Zakladatel = 29,
		Zriadovatel = 30,
		Navrhovatel = 31,
		ClenripravnehoVyboru = 32,
		ZodpovednyZastupca = 33,
		SpravcaKonkurznejPodstaty = 34,
		VyrovnavaciSpravca = 36,
		InaZainteresovanaOsoba = 37,
		SpravcaRestrukturalizacnehoKonania = 39,
		ZriadovatelZO = 87,
		SpravcaVyrovnaciehoKonania = 88,
		VOSKtorejImanieSaPrebera = 89,
		ProkuristaZahranicnejOsoby = 90,
		ClenEZHZ = 91,
		SpolocnikVOSKtoryPrevzalJejImanie = 92,
		LikvidatorZahranicnejOsoby = 93,
		JedinyAkcionarAS = 94,
		ZakladatelStatnehoPodniku = 95,
		VeduciPodnikuOrgZlozkyZahrOsoby = 96,
		VeduciOdstepnehoZavoduInejOrgZlozky = 97,
		ClenDozornehoOrganu = 98,
		SpolocnikVosSro = 99,
		SpravcaNaVykonNutenejSpravy = 86
	}

	//
	// Summary:
	//     Typ štatutárneho orgánu (číselník CL010113)
	public enum StatutoryBodyTypeEnum
	{
		Neurcene = 0,
		Spolocnik = 1,
		Komplementar = 2,
		Konatel = 3,
		PredsedaPredstavenstva = 4,
		ClenPredstavenstva = 5,
		PodpredsedaPredstavenstva = 6,
		VeduciOrganizacnejZlozky = 7,
		GeneralnyRiaditel = 8,
		Riaditel = 9,
		Spravca = 10,
		ClenVyboru = 11,
		Predseda = 12,
		VykonnyRiaditel = 13,
		Starosta = 14,
		Primator = 15,
		Minister = 16,
		InyStatutarnyOrgan = 17,
		Likvidator = 18,
		Podnikatel = 19,
		ZastupcaPodnikatela = 20,
		PoverenaOsoba = 21,
		StatutOrganStatutaraPravnickaOsoba = 90,
		ClenStatutOrganuZahranicnejOsoby = 91,
		ClenStatutOrganu = 92,
		SpravnaRada = 93,
		ClenDruzstvaPoverenyClenSchodzou = 94,
		Podpredseda = 95,
		VykonnyVybor = 96,
		VeduciPodnikuOrgZlozky = 97,
		ZastupcaRiaditela = 98,
		Predstavenstvo = 99,
		Prokurista = 84,
		VeduciOdstepnehoZavodu = 85,
		LikvidatorZahranicnejOsoby = 86,
		SpravcaNaVykonNutenejSpravy = 87,
		ProkuristaZahranicnejOsoby = 88,
		ClenSpravnejRady = 89,
		Rektor = 22,
		ClenskaSchodza = 23,
		VeduciUradu = 24,
		ZastupcaGeneralnehoRiaditela = 25,
		Guverner = 26,
		Viceguverner = 27,
		Prezident = 28,
		RegionalnyHygienik = 29,
		GeneralnyKonzul = 30,
		Konzul = 31,
		Veľvyslanec = 32,
		GeneralnyTajomnikSlužobnehoUradu = 33,
		GeneralnyTajomnik = 34,
		GeneralnySekretar = 35,
		Arcibiskup = 36,
		Biskup = 37,
		Farár = 38,
		StatnyTajomnik = 39,
		ClenRadyUrcenyRadou = 40,
		GeneralnyProkurator = 41
	}

	//
	// Summary:
	//     Názov zdrojového registra (číselník CL010112)
	public enum SourceRegisterEnum
	{
		Neurcene = 0,
		ObchodnyRegister = 1,
		ZivnostenskyRegister = 2,
		RegisterPolitickychStranAHnuti = 3,
		RegisterPrevadzkovatelovCestnejDopravy = 10,
		RegisterZdravotnickychPracovnikov = 11,
		RegisterZdruzeniObci = 12,
		RegisterSpolocVlastníkovBytovNebytPriestorov = 13,
		RegisterZaujmovychZdruzeni = 14,
		RegisterVysokychSkol = 15,
		EvidenciaPodnikovElSieteSluzby = 16,
		RegisterAgenturDocasnehoZamestnavania = 20,
		RegisterAgenturPodporovanhoZamestnavania = 21,
		RegisterCirkviANabozSpolocnosti = 22,
		RegisterEUZostkupeniUzemnejSpoluprace = 23,
		RegisterPostovychPodnikov = 24,
		EvidenciaSamostHospodarRolnikov = 30,
		EvidenciaPovoleniLetovejDopravy = 31,
		ZoznamAutorizArchitektov = 37
	}
}