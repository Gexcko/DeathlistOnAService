using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushoverClient;

namespace DLoaS {
    class Program {
        static Program(){
            CosturaUtility.Initialize();
        }
        

        static string[] d = new string[177];        //Anlegen der Link-Liste mit URLS
        static ArrayList dl = new ArrayList();      //Anlegen der Liste der bereits verstorbenen


        //Arrays beinhalten Index Nummern der "Opfer" in den Deathlists der Teilnehmer
        static int[] emil;
        static int[] fabian;
        static int[] jakob;
        static int[] jasi;
        static int[] flo;
        static int[] manu;
        static int[] michi;
        static int[] miri;
        static int[] phoenix;
        static int[] valentin;
        static int[] volker;

        //Countervariablen der Teilnehmer
        static int emilScore = 0;
        static int fabianScore = 0;
        static int floScore = 0;
        static int jasiScore = 0;
        static int jakobScore = 0;
        static int manuScore = 0;
        static int michiScore = 0;
        static int miriScore = 0;
        static int phoenixScore = 0;
        static int valentinScore = 0;
        static int volkerScore = 0;

        static void Main(string[] args) {
            
            dlInit();                               //Befüllen der Link-Liste

            //Befüllen der Deathlists
            emil = new int[] { 0, 3, 5, 12, 18, 20, 24, 50, 66, 80, 83, 94, 105, 118, 120, 121, 125, 131, 144, 145, 147, 158, 162, 166, 172 };
            fabian = new int[] { 5, 12, 15, 18, 20, 21, 30, 33, 55, 78, 81, 89, 94, 97, 99, 113, 118, 122, 131, 133, 146, 156, 163, 169, 172 };
            flo = new int[] { 0, 1, 5, 9, 12, 17, 19, 42, 56, 63, 76, 77, 90, 94, 98, 99, 107, 116, 118, 119, 124, 128, 130, 131, 144 };
            jakob = new int[] { 11, 25, 29, 44, 53, 60, 61, 64, 70, 74, 82, 91, 92, 95, 103, 104, 106, 115, 125, 126, 131, 153, 154, 155, 173 };
            jasi = new int[] { 6, 9, 19, 25, 28, 32, 34, 39, 41, 44, 54, 59, 61, 94, 96, 109, 110, 112, 117, 123, 131, 139, 147, 152, 175 };
            manu = new int[] { 8, 18, 25, 31, 39, 45, 54, 67, 72, 78, 80, 81, 84, 85, 94, 99, 109, 121, 122, 128, 131, 141, 147, 148, 172 };
            michi = new int[] { 18, 20, 27, 31, 39, 44, 54, 77, 87, 94, 112, 114, 121, 131, 132, 143, 145, 147, 149, 151, 160, 164, 166, 167, 168 };
            miri = new int[] { 18, 19, 22, 25, 41, 54, 57, 59, 62, 68, 75, 80, 86, 94, 100, 109, 131, 135, 138, 139, 140, 152, 174, 175, 176 };
            phoenix = new int[] { 7, 10, 13, 14, 38, 43, 46, 47, 49, 51, 65, 71, 88, 92, 93, 94, 102, 111, 126, 127, 129, 152, 161, 165, 175 };
            valentin = new int[] { 2, 6, 20, 21, 23, 26, 28, 49, 69, 79, 80, 87, 94, 102, 108, 109, 112, 123, 125, 131, 139, 145, 171, 174, 176 };
            volker = new int[] { 5, 15, 18, 35, 36, 37, 40, 48, 52, 58, 73, 94, 99, 101, 128, 136, 137, 141, 142, 150, 156, 157, 159, 166, 170 };


            for (int i = 0; i<d.Length;i++) {                                                                           //Jeden Link im Array durchgehen
                string urlAddress = d[i];                                                                               //und die jeweilige URL verwenden
                string data = "-";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);                                 //HTTP Anfrage
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();                                      //HTTP Antwort

                if (response.StatusCode == HttpStatusCode.OK) {                                                         //Wenn die Antwort ohne Fehler ankommt (Code: 200) dann weitermachen
                    Stream receiveStream = response.GetResponseStream();                                                //Stream um den Text der Webseite verwenden zu können
                    StreamReader readStream = null;

                    if (response.CharacterSet == null) {
                        readStream = new StreamReader(receiveStream);
                        Console.WriteLine("");
                    } else {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        //Console.WriteLine("");
                    }

                    data = readStream.ReadToEnd();                                                                      //String data beinhaltet den Quellcode

                    if (data.Contains("2020 deaths")) {                                                                 //Abfrage ob "2020 deaths" in den Categories vorkommt (engl. Wiki)
                        dl.Add(i);                                                                                          //In die Todes-Liste hinzufügen
                        string s = d[i].Substring(30).Replace("_", " ");                                                    //Vorbereitung für Console-Output
                        Console.WriteLine(i+" - "+s);                                                                       //Console-Output
                    }else if(data.Contains("Gestorben 2020")) {                                                         //Abfrage ob "Gestorben 2020" in den Categories vorkommt (deu. Wiki)
                        dl.Add(i);                                                                                          //Das gleiche in Deutsch
                        string s = d[i].Substring(30).Replace("_", " ");                                                    //
                        Console.WriteLine(i + " - " + s);                                                                   //
                    }                                                                                                   //

                    

                    response.Close();                                                                                   //Die Streams müssen geschlossen werden.
                    readStream.Close();
                }
            }

