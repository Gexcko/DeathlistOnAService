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
        

        static string[] d = new string[153];        //Anlegen der Link-Liste mit URLS
        static ArrayList dl = new ArrayList();      //Anlegen der Liste der bereits verstorbenen


        //Arrays beinhalten Index Nummern der "Opfer" in den Deathlists der Teilnehmer
        static int[] emil;
        static int[] fabian;
        static int[] jasi;
        static int[] manu;
        static int[] michi;
        static int[] valentin;
        static int[] rene;

        //Countervariablen der Teilnehmer
        static int emilScore = 0;
        static int fabianScore = 0;
        static int jasiScore = 0;
        static int manuScore = 0;
        static int michiScore = 0;
        static int valentinScore = 0;
        static int reneScore = 0;

        static void Main(string[] args) {
            
            dlInit();                               //Befüllen der Link-Liste

            //Befüllen der Deathlists
            emil        = new int[] { 0, 1, 2, 3, 11, 12, 14, 17, 18, 19, 38, 60, 66, 80, 82, 83, 84, 91, 100, 101, 115, 116, 123, 126, 130, 134, 135, 141, 143, 144 };
            fabian      = new int[] { 0, 3, 4, 14, 20, 25, 26, 27, 37, 40, 47, 53, 66, 70, 73, 82, 97, 101, 107, 114, 123, 124, 125, 126, 129, 130, 133, 136, 143, 144 };
            jasi        = new int[] { 5, 7, 9, 12, 15, 16, 29, 33, 35, 39, 42, 43, 45, 54, 55, 56, 81, 85, 88, 92, 93, 94, 95, 98, 102, 113, 117, 132, 141, 150 };
            manu        = new int[] { 4, 6, 10, 15, 29, 34, 42, 43, 46, 54, 64, 69, 70, 74, 75, 84, 88, 95, 101, 102, 104, 106, 112, 113, 122, 127, 133, 137, 142, 147 };
            michi       = new int[] { 15, 19, 22, 29, 31, 34, 41, 42, 50, 54, 61, 67, 76, 77, 90, 94, 95, 99, 101, 108, 110, 127, 128, 131, 140, 141, 142, 143, 144, 149 };
            rene        = new int[] { 4, 13, 21, 23, 28, 36, 44, 49, 52, 55, 57, 58, 59, 63, 65, 72, 78, 87, 87, 96, 103, 105, 109, 118, 119, 120, 121, 133, 139, 146 };
            valentin    = new int[] { 5, 8, 12, 19, 20, 24, 30, 32, 33, 48, 51, 62, 68, 69, 71, 79, 86, 88, 93, 94, 102, 111, 113, 116, 117, 138, 145, 148, 151, 152 };


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

                    if (data.Contains("2021 deaths")) {                                                                 //Abfrage ob "2021 deaths" in den Categories vorkommt (engl. Wiki)
                        dl.Add(i);                                                                                          //In die Todes-Liste hinzufügen
                        string s = d[i].Substring(30).Replace("_", " ");                                                    //Vorbereitung für Console-Output
                        Console.WriteLine(i+" - "+s);                                                                       //Console-Output
                    }else if(data.Contains("Gestorben 2021")) {                                                         //Abfrage ob "Gestorben 2021" in den Categories vorkommt (deu. Wiki)
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
            jasiScore = fastSearch(dl, jasi);
            manuScore = fastSearch(dl, manu);
            michiScore = fastSearch(dl, michi);
            reneScore = fastSearch(dl, rene);
            valentinScore = fastSearch(dl, valentin);
            

            //Console-Output mit allen Teilnehmern und deren Punkten
            Console.WriteLine("Emil: " + emilScore);
            Console.WriteLine("Fabian: " + fabianScore);
            Console.WriteLine("Jasi: " + jasiScore);
            Console.WriteLine("Manu: " + manuScore);
            Console.WriteLine("Michi: " + michiScore);
            Console.WriteLine("Rene: " + reneScore);
            Console.WriteLine("Valentin: " + valentinScore);
            

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
                whoPointed += pointSearch(Int32.Parse(oDiff), jasi, "Jasi ");
                whoPointed += pointSearch(Int32.Parse(oDiff), manu, "Manu ");
                whoPointed += pointSearch(Int32.Parse(oDiff), michi, "Michi ");
                whoPointed += pointSearch(Int32.Parse(oDiff), rene, "Rene ");
                whoPointed += pointSearch(Int32.Parse(oDiff), valentin, "Valentin ");
                

                //Client erstellen
                PushbulletClient client = new PushbulletClient("TOKEN");

                //Informationen über unseren Account abholen
                var currentUserInformation = client.CurrentUsersInformation();

                //Prüfen, ob Accountinfos geladen wurden
                if (currentUserInformation != null) {
                    //Anfrage erzeugen
                    PushNoteRequest request = new PushNoteRequest {
                        Email = currentUserInformation.Email,
                        Title = "Deathlist 2021",
                        Body = "RIP "+ d[Int32.Parse(oDiff)].Substring(30).Replace("_", " ") + "\n" + whoPointed
                    };

                    PushbulletSharp.Models.Responses.PushResponse response = client.PushNote(request);
                }

                Pushover pclient = new Pushover("TOKEN");
                PushoverClient.PushResponse responsePO = pclient.Push(
                              "Deathlist 2021",
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
            d[1] = ("https://en.wikipedia.org/wiki/Abdelaziz_Bouteflika");
            d[2] = ("https://en.wikipedia.org/wiki/Abdullah_the_Butcher");
            d[3] = ("https://en.wikipedia.org/wiki/Ady_Barkan");
            d[4] = ("https://en.wikipedia.org/wiki/Akihito");
            d[5] = ("https://en.wikipedia.org/wiki/Alice_Cooper");
            d[6] = ("https://en.wikipedia.org/wiki/Amanda_Bynes");
            d[7] = ("https://en.wikipedia.org/wiki/Angela_Lansbury");
            d[8] = ("https://en.wikipedia.org/wiki/Angus_Young");
            d[9] = ("https://en.wikipedia.org/wiki/Anthony_Fauci");
            d[10] = ("https://en.wikipedia.org/wiki/Anthony_Hopkins");
            d[11] = ("https://en.wikipedia.org/wiki/Ayman_al-Zawahiri");
            d[12] = ("https://en.wikipedia.org/wiki/Pope_Benedict_XVI");
            d[13] = ("https://en.wikipedia.org/wiki/Bernie_Ecclestone");
            d[14] = ("https://en.wikipedia.org/wiki/Bernie_Madoff");
            d[15] = ("https://en.wikipedia.org/wiki/Betty_White");
            d[16] = ("https://en.wikipedia.org/wiki/Bill_Cosby");
            d[17] = ("https://en.wikipedia.org/wiki/Bill_Turnbull");
            d[18] = ("https://en.wikipedia.org/wiki/Billy_Connolly");
            d[19] = ("https://en.wikipedia.org/wiki/Bob_Barker");
            d[20] = ("https://en.wikipedia.org/wiki/Bob_Dole");
            d[21] = ("https://en.wikipedia.org/wiki/Bob_Kahn");
            d[22] = ("https://en.wikipedia.org/wiki/Bobby_Charlton");
            d[23] = ("https://en.wikipedia.org/wiki/Bruce_Dickinson");
            d[24] = ("https://en.wikipedia.org/wiki/Buzz_Aldrin");
            d[25] = ("https://en.wikipedia.org/wiki/Carlo_Tognoli");
            d[26] = ("https://en.wikipedia.org/wiki/Carlos_Menem");
            d[27] = ("https://en.wikipedia.org/wiki/Carmen_Sevilla");
            d[28] = ("https://en.wikipedia.org/wiki/Charles,_Prince_of_Wales");
            d[29] = ("https://en.wikipedia.org/wiki/Charlie_Sheen");
            d[30] = ("https://en.wikipedia.org/wiki/Cher");
            d[31] = ("https://en.wikipedia.org/wiki/Christine_Neubauer");
            d[32] = ("https://en.wikipedia.org/wiki/Clarence_Thomas");
            d[33] = ("https://en.wikipedia.org/wiki/Clint_Eastwood");
            d[34] = ("https://en.wikipedia.org/wiki/Daniel_K%C3%BCblb%C3%B6ck");
            d[35] = ("https://en.wikipedia.org/wiki/Danny_DeVito");
            d[36] = ("https://en.wikipedia.org/wiki/David_Attenborough");
            d[37] = ("https://en.wikipedia.org/wiki/David_Crosby");
            d[38] = ("https://en.wikipedia.org/wiki/David_Gulpilil");
            d[39] = ("https://en.wikipedia.org/wiki/David_Hasselhoff");
            d[40] = ("https://en.wikipedia.org/wiki/Desmond_Tutu");
            d[41] = ("https://en.wikipedia.org/wiki/Dick_Cheney");
            d[42] = ("https://en.wikipedia.org/wiki/Dick_Van_Dyke");
            d[43] = ("https://en.wikipedia.org/wiki/Donald_Trump");
            d[44] = ("https://en.wikipedia.org/wiki/Edmund_Stoiber");
            d[45] = ("https://en.wikipedia.org/wiki/Elizabeth_II");
            d[46] = ("https://en.wikipedia.org/wiki/Elton_John");
            d[47] = ("https://en.wikipedia.org/wiki/Floyd_Little");
            d[48] = ("https://en.wikipedia.org/wiki/Frank_Stronach");
            d[49] = ("https://en.wikipedia.org/wiki/Frank_Williams_(Formula_One)");
            d[50] = ("https://en.wikipedia.org/wiki/Frankie_Muniz");
            d[51] = ("https://en.wikipedia.org/wiki/Garry_Kasparov");
            d[52] = ("https://en.wikipedia.org/wiki/Gaston_Glock");
            d[53] = ("https://en.wikipedia.org/wiki/George_J._Mitchell");
            d[54] = ("https://en.wikipedia.org/wiki/George_R._R._Martin");
            d[55] = ("https://en.wikipedia.org/wiki/Hansi_Hinterseer");
            d[56] = ("https://en.wikipedia.org/wiki/Harrison_Ford");
            d[57] = ("https://en.wikipedia.org/wiki/Hayao_Miyazaki");
            d[58] = ("https://de.wikipedia.org/wiki/Heinz_Pr%C3%BCller");
            d[59] = ("https://en.wikipedia.org/wiki/Heinz-Christian_Strache");
            d[60] = ("https://en.wikipedia.org/wiki/Henry_Kissinger");
            d[61] = ("https://en.wikipedia.org/wiki/Hugh_Jackman");
            d[62] = ("https://de.wikipedia.org/wiki/Hugo_Portisch");
            d[63] = ("https://en.wikipedia.org/wiki/Ian_McKellen");
            d[64] = ("https://en.wikipedia.org/wiki/Ian_St_John");
            d[65] = ("https://en.wikipedia.org/wiki/Jackie_Stewart");
            d[66] = ("https://en.wikipedia.org/wiki/James_Whale_(radio_presenter)");
            d[67] = ("https://en.wikipedia.org/wiki/Jeff_Bridges");
            d[68] = ("https://en.wikipedia.org/wiki/Jim_Carrey");
            d[69] = ("https://en.wikipedia.org/wiki/Jimmy_Carter");
            d[70] = ("https://en.wikipedia.org/wiki/Jimmy_Greaves");
            d[71] = ("https://en.wikipedia.org/wiki/Joe_Biden");
            d[72] = ("https://en.wikipedia.org/wiki/John_Cleese");
            d[73] = ("https://en.wikipedia.org/wiki/John_Nord");
            d[74] = ("https://en.wikipedia.org/wiki/Johnny_Depp");
            d[75] = ("https://en.wikipedia.org/wiki/Joni_Mitchell");
            d[76] = ("https://de.wikipedia.org/wiki/Die_Autoh%C3%A4ndler"); //Jörg Krusche
            d[77] = ("https://en.wikipedia.org/wiki/J%C3%B6rg_Pilawa");
            d[78] = ("https://en.wikipedia.org/wiki/Joss_Whedon");
            d[79] = ("https://en.wikipedia.org/wiki/Keith_Richards");
            d[80] = ("https://en.wikipedia.org/wiki/Kim_Jong-un");
            d[81] = ("https://en.wikipedia.org/wiki/Larry_King");
            d[82] = ("https://en.wikipedia.org/wiki/Leon_Spinks");
            d[83] = ("https://en.wikipedia.org/wiki/Lester_Piggott");
            d[84] = ("https://en.wikipedia.org/wiki/Linda_Nolan");
            d[85] = ("https://en.wikipedia.org/wiki/Lindsay_Lohan");
            d[86] = ("https://en.wikipedia.org/wiki/Maggie_Smith");
            d[87] = ("https://en.wikipedia.org/wiki/Mark_Boone_Junior");
            d[88] = ("https://en.wikipedia.org/wiki/Mel_Brooks");
            d[89] = ("https://en.wikipedia.org/wiki/Mel_Gibson");
            d[90] = ("https://en.wikipedia.org/wiki/Mette-Marit,_Crown_Princess_of_Norway");
            d[91] = ("https://en.wikipedia.org/wiki/Michael_Cullen_(politician)");
            d[92] = ("https://en.wikipedia.org/wiki/Michael_Douglas");
            d[93] = ("https://en.wikipedia.org/wiki/Michael_Gambon");
            d[94] = ("https://en.wikipedia.org/wiki/Michael_J._Fox");
            d[95] = ("https://en.wikipedia.org/wiki/Michael_Schumacher");
            d[96] = ("https://en.wikipedia.org/wiki/Michio_Kaku");
            d[97] = ("https://en.wikipedia.org/wiki/Necro_Butcher");
            d[98] = ("https://en.wikipedia.org/wiki/Nina_Hagen");
            d[99] = ("https://en.wikipedia.org/wiki/Nino_de_Angelo");
            d[100] = ("https://en.wikipedia.org/wiki/O._J._Brigance");
            d[101] = ("https://en.wikipedia.org/wiki/Olivia_Newton-John");
            d[102] = ("https://en.wikipedia.org/wiki/Ottfried_Fischer");
            d[103] = ("https://en.wikipedia.org/wiki/Otto_Waalkes");
            d[104] = ("https://en.wikipedia.org/wiki/Ozzy_Osbourne");
            d[105] = ("https://en.wikipedia.org/wiki/Patrick_Stewart");
            d[106] = ("https://en.wikipedia.org/wiki/Paul_Gascoigne");
            d[107] = ("https://en.wikipedia.org/wiki/Paul_Westphal");
            d[108] = ("https://en.wikipedia.org/wiki/Klagenfurt"); //Pauline Schubert
            d[109] = ("https://en.wikipedia.org/wiki/Peter_Higgs");
            d[110] = ("https://en.wikipedia.org/wiki/Peter_Tobin");
            d[111] = ("https://en.wikipedia.org/wiki/Phil_Collins");
            d[112] = ("https://de.wikipedia.org/wiki/The_Real_Life_Guys"); //Philipp Mickenbecker
            d[113] = ("https://en.wikipedia.org/wiki/Prince_Philip,_Duke_of_Edinburgh");
            d[114] = ("https://en.wikipedia.org/wiki/Prunella_Scales");
            d[115] = ("https://en.wikipedia.org/wiki/Rainer_Langhans");
            d[116] = ("https://en.wikipedia.org/wiki/Ric_Flair");
            d[117] = ("https://en.wikipedia.org/wiki/Richard_Lugner");
            d[118] = ("https://en.wikipedia.org/wiki/Rick_Priestley");
            d[119] = ("https://en.wikipedia.org/wiki/Rod_Roddenberry");
            d[120] = ("https://en.wikipedia.org/wiki/Roger_Waters");
            d[121] = ("https://en.wikipedia.org/wiki/Ron_Perlman");
            d[122] = ("https://en.wikipedia.org/wiki/Ronnie_Wood");
            d[123] = ("https://en.wikipedia.org/wiki/Rush_Limbaugh");
            d[124] = ("https://en.wikipedia.org/wiki/Ryan_O%27Neal");
            d[125] = ("https://en.wikipedia.org/wiki/Samu_(wrestler)");
            d[126] = ("https://en.wikipedia.org/wiki/Sarah_Harding");
            d[127] = ("https://en.wikipedia.org/wiki/Selena_Gomez");
            d[128] = ("https://en.wikipedia.org/wiki/Selma_Blair");
            d[129] = ("https://en.wikipedia.org/wiki/Shane_MacGowan");
            d[130] = ("https://en.wikipedia.org/wiki/Shannen_Doherty");
            d[131] = ("https://en.wikipedia.org/wiki/Sharon_Osbourne");
            d[132] = ("https://en.wikipedia.org/wiki/Silvio_Berlusconi");
            d[133] = ("https://en.wikipedia.org/wiki/Slick_Woods");
            d[134] = ("https://en.wikipedia.org/wiki/Stephen_Darby");
            d[135] = ("https://en.wikipedia.org/wiki/Steve_Gleason");
            d[136] = ("https://en.wikipedia.org/wiki/Superstar_Billy_Graham");
            d[137] = ("https://en.wikipedia.org/wiki/Terence_Hill");
            d[138] = ("https://en.wikipedia.org/wiki/Hulk_Hogan");
            d[139] = ("https://en.wikipedia.org/wiki/Scott_Glenn");
            d[140] = ("https://en.wikipedia.org/wiki/Thomas_Danneberg");
            d[141] = ("https://en.wikipedia.org/wiki/Tina_Turner");
            d[142] = ("https://en.wikipedia.org/wiki/Tom_Hanks");
            d[143] = ("https://en.wikipedia.org/wiki/Tom_Parker_(singer)");
            d[144] = ("https://en.wikipedia.org/wiki/Tom_Smith_(rugby_union,_born_1971)");
            d[145] = ("https://en.wikipedia.org/wiki/Val_Kilmer");
            d[146] = ("https://en.wikipedia.org/wiki/Vint_Cerf");
            d[147] = ("https://en.wikipedia.org/wiki/William_Hurt");
            d[148] = ("https://en.wikipedia.org/wiki/William_Shatner");
            d[149] = ("https://en.wikipedia.org/wiki/Willie_Nelson");
            d[150] = ("https://en.wikipedia.org/wiki/Vladimir_Putin");
            d[151] = ("https://en.wikipedia.org/wiki/Wolfgang_Ambros");
            d[152] = ("https://en.wikipedia.org/wiki/Yoko_Ono");
        }
    }
}
