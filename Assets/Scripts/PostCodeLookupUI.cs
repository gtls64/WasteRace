using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PostCodeLookupUI : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TMP_InputField postCodeInput;
    public Button lookupButton;

    public GameObject resultCanvas;
    public Button nextSceneButton;

    [System.Serializable]
    public class CSVRow
    {
        public string _id;
        public string Name;
        public int PostCode;
    }

    public List<CSVRow> csvData = new List<CSVRow>();

    private void Start()
    {
        LoadCSVData();
        lookupButton.onClick.AddListener(LookupPostCode);
        nextSceneButton.onClick.AddListener(GoToNextScene);

        resultCanvas.SetActive(false); // Hide the result canvas initially
    }

    private void LoadCSVData()
    {
        string csvDataText = @"id,Name,Office phone,Website,LGA,Address,Suburb,Post code,Facility type,Latitude,Longitude
1,Aurukun Tip,(07) 4060 6800,http://aurukun.qld.gov.au,Aurukun Shire Council,3.5 Kms East of Aurukun,Aurukun,4871,Landfill,-13.350689,141.762644
2,Bollon Landfill,(07) 4620 8888,http://www.balonne.qld.gov.au,Balonne Shire Council,Balonne Hwy,Bollon,4488,Landfill,-28.028423,147.466715
3,Dirranbandi Landfill,(07) 4620 8888,http://www.balonne.qld.gov.au,Balonne Shire Council,Whyenbah Road,Dirranbandi,4486,Landfill,-28.561892,148.246103
4,Hebel Landfill,(07) 4620 8888,http://www.balonne.qld.gov.au/landfills,Balonne Shire Council,Castlereagh Highway,Hebel,4486,Landfill,-28.977782,147.801585
5,St George Landfill,(07) 4620 8888,http://www.balonne.qld.gov.au,Balonne Shire Council,Kemp Road,St George,4487,Landfill,-28.065335,148.588035
6,Thallon Landfill,(07) 4620 8888,http://www.balonne.qld.gov.au,Balonne Shire Council,1km off Carnarvon Highway,Thallon,4497,Landfill,-28.646928,148.864592
7,Banana Transfer Station,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Barfield Road,Banana,4702,Transfer station,-24.492026,150.184244
8,Baralaba Transfer Station,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Ashfield Road Extension,Baralaba,4702,Transfer station,-24.190342,149.813593
9,Biloela Transfer Station,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Linkes Road (WEBSITE says: Calvale Road),Biloela,4715,Transfer station,-24.384709,150.5454
10,Biloela Trap Gully Landfill,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Forestry Road,Biloela,4715,Landfill,-24.284485,150.556892
11,Cracow Landfill,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Nathan Gorge Road,Cracow,4719,Landfill,-25.308693,150.305032
12,Jambin Transfer Station,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Jambin - Goovigen Road,Jambin,4702,Transfer station,-24.1909,150.361851
13,Moura Transfer Station and Landfill,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Dawson Highway West of Moura,Moura,4718,Transfer station,-24.587855,149.957968
14,Taroom Landfill,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Leichhardt Highway,Taroom,4420,Landfill,-25.652933,149.807848
15,Thangool Transfer Station,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Cnr Burnett Highway and the Cocks - Millard Road,Thangool,4716,Transfer station,-24.494972,150.592667
16,Theodore Landfill,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Goolaara - Heinekes Road,Theodore,4719,Transfer station,-24.911648,150.085172
17,Wowan Landfill,(07) 4992 9500,http://www.banana.qld.gov.au,Banana Shire Council,Dee River Road,Wowan,4702,Transfer station,-23.900431,150.227331
18,Alpha Refuse Disposal Site,(07) 4651 5600,http://www.barcaldinerc.qld.gov.au,Barcaldine Regional Council,Gordon Street,Alpha,4724,Landfill,-23.658043,146.621495
19,Barcaldine Waste Landfill,(07) 4651 5600,http://www.barcaldinerc.qld.gov.au,Barcaldine Regional Council,Landsborough Highway,Barcaldine,4725,Landfill,-23.589264,145.27856
20,Jericho Refuse Disposal Site,(07) 4651 5600,http://www.barcaldinerc.qld.gov.au,Barcaldine Regional Council,Jericho - Aramac Road,Jericho,4728,Landfill,-23.59012,146.124834
21,Muttaburra Refuse Disposal Site,(07) 4651 5600,http://www.barcaldinerc.qld.gov.au,Barcaldine Regional Council,Hospital Road,Muttaburra,4732,Landfill,-22.601405,144.52918
22,Jundah Refuse Tip,(07) 4658 6900,http://www.barcoo.qld.gov.au,Barcoo Shire Council,Off Jundah - Quilpie Road,Jundah,4736,Landfill,-24.842919,143.083336
23,Stonehenge Refuse Tip,(07) 4658 6900,http://www.barcoo.qld.gov.au,Barcoo Shire Council,Off Stonehenge Access Road,Stonehenge,4730,Landfill,-24.352673,143.30641
24,Windorah Refuse Tip,(07) 4658 6900,http://www.barcoo.qld.gov.au,Barcoo Shire Council,Off Albert Street,Windorah,4481,Landfill,-25.413237,142.649668
25,Blackall Landfill,(07) 4621 6600,http://www.btrc.qld.gov.au,Blackall - Tambo Regional Council,Evora Road,Blackall,4472,Landfill,-24.403807,145.475478
26,Tambo Landfill,(07) 4621 6600,http://www.btrc.qld.gov.au,Blackall - Tambo Regional Council,Springsure Road,Tambo,4478,Landfill,-24.85227,146.256298
27,Boulia Rubbish Tip,(07) 4746 3188,http://www.boulia.qld.gov.au/,Boulia Shire Council,Boulia - Bedourie Road,Boulia,4829,Landfill,-22.930035,139.916856
28,Admax Processing - Virginia,(07) 3865 2411,"",Brisbane City Council,48 Telford Street,Virginia,4014,Metal recycling,-27.367965,153.06425
29,Alex Fraser, Nudgee,136 135,http://www.alexfraser.com.au,Brisbane City Council,1512 Nudgee Road,Nudgee Beach,4014,Construction and demolition recycling,-27.364564,153.100649
30,AMR RECYCLERS -Coopers Plains,(07) 3344 4444,http://www.actionmetalrecyclers.com.au/,Brisbane City Council,68 Selhurst Street,Coopers Plains,4108,Metal recycling,-27.571354,153.03233
        31,Bat Rec -Wacol,(07) 3879 4507,http://www.batrec.com/,Brisbane City Council,Shed 3A/61 River Road,Redbank,4076,Battery recycling,-27.599902,152.935709
        32,BMI Nudgee Road - Hendra,(07) 3268 2677,https://www.bmigroup.com.au/portfolio/waste-disposal-brisbane/,Brisbane City Council,538 Nudgee Road,Hendra,4011,Transfer station,-27.40886,153.07406
        33,Chandler Transfer Station,(07) 3027 4655,http://www.brisbane.qld.gov.au,Brisbane City Council,728 Tilley Road,Chandler,4155,Transfer station,-27.514202,153.15159
        34,Ferny Grove Transfer Station,(07) 3027 4655,http://www.brisbane.qld.gov.au,Brisbane City Council,101 Upper Kedron Road,Ferny Grove,4055,Transfer station,-27.410861,152.937937
        35,J.J.Richards - Wacol,(07) 3488 9600,http://www.jjrichards.com.au,Brisbane City Council,15 Production Street,Wacol,4076,Transfer station,-27.589565,152.932129
        36,Nudgee Transfer Station,(07) 3027 4655,http://www.brisbane.qld.gov.au,Brisbane City Council,1402 Nudgee Road,Nudgee Beach,4123,Transfer station,-27.361005,153.100585
        37,ONESTEEL Recycling -Hemmant,(07) 3131 2300,http://www.onesteel.com,Brisbane City Council,61 Anton Road,Hemmant,4173,Metal recycling,-27.435189,153.134071
        38,Pinkenba Waste Transfer Station,(07) 3260 2100,http://www.glbquarry.com.au,Brisbane City Council,45 Eagle Farm Road,Pinkenba,4008,Construction and demolition recycling,-27.428282,153.115381
        39,Tall Ingots -Yeerongpilly,(07) 3892 2033,http://www.tallingots.com.au/,Brisbane City Council,12 Tennyson Memorial Avenue,Yeerongpilly,4105,Metal recycling,-27.527422,153.012283
        40,Transpacific Resource Recycling - Willawong,(07) 3723 7600,http://www.transpacific.com.au,Brisbane City Council,343 Bowhill Road,Willawong,4110,Construction and demolition recycling,-27.582508,152.99729
        41,Willawong Transfer Station,(07) 3027 4655,http://www.brisbane.qld.gov.au,Brisbane City Council,360 Sherbrooke Road,Willawong,4110,Transfer station,-27.599995,153.004013
        42,Hungerford Landfill,(07) 4621 8000,http://www.bulloo.qld.gov.au,Bulloo Shire Council,Chale Street,Hungerford,4493,Landfill,-28.991986,144.415258
        43,Noccundra Landfill,(07) 4621 8000,http://www.bulloo.qld.gov.au,Bulloo Shire Council,Warry Gate Road,Noccundra,4492,Landfill,-27.8178,142.582888
        44,Thargomindah Landfill and Transfer Station,(07) 4621 8000,http://www.bulloo.qld.gov.au,Bulloo Shire Council,Refuse Lane,Thargomindah,4492,Landfill,-27.981892,143.820842
        45,Avondale Waste Management Facility,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,17 Pollocks Road,Avondale,4670,Landfill,-24.764123,152.1511
        46,Booyal Transfer Station,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,Old School Road,Booyal,4671,Transfer station,-25.208147,152.052977
        47,Bundaberg Waste Management Facility,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,46 University Drive (Samuels Road),Bundaberg,4670,Landfill,-24.89783,152.309994
        48,Buxton Transfer Station,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,Gregory Street (Website says ""Powers street""),Buxton,4660,Transfer station,-25.195982,152.535493
        49,Childers Waste Management Facility,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,Nissens Lane,Childers,4660,Landfill,-25.246672,152.265092
        50,Cordalba Transfer Station,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,Cemetary Road,Cordalba,4660,Transfer station,-25.153803,152.214991
        51,Meadowvale Waste Management Facility,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,443 Rosedale Road,Meadowvale,4670,Transfer station,-24.82426,152.273301
        52,Qunaba Waste Management Facility,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,180 Potters Road,Mon Repos (website says Bargara),4670,Landfill,-24.810127,152.428908
        53,South Kolan Transfer Station,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,446 Birthamba Road,South Kolan,4670,Transfer station,-24.894936,152.165248
        54,Tirroan Waste Management Facility,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,Dalesford Road (website says 'Tablelands road, Dalesford via Gin Gin') ,Tirroan,4671,Landfill,-25.004054,151.919707
        55,Wide Bay Capricorn Battery Recyclers - Bundaberg North,(07) 4151 4600,http://www.wbcbatteryrecyclers.com.au,Bundaberg Regional Council,96 Mt Perry Road Shed C,Bundaberg North,4670,Battery recycling,-24.85078,152.322617
        56,Woodgate Waste Transfer Station,(07) 4130 4420,http://bundaberg.qld.gov.au/,Bundaberg Regional Council,1683 Woodgate Road,Woodgate,4660,Transfer station,-25.106099,152.537061
        57,Giru Transfer Station,(07) 4783 9800,http://www.burdekin.qld.gov.au,Burdekin Shire Council,Boat Ramp Road (COUNCIL WEBSITE: Cromarty Creek Landing Road),Giru,4809,Transfer station,-19.49508,147.095455
        58,Kirknie Road Landfill,(07) 4783 9800,http://www.burdekin.qld.gov.au,Burdekin Shire Council,1608 Kirknie Road,Home Hill,4806,Landfill,-19.737002,147.291939
        59,Burketown Waste Disposal Facility,(07) 4745 5100,http://www.burke.qld.gov.au,Burke Shire Council,Wills Developmental Road,Burketown,4830,Landfill,-17.75211,139.524547
        60,Babinda Transfer Station,(07) 4044 3044,http://www.cairns.qld.gov.au,Cairns Regional Council,Lot 1 Kruckow Road,Babinda,4861,Transfer station,-17.313016,145.961782
        61,Cairns Mulch,(07) 4067 1555,http://www.cairnsmulch.com.au,Cairns Regional Council,44 - 56 Jackson Drive,Woree,4870,Organic processing,-16.959385,145.750387
        62,Evergreen Top Dressing and Sand,(07) 4056 1024,"",Cairns Regional Council,205 Moller Road, Aloomba,4871,Landfill,-17.128536,145.835044
63,Gordonvale Transfer Station,(07) 4044 3044,http://www.cairns.qld.gov.au,Cairns Regional Council,Lot 1 Bruce Highway,Gordonvale,4871,Transfer station,-17.121097,145.816642
        64,Lemura Sand Co Pty Ltd,(07) 4058 1187,"",Cairns Regional Council,Brinsmead - Kamerunga Road,Lake Placid,4878,Landfill,-16.871667,145.690556
65,Medalfield - Kamerunga,(07) 4034 1784,"",Cairns Regional Council,76 Lower Freshwater Road,Kamerunga,4870,Landfill,-16.874946,145.692524
66,Northern Sands Pty Ltd,(07) 4055 9585,http://northernsands.com.au,Cairns Regional Council,Lot 5 Captain Cook Highway,Holloways Beach,4870,Landfill,-16.856355,145.722666
        67,Portsmith Transfer Station,(07) 4044 3044,http://www.cairns.qld.gov.au,Cairns Regional Council,37-51 Lyons Street,Portsmith,4870,Transfer station,-16.948788,145.762655
        68,Smithfield Transfer Station,(07) 4044 3044,http://www.cairns.qld.gov.au,Cairns Regional Council,Lot 1 Dunne Road,Smithfield,4871,Transfer station,-16.825862,145.704854
        69,Karumba Waste Facility,(07) 4745 2200,http://www.carpentaria.qld.gov.au,Carpentaria Shire Council,Cnr Karumba Dev Road & Karumba Point Road,Karumba,4890,Landfill,-17.459391,140.857419
        70,Normanton Waste Disposal Facility,(07) 4745 2200,http://www.carpentaria.qld.gov.au,Carpentaria Shire Council,Burke Development Road,Normanton,4890,Landfill,-17.687845,141.055627
        71,Bells Creek Waste Transfer Station,(07) 4043 9140,http://www.cassowarycoast.qld.gov.au,Cassowary Coast Regional Council,Off the Bruce Highway between Silkwood and El Arish,Daveson,4855,Transfer station,-17.780339,146.029509
        72,Cardwell Waste Transfer Station,(07) 4043 9140,http://www.cassowarycoast.qld.gov.au,Cassowary Coast Regional Council,Lawson Drive,Cardwell,4849,Transfer station,-18.244934,146.008634
        73,Stoters Hill Waste Transfer Station and Landfill,(07) 4043 9140,http://www.cassowarycoast.qld.gov.au,Cassowary Coast Regional Council,Quarry Drive off Palmerston Highway,Stoters Hill,4860,Landfill,-17.539163,145.971866
        74,Tully Landfill / Tully Transfer Station,(07) 4043 9140,http://www.cassowarycoast.qld.gov.au,Cassowary Coast Regional Council,374 Tully Gorge Road,Tully,4860,Landfill,-17.939936,145.892538
        75,Blackwater Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,31 Ardurad Road,Blackwater,4717,Landfill,-23.63185,148.874626
        76,Bluff Transfer Station,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Bluff - Jellinbah Road,Bluff,4720,Transfer station,-23.571856,149.060597
        77,Bogantungan Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Capricorn Highway West of Emerald,Bogantungan,4720,Landfill,-23.657347,147.298054
        78,Capella Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Capella-Clermont Highway (approx 3 km from Capella Post Office),Capella,4723,Landfill,-23.070772,148.008417
        79,Dingo Transfer Station,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,56 Normanby Street,Dingo,4720,Transfer station,-23.642444,149.325122
        80,Duaringa Transfer Station,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,50 Elizabeth Street,Duaringa,4720,Transfer station,-23.718354,149.683198
        81,Lochlees Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Lochlees Road,Emerald,4720,Landfill,-23.552146,148.26774
        82,Rolleston Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Rolleston Aerodrome Road ,Rolleston,4702,Landfill,-24.456492,148.62353
        83,Sapphire Rubyvale Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Sapphire - Rubyvale Road,Rubyvale,4720,Landfill,-23.437884,147.724376
        84,Springsure Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Springsure - Tambo Road (approx 3 km from town),Springsure,4722,Landfill,-24.114006,148.068788
        85,Tieri Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,Capella - Tieri Road (approx 3km from Tieri),Tieri,4723,Transfer station,-23.043828,148.328257
        86,Willows Landfill,1300 242 686,http://www.centralhighlands.qld.gov.au,Central Highlands Regional Council,942 Willows Road,Willows,4720,Landfill,-23.731466,147.531267
        87,Greenvale Landfill,(07) 4761 5300,http://www.charterstowers.qld.gov.au,Charters Towers Regional Council,Gregory Development Road,Greenvale,4816,Landfill,-18.99672,144.950974
        88,Pentland Landfill,(07) 4761 5300,http://www.charterstowers.qld.gov.au,Charters Towers Regional Council,Council Reserve off Aramac Road,Pentland,4816,Landfill,-20.532645,145.39935
        89,Ravenswood Landfill,(07) 4761 5300,http://www.charterstowers.qld.gov.au,Charters Towers Regional Council,Burdekin Falls Dam Road,Ravenswood,4816,Landfill,-20.12348,146.885619
        90,Stubley Street Landfill,(07) 4761 5300,http://www.charterstowers.qld.gov.au,Charters Towers Regional Council,1 Stubley Street,Charters Towers,4820,Landfill,-20.056713,146.244571
        91,Cherbourg Rubbish Tip,(07) 4168 1866,http://www.cherbourg.qld.gov.au,Cherbourg Aboriginal Shire Council,1 km SW Bulgi Street,Cherbourg,4605,Landfill,-26.299921,151.946985
        92,Cloncurry Regulated Waste Facility,(07) 4742 4100,http://www.cloncurry.qld.gov.au,Cloncurry Shire Council,Zingari Road 6 km North town,Cloncurry,4824,Landfill,-20.652099,140.518153
        93,Cloncurry Waste Facility,(07) 4742 4100,http://www.cloncurry.qld.gov.au,Cloncurry Shire Council,Burke Development Road 4.5 km NW town,Cloncurry,4824,Landfill,-20.672657,140.478858
        94,Dajarra Waste Facility,(07) 4742 4100,http://www.cloncurry.qld.gov.au,Cloncurry Shire Council,Diamantina Development Road 2 km SW of town,Dajarra,4825,Landfill,-21.705116,139.493894
        95,Ayton Landfill,(07) 4069 5444,http://www.cook.qld.gov.au,Cook Shire Council,2703 Bloomfield Road,Ayton,4895,Transfer station,-15.899773,145.342273
        96,Coen Landfill,(07) 4069 5444,http://www.cook.qld.gov.au,Cook Shire Council,Oscar Creek Road,Coen,4892,Landfill,-13.952374,143.203309
        97,Cooktown Landfill,(07) 4069 5444,http://www.cook.qld.gov.au,Cook Shire Council,Macmillan Street,Cooktown,4895,Transfer station,-15.470947,145.214618
        98,Lakeland Landfill,(07) 4069 5444,http://www.cook.qld.gov.au,Cook Shire Council,Honey Dam Road,Lakeland,4895,Transfer station,-15.868615,144.821714
        99,Laura Landfill,(07) 4069 5444,http://www.cook.qld.gov.au,Cook Shire Council,Peninsula Developmental Road,Laura,4871,Landfill,-15.558597,144.440637
        100,Croydon Waste Facility,(07) 4748 7100,http://www.croydon.qld.gov.au,Croydon Shire Council,Claraville Road,Croydon,4871,Landfill,-18.226319,142.241441
        101,Bedourie Landfill,(07) 4746 1202,http://www.diamantina.qld.gov.au,Diamantina Shire Council,Cane Grass Road is the closest road -24.3418 139.461778,Bedourie,4829,Landfill,-24.341805,139.461778
        102,Birdsville Landfill,(07) 4746 1202,http://www.diamantina.qld.gov.au,Diamantina Shire Council,Adelaide Street is the closest street - 25.9003 139.338257,Birdsville,4829,Landfill,-25.900307,139.338257
        103,Doomadgee Waste Disposal Facility,(07) 4745 8263,"",Doomadgee Aboriginal Shire Council, Wollagarang Road,Doomadgee,4830,Landfill,-17.932806,138.83199
104,Killaloe Landfill,(07) 4099 9444,http://www.douglas.qld.gov.au,Douglas Shire Council,Lot 170 Killaloe Dump Road,Killaloe,4877,Landfill,-16.482295,145.421132
        105,Newell Beach Transfer Station &Landfill,(07) 4099 9444,http://www.douglas.qld.gov.au,Douglas Shire Council,Lot 287 Rankin Street,Newell Beach,4877,Landfill,-16.43234,145.403144
        106,Einasleigh Landfill,(07) 4062 1233,http://www.etheridge.qld.gov.au,Etheridge Shire Council,Baroota Street,Einasleigh,4871,Landfill,-18.527712,144.088169
        107,Forsayth Landfill,(07) 4062 1233,http://www.etheridge.qld.gov.au,Etheridge Shire Council,North Heads Road,Forsayth,4871,Landfill,-18.584136,143.591787
        108,Georgetown Landfill,(07) 4062 1233,http://www.etheridge.qld.gov.au,Etheridge Shire Council,Georgetown to Forsayth Road,Georgetown,4871,Landfill,-18.301157,143.542031
        109,Mt Surprise Landfill,(07) 4062 1233,http://www.etheridge.qld.gov.au,Etheridge Shire Council,Gulf Development Road,Mt Surprise,4871,Landfill,-18.130066,144.343748
        110,Hughenden Landfill,(07) 4741 2900,http://www.flinders.qld.gov.au,Flinders Shire Council,at the end of McLaren Street,Hughenden,4821,Landfill,-20.848379,144.172951
        111,Prairie Landfill,(07) 4741 2900,http://www.flinders.qld.gov.au,Flinders Shire Council,Prairie - Muttaburra Road,Prairie,4821,Landfill,-20.878662,144.603571
        112,Stamford Landfill,(07) 4741 2900,http://www.flinders.qld.gov.au,Flinders Shire Council,Winton Road,Stamford,4821,Landfill,-21.262126,143.810967
        113,Torrens Creek Landfill,(07) 4741 2900,http://www.flinders.qld.gov.au,Flinders Shire Council,Bedford Street,Torrens Creek,4821,Landfill,-20.769176,145.013705
        114,Aramara Landfill,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Maryborough - Biggenden Road,Aramara,4650,Landfill,-25.607163,152.282098
        115,Bauple Transfer Station,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Bauple Drive,Bauple,4650,Transfer station,-25.774997,152.601486
        116,Boonooroo Landfill,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Boonooroo Road,Boonooroo,4650,Landfill,-25.649486,152.857179
        117,Burrum Heads Transfer Station,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Orchid Drive,Burrum Heads,4659,Transfer station,-25.2086,152.614676
        118,Granville Landfill,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Maryborough - Cooloola Road,Granville,4650,Landfill,-25.559173,152.740414
        119,Howard Transfer Station,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Powerhouse Road,Howard,4659,Transfer station,-25.322419,152.573955
        120,Maryborough Landfill,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Saltwater Creek Road,Maryborough,4650,Landfill,-25.50692,152.709473
        121,Nikenbah Transfer Station,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Aalborg Road North,Nikenbah,4655,Transfer station ,-25.315444,152.804513
        122,Tinana Landfill,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Bosel Road,Tinana,4650,Landfill,-25.548524,152.646958
        123,Toogoom Landfill,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,O'Regan Creek Road,Toogoom,4655,Landfill,-25.260337,152.67245
        124,Transpacific Cleanaway -Hervey Bay MRF,(07) 3888 0341,http://www.transpacific.com.au/content/home.aspx?navId=295,Fraser Coast Regional Council,48-50 Industrial Avenue,Dundowran,4655,Transfer station,-25.297266,152.792428
        125,Yengarie Transfer Station,1300 794 929,http://www.frasercoast.qld.gov.au,Fraser Coast Regional Council,Cnr Mungar & Quarry Roads,Yengarie,4650,Transfer station,-25.532889,152.620364
        126,Benaraby Regional Landfill,(07) 4970 0700,http://www.gladstone.qld.gov.au,Gladstone Regional Council,48567 Bruce Highway,Benaraby,4680,Landfill,-24.02285,151.353752
        127,Bororen Transfer Station,(07) 4970 0700,http://www.gladstone.qld.gov.au,Gladstone Regional Council,Bruce Highway,Bororen,4678,Transfer station,-24.22253,151.483872
        128,Calliope Transfer Station,(07) 4970 0700,http://www.gladstone.qld.gov.au,Gladstone Regional Council,131 Racecourse Road,Calliope,4680,Transfer station,-24.02576,151.201196
        129,Gladstone Transfer Station,(07) 4970 0700,http://www.gladstone.qld.gov.au,Gladstone Regional Council,Joe Joseph Drive,Gladstone,4680,Transfer station,-23.859407,151.23653
        130,J.J.Richards - Gladstone,(07) 3488 9600,http://www.jjrichards.com.au,Gladstone Regional Council,46 Bensted Street,Callemondah,4680,Transfer station,-23.860944,151.224676
        131,Lowmead Transfer Station,(07) 4970 0700,http://www.gladstone.qld.gov.au,Gladstone Regional Council,Claytons Road,Lowmead,4676,Transfer station,-24.53352,151.753687
        132,Rosedale Transfer Station,(07) 4970 0700,http://www.gladstone.qld.gov.au,Gladstone Regional Council,Lowmead Bundaberg Road,Rosedale,4676,Transfer station,-24.627073,151.895035
        133,Crushcon Burleigh,0416 027 753,http://www.crushconqld.com.au,Gold Coast City Council,22 Rudman Parade,Burleigh Heads,4220,Construction and demolition recycling,-28.105398,153.420253
        134,Currumbin Transfer Station,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,Currumbin Valley Road,Currumbin,4223,Transfer station,-28.202718,153.399732
        135,Gold Coast Resource Recovery,0400 468 160,"",Gold Coast City Council, Unit 3 / 3 Transport Place, Molendinar,4214,Battery recycling,-27.976909,153.356766
136,HBH Recycling -Coomera,1800 721 721,http://www.hbhrecycling.com.au,Gold Coast City Council,Lot 10 Old Pacific Highway,Coomera,4209,Construction and demolition recycling,-27.854728,153.31022
        137,Helensvale Transfer Station,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,70 Helensvale Road,Helensvale,4212,Transfer station,-27.895804,153.320792
        138,J.J.Richards - North End,(07) 3488 9600,http://www.jjrichards.com.au,Gold Coast City Council,241 Tamborine Oxenford Road,Oxenford,4210,Transfer station,-27.901563,153.297437
        139,Jacobs Well Transfer Station,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,250 Skopps Road,Jacobs Well,4208,Transfer station,-27.775563,153.33686
        140,Marlyn Compost -Norwell,(07) 5546 2289,"",Gold Coast City Council,148 Skopps Road, Norwell,4208,Organic processing,-27.77652,153.331706
141,Merrimac Transfer Station,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,Boonwaggan Road,Merrimac,4226,Transfer station,-28.044697,153.384982
        142,Molendinar Landfill,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,Corner Jacobs Road and Herbertson Drive,Molendinar,4214,Landfill,-27.974988,153.354702
        143,Mudgeeraba Transfer Station,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,287 Mudgeeraba Road,Mudgeeraba,4213,Transfer station,-28.072518,153.359681
        144,Phoenix Power Recyclers - Yatala,(07) 3807 5699,http://www.phoenixpower.com.au,Gold Coast City Council,126 Sandy Creek road,Yatala,4207,Organic processing,-27.76079,153.220288
        145,Queensland Metal Recyclers - Yatala,(07) 3386 1477,http://www.qldmetals.com,Gold Coast City Council,Yard 15 Darlington Park Industrial Estate,Yatala,4207,Metal recycling,-27.77318,153.22092
        146,Recycling Developments -Yatala,(07) 5546 7294,"",Gold Coast City Council,38 Nyholt Drive, Yatala,4207,Construction and demolition recycling,-27.730661,153.220642
147,Reedy Creek,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,45 Hutchinson Street,Burleigh Heads,4220,Landfill,-28.107256,153.420081
        148,Reedy Creek Landfill,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,82 Hutchinson Street,Burleigh Heads,4220,Landfill,-28.112352,153.415054
        149,Retrac Waste -Arundel,(07) 5528 9011,http://www.retracskips.com.au,Gold Coast City Council,24 Gibbs Street,Arundel,4212,Transfer station,-27.936005,153.383416
        150,Stapylton Green Energy - Stapylton Transfer Station,(07) 3254 2933,https://www.bmigroup.com.au/portfolio/construction-waste-disposal-brisbane/,Gold Coast City Council,215 Burnside Road,Stapylton,4207,Transfer station,-27.747914,153.25744
        151,Stapylton Landfill,1300 465 326,http://www.goldcoast.qld.gov.au,Gold Coast City Council,16 - 32 Rossmans Road,Stapylton,4207,Landfill,-27.732138,153.244731
        152,Bungunya,(07) 4671 7440,http://www.goondiwindirc.qld.gov.au,Goondiwindi Regional Council,Barwon Highway,Bungunya,4494,Landfill,-28.408485,149.676762
        153,Goondiwindi,(07) 4671 7440,http://www.goondiwindirc.qld.gov.au,Goondiwindi Regional Council,Refuse Tip Road (off Kildonan Road),Goondiwindi,4390,Landfill,-28.557223,150.332818
        154,Inglewood Landfill,(07) 4671 7440,http://www.goondiwindirc.qld.gov.au,Goondiwindi Regional Council,Inglewood - Texas Road,Inglewood,4387,Landfill,-28.431125,151.084618
        155,Talwood Landfill,(07) 4671 7440,http://www.goondiwindirc.qld.gov.au,Goondiwindi Regional Council,Mungindi Road,Talwood,4496,Landfill,-28.490831,149.461998
        156,Texas Transfer Station,(07) 4671 7440,http://www.goondiwindirc.qld.gov.au,Goondiwindi Regional Council,Pump Station Road,Texas,4385,Transfer station,-28.868869,151.185502
        157,Toobeah,(07) 4671 7440,http://www.goondiwindirc.qld.gov.au,Goondiwindi Regional Council,Barwon Highway,Toobeah,4498,Landfill,-28.418394,149.86616
        158,Yelarbon Landfill,(07) 4671 7440,http://www.goondiwindirc.qld.gov.au,Goondiwindi Regional Council,East of Sawmill Road,Yelarbon,4388,Landfill,-28.569139,150.769785
        159,Goomeri Waste Management Facility,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,Lot 108 Burnett Highway,Goomeri,4601,Landfill,-26.155487,152.066695
        160,Gunalda Waste Management Facility,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,Lot 141 Balkin Street,Gunalda,4570,Landfill,-25.999876,152.562833
        161,Gympie Landfill,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,Lot 542 Bonnick Road,Gympie,4570,Landfill,-26.168545,152.655048
        162,Kilkivan Waste Management Facility,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,McKewen Road,Kilkivan,4570,Landfill,-26.076748,152.228443
        163,Mary Valley Waste Transfer Station,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,Lot 1 Kandanga-Imbil Road,Imbil,4570,Transfer station,-26.438599,152.686006
        164,Rainbow Beach Waste Management Facility,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,Lot 3 Carlo Road,Rainbow Beach,4581,Landfill,-25.91382,153.076695
        165,Southside Waste Management Facility,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,Glastonbury Road,Widgee Crossing South,4570,Landfill,-26.203059,152.609518
        166,Tin Can Bay Waste Management Facility,1300 307 800,http://www.gympie.qld.gov.au,Gympie Regional Council,Lot 58 Snapper Creek Road,Tin Can Bay,4580,Landfill,-25.917401,152.988199
        167,Warrens Hill Waste Management Facility,(07) 4776 4600,http://www.hinchinbrook.qld.gov.au,Hinchinbrook Shire Council,45 Bosworths Road,Warrens Hill,4850,Landfill,-18.692822,146.213056
        168,Hope Vale Landfill,(07) 4083 8000,http://www.hopevale.qld.gov.au,Hope Vale Aboriginal Shire Council,Dump Road,Hope Vale,4895,Landfill,-15.283955,145.113727
        169,Chip Tyre -New Chum,(07) 3816 2711,http://www.chiptyre.com,Ipswich City Council,251 Austin Street,New Chum,4303,Tyre recycling,-27.630883,152.819987
        170,Haggarty Group -West Ipswich,(07) 3281 8144,http://www.haggarty.com.au,Ipswich City Council,347-349 Brisbane Street,West Ipswich,4305,Metal recycling,-27.621208,152.747419
        171,Riverview Waste Transfer Station,(07) 3810 6666,http://www.ipswich.qld.gov.au,Ipswich City Council,81 Riverview Road,Riverview,4303,Transfer station,-27.590725,152.840456
        172,Rosewood Waste Transfer Station,(07) 3810 6666,http://www.ipswich.qld.gov.au,Ipswich City Council,94 Oakleigh Colliery Road,Rosewood,4340,Transfer station,-27.615591,152.581753
        173,Clermont Resource Recovery Centre,1300 472 227,http://www.isaac.qld.gov.au,Isaac Regional Council,Turruma Road,Clermont,4721,Landfill,-22.808458,147.638298
        174,Dysart Landfill,1300 472 227,http://www.isaac.qld.gov.au,Isaac Regional Council,Dysart - Clermont Road,Dysart,4746,Landfill,-22.596267,148.330338
        175,Glenden Landfill,1300 472 227,http://www.isaac.qld.gov.au,Isaac Regional Council,Ewan Drive,Glenden,4742,Landfill,-21.371694,148.116075
        176,Middlemount Landfill,1300 472 227,http://www.isaac.qld.gov.au,Isaac Regional Council,Nolan Drive,Middlemount,4746,Landfill,-22.814772,148.686499
        177,Moranbah Resource Recovery Centre,1300 472 227,http://www.isaac.qld.gov.au,Isaac Regional Council,Thorpe Street,Moranbah,4744,Landfill,-21.986012,148.020268
        178,Nebo Waste Facility,1300 472 227,http://www.isaac.qld.gov.au,Isaac Regional Council,Peak Downs Highway,Nebo,4742,Landfill,-21.678964,148.693782
        179,St Lawrence Resource Recovery Centre,1300 472 227,http://www.isaac.qld.gov.au,Isaac Regional Council,St Lawrence Connection Road,St Lawrence,4707,Landfill,-22.361713,149.507065
        180,Koyanyama Refuse Tip,(07) 4083 7100,http://www.kowanyama.qld.gov.au,Kowanyama Aboriginal Shire Council,Cnr Koltmonun and Pindi Street,Kowanyama,4892,Landfill,-15.473954,141.739462
        181,Cawarral Waste Management Facility,(07) 4939 9981,http://www.livingstone.qld.gov.au,Livingstone Shire Council,Cnr Cawarral & Botos Roads,Cawarral,4702,Transfer station,-23.288984,150.673207
        182,Emu Park Waste Management Facility,(07) 4939 9981,http://www.livingstone.qld.gov.au,Livingstone Shire Council,Scenic Highway,Emu Park,4710,Transfer station,-23.233491,150.808225
        183,Stanage Bay Waste Management Facility,(07) 4939 9981,http://www.livingstone.qld.gov.au,Livingstone Shire Council,King Street (off Stanage Bay Road),Stanage Bay,4702,Landfill,-22.139259,150.046623
        184,Yeppoon Landfill,(07) 4939 9981,http://www.livingstone.qld.gov.au,Livingstone Shire Council,2745 Yeppoon Road,Barmaryee,4703,Landfill,-23.15668,150.707308
        185,Lockhart River Tip,(07) 4060 7144,http://www.lockhart.qld.gov.au,Lockhart River Aboriginal Shire Council,1.5 km south of Lockhart River,Lockhart River,4871,Landfill,-12.798941,143.345622
        186,Gatton Landfill,1300 005 872,http://www.lockyervalley.qld.gov.au,Lockyer Valley Regional Council,32 Treatment Plant Road / Fords Road,Gatton,4343,Landfill,-27.536055,152.281834
        187,Grantham Transfer Station,1300 005 872,http://www.lockyervalley.qld.gov.au,Lockyer Valley Regional Council,Back Ma Ma Road,Grantham,4347,Transfer station,-27.599784,152.202869
        188,Helidon Transfer Station,1300 005 872,http://www.lockyervalley.qld.gov.au,Lockyer Valley Regional Council,70 Seventeen Mile Road,Helidon,4344,Transfer station,-27.543754,152.133885
        189,Laidley Transfer Station,1300 005 872,http://www.lockyervalley.qld.gov.au,Lockyer Valley Regional Council,Burgess & Glencairn Roads,Laidley Heights,4341,Transfer station,-27.62805,152.363164
        190,Murphys Creek Transfer Station,1300 005 872,http://www.lockyervalley.qld.gov.au,Lockyer Valley Regional Council,90 Milora Road,Upper Lockyer,4352,Transfer station,-27.478147,152.061908
        191,Sunstate Recyclers -Laidley,(07) 5465 1006,"",Lockyer Valley Regional Council,33 Vaux Street, Laidley,4341,Metal recycling,-27.645455,152.389254
192,Withcott Transfer Station,1300 005 872,http://www.lockyervalley.qld.gov.au,Lockyer Valley Regional Council,Spa Water Road,Blanchview,4352,Transfer station,-27.553557,152.051979
        193,Browns Plains Smart Tip,(07) 3412 3412,http://www.logan.qld.gov.au,Logan City Council,349 Browns Plains Road,Heritage Park,4118,Landfill,-27.674202,153.068901
        194,Carbrook Transfer Station,(07) 3412 3412,http://www.logan.qld.gov.au,Logan City Council,1801 Mount Cotton Road,Cornubia,4130,Transfer station,-27.653215,153.234631
        195,Greenbank Transfer Station,(07) 3412 3412,http://www.logan.qld.gov.au,Logan City Council,124-142 Pub Lane (cnr Equestrian Drive),Greenbank,4124,Transfer station,-27.737705,152.971314
        196,Logan Village Transfer Station,(07) 3412 3412,http://www.logan.qld.gov.au,Logan City Council,1406 - 1432 Waterford - Tamborine Road,Logan Village,4207,Transfer station,-27.787136,153.101578
        197,Molectra Technologies -Loganholme,(07) 3440 8900,http://www.molectra.com.au,Logan City Council,1 Riverland Drive,Loganholme,4129,Tyre recycling,-27.684175,153.191222
        198,V Resource -Loganholme,(07) 3806 3306,http://www.vh-int.com,Logan City Council,33 Henry Street,Loganholme,4129,Battery recycling,-27.681483,153.189873
        199,Isisford Landfill,(07) 4658 4111,http://www.longreach.qld.gov.au,Longreach Regional Council,Saint Osyth Street,Isisford,4731,Landfill,-24.262537,144.432826
        200,Longreach Landfill,(07) 4658 4111,http://www.longreach.qld.gov.au,Longreach Regional Council,Longreach -Tocal Road,Longreach,4730,Landfill,-23.472024,144.205556
        201,Yaraka Landfill,(07) 4658 4111,http://www.longreach.qld.gov.au,Longreach Regional Council,Emmet - Yaraka Road,Yaraka,4702,Landfill,-24.87762,144.072176
        202,AJK Contracting -Mackay,(07) 4942 6363,http://www.ajkmackay.com.au,Mackay Regional Council,59 Talty Road,Mackay,4740,Construction and demolition recycling, Organic processing,-21.141951,149.151789
        203,Bayersville Green Waste facility,1300 622 529,http://www.mackay.qld.gov.au,Mackay Regional Council,Harbour Road,Mackay Harbour,4740,Transfer station,-21.115088,149.203857
        204,Eungella Transfer Station,1300 622 529,http://www.mackay.qld.gov.au,Mackay Regional Council,Bee Creek Road,Eungella,4576,Transfer station,-21.130128,148.485207
        205,Finch Hatton Transfer Station,1300 622 529,http://www.mackay.qld.gov.au,Mackay Regional Council,Trueman Depot Road,Finch Hatton,4741,Transfer station,-21.140541,148.658577
        206,Gargett Rural Transfer Station,1300 622 529,http://www.mackay.qld.gov.au/,Mackay Regional Council,Dump Road,Gargett,4741,Transfer station,-21.156155,148.760325
        207,Hay Point Rural Transfer Station ,1300 622 529,http://www.mackay.qld.gov.au/,Mackay Regional Council,Cedar Street,Hay Point,4740,Transfer station,-21.287148,149.258665
        208,J.J.Richards - Mackay,(07) 3488 9600,http://www.jjrichards.com.au,Mackay Regional Council,17-21 Michelmore Street,Paget,4740,Transfer station,-21.178899,149.167684
        209,Kolijo Rural Transfer Station,1300 622 529,http://www.mackay.qld.gov.au,Mackay Regional Council,Pelion Road,Calen,4798,Transfer station,-20.904208,148.791368
        210,Koumala Transfer Station,1300 622 529,http://www.mackay.qld.gov.au,Mackay Regional Council,Bolingbroke Road,Koumala,4798,Transfer station,-21.608764,149.242014
        211,Olgivie Constructions -Blue River Landscape Supplies,(07) 4942 0154,http://www.blueriver.net.au/,Mackay Regional Council,12 Old Foulden Road,Foulden,4740,Organic processing,-21.141271,149.146207
        212,Otterburn Rural Transfer Station,1300 622 529,http://www.mackay.qld.gov.au,Mackay Regional Council,Brand Road,Mirani,4754,Transfer station,-21.16951,148.896418
        213,Paget Waste Management Centre,1300 622 529,http://www.mackay.qld.gov.au,Mackay Regional Council,42 Crichtons Road,Paget,4740,Transfer station,-21.193193,149.163421
        214,Sarina Rural Transfer Station,1300 622 529,http://www.mackay.qld.gov.au/services/waste/projects,Mackay Regional Council,Brooks Road,Sarina,4737,Transfer station,-21.416567,149.241435
        215,Seaforth Rural Transfer Station,1300 622 529,http://www.mackay.qld.gov.au/services/waste/projects,Mackay Regional Council,Yakapari - Seaforth Road,Seaforth,4741,Transfer station,-20.906624,148.964753
        216,Mapoon Waste Facility,(07) 4090 9124,http://www.mapoon.com,Mapoon Aboriginal Shire Council,Andoom Road (1 km south of Airport),Mapoon,4874,Landfill,-12.075116,141.902413
        217,Injune Refuse Site,(07) 4624 0629,http://www.maranoa.qld.gov.au,Maranoa Regional Council,Womblebank Gap Road,Injune,4454,Landfill,-25.827979,148.552924
        218,Jackson Refuse Site,(07) 4624 0629,http://www.maranoa.qld.gov.au,Maranoa Regional Council,Pei Road,Jackson,4426,Landfill,-26.648429,149.630088
        219,Mitchell Refuse Site,(07) 4624 0629,http://www.maranoa.qld.gov.au,Maranoa Regional Council,St George Road,Mitchell,4465,Landfill,-26.498928,147.991379
        220,Roma Waste Facility,(07) 4624 0629,http://www.maranoa.qld.gov.au,Maranoa Regional Council,269 Short Street,Roma,4455,Landfill,-26.551098,148.806238
        221,Surat Refuse Site,(07) 4624 0629,http://www.maranoa.qld.gov.au,Maranoa Regional Council,Silver Springs Road,Surat,4417,Landfill,-27.172443,149.083841
        222,Wallumbilla Refuse Site,(07) 4624 0629,http://www.maranoa.qld.gov.au,Maranoa Regional Council,Tip Road,Wallumbilla,4428,Landfill,-26.58223,149.198928
        223,Almaden Landfill,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,Burke Development Road,Almaden,4871,Landfill,-17.340085,144.670565
        224,Chillagoe Landfill,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,35 Smelter Road,Chillagoe,4871,Landfill,-17.1485,144.511434
        225,Dimbulah Transfer Station,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,105 Raleigh Street,Dimbulah,4872,Transfer station,-17.13987,145.117488
        226,Irvinebank Transfer Station,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,Herberton - Petford Road,Irvinebank,4887,Transfer station,-17.420731,145.220643
        227,Julatten Transfer Station,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,1027 Euluma Creek Road,Julatten,4871,Transfer station,-16.604791,145.353234
        228,Kuranda Transfer Station,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,2186 Kennedy Highway,Kuranda,4881,Transfer station,-16.87033,145.585001
        229,Mareeba Landfill,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,38 Vaughan Street,Mareeba,4880,Landfill,-16.9897,145.408298
        230,Mount Carbine Transfer Station,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,Cemetery Road,Mount Carbine,4871,Transfer station,-16.527613,145.114829
        231,Mount Molloy Transfer Station,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,97 Bakers Road,Mount Molloy,4871,Transfer station,-16.688084,145.323714
        232,Mutchilba Transfer Station,(07) 4086 4662,http://www.msc.qld.gov.au,Mareeba Shire Council,3834 Mareeba - Dimbula Road,Mutchilba,4872,Transfer station,-17.129153,145.190916
        233,Julia Creek Recycling and Waste Management Facility,(07) 4746 7166,http://www.mckinlay.qld.gov.au,McKinlay Shire Council,Old Normanton Road,Julia Creek,4825,Landfill,-20.647954,141.732667
        234,Kynuna Landfill,(07) 4746 7166,http://www.mckinlay.qld.gov.au,McKinlay Shire Council,Landsborough Highway,Kynuna,4823,Landfill,-21.580861,141.906969
        235,McKinlay Landfill,(07) 4746 7166,http://www.mckinlay.qld.gov.au,McKinlay Shire Council,Landsborough Highway,McKinlay,4823,Landfill,-21.281551,141.307464
        236,Nelia Landfill,(07) 4746 7166,http://www.mckinlay.qld.gov.au,McKinlay Shire Council,Nelia - Bunda Road,Nelia,4823,Landfill,-20.663408,142.208559
        237,Bunya Landfill,(07) 3205 0555,http://www.moretonbay.qld.gov.au,Moreton Bay Regional Council,384 Bunya Road,Bunya,4055,Landfill,-27.382936,152.941445
        238,Caboolture Landfill,(07) 3205 0555,http://www.moretonbay.qld.gov.au,Moreton Bay Regional Council,51 McNaught Road,Caboolture,4510,Landfill,-27.07375,152.990718
        239,Caylamax Recycling -Brendale,(07) 3481 5888,http://www.caylamax.com.au,Moreton Bay Regional Council,83 Kremzow Road,Brendale,4500,Construction and demolition recycling,-27.316312,152.973014
        240,Dakabin Landfill,(07) 3205 0555,http://www.moretonbay.qld.gov.au,Moreton Bay Regional Council,336 Old Gympie Road,Dakabin,4503,Landfill,-27.224876,152.991521
        241,Kennedys Timbers and Narangba Resource Recovery Centre,(07) 3293 0528,http://www.kennedystimbers.com.au,Moreton Bay Regional Council,200-228 Potassium Street,Narangba,4504,Construction and demolition recycling, Transfer station,-27.198887,152.989906
        242,Moreton Bay Recycling - Narangba,(07) 3293 4949,"",Moreton Bay Regional Council,179 Boundary Road, Narangba,4504,Construction and demolition recycling,-27.205704,152.999114
243,Peninsula Metal Recycling - Redcliffe,(07) 3284 4996,http://pmrecycling.com.au,Moreton Bay Regional Council,268 Duffield Road,Clontarf Beach,4019,Metal recycling,-27.240945,153.080864
        244,Redcliffe Transfer Station,(07) 3205 0555,http://www.moretonbay.qld.gov.au,Moreton Bay Regional Council,261 Duffield Road,Clontarf,4019,Transfer station,-27.241081,153.079514
        245,Mt Isa Landfill,(07) 4747 3223,http://www.mountisa.qld.gov.au,Mt Isa City Council,Jessop Drive (from the Sunset Cemetery for 1.8km),Mt Isa,4825,Landfill,-20.7078,139.52136
        246,Mt Isa Metal Recyclers -Duchess Road,(07) 4749 1235,"",Mt Isa City Council,195 - 199 Duchess Road, Mt Isa,4825,Metal recycling,-20.764743,139.498107
247,Augathella Landfill,(07) 4656 8355,http://www.murweh.qld.gov.au,Murweh Shire Council,Lot 111 275 Old Tambo Road,Augathella,4477,Landfill,-25.772562,146.588625
        248,Charleville Landfill,(07) 4656 8355,http://www.murweh.qld.gov.au,Murweh Shire Council,Lot 150 282 Bollon Road,Charleville,4470,Landfill,-26.428756,146.257131
        249,Morven Landfill,(07) 4656 8355,http://www.murweh.qld.gov.au,Murweh Shire Council,Mt Maria Road,Morven,4468,Landfill,-26.398758,147.119677
        250,Eumundi Road Landfill,(07) 5329 6300,http://www.noosa.qld.gov.au,Noosa Shire Council,561 Eumundi - Noosa Road,Doonan,4562,Landfill,-26.436671,153.024941
        251,Biggenden Waste Management Facility,(07) 4160 3553,http://www.northburnett.qld.gov.au,North Burnett Regional Council,Old Coach Road,Biggenden,4646,Landfill,-25.500367,152.056917
        252,Eidsvold Waste Management Facility,(07) 4160 3553,http://www.northburnett.qld.gov.au,North Burnett Regional Council,Hollywell Road,Eidsvold,4627,Landfill,-25.369491,151.13433
        253,Gayndah Waste Management Facility,(07) 4160 3553,http://www.northburnett.qld.gov.au,North Burnett Regional Council,Rifle Range Road,Gayndah,4625,Landfill,-25.606498,151.613389
        254,Monto Waste Management Facility,(07) 4160 3553,http://www.northburnett.qld.gov.au,North Burnett Regional Council,34 Langs Road,Monto,4630,Landfill,-24.84779,151.147119
        255,Mt Perry Waste Management Facility,(07) 4160 3553,http://www.northburnett.qld.gov.au,North Burnett Regional Council,Gayndah - Mt Perry Road,Mt Perry,4671,Landfill,-25.1945,151.656083
        256,Munduberra Waste Management Facility,(07) 4160 3553,http://www.northburnett.qld.gov.au,North Burnett Regional Council,Middle Boyne Road,Boynewood,4626,Landfill,-25.608421,151.305178
        257,Northern Peninsula Area Regional Council Landfill,(07) 4090 4100,http://www.nparc.qld.gov.au,Northern Peninsula Area Regional Council,Injinoo - Umagico Road,Bamaga,4876,Landfill,-10.894849,142.336339
        258,Palm Island Transfer Station,(07) 4770 0200,http://www.piac.com.au,Palm Island Aboriginal Shire Council,Manbarra Road,Palm Island,4886,Landfill,-18.718733,146.586244
        259,Cunnamulla Landfill,(07) 4655 8400,http://www.paroo.qld.gov.au,Paroo Shire Council,Arthur Street,Cunnamulla,4480,Landfill,-28.076948,145.696389
        260,Pormpuraaw Council Landfill,(07) 4060 4155,http://www.pormpuraaw.qld.gov.au,Pormpuraaw Aboriginal Shire Council,Pormpuraaw Road,Pormpuraaw,4892,Landfill,-14.894888,141.626924
        261,Adavale Landfill,(07) 4656 0500,http://www.quilpie.qld.gov.au,Quilpie Shire Council,Patricia Park Road,Adavale,4474,Landfill,-25.896763,144.612231
        262,Eromanga Landfill,(07) 4656 0500,http://www.quilpie.qld.gov.au,Quilpie Shire Council,Cooper Development Road,Eromanga,4480,Landfill,-26.654567,143.292033
        263,Quilpie Landfill,(07) 4656 0500,http://www.quilpie.qld.gov.au,Quilpie Shire Council,Cemetery Road,Quilpie,4480,Landfill,-26.601308,144.239755
        264,Birkdale Waste Transfer Station,(07) 3829 8999,http://www.redland.qld.gov.au,Redland City Council,555-589 Old Cleveland Road East,Birkdale,4159,Transfer station,-27.508988,153.225306
        265,North Stradbroke Island Transfer Station,(07) 3829 8999,http://www.redland.qld.gov.au,Redland City Council,East Coast Road, Myora,North Stradbroke Island,4184,Transfer station,-27.451365,153.453496
        266,Redland Bay Transfer Station,(07) 3829 8999,http://www.redland.qld.gov.au,Redland City Council,761-789 German Church Road,Redland Bay,4165,Transfer station,-27.627586,153.286398
        267,Resource Recoveries and Recycling -Mt Cotton,(07) 3206 0022,http://www.rrrecycle.com.au/,Redland City Council,706 Mount Cotton Road,Sheldon,4157,Construction and demolition recycling,-27.576028,153.220818
        268,Richmond Waste Disposal Facility,(07) 4741 3277,http://www.richmond.qld.gov.au,Richmond Shire Council,Saleyard Road,Richmond,4822,Landfill,-20.751022,143.137814
        269,Alton Downs Waste Management Facility,1300 225 577,http://www.rockhamptonregion.qld.gov.au,Rockhampton Regional Council,1890 Ridgelands Road,Alton Downs,4702,Transfer station,-23.294625,150.32789
        270,Bouldercombe Transfer Station,1300 225 577,http://www.rockhamptonregion.qld.gov.au,Rockhampton Regional Council,116 Inslay Avenue,Bouldercombe,4702,Transfer station,-23.554099,150.463658
        271,Gracemere Landfill,1300 225 577,http://www.rockhamptonregion.qld.gov.au,Rockhampton Regional Council,Cnr Allen & Lucas Roads,Gracemere,4702,Landfill,-23.46082,150.473453
        272,Lakes Creek Road Landfill,1300 225 577,http://www.rockhamptonregion.qld.gov.au,Rockhampton Regional Council,152 Lakes Creek Road,Lakes Creek,4702,Landfill,-23.374593,150.532722
        273,Mount Morgan Transfer Station,1300 225 577,http://www.rockhamptonregion.qld.gov.au,Rockhampton Regional Council,Racecourse / Glenprairie Road,Mt Morgan,4714,Transfer station,-23.658862,150.380213
        274,Queensland Metal Recyclers - North Rockhampton,(07) 3386 1477,http://www.qldmetals.com,Rockhampton Regional Council,41 Dooley Street,North Rockhampton,4701,Metal recycling,-23.36127,150.511723
        275,Central Waste Management Facility,(07) 5540 5111,http://www.scenicrim.qld.gov.au,Scenic Rim Regional Council,Waste Facility Road,Bromelton,4285,Landfill,-27.973128,152.930575
        276,Esk Refuse and Recycling Centre and Landfill,(07) 5424 4000,http://www.somerset.qld.gov.au,Somerset Regional Council,Murrumba Road (3km out off Esk-Kilcoy Road),Coal Creek,4312,Landfill,-27.201886,152.440289
        277,Blackbutt Transfer Station,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,D'Aguilar Highway,Blackbutt,4306,Transfer station,-26.893493,152.128352
        278,Brigooda Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Proston - Boondooma Road,Brigooda,4613,Landfill,-26.26642,151.378767
        279,Cloyna Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Cloyna Road West,Cloyna,4605,Landfill,-26.110964,151.828404
        280,Durong Waste Facility ,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Wondai Chinchilla Road,Wondai,4610,Landfill,-26.392118,151.288187
        281,Hivesville Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Oberles Road,Hivesville,4613,Landfill,-26.173902,151.699144
        282,Kingaroy Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Luck Road,Kingaroy,4610,Landfill,-26.553863,151.792504
        283,Kumbia Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au/,South Burnett Regional Council,Kearneys Road,Kumbia,4610,Landfill,-26.690193,151.660099
        284,Murgon Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Borcherts Hill Road,Murgon,4605,Landfill,-26.253931,151.979871
        285,Nanango Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Finlay Road,Nanango,4615,Landfill,-26.697898,151.99345
        286,Proston Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Bereford Road,Proston,4613,Landfill,-26.1692,151.605331
        287,Wondai Waste Facility,(07) 4189 9100,http://www.southburnett.qld.gov.au,South Burnett Regional Council,Charlestown Road,Wondai,4606,Landfill,-26.334581,151.859065
        288,Allora Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Burges Road,Allora,4362,Transfer station,-28.026808,151.986958
        289,Dalveen Landfill,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Old Stanthorpe Road,Dalveen,4370,Landfill,-28.476953,151.980924
        290,Forest Springs Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Forest Springs - Goomburra Road,Forest Springs,4370,Transfer station,-27.997738,152.103913
        291,Greymare Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Mountain Maid Road,Greymare,4370,Transfer station,-28.217232,151.757679
        292,Karara Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Toowoomba Karara Road,Karara,4352,Transfer station,-28.205108,151.561273
        293,Killarney Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Killarney Barlows Gate Road,Killarney,4373,Transfer station,-28.351151,152.27689
        294,Leyburn Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Donavon Road,Leyburn,4365,Transfer station,-28.028091,151.590412
        295,Maryvale Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Whites Road,Maryvale,4370,Transfer station,-28.08124,152.23964
        296,Pratten Waste Transfer Station,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Springate Lane,Pratten,4370,Transfer station,-28.080686,151.780542
        297,Stanthorpe Waste Management Facility,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Rifle Range Road,Stanthorpe,4380,Landfill,-28.67942,151.938151
        298,Wallangarra Waste Transfer Station,1300 697 372,http://www.sdrc.qld.gov.au/council/contact-us,Southern Downs Regional Council,Hines Road,Wallangarra,4383,Transfer station,-28.911461,151.925513
        299,Warwick Central Waste Management Facility,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Old Stanthorpe Road,Warwick,4370,Landfill,-28.255077,152.046412
        300,Warwick Scrap Metal and Recycling,(07) 4661 7922,"",Southern Downs Regional Council,43 Progress Street, Warwick,4370,Metal recycling,-28.238386,152.03949
301,Yangan Landfill,(07) 4661 0410,http://www.sdrc.qld.gov.au/,Southern Downs Regional Council,Studwick's Road,Yangan,4371,Landfill,-28.226148,152.20424
        302,Beerwah Resource Recovery Centre,(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,121 Roberts Road,Beerwah,4519,Transfer station,-26.859784,152.946241
        303,Buderim Resource Recovery Centre,(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,Syd Lingard Drive,Buderim,4556,Transfer station,-26.684246,153.098898
        304,Caloundra Landfill and Resource Recovery Centre,(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,171 Pierce Avenue,Caloundra,4551,Landfill,-26.786545,153.068502
        305,Kenilworth Transfer Station(Council website says: Brooloo Road),(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,Cambroon Lane,Kenilworth,4574,Transfer station,-26.582768,152.721858
        306,Mapleton Transfer Station,(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,111 Delicia Road,Mapleton,4560,Transfer station,-26.62595,152.854928
        307,Nambour Resource and Recovery Centre / Landfill,(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,26 - 40 Cooney Road,Nambour,4560,Landfill,-26.614702,152.980316
        308,Witta Resource Recovery Centre,(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,Cnr Cooke and Witta Roads,Witta,4552,Transfer station,-26.705368,152.826731
        309,Yandina Transfer Station,(07) 5475 7272,http://www.sunshinecoast.qld.gov.au/,Sunshine Coast Regional Council,Browns Creek Road,Yandina,4561,Transfer station,-26.547998,152.94127
        310,Bassett Barks -Mt Beerwah,(07) 5439 3461,http://www.bassettbarks.com.au/,Sunshine Coast Regional Council ,57 Mt Beerwah Road,Glasshouse Mountains,4518,Organic processing,-26.916841,152.918934
        311,Atherton Transfer Station, Landfill & Recycling Centre,(07) 4043 4758,http://www.trc.qld.gov.au,Tablelands Regional Council,310 Herberton Road,Atherton,4883,Landfill,-17.2914,145.4726
        312,Herberton Transfer Station,1300 362 242,http://www.trc.qld.gov.au/,Tablelands Regional Council,Syme Road,Herberton,4887,Transfer station,-17.395188,145.374098
        313,Innot Hot Springs Transfer Station & Landfill OR-- is it on the Kennedy highway ?,(07) 4043 4758,http://www.trc.qld.gov.au,Tablelands Regional Council,Kennedy Highway,Innot Hot Springs,4872,Landfill,-17.693153,145.217876
        314,Malanda Transfer Station,(07) 4095 1200,http://www.trc.qld.gov.au/locations/malanda-transfer-station/,Tablelands Regional Council,228 English Road,Malanda,4885,Transfer station,-17.329717,145.606492
        315,Millaa Millaa Transfer Station,(07) 4043 4758,http://www.trc.qld.gov.au,Tablelands Regional Council,Theresa Creek Road,Millaa Millaa,4886,Transfer station,-17.500971,145.616197
        316,Mt Garnet Transfer Station,(07) 4043 4758,http://www.trc.qld.gov.au,Tablelands Regional Council,Agate Street,Mt Garnet,4872,Transfer station,-17.675527,145.12187
        317,Ravenshoe Transfer Station,(07) 4043 4758,http://www.trc.qld.gov.au,Tablelands Regional Council,Cemetery Street,Ravenshoe,4888,Transfer station,-17.622633,145.470747
        318,Yungaburra Transfer Station,(07) 4043 4758,http://www.trc.qld.gov.au,Tablelands Regional Council,Mulgrave Road,Yungaburra,4884,Transfer station,-17.260686,145.57955
        319,Beutel Oughtred &Sons - Toowoomba,(07) 4638 4438,"",Toowoomba Regional Council,38 - 72 Griffith Street, Toowoomba,4350,Construction and demolition recycling,-27.531643,151.948903
320,Bringalily Skip Bin Site,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Bringalily Creek Road,Millmerran,4357,Transfer station,-28.098247,151.162623
        321,Cecil Plains Keyed Landfill,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Cecil Plains - Cemetery Road,Cecil Plains,4407,Landfill,-27.522391,151.191591
        322,Clifton Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Logan Road,Clifton,4361,Landfill,-27.945265,151.902647
        323,Cooyar Keyed Landfill,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Cooyar - Mt Binga Road,Cooyar,4402,Landfill,-26.983458,151.854234
        324,Crows Nest Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,South Street,Crows Nest,4355,Landfill,-27.26742,152.067395
        325,Emu Creek Keyed Landfill,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Emu Creek Secondary Road,Emu Creek,4352,Landfill,-27.132044,151.962033
        326,Evergreen Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Daffodil Street,Evergreen,4404,Landfill,-27.157286,151.724121
        327,Goombungee Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Centenary Road,Goombungee,4354,Landfill,-27.284784,151.835639
        328,Greenmount Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Faulkner Road,Greenmount,4359,Landfill,-27.763898,151.927512
        329,Jondaryan Waste Management Centre,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Warrego Highway, entrace via Grants Road,Jondaryan,4403,Landfill,-27.376646,151.60311
        330,Kleinton Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Kleinton School Road,Kleinton,4352,Landfill,-27.413556,151.947723
        331,Millmerran Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,3571 Owens Scrub Road,Millmerran,4357,Landfill,-27.906679,151.292522
        332,Oakey Waste Facility ,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Lorrimer Street,Oakey,4401,Landfill,-27.439943,151.709047
        333,Pittsworth Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Tip Road (Old Gore Highway),Pittsworth,4356,Landfill,-27.720567,151.606754
        334,Planet Paints -Toowoomba,(07) 4633 3544,http://www.planetpaints.com.au,Toowoomba Regional Council,7 Allen Court,Torrington,4350,Paint recycling,-27.544188,151.892476
        335,Ravensbourne Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Post Office Road,Ravensbourne,4352,Landfill,-27.358929,152.16624
        336,Rural Residential No. 2 Skip Bin Site,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Paddys Creek Road,Millmerran,4357,Transfer station,-27.958701,151.013003
        337,Toowoomba Waste Management Centre(Bedford Street),131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Bedford Street,Cranley,4350,Landfill,-27.512528,151.923968
        338,Transpacific Technical Services - Toowoomba,(07) 4638 3711,http://www.transpacific.com.au,Toowoomba Regional Council,27 - 35 Wilkinson Street,Toowoomba,4350,Transfer station,-27.538853,151.951327
        339,Turallin Skip Bin Site,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,Western Creek Road,Millmerran,4357,Transfer station,-27.826618,151.202214
        340,Yarraman Waste Facility,131 872,http://www.toowoombarc.qld.gov.au,Toowoomba Regional Council,D'Aguilar Road,Yarraman,4614,Landfill,-26.856433,151.987614
        341,Torres SC -Thursday Island Transfer Station,(07) 4069 1336,http://www.torres.qld.gov.au,Torres Shire Council,Waiben Esplanade,Thursday Island,4875,Transfer station,-10.573977,142.220264
        342,TSIRC - Badu Island Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Badu Island,4875,Landfill,-10.168626,142.159407
        343,TSIRC - Boigu Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Boigu Island,4875,Landfill,-9.232261,142.21378
        344,TSIRC - Dauan Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Dauan Island,4875,Landfill,-9.411426,142.530245
        345,TSIRC - Erub Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Erub Island,4875,Landfill,-9.589451,143.763119
        346,TSIRC - Hammond Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Hammond Island,4875,Landfill,-10.535329,142.226349
        347,TSIRC - Kubin Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Kubin Community, Moa Island,4875,Landfill,-10.228777,142.21963
        348,TSIRC - Mabuiag Island Community Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Mabuiag Island,4875,Landfill,-9.949584,142.195912
        349,TSIRC - Masig Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Lawrence's Road,Yorke Island,4875,Landfill,-9.752189,143.401107
        350,TSIRC - Mer Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Mer Island,4875,Landfill,-9.917569,144.050574
        351,TSIRC - Poruma Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Poroma Island,4085,Landfill,-10.050096,143.076111
        352,TSIRC - Saibai Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Saibai Island,4875,Landfill,-9.382774,142.614743
        353,TSIRC - St Pauls Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,St Pauls Community, Mou Island,4875,Landfill,-10.18485,142.318067
        354,TSIRC - Ugar Community Council Tig,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Stephen Island,4875,Landfill,-9.508397,143.542665
        355,TSIRC - Warraber Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Warraber,4875,Landfill,-10.209728,142.819794
        356,TSIRC - Yam Community Council Tip,(07) 4048 6009,http://www.tsirc.qld.gov.au,Torres Strait Island Regional Council,Tip Road,Yam Island,4875,Landfill,-9.898966,142.77564
        357,Hervey Range Landfill and Transfer Station,(07) 4773 8335,http://www.townsville.qld.gov.au,Townsville City Council,Hervey Range Road,Bohle Plains,4817,Landfill,-19.316942,146.653265
        358,J.J.Richards - Townsville,(07) 3488 9600,http://www.jjrichards.com.au,Townsville City Council,638 Ingham Road,Bohle,4818,Transfer station,-19.262706,146.740124
        359,Jensen Landfill and Transfer Station,(07) 4773 8335,http://www.townsville.qld.gov.au,Townsville City Council,214 Geaney Lane,Deeragun,4818,Landfill,-19.254213,146.657983
        360,Magnetic Island Landfill,(07) 4773 8335,http://www.townsville.qld.gov.au,Townsville City Council,33 Magnetic Street,Picnic Bay,4816,Landfill,-19.172985,146.840152
        361,McCahills Earthmoving and Supplies Pty Ltd -Stuart,(07) 4778 1275,http://www.mccahillslandscapingsupplies.com.au,Townsville City Council,35 Vantassel Street,Stuart,4811,Organic processing,-19.338606,146.866305
        362,Stuart Waste Disposal,(07) 4773 8335,http://www.townsville.qld.gov.au,Townsville City Council,24 Vantassel Street,Stuart,4811,Landfill,-19.339308,146.866201
        363,The New Magnetic Island Waste Facility,(07) 4773 8645,http://www.townsville.qld.gov.au,Townsville City Council,11-63 West Point Road,Magnetic Island,4818,Landfill,-19.170676,146.82844
        364,Evan's Landing Landfill,(07) 4069 9991,http://www.riotintoalcan.com,Weipa Town Authority,Lot 186 Kerr Point Road,Weipa (Evan's Landing),4874,Landfill,-12.658967,141.837609
365,Bell Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Tip Road,Bell,4408,Landfill,-26.939766,151.431359
        366,Burra Burri Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Burra Burri Creek Road,Chinchilla,4413,Landfill,-26.508935,150.968707
        367,Chinchilla Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Slessar Street,Chinchilla,4413,Landfill,-26.741806,150.598833
        368,Condamine Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Condamine/Roma Street,Condamine,4416,Landfill,-26.927378,150.127095
        369,Dalby Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,18765 Warrego Highway,Dalby,4405,Landfill,-27.147752,151.208693
        370,Drillham Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Warrego Highway,Drillham,4424,Landfill,-26.639954,149.973236
        371,Dulacca Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,10 South Dulacca Road,Dulacca,4425,Landfill,-26.644396,149.746645
        372,Glenmorgan Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Surat Development Road,Glenmorgan,4423,Landfill,-27.262487,149.658692
        373,Jandowae Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Jandowae-Kingaroy Road,Jandowae,4410,Landfill,-26.751198,151.095315
        374,Kaimkillenbun Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Towns Road,Kaimkillenbun,4406,Landfill,-27.056143,151.439796
        375,Kogan Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Condamine Highway,Kogan,4406,Landfill,-27.029479,150.748859
        376,Macalister Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Daandine - Macalister Road,Macalister,4406,Landfill,-27.046492,151.072881
        377,Meandarra Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Meacle Street,Meandarra,4422,Landfill,-27.330177,149.882386
        378,Miles Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Pound Road off Leichhardt Highway,Miles,4415,Landfill,-26.668966,150.182153
        379,Tara Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Fry Street,Tara,4421,Landfill,-27.289242,150.460352
        380,Wandoan Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Tip Road,Wandoan,4419,Landfill,-26.11019,149.971622
        381,Warra Waste Disposal Facility,1300 268 624,http://www.wdrc.qld.gov.au,Western Downs Regional Council,Warrego Highway,Warra,4411,Landfill,-26.939414,150.921093
        382,Bowen Landfill,(07) 4945 0266,http://www.whitsunday.qld.gov.au,Whitsunday Regional Council,908 Collinsville Road,Bowen,4805,Landfill,-20.073725,148.150838
        383,Cannonvale Transfer Station,(07) 4945 0266,http://www.whitsunday.qld.gov.au,Whitsunday Regional Council,Carlo Drive,Cannonvale,4802,Transfer station,-20.283701,148.674623
        384,Collinsville Transfer Station,(07) 4945 0266,http://www.whitsunday.qld.gov.au,Whitsunday Regional Council,155 Scottville Road (Cnr Cemetery Road),Collinsville,4804,Transfer station,-20.567329,147.836506
        385,Kelsey Creek Landfill,(07) 4945 0266,http://www.whitsunday.qld.gov.au,Whitsunday Regional Council,79 Kelsey Creek Road,Proserpine,4800,Landfill,-20.3882,148.551396
        386,Winton Landfill,(07) 4657 2666,http://www.winton.qld.gov.au,Winton Shire Council,Dump Road,Winton,4735,Landfill,-22.376141,143.0322
        387,Woorabinda Aboriginal Shire Council,(07) 4925 9800,http://www.woorabinda.qld.gov.au,Woorabinda Aboriginal Shire Council,Baralaba Woorabinda  Road,Woorabinda,4713,Landfill,-24.15185,149.47907
        388,Yarrabah Transfer Station,(07) 4056 9120,http://www.yarrabah.qld.gov.au,Yarrabah Aboriginal Shire Council,Workshop Road,Yarrabah,4871,Landfill,-16.910106,145.878565";

        string[] lines = csvDataText.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header
        {
            string[] values = lines[i].Split(',');

            if (values.Length >= 11)
            {
                int postCode;
                if (int.TryParse(values[7], out postCode)) // Convert to int if possible
                {
                    CSVRow row = new CSVRow
                    {
                        _id = values[0],
                        Name = values[1],
                        PostCode = postCode
                    };

                    csvData.Add(row);
                }
            }
        }
    }

    public void LookupPostCode()
    {
        int inputPostCode;
        if (int.TryParse(postCodeInput.text, out inputPostCode))
        {
            CSVRow matchingRow = null;
            int smallestDifference = int.MaxValue;

            foreach (CSVRow row in csvData)
            {
                int difference = Mathf.Abs(row.PostCode - inputPostCode);
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    matchingRow = row;
                }
            }

            if (matchingRow != null)
            {
                resultText.text = "Nearest Waste Facility : " + matchingRow.Name;
                resultCanvas.SetActive(true); // Show the result canvas
            }
            else
            {
                resultText.text = "No matching or nearest name found.";
            }
        }
        else
        {
            resultText.text = "Invalid postal code format.";
        }
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene("Scene3"); // Replace "SceneNameHere" with the actual name of your third scene
    }
}