            //Console.WriteLine(data);
            //Console.ReadKey();
            //System.Environment.Exit(0);
            Console.WriteLine();


            //Nachdem alle Einträge der Link-Liste durch sind wird die Liste der Verstorbenen mit den Deathlists der Teilnehmer verglichen.
            //Dazu dient ein Unterprogramm "fastSearch" selbstgeschrieben als Platzhalter. (unten weiter zu finden)
            emilScore = fastSearch(dl, emil);
            fabianScore = fastSearch(dl, fabian);
            floScore = fastSearch(dl, flo);
            jasiScore = fastSearch(dl, jasi);
            jakobScore = fastSearch(dl, jakob);
            manuScore = fastSearch(dl, manu);
            michiScore = fastSearch(dl, michi);
            miriScore = fastSearch(dl, miri);
            phoenixScore = fastSearch(dl, phoenix);
            valentinScore = fastSearch(dl, valentin);
            volkerScore = fastSearch(dl, volker);

            //Console-Output mit allen Teilnehmern und deren Punkten
            Console.WriteLine("Emil: " + emilScore);
            Console.WriteLine("Fabian: " + fabianScore);
            Console.WriteLine("Flo: " + floScore);
            Console.WriteLine("Jakob: " + jakobScore);
            Console.WriteLine("Jasi: " + jasiScore);
            Console.WriteLine("Manu: " + manuScore);
            Console.WriteLine("Michi: " + michiScore);
            Console.WriteLine("Miri: " + miriScore);
            Console.WriteLine("Phoenix: " + phoenixScore);
            Console.WriteLine("Valentin: " + valentinScore);
            Console.WriteLine("Volker: " + volkerScore);

