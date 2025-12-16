using System;

namespace GDFFoundation
{
    #if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_ANDROID
    /// <summary>
    /// Represents a collection of countries.
    /// This enumeration can be used to specify countries within the application.
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(CountryConverter))]
    #endif
    [Newtonsoft.Json.JsonConverter(typeof(NewtonCountryConverter))]
    public enum Country : short
    {
        /// <summary>
        /// Represents an undefined or uninitialized value within the <see cref="Country"/> enumeration.
        /// This member can be used as a default or placeholder value when no specific country is applicable.
        /// </summary>
        None                                    = -1,

        // FullName

        /// <summary>
        /// Represents Afghanistan in the <see cref="Country"/> enumeration.
        /// </summary>
        Afghanistan                             = 004,

        /// <summary>
        /// Represents the country Albania within the <see cref="Country"/> enum.
        /// </summary>
        Albania                                 = 008,

        /// <summary>
        /// Represents a placeholder or marker type for the concept of Antarctica.
        /// This class is currently empty and may be used for future implementations
        /// related to Antarctica.
        /// </summary>
        Antarctica                              = 010,

        /// <summary>
        /// Represents the country Algeria within the <see cref="Country"/> enumeration.
        /// </summary>
        Algeria                                 = 012,

        /// <summary>
        /// Represents the country or territory of American Samoa, a United States territory located in the South Pacific.
        /// This enum member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        AmericanSamoa                           = 016,

        /// <summary>
        /// Represents the country Andorra in the <see cref="Country"/> enum.
        /// </summary>
        Andorra                                 = 020,

        /// <summary>
        /// Represents the country of Angola.
        /// </summary>
        Angola                                  = 024,

        /// <summary>
        /// Represents the country Antigua and Barbuda in the <see cref="Country"/> enumeration.
        /// </summary>
        AntiguaAndBarbuda                       = 028,

        /// <summary>
        /// Represents the country Azerbaijan within the <see cref="Country"/> enum.
        /// </summary>
        Azerbaijan                              = 031,

        /// <summary>
        /// Represents the country Argentina in the <see cref="Country"/> enumeration.
        /// </summary>
        Argentina                               = 032,

        /// <summary>
        /// Represents the country Australia in the <see cref="Country"/> enumeration.
        /// </summary>
        Australia                               = 036,

        /// <summary>
        /// Represents the country Austria in the <see cref="Country"/> enumeration.
        /// </summary>
        Austria                                 = 040,

        /// <summary>
        /// Represents the country Bahamas in the <see cref="Country"/> enumeration.
        /// </summary>
        Bahamas                                 = 044,

        /// <summary>
        /// Represents the country Bahrain in the <see cref="Country"/> enumeration.
        /// </summary>
        Bahrain                                 = 048,

        /// <summary>
        /// Represents the country Bangladesh as an enumeration member of <see cref="Country"/>.
        /// </summary>
        Bangladesh                              = 050,

        /// <summary>
        /// Represents the country Armenia as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Armenia                                 = 051,

        /// <summary>
        /// Represents the country Barbados in the <see cref="Country"/> enumeration.
        /// </summary>
        Barbados                                = 052,

        /// <summary>
        /// Represents the country Belgium in the <see cref="Country"/> enumeration.
        /// </summary>
        Belgium                                 = 056,

        /// <summary>
        /// Represents the country Bermuda in the <see cref="Country"/> enumeration.
        /// </summary>
        Bermuda                                 = 060,

        /// <summary>
        /// Represents a class named <see cref="Bhutan"/>.
        /// The class functionality or purpose is not specified in the provided code.
        /// </summary>
        Bhutan                                  = 064,

        /// <summary>
        /// Represents the country Bolivia in the <see cref="Country"/> enumeration.
        /// </summary>
        Bolivia                                 = 068,

        /// <summary>
        /// Represents the country Bosnia and Herzegovina in the <see cref="Country"/> enumeration.
        /// </summary>
        BosniaAndHerzegovina                    = 070,

        /// <summary>
        /// Represents the country Botswana in the <see cref="Country"/> enumeration.
        /// </summary>
        Botswana                                = 072,

        /// <summary>
        /// Represents the country Bouvet Island in the <see cref="Country"/> enumeration.
        /// </summary>
        BouvetIsland                            = 074,

        /// <summary>
        /// Represents the country Brazil as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Brazil                                  = 076,

        /// <summary>
        /// Represents the country Belize in the <see cref="Country"/> enumeration.
        /// </summary>
        Belize                                  = 084,

        /// <summary>
        /// Represents the British Indian Ocean Territory country/region.
        /// </summary>
        /// <remarks>
        /// This value is a member of the <see cref="Country"/> enum, which is used to represent countries and regions.
        /// </remarks>
        BritishIndianOceanTerritory             = 086,

        /// <summary>
        /// Represents the country Solomon Islands in the <see cref="Country"/> enum.
        /// </summary>
        SolomonIslands                          = 090,

        /// <summary>
        /// Represents the British Virgin Islands, a British Overseas Territory in the Caribbean.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="Country"/> enum, which defines a set of countries or territories.
        /// </remarks>
        VirginIslandsBritish                    = 092,

        /// <summary>
        /// Represents the country Brunei Darussalam in the <see cref="Country"/> enumeration.
        /// </summary>
        BruneiDarussalam                        = 096,

        /// <summary>
        /// Represents the country Bulgaria in the enumeration <see cref="Country"/>.
        /// </summary>
        Bulgaria                                = 100,

        /// <summary>
        /// Represents the country Myanmar in the <see cref="Country"/> enum.
        /// </summary>
        Myanmar                                 = 104,

        /// <summary>
        /// Represents the country Burundi in the <see cref="Country"/> enumeration.
        /// </summary>
        Burundi                                 = 108,

        /// <summary>
        /// Represents the country Belarus in the <see cref="Country"/> enumeration.
        /// </summary>
        Belarus                                 = 112,

        /// <summary>
        /// Represents the country Cambodia as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Cambodia                                = 116,

        /// <summary>
        /// Represents the country Cameroon in the <see cref="Country"/> enumeration.
        /// </summary>
        Cameroon                                = 120,

        /// <summary>
        /// Represents Canada as a country enumeration value within the <see cref="Country"/> enum.
        /// </summary>
        Canada                                  = 124,

        /// <summary>
        /// Represents the country Cabo Verde in the <see cref="Country"/> enumeration,
        /// which defines a list of countries for identification or categorization purposes.
        /// </summary>
        CaboVerde                               = 132,

        /// <summary>
        /// Represents the country Cayman Islands as a member of the <see cref="Country"/> enum.
        /// </summary>
        CaymanIslands                           = 136,

        /// <summary>
        /// Represents the country Central African Republic in the <see cref="Country"/> enumeration.
        /// </summary>
        CentralAfricanRepublic                  = 140,

        /// <summary>
        /// Represents the country Sri Lanka in the <see cref="Country"/> enumeration.
        /// </summary>
        SriLanka                                = 144,

        /// <summary>
        /// Represents the country Chad in the <see cref="Country"/> enumeration.
        /// </summary>
        Chad                                    = 148,

        /// <summary>
        /// Represents the country Chile in the <see cref="Country"/> enumeration.
        /// </summary>
        Chile                                   = 152,

        /// <summary>
        /// Represents the country China in the <see cref="Country"/> enumeration.
        /// </summary>
        China                                   = 156,

        /// <summary>
        /// Represents the country Taiwan in the <see cref="Country"/> enumeration.
        /// </summary>
        Taiwan                                  = 158,

        /// <summary>
        /// Represents the country Christmas Island within the <see cref="Country"/> enumeration.
        /// </summary>
        ChristmasIsland                         = 162,

        /// <summary>
        /// Represents the Cocos (Keeling) Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        CocosIslands                            = 166,

        /// <summary>
        /// Represents the country Colombia in the <see cref="Country"/> enumeration.
        /// </summary>
        Colombia                                = 170,

        /// <summary>
        /// Represents the country Comoros as a member of the <see cref="Country"/> enum.
        /// </summary>
        Comoros                                 = 174,

        /// <summary>
        /// Represents a container for the region-specific implementation or business logic related to Mayotte.
        /// This class could include properties, methods, and functions pertinent to Mayotte.
        /// </summary>
        Mayotte                                 = 175,

        /// <summary>
        /// Represents the country Congo as a member of the <see cref="Country"/> enum.
        /// </summary>
        Congo                                   = 178,

        /// <summary>
        /// Represents the Democratic Republic of the Congo in the <see cref="Country"/> enumeration.
        /// </summary>
        CongoDemocraticRepublic                 = 180,

        /// <summary>
        /// Represents the country Cook Islands in the <see cref="Country"/> enum.
        /// The Cook Islands is a self-governing island country in free association with New Zealand,
        /// located in the South Pacific Ocean.
        /// </summary>
        CookIslands                             = 184,

        /// <summary>
        /// Represents the country Costa Rica in the <see cref="Country"/> enum.
        /// </summary>
        CostaRica                               = 188,

        /// <summary>
        /// Represents the country Croatia in the <see cref="Country"/> enumeration.
        /// </summary>
        Croatia                                 = 191,

        /// <summary>
        /// Represents the country Cuba as a member of the <see cref="Country"/> enum.
        /// </summary>
        Cuba                                    = 192,

        /// <summary>
        /// Represents the country Cyprus in the <see cref="Country"/> enumeration.
        /// </summary>
        Cyprus                                  = 196,

        /// <summary>
        /// Represents the country Czechia in the <see cref="Country"/> enumeration.
        /// </summary>
        Czechia                                 = 203,

        /// <summary>
        /// Represents the country Benin in the <see cref="Country"/> enumeration.
        /// </summary>
        Benin                                   = 204,

        /// <summary>
        /// Represents the country Denmark in the <see cref="Country"/> enumeration.
        /// </summary>
        Denmark                                 = 208,

        /// <summary>
        /// Represents the country Dominica in the <see cref="Country"/> enumeration.
        /// </summary>
        Dominica                                = 212,

        /// <summary>
        /// Represents the Dominican Republic in the <see cref="Country"/> enumeration.
        /// </summary>
        DominicanRepublic                       = 214,

        /// <summary>
        /// Represents the country Ecuador in the <see cref="Country"/> enumeration.
        /// </summary>
        Ecuador                                 = 218,

        /// <summary>
        /// Represents the country El Salvador in the <see cref="Country"/> enumeration.
        /// </summary>
        ElSalvador                              = 222,

        /// <summary>
        /// Represents the country Equatorial Guinea in the <see cref="Country"/> enumeration.
        /// </summary>
        EquatorialGuinea                        = 226,

        /// <summary>
        /// Represents the country Ethiopia in the <see cref="Country"/> enum.
        /// </summary>
        Ethiopia                                = 231,

        /// <summary>
        /// Represents the country Eritrea in the <see cref="Country"/> enumeration.
        /// </summary>
        Eritrea                                 = 232,

        /// <summary>
        /// Represents the <see cref="Estonia"/> class.
        /// </summary>
        Estonia                                 = 233,

        /// <summary>
        /// Represents the Faroe Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        FaroeIslands                            = 234,

        /// <summary>
        /// Represents the country Falkland Islands.
        /// </summary>
        FalklandIslands                         = 238,

        /// <summary>
        /// Represents the country South Georgia and the South Sandwich Islands within the <see cref="Country"/> enumeration.
        /// </summary>
        SouthGeorgiaAndTheSouthSandwichIslands  = 239,

        /// <summary>
        /// Represents the country Fiji within the <see cref="Country"/> enumeration.
        /// </summary>
        Fiji                                    = 242,

        /// <summary>
        /// Represents the country Finland in the <see cref="Country"/> enumeration.
        /// </summary>
        Finland                                 = 246,

        /// <summary>
        /// Represents the Åland Islands in the <see cref="Country"/> enum.
        /// </summary>
        AlandIslands                            = 248,

        /// <summary>
        /// Represents the country France in the <see cref="Country"/> enumeration.
        /// </summary>
        France                                  = 250,

        /// <summary>
        /// Represents the country French Guiana in the <see cref="Country"/> enumeration.
        /// </summary>
        FrenchGuiana                            = 254,

        /// <summary>
        /// Represents the country French Polynesia in the <see cref="Country"/> enum.
        /// </summary>
        FrenchPolynesia                         = 258,

        /// <summary>
        /// Represents the country French Southern Territories in the <see cref="Country"/> enum.
        /// </summary>
        FrenchSouthernTerritories               = 260,

        /// <summary>
        /// Represents the country Djibouti in the <see cref="Country"/> enumeration.
        /// </summary>
        Djibouti                                = 262,

        /// <summary>
        /// Represents the country Gabon in the <see cref="Country"/> enum.
        /// </summary>
        Gabon                                   = 266,

        /// <summary>
        /// Represents the country Georgia as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Georgia                                 = 268,

        /// <summary>
        /// Represents the country Gambia in the <see cref="Country"/> enumeration.
        /// </summary>
        Gambia                                  = 270,

        /// <summary>
        /// Represents the country Palestine in the <see cref="Country"/> enumeration.
        /// </summary>
        Palestine                               = 275,

        /// <summary>
        /// Represents Germany as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Germany                                 = 276,

        /// <summary>
        /// Represents the country Ghana in the <see cref="Country"/> enumeration.
        /// </summary>
        Ghana                                   = 288,

        /// <summary>
        /// Represents Gibraltar as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Gibraltar                               = 292,

        /// <summary>
        /// Represents the country Kiribati in the <see cref="Country"/> enumeration.
        /// </summary>
        Kiribati                                = 296,

        /// <summary>
        /// Represents Greece as a member of the <see cref="Country"/> enum.
        /// </summary>
        Greece                                  = 300,

        /// <summary>
        /// Represents the country Greenland in the <see cref="Country"/> enum.
        /// </summary>
        Greenland                               = 304,

        /// <summary>
        /// Represents the country Grenada in the <see cref="Country"/> enumeration.
        /// </summary>
        Grenada                                 = 308,

        /// <summary>
        /// Represents the country Guadeloupe in the <see cref="Country"/> enumeration.
        /// </summary>
        Guadeloupe                              = 312,

        /// <summary>
        /// Represents the country Guam in the <see cref="Country"/> enumeration.
        /// </summary>
        Guam                                    = 316,

        /// <summary>
        /// Represents the country Guatemala in the <see cref="Country"/> enumeration.
        /// </summary>
        Guatemala                               = 320,

        /// <summary>
        /// Represents the country Guinea within the <see cref="Country"/> enumeration.
        /// </summary>
        Guinea                                  = 324,

        /// <summary>
        /// Represents the country Guyana as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Guyana                                  = 328,

        /// <summary>
        /// Represents the country Haiti in the <see cref="Country"/> enum.
        /// </summary>
        Haiti                                   = 332,

        /// <summary>
        /// Represents the country entry for Heard Island and McDonald Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        HeardIslandAndMcDonaldIslands           = 334,

        /// <summary>
        /// Represents the Holy See (Vatican City) as a country within the <see cref="Country"/> enumeration.
        /// </summary>
        HolySee                                 = 336,

        /// <summary>
        /// Represents Honduras as a value of the <see cref="Country"/> enumeration.
        /// </summary>
        Honduras                                = 340,

        /// <summary>
        /// Represents Hong Kong as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        HongKong                                = 344,

        /// <summary>
        /// Represents the country Hungary in the <see cref="Country"/> enumeration.
        /// </summary>
        Hungary                                 = 348,

        /// <summary>
        /// Represents Iceland as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Iceland                                 = 352,

        /// <summary>
        /// Represents the country India as a member of the <see cref="Country"/> enum.
        /// </summary>
        India                                   = 356,

        /// <summary>
        /// Represents the country Indonesia as a member of the <see cref="Country"/> enum.
        /// </summary>
        Indonesia                               = 360,

        /// <summary>
        /// Represents the country Iran in the <see cref="Country"/> enumeration.
        /// </summary>
        Iran                                    = 364,

        /// <summary>
        /// Represents the country Iraq as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Iraq                                    = 368,

        /// <summary>
        /// Represents Ireland as a country in the <see cref="Country"/> enumeration.
        /// </summary>
        Ireland                                 = 372,

        /// <summary>
        /// Represents the country Israel in the <see cref="Country"/> enumeration.
        /// </summary>
        Israel                                  = 376,

        /// <summary>
        /// Represents the country Italy in the <see cref="Country"/> enumeration.
        /// </summary>
        Italy                                   = 380,

        /// <summary>
        /// Represents the country Côte d'Ivoire in the <see cref="Country"/> enumeration.
        /// </summary>
        CoteDIvoire                             = 384,

        /// <summary>
        /// Represents the country Jamaica in the <see cref="Country"/> enum.
        /// </summary>
        Jamaica                                 = 388,

        /// <summary>
        /// Represents the country Japan in the <see cref="Country"/> enumeration.
        /// </summary>
        Japan                                   = 392,

        /// <summary>
        /// Represents Kazakhstan in the <see cref="Country"/> enumeration.
        /// </summary>
        Kazakhstan                              = 398,

        /// <summary>
        /// Represents the country Jordan in the <see cref="Country"/> enumeration.
        /// </summary>
        Jordan                                  = 400,

        /// <summary>
        /// Represents Kenya as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Kenya                                   = 404,

        /// <summary>
        /// Represents the country Korea, Democratic People's Republic.
        /// This member corresponds to the nation commonly known as North Korea.
        /// </summary>
        KoreaDemocraticPeoplesRepublic          = 408,

        /// <summary>
        /// Represents the country South Korea (Republic of Korea) as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        KoreaRepublic                           = 410,

        /// <summary>
        /// Represents the country Kuwait in the <see cref="Country"/> enumeration.
        /// </summary>
        Kuwait                                  = 414,

        /// <summary>
        /// Represents the country Kyrgyzstan in the <see cref="Country"/> enumeration.
        /// </summary>
        Kyrgyzstan                              = 417,

        /// <summary>
        /// Represents the Lao People's Democratic Republic in the <see cref="Country"/> enumeration.
        /// </summary>
        LaoPeoplesDemocraticRepublic            = 418,

        /// <summary>
        /// Represents the country Lebanon in the <see cref="Country"/> enumeration.
        /// </summary>
        Lebanon                                 = 422,

        /// <summary>
        /// Represents the country Lesotho as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Lesotho                                 = 426,

        /// <summary>
        /// Represents the country Latvia in the <see cref="Country"/> enum.
        /// </summary>
        Latvia                                  = 428,

        /// <summary>
        /// Represents the country Liberia in the <see cref="Country"/> enumeration.
        /// </summary>
        Liberia                                 = 430,

        /// <summary>
        /// Represents the country Libya in the enumeration.
        /// This member is part of the <see cref="Country"/> enum,
        /// which defines a set of valid country identifiers.
        /// </summary>
        Libya                                   = 434,

        /// <summary>
        /// Represents the country Liechtenstein in the <see cref="Country"/> enumeration.
        /// </summary>
        Liechtenstein                           = 438,

        /// <summary>
        /// Represents the country Lithuania in the <see cref="Country"/> enumeration.
        /// </summary>
        Lithuania                               = 440,

        /// <summary>
        /// Represents the country Luxembourg as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Luxembourg                              = 442,

        /// <summary>
        /// Represents the region of Macao in the <see cref="Country"/> enumeration.
        /// </summary>
        Macao                                   = 446,

        /// <summary>
        /// Represents the country Madagascar as a member of the <see cref="Country"/> enum.
        /// </summary>
        Madagascar                              = 450,

        /// <summary>
        /// Represents the country Malawi in the <see cref="Country"/> enumeration.
        /// </summary>
        Malawi                                  = 454,

        /// <summary>
        /// Represents the country Malaysia as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Malaysia                                = 458,

        /// <summary>
        /// Represents the country Maldives in the <see cref="Country"/> enumeration.
        /// </summary>
        Maldives                                = 462,

        /// <summary>
        /// Represents the country Mali in the <see cref="Country"/> enum.
        /// </summary>
        Mali                                    = 466,

        /// <summary>
        /// Represents the country Malta in the <see cref="Country"/> enumeration.
        /// Malta is a Southern European island nation in the Mediterranean.
        /// </summary>
        Malta                                   = 470,

        /// <summary>
        /// Represents the country Martinique in the <see cref="Country"/> enumeration.
        /// </summary>
        Martinique                              = 474,

        /// <summary>
        /// Represents the country Mauritania in the <see cref="Country"/> enumeration.
        /// </summary>
        Mauritania                              = 478,

        /// <summary>
        /// Represents the country Mauritius in the <see cref="Country"/> enumeration.
        /// </summary>
        Mauritius                               = 480,

        /// <summary>
        /// Represents the country Mexico within the <see cref="Country"/> enumeration.
        /// </summary>
        Mexico                                  = 484,

        /// <summary>
        /// Represents the country Monaco in the <see cref="Country"/> enumeration.
        /// </summary>
        Monaco                                  = 492,

        /// <summary>
        /// Represents the country Mongolia in the <see cref="Country"/> enumeration.
        /// </summary>
        Mongolia                                = 496,

        /// <summary>
        /// Represents the country Moldova in the <see cref="Country"/> enumeration.
        /// </summary>
        Moldova                                 = 498,

        /// <summary>
        /// Represents the country Montenegro in the <see cref="Country"/> enumeration.
        /// </summary>
        Montenegro                              = 499,

        /// <summary>
        /// Represents the country Montserrat in the <see cref="Country"/> enumeration.
        /// </summary>
        Montserrat                              = 500,

        /// <summary>
        /// Represents the country Morocco in the <see cref="Country"/> enumeration.
        /// </summary>
        Morocco                                 = 504,

        /// <summary>
        /// Represents the country Mozambique in the <see cref="Country"/> enum.
        /// </summary>
        Mozambique                              = 508,

        /// <summary>
        /// Represents the country Oman in the <see cref="Country"/> enumeration.
        /// </summary>
        Oman                                    = 512,

        /// <summary>
        /// Represents the country Namibia in the enumeration <see cref="Country"/>.
        /// </summary>
        Namibia                                 = 516,

        /// <summary>
        /// Represents the country Nauru in the <see cref="Country"/> enumeration.
        /// </summary>
        Nauru                                   = 520,

        /// <summary>
        /// Represents Nepal as a member of the <see cref="Country"/> enum.
        /// </summary>
        Nepal                                   = 524,

        /// <summary>
        /// Represents the Netherlands country within the <see cref="Country"/> enumeration.
        /// </summary>
        Netherlands                             = 528,

        /// <summary>
        /// Specifies the country as Curaçao.
        /// </summary>
        /// <remarks>
        /// Curaçao is a constituent country of the Kingdom of the Netherlands, located in the Caribbean.
        /// This member represents Curaçao within the <see cref="Country"/> enum.
        /// </remarks>
        Curacao                                 = 531,

        /// <summary>
        /// Represents the country Aruba in the <see cref="Country"/> enumeration.
        /// </summary>
        Aruba                                   = 533,

        /// <summary>
        /// Represents the country Sint Maarten.
        /// </summary>
        /// <remarks>
        /// Sint Maarten is included as a member of the <see cref="Country"/> enumeration.
        /// This value is used to identify the country of Sint Maarten in relevant contexts.
        /// </remarks>
        SintMaarten                             = 534,

        /// <summary>
        /// Represents the country Bonaire, Sint Eustatius, and Saba in the <see cref="Country"/> enumeration.
        /// </summary>
        BonaireSintEustatiusAndSaba             = 535,

        /// <summary>
        /// Represents the country New Caledonia in the <see cref="Country"/> enum.
        /// </summary>
        NewCaledonia                            = 540,

        /// <summary>
        /// Represents the country Vanuatu in the <see cref="Country"/> enumeration.
        /// </summary>
        Vanuatu                                 = 548,

        /// <summary>
        /// Represents the country New Zealand within the <see cref="Country"/> enumeration.
        /// </summary>
        NewZealand                              = 554,

        /// <summary>
        /// Represents the country Nicaragua as a member of the <see cref="Country"/> enum.
        /// </summary>
        Nicaragua                               = 558,

        /// <summary>
        /// Represents the country Niger in the <see cref="Country"/> enum.
        /// </summary>
        Niger                                   = 562,

        /// <summary>
        /// Represents the country Nigeria as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Nigeria                                 = 566,

        /// <summary>
        /// Represents the country Niue.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="Country"/> enum, which represents various countries.
        /// </remarks>
        Niue                                    = 570,

        /// <summary>
        /// Represents the country Norfolk Island in the <see cref="Country"/> enum.
        /// </summary>
        NorfolkIsland                           = 574,

        /// <summary>
        /// Represents the country Norway in the <see cref="Country"/> enum.
        /// </summary>
        Norway                                  = 578,

        /// <summary>
        /// Represents the Northern Mariana Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        NorthernMarianaIslands                  = 580,

        /// <summary>
        /// Represents the country designation for the United States Minor Outlying Islands.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        UnitedStatesMinorOutlyingIslands        = 581,

        /// <summary>
        /// Represents the country Micronesia in the <see cref="Country"/> enumeration.
        /// </summary>
        Micronesia                              = 583,

        /// <summary>
        /// Represents the country Marshall Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        MarshallIslands                         = 584,

        /// <summary>
        /// Represents the country Palau in the <see cref="Country"/> enum.
        /// </summary>
        Palau                                   = 585,

        /// <summary>
        /// Represents Pakistan as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Pakistan                                = 586,

        /// <summary>
        /// Represents the country Panama in the <see cref="Country"/> enum.
        /// </summary>
        Panama                                  = 591,

        /// <summary>
        /// Represents the country Papua New Guinea in the <see cref="Country"/> enumeration.
        /// </summary>
        PapuaNewGuinea                          = 598,

        /// <summary>
        /// Represents the country Paraguay as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Paraguay                                = 600,

        /// <summary>
        /// Represents the country Peru in the <see cref="Country"/> enum.
        /// </summary>
        Peru                                    = 604,

        /// <summary>
        /// Represents the country Philippines in the <see cref="Country"/> enumeration.
        /// </summary>
        Philippines                             = 608,

        /// <summary>
        /// Represents the country Pitcairn as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Pitcairn                                = 612,

        /// <summary>
        /// Represents the country Poland in the <see cref="Country"/> enumeration.
        /// </summary>
        Poland                                  = 616,

        /// <summary>
        /// Represents the country Portugal in the <see cref="Country"/> enumeration.
        /// </summary>
        Portugal                                = 620,

        /// <summary>
        /// Represents the country Guinea-Bissau in the <see cref="Country"/> enumeration.
        /// </summary>
        GuineaBissau                            = 624,

        /// <summary>
        /// Represents the country Timor-Leste as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        TimorLeste                              = 626,

        /// <summary>
        /// Represents the territory of Puerto Rico in the <see cref="Country"/> enumeration.
        /// </summary>
        PuertoRico                              = 630,

        /// <summary>
        /// Represents the country Qatar in the <see cref="Country"/> enumeration.
        /// </summary>
        Qatar                                   = 634,

        /// <summary>
        /// Represents the country Reunion as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Reunion                                 = 638,

        /// <summary>
        /// Represents the country Romania in the <see cref="Country"/> enumeration.
        /// </summary>
        Romania                                 = 642,

        /// <summary>
        /// Represents the country Russian Federation as a member of the <see cref="Country"/> enum.
        /// </summary>
        RussianFederation                       = 643,

        /// <summary>
        /// Represents the country Rwanda in the <see cref="Country"/> enum.
        /// </summary>
        Rwanda                                  = 646,

        /// <summary>
        /// Represents the country Saint Barthélemy in the <see cref="Country"/> enumeration.
        /// </summary>
        SaintBarthelemy                         = 652,

        /// <summary>
        /// Represents Saint Helena in the <see cref="Country"/> enumeration.
        /// </summary>
        SaintHelena                             = 654,

        /// <summary>
        /// Represents the country Saint Kitts and Nevis in the <see cref="Country"/> enum.
        /// </summary>
        SaintKittsAndNevis                      = 659,

        /// <summary>
        /// Represents the country Anguilla in the <see cref="Country"/> enum.
        /// </summary>
        Anguilla                                = 660,

        /// <summary>
        /// Represents the country Saint Lucia in the <see cref="Country"/> enumeration.
        /// </summary>
        SaintLucia                              = 662,

        /// <summary>
        /// Represents the country Saint Martin in the <see cref="Country"/> enumeration.
        /// </summary>
        SaintMartin                             = 663,

        /// <summary>
        /// Represents the country Saint Pierre and Miquelon in the <see cref="Country"/> enum.
        /// </summary>
        SaintPierreAndMiquelon                  = 666,

        /// <summary>
        /// Represents the country Saint Vincent and the Grenadines in the <see cref="Country"/> enumeration.
        /// </summary>
        SaintVincentAndTheGrenadines            = 670,

        /// <summary>
        /// Represents the country San Marino within the <see cref="Country"/> enum.
        /// </summary>
        SanMarino                               = 674,

        /// <summary>
        /// Represents the country Sao Tome and Principe within the <see cref="Country"/> enumeration.
        /// </summary>
        SaoTomeAndPrincipe                      = 678,

        /// <summary>
        /// Represents the country Saudi Arabia in the <see cref="Country"/> enumeration.
        /// </summary>
        SaudiArabia                             = 682,

        /// <summary>
        /// Represents the country Senegal in the <see cref="Country"/> enumeration.
        /// </summary>
        Senegal                                 = 686,

        /// <summary>
        /// Represents the country Serbia in the <see cref="Country"/> enum.
        /// </summary>
        Serbia                                  = 688,

        /// <summary>
        /// Represents the country Seychelles in the <see cref="Country"/> enum.
        /// </summary>
        Seychelles                              = 690,

        /// <summary>
        /// Represents the country Sierra Leone.
        /// This member is a specific value of the <see cref="Country"/> enum.
        /// </summary>
        SierraLeone                             = 694,

        /// <summary>
        /// Represents the country Singapore in the <see cref="Country"/> enumeration.
        /// </summary>
        Singapore                               = 702,

        /// <summary>
        /// Represents the country Slovakia as a member of the <see cref="Country"/> enum.
        /// </summary>
        Slovakia                                = 703,

        /// <summary>
        /// Represents the country Vietnam in the <see cref="Country"/> enumeration.
        /// </summary>
        Vietnam                                 = 704,

        /// <summary>
        /// Represents the country of Slovenia in the <see cref="Country"/> enumeration.
        /// </summary>
        Slovenia                                = 705,

        /// <summary>
        /// Represents the country Somalia in the <see cref="Country"/> enumeration.
        /// </summary>
        Somalia                                 = 706,

        /// <summary>
        /// Represents the country South Africa in the <see cref="Country"/> enumeration.
        /// </summary>
        SouthAfrica                             = 710,

        /// <summary>
        /// Represents the country Zimbabwe as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Zimbabwe                                = 716,

        /// <summary>
        /// Represents the country Spain as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Spain                                   = 724,

        /// <summary>
        /// Represents South Sudan as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        SouthSudan                              = 728,

        /// <summary>
        /// Represents the country Sudan in the <see cref="Country"/> enumeration.
        /// </summary>
        Sudan                                   = 729,

        /// <summary>
        /// Represents the country Western Sahara.
        /// </summary>
        /// <remarks>
        /// This value is a member of the <see cref="Country"/> enumeration.
        /// It is used to represent the region of Western Sahara in geographical or political contexts.
        /// </remarks>
        WesternSahara                           = 732,

        /// <summary>
        /// Represents the country Suriname as a member of the <see cref="Country"/> enum.
        /// </summary>
        Suriname                                = 740,

        /// <summary>
        /// Represents the country of Svalbard and Jan Mayen.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        SvalbardAndJanMayen                     = 744,

        /// <summary>
        /// Represents the country Eswatini as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Eswatini                                = 748,

        /// <summary>
        /// Represents the country Sweden in the <see cref="Country"/> enum.
        /// </summary>
        Sweden                                  = 752,

        /// <summary>
        /// Represents the country Switzerland in the <see cref="Country"/> enumeration.
        /// </summary>
        Switzerland                             = 756,

        /// <summary>
        /// Represents the country Syria in the <see cref="Country"/> enumeration.
        /// </summary>
        SyrianArabRepublic                      = 760,

        /// <summary>
        /// Represents the country Tajikistan in the <see cref="Country"/> enumeration.
        /// </summary>
        Tajikistan                              = 762,

        /// <summary>
        /// Represents the country Thailand in the <see cref="Country"/> enumeration.
        /// </summary>
        Thailand                                = 764,

        /// <summary>
        /// Represents the country Togo.
        /// This is a member of the <see cref="Country"/> enum.
        /// </summary>
        Togo                                    = 768,

        /// <summary>
        /// Represents Tokelau in the <see cref="Country"/> enum.
        /// </summary>
        Tokelau                                 = 772,

        /// <summary>
        /// Represents the country Tonga in the <see cref="Country"/> enumeration.
        /// </summary>
        Tonga                                   = 776,

        /// <summary>
        /// Represents the country Trinidad and Tobago in the <see cref="Country"/> enumeration.
        /// </summary>
        TrinidadAndTobago                       = 780,

        /// <summary>
        /// Represents the country United Arab Emirates in the <see cref="Country"/> enum.
        /// </summary>
        UnitedArabEmirates                      = 784,

        /// <summary>
        /// Represents the country Tunisia in the <see cref="Country"/> enumeration.
        /// </summary>
        Tunisia                                 = 788,

        /// <summary>
        /// Represents the country Turkey as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Turkey                                  = 792,

        /// <summary>
        /// Represents the country Turkmenistan in the <see cref="Country"/> enum.
        /// </summary>
        Turkmenistan                            = 795,

        /// <summary>
        /// Represents the Turks and Caicos Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        TurksAndCaicosIslands                   = 796,

        /// <summary>
        /// Represents the country Tuvalu in the <see cref="Country"/> enumeration.
        /// </summary>
        Tuvalu                                  = 798,

        /// <summary>
        /// Represents the country Uganda as a member of the <see cref="Country"/> enum.
        /// </summary>
        Uganda                                  = 800,

        /// <summary>
        /// Represents the country Ukraine as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Ukraine                                 = 804,

        /// <summary>
        /// Represents the country North Macedonia in the <see cref="Country"/> enumeration.
        /// </summary>
        NorthMacedonia                          = 807,

        /// <summary>
        /// Represents the country Egypt in the <see cref="Country"/> enum.
        /// </summary>
        Egypt                                   = 818,

        /// <summary>
        /// Represents the United Kingdom in the <see cref="Country"/> enumeration.
        /// </summary>
        UnitedKingdom                           = 826,

        /// <summary>
        /// Represents the country Guernsey in the <see cref="Country"/> enumeration.
        /// </summary>
        Guernsey                                = 831,

        /// <summary>
        /// Represents the country Jersey as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Jersey                                  = 832,

        /// <summary>
        /// Represents the Isle of Man as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        IsleOfMan                               = 833,

        /// <summary>
        /// Represents the country Tanzania in the <see cref="Country"/> enumeration.
        /// </summary>
        Tanzania                                = 834,

        /// <summary>
        /// Represents the country United States of America within the <see cref="Country"/> enumeration.
        /// </summary>
        UnitedStatesOfAmerica                   = 840,

        /// <summary>
        /// Represents the United States Virgin Islands as a country.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        VirginIslandsUS                         = 850,

        /// <summary>
        /// Represents the country Burkina Faso in the <see cref="Country"/> enumeration.
        /// </summary>
        BurkinaFaso                             = 854,

        /// <summary>
        /// Represents the country Uruguay in the <see cref="Country"/> enum.
        /// </summary>
        Uruguay                                 = 858,

        /// <summary>
        /// Represents the country Uzbekistan in the <see cref="Country"/> enumeration.
        /// </summary>
        Uzbekistan                              = 860,

        /// <summary>
        /// Represents the country Venezuela in the <see cref="Country"/> enumeration.
        /// </summary>
        Venezuela                               = 862,

        /// <summary>
        /// Represents the country Wallis and Futuna in the <see cref="Country"/> enumeration.
        /// </summary>
        WallisAndFutuna                         = 876,

        /// <summary>
        /// Represents the country Samoa in the <see cref="Country"/> enumeration.
        /// </summary>
        Samoa                                   = 882,

        /// <summary>
        /// Represents Yemen as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        Yemen                                   = 887,

        /// <summary>
        /// Represents the country Zambia in the <see cref="Country"/> enumeration.
        /// </summary>
        Zambia                                  = 894,

        // Two Letters

        /// <summary>
        /// Represents Afghanistan as a member of the <see cref="Country"/> enum.
        /// </summary>
        AF = Afghanistan,

        /// <summary>
        /// Represents the country code for Albania in the <see cref="Country"/> enumeration.
        /// </summary>
        AL = Albania,

        /// <summary>
        /// Represents Antarctica in the <see cref="Country"/> enumeration.
        /// </summary>
        AQ = Antarctica,

        /// <summary>
        /// Represents the country Algeria in the <see cref="Country"/> enumeration.
        /// </summary>
        DZ = Algeria,

        /// <summary>
        /// Represents the country code for American Samoa.
        /// </summary>
        /// <remarks>
        /// This enum member is part of the <see cref="Country"/> enumeration.
        /// Use <see cref="AS"/> to identify or reference American Samoa in the context
        /// of country codes represented by the <see cref="Country"/> enum.
        /// </remarks>
        AS = AmericanSamoa,

        /// <summary>
        /// Represents the country Andorra within the <see cref="Country"/> enumeration.
        /// </summary>
        AD = Andorra,

        /// <summary>
        /// Represents the country Angola in the <see cref="Country"/> enumeration.
        /// </summary>
        AO = Angola,

        /// <summary>
        /// Represents the country code for Antigua and Barbuda in the <see cref="Country"/> enumeration.
        /// </summary>
        AG = AntiguaAndBarbuda,

        /// <summary>
        /// Represents the country code for Azerbaijan in the <see cref="Country"/> enum.
        /// </summary>
        AZ = Azerbaijan,

        /// <summary>
        /// Represents Argentina in the <see cref="Country"/> enumeration.
        /// </summary>
        AR = Argentina,

        /// <summary>
        /// Represents the country code for Australia within the <see cref="Country"/> enumeration.
        /// </summary>
        AU = Australia,

        /// <summary>
        /// Represents the country code for Austria in the <see cref="Country"/> enumeration.
        /// </summary>
        AT = Austria,

        /// <summary>
        /// Represents the country code for the Bahamas.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="Country"/> enumeration.
        /// It is used to refer specifically to the country identified as the Bahamas.
        /// </remarks>
        BS = Bahamas,

        /// <summary>
        /// Represents the country code for Bahrain in the <see cref="Country"/> enumeration.
        /// </summary>
        BH = Bahrain,

        /// <summary>
        /// Represents the country Bangladesh in the <see cref="Country"/> enumeration.
        /// </summary>
        BD = Bangladesh,

        /// <summary>
        /// Represents the country Armenia in the <see cref="Country"/> enumeration.
        /// </summary>
        AM = Armenia,

        /// <summary>
        /// Represents the country code for Barbados in the <see cref="Country"/> enumeration.
        /// </summary>
        BB = Barbados,

        /// <summary>
        /// Represents the country Belgium in the <see cref="Country"/> enum.
        /// </summary>
        BE = Belgium,

        /// <summary>
        /// Represents Bermuda in the <see cref="Country"/> enumeration.
        /// </summary>
        BM = Bermuda,

        /// <summary>
        /// Represents the country code for Bhutan in the <see cref="Country"/> enum.
        /// </summary>
        BT = Bhutan,

        /// <summary>
        /// Represents the country Bolivia.
        /// </summary>
        BO = Bolivia,

        /// <summary>
        /// Represents the country code for Bosnia and Herzegovina. This is a member of the <see cref="Country"/> enum.
        /// </summary>
        BA = BosniaAndHerzegovina,

        /// <summary>
        /// Represents the country Botswana in the <see cref="Country"/> enumeration.
        /// </summary>
        BW = Botswana,

        /// <summary>
        /// Represents the country Bouvet Island in the <see cref="Country"/> enumeration.
        /// </summary>
        BV = BouvetIsland,

        /// <summary>
        /// Represents the country Brazil.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        BR = Brazil,

        /// <summary>
        /// Represents the country code for Belize in the <see cref="Country"/> enumeration.
        /// </summary>
        BZ = Belize,

        /// <summary>
        /// Represents the British Indian Ocean Territory in the <see cref="Country"/> enum.
        /// </summary>
        IO = BritishIndianOceanTerritory,

        /// <summary>
        /// Represents the country code for Solomon Islands in the <see cref="Country"/> enum.
        /// </summary>
        SB = SolomonIslands,

        /// <summary>
        /// Represents the country code for the British Virgin Islands.
        /// This is a member of the <see cref="Country"/> enumeration.
        /// </summary>
        VG = VirginIslandsBritish,

        /// <summary>
        /// Represents the country code for Brunei in the <see cref="Country"/> enumeration.
        /// </summary>
        BN = BruneiDarussalam,

        /// <summary>
        /// Represents the country code for Bulgaria in the <see cref="Country"/> enumeration.
        /// </summary>
        BG = Bulgaria,

        /// <summary>
        /// Represents the country Myanmar in the <see cref="Country"/> enumeration.
        /// </summary>
        MM = Myanmar,

        /// <summary>
        /// Represents the country code for Burundi in the <see cref="Country"/> enumeration.
        /// </summary>
        BI = Burundi,

        /// <summary>
        /// Represents the country Belarus in the <see cref="Country"/> enumeration.
        /// </summary>
        BY = Belarus,

        /// <summary>
        /// Represents the country Cambodia in the <see cref="Country"/> enumeration.
        /// </summary>
        KH = Cambodia,

        /// <summary>
        /// Represents the country code for Cameroon in the <see cref="Country"/> enumeration.
        /// </summary>
        CM = Cameroon,

        /// <summary>
        /// Represents the country Canada within the <see cref="Country"/> enum.
        /// </summary>
        CA = Canada,

        /// <summary>
        /// Represents the country Cabo Verde in the <see cref="Country"/> enumeration.
        /// </summary>
        CV = CaboVerde,

        /// <summary>
        /// Represents the country code for the Cayman Islands within the <see cref="Country"/> enumeration.
        /// </summary>
        KY = CaymanIslands,

        /// <summary>
        /// Represents the country code for the Central African Republic in the <see cref="Country"/> enumeration.
        /// </summary>
        CF = CentralAfricanRepublic,

        /// <summary>
        /// Represents the country Sri Lanka in the <see cref="Country"/> enumeration.
        /// </summary>
        LK = SriLanka,

        /// <summary>
        /// Represents the country Chad in the <see cref="Country"/> enumeration.
        /// </summary>
        TD = Chad,

        /// <summary>
        /// Represents Chile in the <see cref="Country"/> enumeration.
        /// </summary>
        CL = Chile,

        /// <summary>
        /// Represents the country Canada in the <see cref="Country"/> enumeration.
        /// </summary>
        CN = China,

        /// <summary>
        /// Represents Taiwan in the <see cref="Country"/> enum.
        /// </summary>
        TW = Taiwan,

        /// <summary>
        /// Represents the country code for Christmas Island.
        /// </summary>
        /// <remarks>
        /// <see cref="Country"/> is an enumeration containing various country codes.
        /// The <see cref="Country.CX"/> member corresponds to Christmas Island.
        /// </remarks>
        CX = ChristmasIsland,

        /// <summary>
        /// Represents the country code for Cocos (Keeling) Islands in the <see cref="Country"/> enum.
        /// </summary>
        CC = CocosIslands,

        /// <summary>
        /// Represents the country Colombia in the <see cref="Country"/> enumeration.
        /// </summary>
        CO = Colombia,

        /// <summary>
        /// Represents the country code for Comoros in the <see cref="Country"/> enumeration.
        /// </summary>
        KM = Comoros,

        /// <summary>
        /// Represents Mayotte as defined in the <see cref="Country"/> enum.
        /// </summary>
        YT = Mayotte,

        /// <summary>
        /// Represents the country code for Congo (Brazzaville) in the <see cref="Country"/> enumeration.
        /// </summary>
        CG = Congo,

        /// <summary>
        /// Represents the Democratic Republic of the Congo within the <see cref="Country"/> enum.
        /// </summary>
        CD = CongoDemocraticRepublic,

        /// <summary>
        /// Represents the country code for the Cook Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        CK = CookIslands,

        /// <summary>
        /// Represents the country code for Costa Rica in the <see cref="Country"/> enumeration.
        /// </summary>
        CR = CostaRica,

        /// <summary>
        /// Represents the country Croatia in the <see cref="Country"/> enumeration.
        /// </summary>
        HR = Croatia,

        /// <summary>
        /// Represents the country code for Cuba in the <see cref="Country"/> enumeration.
        /// </summary>
        CU = Cuba,

        /// <summary>
        /// Represents the country Cyprus in the <see cref="Country"/> enumeration.
        /// </summary>
        CY = Cyprus,

        /// <summary>
        /// Represents the country code for the Czech Republic in the <see cref="Country"/> enumeration.
        /// </summary>
        CZ = Czechia,

        /// <summary>
        /// Represents the country Benin with the country code BJ in the <see cref="Country"/> enumeration.
        /// </summary>
        BJ = Benin,

        /// <summary>
        /// Represents the country Denmark in the <see cref="Country"/> enumeration.
        /// </summary>
        DK = Denmark,

        /// <summary>
        /// Represents the country code for Dominica in the <see cref="Country"/> enumeration.
        /// </summary>
        DM = Dominica,

        /// <summary>
        /// Represents the Dominican Republic in the <see cref="Country"/> enum.
        /// </summary>
        DO = DominicanRepublic,

        /// <summary>
        /// Represents the country code for Ecuador.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="Country"/> enumeration and identifies Ecuador as a specific country designation.
        /// </remarks>
        EC = Ecuador,

        /// <summary>
        /// Represents the country El Salvador in the <see cref="Country"/> enumeration.
        /// </summary>
        SV = ElSalvador,

        /// <summary>
        /// Represents the country Equatorial Guinea in the <see cref="Country"/> enumeration.
        /// </summary>
        GQ = EquatorialGuinea,

        /// <summary>
        /// Represents the country Estonia in the <see cref="Country"/> enumeration.
        /// </summary>
        ET = Ethiopia,

        /// <summary>
        /// Represents Eritrea in the <see cref="Country"/> enumeration.
        /// </summary>
        ER = Eritrea,

        /// <summary>
        /// Represents the country Estonia in the <see cref="Country"/> enumeration.
        /// </summary>
        EE = Estonia,

        /// <summary>
        /// Represents the Faroe Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        FO = FaroeIslands,

        /// <summary>
        /// Represents the country code for the Falkland Islands (Malvinas).
        /// This member is part of the <see cref="Country"/> enumeration, which defines country codes.
        /// </summary>
        FK = FalklandIslands,

        /// <summary>
        /// Represents the country code for South Georgia and the South Sandwich Islands.
        /// This member is part of the <see cref="Country"/> enum.
        /// </summary>
        GS = SouthGeorgiaAndTheSouthSandwichIslands,

        /// <summary>
        /// Represents the country Fiji in the <see cref="Country"/> enumeration.
        /// </summary>
        FJ = Fiji,

        /// <summary>
        /// Represents the country Finland in the <see cref="Country"/> enumeration.
        /// </summary>
        FI = Finland,

        /// <summary>
        /// Represents the country code for Åland Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        AX = AlandIslands,

        /// <summary>
        /// Represents the country France.
        /// This member is a part of the <see cref="Country"/> enum.
        /// </summary>
        FR = France,

        /// <summary>
        /// Represents French Guiana in the <see cref="Country"/> enumeration.
        /// </summary>
        GF = FrenchGuiana,

        /// <summary>
        /// Represents the country code for French Polynesia.
        /// </summary>
        /// <remarks>
        /// This member belongs to the <see cref="Country"/> enumeration.
        /// </remarks>
        PF = FrenchPolynesia,

        /// <summary>
        /// Represents the country code for French Southern Territories.
        /// </summary>
        TF = FrenchSouthernTerritories,

        /// <summary>
        /// Represents the country Djibouti in the <see cref="Country"/> enumeration.
        /// </summary>
        DJ = Djibouti,

        /// <summary>
        /// Represents the country Gabon in the <see cref="Country"/> enumeration.
        /// </summary>
        GA = Gabon,

        /// <summary>
        /// Represents the country Georgia in the <see cref="Country"/> enumeration.
        /// </summary>
        GE = Georgia,

        /// <summary>
        /// Represents the country code for Gambia in the <see cref="Country"/> enumeration.
        /// </summary>
        GM = Gambia,

        /// <summary>
        /// Represents the country Palestine as a member of the <see cref="Country"/> enum.
        /// </summary>
        PS = Palestine,

        /// <summary>
        /// Represents Germany in the <see cref="Country"/> enumeration.
        /// </summary>
        DE = Germany,

        /// <summary>
        /// Represents the country code for Ghana in the <see cref="Country"/> enumeration.
        /// </summary>
        GH = Ghana,

        /// <summary>
        /// Represents Gibraltar in the <see cref="Country"/> enum.
        /// </summary>
        GI = Gibraltar,

        /// <summary>
        /// Represents the country Kiribati in the <see cref="Country"/> enumeration.
        /// </summary>
        KI = Kiribati,

        /// <summary>
        /// Represents Greece in the <see cref="Country"/> enumeration.
        /// </summary>
        GR = Greece,

        /// <summary>
        /// Represents the country code for Greenland.
        /// </summary>
        GL = Greenland,

        /// <summary>
        /// Represents the country code for Grenada in the <see cref="Country"/> enum.
        /// </summary>
        GD = Grenada,

        /// <summary>
        /// Represents the country code for Guadeloupe.
        /// This value is a member of the <see cref="Country"/> enumeration.
        /// </summary>
        GP = Guadeloupe,

        /// <summary>
        /// Represents the enumeration member for Guam in the <see cref="Country"/> enum.
        /// </summary>
        GU = Guam,

        /// <summary>
        /// Represents the country code for Guatemala in the <see cref="Country"/> enumeration.
        /// </summary>
        GT = Guatemala,

        /// <summary>
        /// Represents Guinea in the <see cref="Country"/> enumeration.
        /// </summary>
        GN = Guinea,

        /// <summary>
        /// Represents the country code for Guyana in the <see cref="Country"/> enumeration.
        /// </summary>
        GY = Guyana,

        /// <summary>
        /// Represents the country code for Haiti in the <see cref="Country"/> enumeration.
        /// </summary>
        HT = Haiti,

        /// <summary>
        /// Represents the Heard Island and McDonald Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        HM = HeardIslandAndMcDonaldIslands,

        /// <summary>
        /// Represents the country code for the Vatican City.
        /// </summary>
        /// <remarks>
        /// This enum member is part of the <see cref="Country"/> enumeration, which defines country codes.
        /// </remarks>
        VA = HolySee,

        /// <summary>
        /// Represents the country code for Honduras within the <see cref="Country"/> enumeration.
        /// </summary>
        HN = Honduras,

        /// <summary>
        /// Represents Hong Kong in the <see cref="Country"/> enumeration.
        /// </summary>
        HK = HongKong,

        /// <summary>
        /// Represents Hungary in the <see cref="Country"/> enumeration.
        /// </summary>
        HU = Hungary,

        /// <summary>
        /// Represents the country Iceland in the <see cref="Country"/> enumeration.
        /// </summary>
        IS = Iceland,

        /// <summary>
        /// Represents the country code for India.
        /// </summary>
        /// <remarks>
        /// This is a member of the <see cref="Country"/> enumeration.
        /// </remarks>
        IN = India,

        /// <summary>
        /// Represents the unique identifier for a country in the <see cref="Country"/> enum.
        /// </summary>
        ID = Indonesia,

        /// <summary>
        /// Represents the country Iran in the <see cref="Country"/> enumeration.
        /// </summary>
        IR = Iran,

        /// <summary>
        /// Represents the country Iraq within the <see cref="Country"/> enumeration.
        /// </summary>
        IQ = Iraq,

        /// <summary>
        /// Represents the country Ireland in the <see cref="Country"/> enumeration.
        /// </summary>
        IE = Ireland,

        /// <summary>
        /// Represents the country code for Israel in the <see cref="Country"/> enumeration.
        /// </summary>
        IL = Israel,

        /// <summary>
        /// Represents Italy in the <see cref="Country"/> enumeration.
        /// </summary>
        IT = Italy,

        /// <summary>
        /// Represents the country code for Côte d'Ivoire in the <see cref="Country"/> enum.
        /// </summary>
        CI = CoteDIvoire,

        /// <summary>
        /// Represents the country code for Jamaica in the <see cref="Country"/> enumeration.
        /// </summary>
        JM = Jamaica,

        /// <summary>
        /// Represents Japan in the <see cref="Country"/> enumeration.
        /// </summary>
        JP = Japan,

        /// <summary>
        /// Represents the country Kazakhstan within the <see cref="Country"/> enumeration.
        /// </summary>
        KZ = Kazakhstan,

        /// <summary>
        /// Represents the country Jordan in the <see cref="Country"/> enumeration.
        /// </summary>
        JO = Jordan,

        /// <summary>
        /// Represents the country Kenya in the <see cref="Country"/> enumeration.
        /// </summary>
        KE = Kenya,

        /// <summary>
        /// Represents the country code for North Korea in the <see cref="Country"/> enumeration.
        /// </summary>
        KP = KoreaDemocraticPeoplesRepublic,

        /// <summary>
        /// Represents the Republic of Korea in the <see cref="Country"/> enumeration.
        /// </summary>
        KR = KoreaRepublic,

        /// <summary>
        /// Represents the country Kuwait in the <see cref="Country"/> enumeration.
        /// </summary>
        KW = Kuwait,

        /// <summary>
        /// Represents the country Kyrgyzstan in the <see cref="Country"/> enumeration.
        /// </summary>
        KG = Kyrgyzstan,

        /// <summary>
        /// Represents the country Laos in the <see cref="Country"/> enumeration.
        /// </summary>
        LA = LaoPeoplesDemocraticRepublic,

        /// <summary>
        /// Represents the country code for Lebanon.
        /// </summary>
        /// <remarks>
        /// This enum member belongs to the <see cref="Country"/> enumeration, which defines country codes.
        /// </remarks>
        LB = Lebanon,

        /// <summary>
        /// Represents the country code for Lesotho in the <see cref="Country"/> enumeration.
        /// </summary>
        LS = Lesotho,

        /// <summary>
        /// Represents the country Latvia in the <see cref="Country"/> enumeration.
        /// </summary>
        LV = Latvia,

        /// <summary>
        /// Represents the country code for Liberia in the <see cref="Country"/> enumeration.
        /// </summary>
        LR = Liberia,

        /// <summary>
        /// Represents the country code for Libya in the <see cref="Country"/> enumeration.
        /// </summary>
        LY = Libya,

        /// <summary>
        /// Represents the country code for Liechtenstein.
        /// </summary>
        /// <remarks>
        /// This is a member of the <see cref="Country"/> enum, which represents different countries using their specific codes.
        /// </remarks>
        LI = Liechtenstein,

        /// <summary>
        /// Represents the country Lithuania.
        /// This is a member of the <see cref="Country"/> enumeration.
        /// </summary>
        LT = Lithuania,

        /// <summary>
        /// Represents the country Luxembourg in the <see cref="Country"/> enumeration.
        /// </summary>
        LU = Luxembourg,

        /// <summary>
        /// Represents the country code for Monaco in the <see cref="Country"/> enumeration.
        /// </summary>
        MO = Macao,

        /// <summary>
        /// Represents the country code for Madagascar in the <see cref="Country"/> enumeration.
        /// </summary>
        MG = Madagascar,

        /// <summary>
        /// Represents the country Malawi as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        MW = Malawi,

        /// <summary>
        /// Represents the country Malaysia in the <see cref="Country"/> enumeration.
        /// </summary>
        MY = Malaysia,

        /// <summary>
        /// Represents the country code for Maldives.
        /// </summary>
        /// <remarks>
        /// This value is a member of the <see cref="Country"/> enumeration.
        /// </remarks>
        MV = Maldives,

        /// <summary>
        /// Represents the country Mali.
        /// </summary>
        /// <remarks>
        /// This member corresponds to the country Mali in the <see cref="Country"/> enum.
        /// </remarks>
        ML = Mali,

        /// <summary>
        /// Represents the country Malta in the <see cref="Country"/> enumeration.
        /// </summary>
        /// <remarks>
        /// This enum member corresponds to Malta, identified by its ISO 3166-1 alpha-2 code "MT".
        /// </remarks>
        MT = Malta,

        /// <summary>
        /// Represents Martinique as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        MQ = Martinique,

        /// <summary>
        /// Represents the country code for Mauritania.
        /// </summary>
        /// <remarks>
        /// This enum member is part of the <see cref="Country"/> enum and corresponds to the
        /// country identification for Mauritania.
        /// </remarks>
        MR = Mauritania,

        /// <summary>
        /// Represents Mauritius in the <see cref="Country"/> enumeration.
        /// </summary>
        MU = Mauritius,

        /// <summary>
        /// Represents the country Mexico in the <see cref="Country"/> enumeration.
        /// </summary>
        MX = Mexico,

        /// <summary>
        /// Represents the country Monaco within the <see cref="Country"/> enumeration.
        /// </summary>
        MC = Monaco,

        /// <summary>
        /// Represents the country code for Mongolia in the <see cref="Country"/> enumeration.
        /// </summary>
        MN = Mongolia,

        /// <summary>
        /// Represents the country code for Moldova within the <see cref="Country"/> enumeration.
        /// </summary>
        MD = Moldova,

        /// <summary>
        /// Represents the country code for Montenegro in the <see cref="Country"/> enumeration.
        /// </summary>
        ME = Montenegro,

        /// <summary>
        /// Represents the country Montserrat.
        /// This member is a part of the <see cref="Country"/> enumeration.
        /// </summary>
        MS = Montserrat,

        /// <summary>
        /// Represents Morocco in the <see cref="Country"/> enumeration.
        /// </summary>
        MA = Morocco,

        /// <summary>
        /// Represents Mozambique in the <see cref="Country"/> enum.
        /// </summary>
        MZ = Mozambique,

        /// <summary>
        /// Represents the country code for Oman in the <see cref="Country"/> enumeration.
        /// </summary>
        OM = Oman,

        /// <summary>
        /// Represents the country designation for North America.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        NA = Namibia,

        /// <summary>
        /// Represents the country code for Nauru in the <see cref="Country"/> enumeration.
        /// </summary>
        NR = Nauru,

        /// <summary>
        /// Represents Nepal as a country in the <see cref="Country"/> enumeration.
        /// </summary>
        NP = Nepal,

        /// <summary>
        /// Represents the Netherlands as a country in the <see cref="Country"/> enumeration.
        /// </summary>
        NL = Netherlands,

        /// <summary>
        /// Represents the country code for Curaçao.
        /// This enum member is part of the <see cref="Country"/> enum in which each member corresponds to a specific country.
        /// </summary>
        CW = Curacao,

        /// <summary>
        /// Represents the country code for Aruba as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        AW = Aruba,

        /// <summary>
        /// Represents the country code for Sint Maarten.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </remarks>
        SX = SintMaarten,

        /// <summary>
        /// Represents the country Bonaire, Sint Eustatius, and Saba in the <see cref="Country"/> enumeration.
        /// </summary>
        BQ = BonaireSintEustatiusAndSaba,

        /// <summary>
        /// Represents the country code for New Caledonia.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        NC = NewCaledonia,

        /// <summary>
        /// Represents the country code for Vanuatu in the <see cref="Country"/> enumeration.
        /// </summary>
        VU = Vanuatu,

        /// <summary>
        /// Represents the country New Zealand in the <see cref="Country"/> enumeration.
        /// </summary>
        NZ = NewZealand,

        /// <summary>
        /// Represents Northern Ireland as a member of the <see cref="Country"/> enum.
        /// </summary>
        NI = Nicaragua,

        /// <summary>
        /// Represents the country code for Niger.
        /// </summary>
        NE = Niger,

        /// <summary>
        /// Represents Nigeria as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        NG = Nigeria,

        /// <summary>
        /// Represents the Netherlands Antilles in the <see cref="Country"/> enumeration.
        /// </summary>
        NU = Niue,

        /// <summary>
        /// Represents the country code for Newfoundland and Labrador, Canada, in the <see cref="Country"/> enumeration.
        /// </summary>
        NF = NorfolkIsland,

        /// <summary>
        /// Represents the country code for Norway in the <see cref="Country"/> enumeration.
        /// </summary>
        NO = Norway,

        /// <summary>
        /// Represents the Northern Mariana Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        MP = NorthernMarianaIslands,

        /// <summary>
        /// Represents the United States Minor Outlying Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        UM = UnitedStatesMinorOutlyingIslands,

        /// <summary>
        /// Represents the Federated States of Micronesia in the <see cref="Country"/> enumeration.
        /// </summary>
        FM = Micronesia,

        /// <summary>
        /// Represents the Marshall Islands within the <see cref="Country"/> enumeration.
        /// This member corresponds to the Marshall Islands as a country identifier.
        /// </summary>
        MH = MarshallIslands,

        /// <summary>
        /// Represents the country code for Palau in the <see cref="Country"/> enumeration.
        /// </summary>
        PW = Palau,

        /// <summary>
        /// Represents Pakistan in the <see cref="Country"/> enumeration.
        /// </summary>
        PK = Pakistan,

        /// <summary>
        /// Represents the country Panama in the <see cref="Country"/> enum.
        /// </summary>
        PA = Panama,

        /// <summary>
        /// Represents the country Papua New Guinea in the <see cref="Country"/> enumeration.
        /// </summary>
        PG = PapuaNewGuinea,

        /// <summary>
        /// Represents the country Paraguay in the <see cref="Country"/> enumeration.
        /// </summary>
        PY = Paraguay,

        /// <summary>
        /// Represents the country Peru in the enumeration <see cref="Country"/>.
        /// </summary>
        PE = Peru,

        /// <summary>
        /// Represents the country Philippines in the <see cref="Country"/> enumeration.
        /// </summary>
        PH = Philippines,

        /// <summary>
        /// Represents the country Papua New Guinea in the <see cref="Country"/> enumeration.
        /// </summary>
        PN = Pitcairn,

        /// <summary>
        /// Represents the country Poland in the <see cref="Country"/> enumeration.
        /// </summary>
        PL = Poland,

        /// <summary>
        /// Represents the country Portugal as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        PT = Portugal,

        /// <summary>
        /// Represents the country Guinea-Bissau in the <see cref="Country"/> enum.
        /// </summary>
        GW = GuineaBissau,

        /// <summary>
        /// Represents the country Timor-Leste (East Timor) in the <see cref="Country"/> enumeration.
        /// </summary>
        TL = TimorLeste,

        /// <summary>
        /// Represents Puerto Rico in the <see cref="Country"/> enumeration.
        /// </summary>
        PR = PuertoRico,

        /// <summary>
        /// Represents the country Qatar in the <see cref="Country"/> enum.
        /// </summary>
        QA = Qatar,

        /// <summary>
        /// Represents the country entity for Réunion.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        RE = Reunion,

        /// <summary>
        /// Represents the country Romania as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        RO = Romania,

        /// <summary>
        /// Represents the country Russia in the <see cref="Country"/> enumeration.
        /// </summary>
        RU = RussianFederation,

        /// <summary>
        /// Represents the country Rwanda.
        /// </summary>
        /// <remarks>
        /// This enumeration member is part of the <see cref="Country"/> enum.
        /// </remarks>
        RW = Rwanda,

        /// <summary>
        /// Represents the country code for Saint Barthélemy.
        /// </summary>
        /// <remarks>
        /// This is a member of the <see cref="Country"/> enum.
        /// Use this value to refer to Saint Barthélemy in operations utilizing <see cref="Country"/>.
        /// </remarks>
        BL = SaintBarthelemy,

        /// <summary>
        /// Represents Saint Helena, a territory of the United Kingdom.
        /// This member is part of the <see cref="Country"/> enumeration.
        /// </summary>
        SH = SaintHelena,

        /// <summary>
        /// Represents the country code for Saint Kitts and Nevis in the <see cref="Country"/> enumeration.
        /// </summary>
        KN = SaintKittsAndNevis,

        /// <summary>
        /// Represents Anguilla in the <see cref="Country"/> enumeration.
        /// </summary>
        AI = Anguilla,

        /// <summary>
        /// Represents the country Saint Lucia in the <see cref="Country"/> enumeration.
        /// </summary>
        LC = SaintLucia,

        /// <summary>
        /// Represents the country code for Saint Martin (French part) in the <see cref="Country"/> enumeration.
        /// </summary>
        MF = SaintMartin,

        /// <summary>
        /// Represents the enum member <see cref="Country.PM"/> within the <see cref="Country"/> enumeration.
        /// </summary>
        PM = SaintPierreAndMiquelon,

        /// <summary>
        /// Represents Saint Vincent and the Grenadines in the <see cref="Country"/> enumeration.
        /// </summary>
        VC = SaintVincentAndTheGrenadines,

        /// <summary>
        /// Represents San Marino as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        SM = SanMarino,

        /// <summary>
        /// Represents the country Saint Helena, Ascension and Tristan da Cunha as defined in the <see cref="Country"/> enumeration.
        /// </summary>
        ST = SaoTomeAndPrincipe,

        /// <summary>
        /// Represents South Africa in the <see cref="Country"/> <see cref="Enum"/>.
        /// </summary>
        SA = SaudiArabia,

        /// <summary>
        /// Represents the country Senegal in the <see cref="Country"/> enumeration.
        /// </summary>
        SN = Senegal,

        /// <summary>
        /// Represents the country code for Serbia in the <see cref="Country"/> enum.
        /// </summary>
        RS = Serbia,

        /// <summary>
        /// Represents the country code for Seychelles.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="Country"/> enumeration, which defines various country codes.
        /// </remarks>
        SC = Seychelles,

        /// <summary>
        /// Represents the country Sierra Leone in the <see cref="Country"/> enumeration.
        /// </summary>
        SL = SierraLeone,

        /// <summary>
        /// Represents the country Singapore in the <see cref="Country"/> enumeration.
        /// </summary>
        SG = Singapore,

        /// <summary>
        /// Represents Slovakia in the <see cref="Country"/> enumeration.
        /// </summary>
        SK = Slovakia,

        /// <summary>
        /// Represents Vietnam in the <see cref="Country"/> enumeration.
        /// </summary>
        VN = Vietnam,

        /// <summary>
        /// Represents the country Slovenia as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        SI = Slovenia,

        /// <summary>
        /// Represents the country South Sudan in the <see cref="Country"/> enumeration.
        /// </summary>
        SO = Somalia,

        /// <summary>
        /// Represents South Africa in the <see cref="Country"/> enumeration.
        /// </summary>
        ZA = SouthAfrica,

        /// <summary>
        /// Represents Zimbabwe in the <see cref="Country"/> enumeration.
        /// </summary>
        ZW = Zimbabwe,

        /// <summary>
        /// Represents Spain as a member of the <see cref="Country"/> enum.
        /// </summary>
        ES = Spain,

        /// <summary>
        /// Represents South Sudan in the <see cref="Country"/> enumeration.
        /// </summary>
        SS = SouthSudan,

        /// <summary>
        /// Represents Sudan in the <see cref="Country"/> enumeration.
        /// </summary>
        SD = Sudan,

        /// <summary>
        /// Represents the country code for Western Sahara as defined in the <see cref="Country"/> enumeration.
        /// </summary>
        EH = WesternSahara,

        /// <summary>
        /// Represents Serbia in the <see cref="Country"/> enumeration.
        /// </summary>
        SR = Suriname,

        /// <summary>
        /// Represents Svalbard and Jan Mayen in the <see cref="Country"/> enumeration.
        /// </summary>
        SJ = SvalbardAndJanMayen,

        /// <summary>
        /// Represents the country code for Eswatini (Swaziland) in the <see cref="Country"/> enumeration.
        /// </summary>
        SZ = Eswatini,

        /// <summary>
        /// Represents Sweden in the <see cref="Country"/> enumeration.
        /// </summary>
        SE = Sweden,

        /// <summary>
        /// Represents the country code for Switzerland in the <see cref="Country"/> enumeration.
        /// </summary>
        CH = Switzerland,

        /// <summary>
        /// Represents Syria in the <see cref="Country"/> enumeration.
        /// </summary>
        SY = SyrianArabRepublic,

        /// <summary>
        /// Represents the country Tajikistan within the <see cref="Country"/> enumeration.
        /// </summary>
        TJ = Tajikistan,

        /// <summary>
        /// Represents Thailand in the <see cref="Country"/> enumeration.
        /// </summary>
        TH = Thailand,

        /// <summary>
        /// Represents the country Togo in the <see cref="Country"/> enum.
        /// </summary>
        TG = Togo,

        /// <summary>
        /// Represents the country code for Tokelau in the <see cref="Country"/> enumeration.
        /// </summary>
        TK = Tokelau,

        /// <summary>
        /// Represents the country Tonga in the <see cref="Country"/> enum.
        /// </summary>
        TO = Tonga,

        /// <summary>
        /// Represents Trinidad and Tobago in the <see cref="Country"/> enumeration.
        /// </summary>
        TT = TrinidadAndTobago,

        /// <summary>
        /// Represents the country code for the United Arab Emirates in the <see cref="Country"/> enumeration.
        /// </summary>
        AE = UnitedArabEmirates,

        /// <summary>
        /// Represents the country code for Tunisia in the <see cref="Country"/> enumeration.
        /// </summary>
        TN = Tunisia,

        /// <summary>
        /// Represents the country Turkey in the <see cref="Country"/> enumeration.
        /// </summary>
        TR = Turkey,

        /// <summary>
        /// Represents Turkmenistan in the <see cref="Country"/> enumeration.
        /// </summary>
        TM = Turkmenistan,

        /// <summary>
        /// Represents the country  Turks and Caicos Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        TC = TurksAndCaicosIslands,

        /// <summary>
        /// Represents the country Tuvalu in the <see cref="Country"/> enumeration.
        /// </summary>
        TV = Tuvalu,

        /// <summary>
        /// Represents the country Uganda in the <see cref="Country"/> enumeration.
        /// </summary>
        UG = Uganda,

        /// <summary>
        /// Represents Ukraine in the <see cref="Country"/> enumeration.
        /// </summary>
        UA = Ukraine,

        /// <summary>
        /// Represents the country code for North Macedonia as a member of the <see cref="Country"/> enum.
        /// </summary>
        MK = NorthMacedonia,

        /// <summary>
        /// Represents the country Egypt in the enumeration <see cref="Country"/>.
        /// </summary>
        EG = Egypt,

        /// <summary>
        /// Represents Great Britain in the <see cref="Country"/> enumeration.
        /// </summary>
        GB = UnitedKingdom,

        /// <summary>
        /// Represents the country code for Guernsey within the <see cref="Country"/> enumeration.
        /// </summary>
        GG = Guernsey,

        /// <summary>
        /// Represents the country code for Jersey.
        /// </summary>
        JE = Jersey,

        /// <summary>
        /// Represents the country code for Isle of Man.
        /// This is a member of the <see cref="Country"/> enum.
        /// </summary>
        IM = IsleOfMan,

        /// <summary>
        /// Represents the country Tanzania in the <see cref="Country"/> enum.
        /// </summary>
        TZ = Tanzania,

        /// <summary>
        /// Represents the United States in the <see cref="Country"/> enumeration.
        /// </summary>
        US = UnitedStatesOfAmerica,

        /// <summary>
        /// Represents the United States Virgin Islands in the <see cref="Country"/> enumeration.
        /// </summary>
        VI = VirginIslandsUS,

        /// <summary>
        /// Represents the country code for Burkina Faso in the <see cref="Country"/> enumeration.
        /// </summary>
        BF = BurkinaFaso,

        /// <summary>
        /// Represents the country Uruguay as a member of the <see cref="Country"/> enumeration.
        /// </summary>
        UY = Uruguay,

        /// <summary>
        /// Represents the country Uzbekistan in the <see cref="Country"/> enumeration.
        /// </summary>
        UZ = Uzbekistan,

        /// <summary>
        /// Represents Venezuela in the <see cref="Country"/> enumeration.
        /// </summary>
        VE = Venezuela,

        /// <summary>
        /// Represents the country code for Wallis and Futuna in the <see cref="Country"/> enumeration.
        /// </summary>
        WF = WallisAndFutuna,

        /// <summary>
        /// Represents Samoa in the <see cref="Country"/> enumeration.
        /// </summary>
        WS = Samoa,

        /// <summary>
        /// Represents the country Yemen in the <see cref="Country"/> enum.
        /// </summary>
        YE = Yemen,

        /// <summary>
        /// Represents the country Zambia in the enumeration <see cref="Country"/>.
        /// </summary>
        ZM = Zambia,
    }
}