            string[] pD = { };
            try {
                pD = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "pD.txt");                          //Einlesen des Text-Files mit den Verstorbenen, die bereits vor der Abfrage abgedankt haben
            }catch(FileNotFoundException e) {
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "pD.txt", pD);

            }
            string[] caster = new string[dl.Count];                                                                     //Umwandeln der ArrayList zu String Array
            for (int i = 0;i<dl.Count;i++) {                                                                            //
                caster[i] = (dl[i]+"");                                                                                 //
                //Console.WriteLine(i+" "+dl[i]);                                                                       //
            }                                                                                                           //

            
            if (pD.SequenceEqual(caster)) {                                                                             //Besteht ein Unterschied zwischen der letzten Abfrage und der gerade abgefragten Liste?
                Console.WriteLine("Meh");                                                                               //Nein: Kein weiterer ist verstorben
            } else {
                string oDiff = "";                                                                                      //Ja:
                foreach (string diff in caster) {                                                                       //Wer mit welchem Index
                    if (!pD.Contains(diff)) {                                                                           //
                        oDiff = diff;                                                                                   //
                    }                                                                                                   //
                }                                                                                                       //

                Console.WriteLine("Heureka");

                String whoPointed = "";                                                                                 //String wird der Nachricht für Pushbullet angefügt und zeigt, wer gepunktet hat

                whoPointed += pointSearch(Int32.Parse(oDiff), emil, "Emil ");                                           //Ist der Index des neuverstorbenen in der jeweiligen Liste?
                whoPointed += pointSearch(Int32.Parse(oDiff), fabian, "Fabian ");
                whoPointed += pointSearch(Int32.Parse(oDiff), flo, "Flo ");
                whoPointed += pointSearch(Int32.Parse(oDiff), jakob, "Jakob ");
                whoPointed += pointSearch(Int32.Parse(oDiff), jasi, "Jasi ");
                whoPointed += pointSearch(Int32.Parse(oDiff), manu, "Manu ");
                whoPointed += pointSearch(Int32.Parse(oDiff), michi, "Michi ");
                whoPointed += pointSearch(Int32.Parse(oDiff), miri, "Miri ");
                whoPointed += pointSearch(Int32.Parse(oDiff), phoenix, "Phoenix ");
                whoPointed += pointSearch(Int32.Parse(oDiff), valentin, "Valentin ");
                whoPointed += pointSearch(Int32.Parse(oDiff), volker, "Volker ");

                //Client erstellen
                PushbulletClient client = new PushbulletClient("o.xbEidwElVYGVSDK5F1uX0r1ZyVe5ovSt");

                //Informationen über unseren Account abholen
                var currentUserInformation = client.CurrentUsersInformation();

                //Prüfen, ob Accountinfos geladen wurden
                if (currentUserInformation != null) {
                    //Anfrage erzeugen
                    PushNoteRequest request = new PushNoteRequest {
                        Email = currentUserInformation.Email,
                        Title = "Deathlist 2020",
                        Body = "RIP "+ d[Int32.Parse(oDiff)].Substring(30).Replace("_", " ") + "\n" + whoPointed
                    };

                    PushbulletSharp.Models.Responses.PushResponse response = client.PushNote(request);
                }

                Pushover pclient = new Pushover("asf1vbpvec3x6h288p46nu8xj2i6xb");
                PushoverClient.PushResponse responsePO = pclient.Push(
                              "Deathlist 2020",
                              "RIP " + d[Int32.Parse(oDiff)].Substring(30).Replace("_", " ") + "\n" + whoPointed,
                              "g3kf5dfnarwrkemmf35vimf2hxcyyz"
                          );

            }

            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "pD.txt", caster);                               //Neue Liste in die Datei zurückschreiben
        }

        static int fastSearch(ArrayList list, int[] m) {                                                                //ArrayList und Integer Array wird vergleichen, ob ein Eintrag jeweils in beiden Listen vorkommt
            int score = 0;
            for (int i = 0; i < list.Count; i++) {
                for (int y = 0; y < m.Length; y++) {
                    if (list[i].Equals(m[y])) {
                        score++;
                    }
                }
            }
            return score;
        }

        static string pointSearch(int k, int[] m, String name){                                                         //Gleiche Abfrage nur, ob ein Wert in der Liste vorkommt.
            string score = "";
            for (int y = 0; y < m.Length; y++){
                if (k == m[y]){
                    score = name;
                }
            }
            return score;
        }



        static void dlInit() {
            d[0] = ("https://en.wikipedia.org/wiki/6ix9ine");
            d[1] = ("https://en.wikipedia.org/wiki/Abby_Lee_Miller");
            d[2] = ("https://en.wikipedia.org/wiki/Adele");
            d[3] = ("https://en.wikipedia.org/wiki/Ady_Barkan");
            d[4] = ("https://en.wikipedia.org/wiki/Klagenfurt"); //ehem. Aiman az-Zwahiri (duplikat)
            d[5] = ("https://en.wikipedia.org/wiki/Alex_Trebek");
            d[6] = ("https://en.wikipedia.org/wiki/Alice_Cooper");
            d[7] = ("https://en.wikipedia.org/wiki/Alice_Schwarzer");
            d[8] = ("https://en.wikipedia.org/wiki/Amanda_Bynes");
            d[9] = ("https://en.wikipedia.org/wiki/Angela_Lansbury");
            d[10] = ("https://en.wikipedia.org/wiki/Arnold_Schwarzenegger");
            d[11] = ("https://en.wikipedia.org/wiki/Ashley_Olsen");
            d[12] = ("https://en.wikipedia.org/wiki/Ayman_al-Zawahiri");
            d[13] = ("https://en.wikipedia.org/wiki/Ban_Ki_Moon");
            d[14] = ("https://en.wikipedia.org/wiki/Barack_Obama");
            d[15] = ("https://en.wikipedia.org/wiki/Barry_Du_Bois");
            d[16] = ("https://en.wikipedia.org/wiki/Klagenfurt"); //ehem. Benedikt_XVI (duplikat)
            d[17] = ("https://en.wikipedia.org/wiki/Bernard_L._Madoff");
            d[18] = ("https://en.wikipedia.org/wiki/Betty_White");
            d[19] = ("https://en.wikipedia.org/wiki/Bill_Cosby");
            d[20] = ("https://en.wikipedia.org/wiki/Bob_Barker");
            d[21] = ("https://en.wikipedia.org/wiki/Bob_Dole");
            d[22] = ("https://en.wikipedia.org/wiki/Brigitte_Bardot");
            d[23] = ("https://en.wikipedia.org/wiki/Buzz_Aldrin");
            d[24] = ("https://en.wikipedia.org/wiki/Cecil_Souders");
            d[25] = ("https://en.wikipedia.org/wiki/Charlie_Sheen");
            d[26] = ("https://en.wikipedia.org/wiki/Cher");
            d[27] = ("https://en.wikipedia.org/wiki/Christine_Neubauer");
            d[28] = ("https://en.wikipedia.org/wiki/Clint_Eastwood");
            d[29] = ("https://en.wikipedia.org/wiki/Constantine_II_of_Greece");
            d[30] = ("https://en.wikipedia.org/wiki/Daniel_arap_Moi");
            d[31] = ("https://en.wikipedia.org/wiki/Daniel_Küblböck");
            d[32] = ("https://en.wikipedia.org/wiki/Danny_DeVito");
            d[33] = ("https://en.wikipedia.org/wiki/Dave_Mustaine");
            d[34] = ("https://en.wikipedia.org/wiki/David_Hasselhof");
            d[35] = ("https://en.wikipedia.org/wiki/David_Prowse");
            d[36] = ("https://en.wikipedia.org/wiki/Deborah_James");
            d[37] = ("https://en.wikipedia.org/wiki/Dennis_Skinner");
            d[38] = ("https://en.wikipedia.org/wiki/Dick_Cheney");
            d[39] = ("https://en.wikipedia.org/wiki/Dick_Van_Dyke");
            d[40] = ("https://en.wikipedia.org/wiki/Dilip_Kumar");
            d[41] = ("https://en.wikipedia.org/wiki/Donald_Trump");
            d[42] = ("https://en.wikipedia.org/wiki/Earl_Cameron_(actor)");
            d[43] = ("https://en.wikipedia.org/wiki/Elfriede_Jelinek");
            d[44] = ("https://en.wikipedia.org/wiki/Elizabeth_II");
            d[45] = ("https://en.wikipedia.org/wiki/Elton_John");
            d[46] = ("https://en.wikipedia.org/wiki/Erwin_Pröll");
            d[47] = ("https://en.wikipedia.org/wiki/Felix_Baumgartner");
            d[48] = ("https://en.wikipedia.org/wiki/Françoise_Hardy");
            d[49] = ("https://en.wikipedia.org/wiki/Frank_Stronach");
            d[50] = ("https://en.wikipedia.org/wiki/Frankie_Banali");
            d[51] = ("https://en.wikipedia.org/wiki/Franz_Beckenbauer");
            d[52] = ("https://en.wikipedia.org/wiki/Genesis_Breyer_P-Orridge");
            d[53] = ("https://en.wikipedia.org/wiki/Geoff_Ramsey");
            d[54] = ("https://en.wikipedia.org/wiki/George_R._R._Martin");
            d[55] = ("https://en.wikipedia.org/wiki/George_Alagiah");
            d[56] = ("https://en.wikipedia.org/wiki/George_Soros");
            d[57] = ("https://en.wikipedia.org/wiki/George_W._Bush");
            d[58] = ("https://en.wikipedia.org/wiki/Gudrun_Ure");
            d[59] = ("https://en.wikipedia.org/wiki/Hansi_Hinterseer");
            d[60] = ("https://en.wikipedia.org/wiki/Harald_V_of_Norway");
            d[61] = ("https://en.wikipedia.org/wiki/Harrison_Ford");
            d[62] = ("https://en.wikipedia.org/wiki/Harry_Belafonte");
            d[63] = ("https://en.wikipedia.org/wiki/Harvey_Weinstein");
            d[64] = ("https://de.wikipedia.org/wiki/Heinrich_Reiter");// Heinrich_Reiter
            d[65] = ("https://en.wikipedia.org/wiki/Heinz-Christian_Strache");
            d[66] = ("https://en.wikipedia.org/wiki/Henry_Kissinger");
            d[67] = ("https://en.wikipedia.org/wiki/Honor_Blackman");
            d[68] = ("https://en.wikipedia.org/wiki/Hosni_Mubarak");
            d[69] = ("https://de.wikipedia.org/wiki/Hugo_Portisch"); //Hugo_Portisch
            d[70] = ("https://en.wikipedia.org/wiki/Ian_Holm");
            d[71] = ("https://en.wikipedia.org/wiki/Ian_McKellen");
            d[72] = ("https://en.wikipedia.org/wiki/Ian_St_John");
            d[73] = ("https://en.wikipedia.org/wiki/Imelda_Marcos");
            d[74] = ("https://en.wikipedia.org/wiki/J.D._Crowe");
            d[75] = ("https://en.wikipedia.org/wiki/Jean-Marie_le_Pen");
            d[76] = ("https://en.wikipedia.org/wiki/Jerry_Lee_Lewis");
            d[77] = ("https://en.wikipedia.org/wiki/Jerry_Stiller");
            d[78] = ("https://en.wikipedia.org/wiki/Jill_Gascoine");
            d[79] = ("https://en.wikipedia.org/wiki/Jim_Carrey");
            d[80] = ("https://en.wikipedia.org/wiki/Jimmy_Carter");
            d[81] = ("https://en.wikipedia.org/wiki/Jimmy_Greaves");
            d[82] = ("https://en.wikipedia.org/wiki/John_Goodman");
            d[83] = ("https://en.wikipedia.org/wiki/John_Robert_Lewis");
            d[84] = ("https://en.wikipedia.org/wiki/Johnny_Depp");
            d[85] = ("https://en.wikipedia.org/wiki/Joni_Mitchell");
            d[86] = ("https://en.wikipedia.org/wiki/Josef_Pröll");
            d[87] = ("https://de.wikipedia.org/wiki/Joseph_Hannesschläger"); //Joseph_Hannesschläger
            d[88] = ("https://en.wikipedia.org/wiki/Judy_Dench");
            d[89] = ("https://en.wikipedia.org/wiki/Julie_Strain");
            d[90] = ("https://en.wikipedia.org/wiki/June_Lockhart");
            d[91] = ("https://en.wikipedia.org/wiki/Karl-Heinz_Heddergott");
            d[92] = ("https://en.wikipedia.org/wiki/Keith_Richards");
            d[93] = ("https://en.wikipedia.org/wiki/Kim_Jong-Un");
            d[94] = ("https://en.wikipedia.org/wiki/Kirk_Douglas");
            d[95] = ("https://en.wikipedia.org/wiki/Koji_Igarashi");
            d[96] = ("https://en.wikipedia.org/wiki/Larry_King");
            d[97] = ("https://en.wikipedia.org/wiki/Lee_Kerslake");
            d[98] = ("https://en.wikipedia.org/wiki/Lil_Xan");
            d[99] = ("https://en.wikipedia.org/wiki/Linda_Nolan");
            d[100] = ("https://en.wikipedia.org/wiki/Lindsay_Lohan");
            d[101] = ("https://en.wikipedia.org/wiki/Madonna_(entertainer)");
            d[102] = ("https://en.wikipedia.org/wiki/Maggie_Smith");
            d[103] = ("https://en.wikipedia.org/wiki/Mangosuthu_Buthelezi");
            d[104] = ("https://en.wikipedia.org/wiki/Margrethe_II_of_Denmark");
            d[105] = ("https://en.wikipedia.org/wiki/Marsha_Hunt");
            d[106] = ("https://en.wikipedia.org/wiki/Mary-Kate_Olsen");
            d[107] = ("https://en.wikipedia.org/wiki/Max_von_Sydow");
            d[108] = ("https://en.wikipedia.org/wiki/Megan_Fox");
            d[109] = ("https://en.wikipedia.org/wiki/Mel_Brooks");
            d[110] = ("https://en.wikipedia.org/wiki/Michael_Douglas");
            d[111] = ("https://en.wikipedia.org/wiki/Michael_Häupl");
            d[112] = ("https://en.wikipedia.org/wiki/Michael_J._Fox");
            d[113] = ("https://en.wikipedia.org/wiki/Michael_Robinson_(footballer)");
            d[114] = ("https://en.wikipedia.org/wiki/Michael_Schumacher");
            d[115] = ("https://en.wikipedia.org/wiki/Mikhail_Gorbachev");
            d[116] = ("https://en.wikipedia.org/wiki/Nick_Nolte");
            d[117] = ("https://en.wikipedia.org/wiki/Nina_Hagen");
            d[118] = ("https://en.wikipedia.org/wiki/Nobby_Stiles");
            d[119] = ("https://en.wikipedia.org/wiki/Norman_Lloyd");
            d[120] = ("https://en.wikipedia.org/wiki/O._J._Brigance");
            d[121] = ("https://en.wikipedia.org/wiki/Olivia_De_Havilland");
            d[122] = ("https://en.wikipedia.org/wiki/Olivia_Newton-John");
            d[123] = ("https://en.wikipedia.org/wiki/Ottfried_Fischer");
            d[124] = ("https://en.wikipedia.org/wiki/Ozzy_Osbourne");
            d[125] = ("https://en.wikipedia.org/wiki/Pope_Benedict_XVI");
            d[126] = ("https://en.wikipedia.org/wiki/Pope_Francis");
            d[127] = ("https://en.wikipedia.org/wiki/Patrick_Steward");
            d[128] = ("https://en.wikipedia.org/wiki/Paul_Gascoigne");
            d[129] = ("https://en.wikipedia.org/wiki/Peter_Handke");
            d[130] = ("https://en.wikipedia.org/wiki/Phil_Spector");
            d[131] = ("https://en.wikipedia.org/wiki/Prince_Philip,_Duke_of_Edinburgh"); //Prince_Philip,_Duke_of_Edinburgh
            d[132] = ("https://en.wikipedia.org/wiki/Mette-Marit,_Crown_Princess_of_Norway");
            d[133] = ("https://en.wikipedia.org/wiki/Prunella_Scales");
            d[134] = ("https://en.wikipedia.org/wiki/Klagenfurt"); //ehem. Queen Elizabeth II (duplikat)
            d[135] = ("https://en.wikipedia.org/wiki/Rainhard_Fendrich");
            d[136] = ("https://en.wikipedia.org/wiki/Ralph_Lauren");
            d[137] = ("https://en.wikipedia.org/wiki/Ray_Reardon");
            d[138] = ("https://en.wikipedia.org/wiki/Recep_Tayyip_Erdogan");
            d[139] = ("https://en.wikipedia.org/wiki/Richard_Lugner");
            d[140] = ("https://en.wikipedia.org/wiki/Riek_Machar");
            d[141] = ("https://en.wikipedia.org/wiki/Ronnie_Wood");
            d[142] = ("https://en.wikipedia.org/wiki/Rosalynn_Carter");
            d[143] = ("https://en.wikipedia.org/wiki/Klagenfurt"); //Roy_Uwe_Ludwig_Horn
            d[144] = ("https://en.wikipedia.org/wiki/Ruth_Bader_Ginsburg");
            d[145] = ("https://en.wikipedia.org/wiki/Sam_Lloyd");
            d[146] = ("https://en.wikipedia.org/wiki/Samula_Anoa'i");
            d[147] = ("https://en.wikipedia.org/wiki/Sean_Connery");
            d[148] = ("https://en.wikipedia.org/wiki/Selena_Gomez");
            d[149] = ("https://en.wikipedia.org/wiki/Selma_Blair");
            d[150] = ("https://en.wikipedia.org/wiki/Shane_MacGowan");
            d[151] = ("https://en.wikipedia.org/wiki/Sharon_Osbourne");
            d[152] = ("https://en.wikipedia.org/wiki/Silvio_Berlusconi");
            d[153] = ("https://en.wikipedia.org/wiki/Simeon_Saxe-Coburg-Gotha");
            d[154] = ("https://en.wikipedia.org/wiki/Sir_Ian_McKellen");
            d[155] = ("https://en.wikipedia.org/wiki/Sir_Patrick_Stewart");
            d[156] = ("https://en.wikipedia.org/wiki/Slick_Woods");
            d[157] = ("https://en.wikipedia.org/wiki/Spencer_Compton,_7th_Marquess_of_Northampton");
            d[158] = ("https://en.wikipedia.org/wiki/Stephen_Darby");
            d[159] = ("https://en.wikipedia.org/wiki/Sumner_Redstone");
            d[160] = ("https://en.wikipedia.org/wiki/Sylvie_Meis");
            d[161] = ("https://en.wikipedia.org/wiki/Taylor_Lautner");
            d[162] = ("https://en.wikipedia.org/wiki/Teri_Garr");
            d[163] = ("https://en.wikipedia.org/wiki/Terry_Jones");
            d[164] = ("https://en.wikipedia.org/wiki/Thomas_Danneberg");
            d[165] = ("https://en.wikipedia.org/wiki/Thomas_Gottschalk");
            d[166] = ("https://en.wikipedia.org/wiki/Tina_Turner");
            d[167] = ("https://en.wikipedia.org/wiki/Tom_Hanks");
            d[168] = ("https://en.wikipedia.org/wiki/Tom_Long_(actor)");
            d[169] = ("https://en.wikipedia.org/wiki/Trevor_Peacock");
            d[170] = ("https://en.wikipedia.org/wiki/Akihito");
            d[171] = ("https://en.wikipedia.org/wiki/Val_Kilmer");
            d[172] = ("https://en.wikipedia.org/wiki/Vera_Lynn");
            d[173] = ("https://de.wikipedia.org/wiki/Werner_Ludwig"); //Werner_Ludwig
            d[174] = ("https://en.wikipedia.org/wiki/William_Shatner");
            d[175] = ("https://en.wikipedia.org/wiki/Vladimir_Putin");
            d[176] = ("https://en.wikipedia.org/wiki/Yoko_Ono");
        }
    }
}
