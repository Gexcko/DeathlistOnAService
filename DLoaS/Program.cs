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
        static string year = "2022";

        //static string[] d = new string[153];        //Anlegen der Link-Liste mit URLS
        static string[] d = dlInitA();
        static ArrayList dl = new ArrayList();      //Anlegen der Liste der bereits verstorbenen


        //Arrays beinhalten Index Nummern der "Opfer" in den Deathlists der Teilnehmer
        static int[] emil;
        static int[] fabian;
        static int[] jasi;
        static int[] manu;
        static int[] michi;
        static int[] valentin;
        static int[] miri;
        static int[] volker;
        //static int[] rene;

        //Countervariablen der Teilnehmer
        static int emilScore = 0;
        static int fabianScore = 0;
        static int jasiScore = 0;
        static int manuScore = 0;
        static int michiScore = 0;
        static int valentinScore = 0;
        static int miriScore = 0;
        static int volkerScore = 0;
        //static int reneScore = 0;

        static void Main(string[] args) {
            Console.Title = "Deathlist "+year;
            //dlInit();                               //Befüllen der Link-Liste

            //Befüllen der Deathlists
            emil        = new int[] {0,1,2,12,13,17,19,42,55,57,60,62,78,79,91,93,96,97,103,108,111,120,125,126,127,135,136,138,139,142};
            fabian      = new int[] {0,2,3,18,25,31,35,45,46,47,60,62,69,84,90,92,96,97,103,107,116,117,120,120,124,125,126,127,138,139};
            jasi        = new int[] {6,8,9,13,16,26,29,30,32,34,36,38,48,52,53,80,85,86,87,88,89,94,97,98,112,120,123,136,138,146};
            manu        = new int[] {3,7,10,26,34,36,37,38,39,48,64,67,70,71,79,85,86,89,97,98,99,100,103,115,118,121,124,129,137,143};
            michi       = new int[] {19,21,26,28,33,34,44,48,58,64,72,73,86,88,89,95,97,102,103,104,106,118,119,122,132,136,137,138,139,145};
            //rene        = new int[] { 4, 13, 21, 23, 28, 36, 44, 49, 52, 55, 57, 58, 59, 63, 65, 72, 78, 87, 87, 96, 103, 105, 109, 118, 119, 120, 121, 133, 139, 146 };
            valentin    = new int[] {6,13,14,19,24,27,29,43,50,59,66,67,77,83,85,87,88,98,101,105,38,111,112,128,131,134,141,144,147,148};
            miri    = new int[] {11,16,22,23,26,33,36,48,49,52,54,54,56,63,67,74,76,81,82,85,89,107,109,110,112,113,123,144,146,148};
            volker    = new int[] {4,5,13,15,20,33,35,40,41,46,51,60,61,65,68,75,88,97,103,107,114,117,126,127,130,133,136,138,140,148};


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

                    if (data.Contains(year+" deaths")) {                                                                 //Abfrage ob "2021 deaths" in den Categories vorkommt (engl. Wiki)
                        dl.Add(i);                                                                                          //In die Todes-Liste hinzufügen
                        string s = d[i].Substring(30).Replace("_", " ");                                                    //Vorbereitung für Console-Output
                        Console.WriteLine(i+" - "+s);                                                                       //Console-Output
                    }else if(data.Contains("Gestorben "+year)) {                                                         //Abfrage ob "Gestorben 2021" in den Categories vorkommt (deu. Wiki)
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
            //reneScore = fastSearch(dl, rene);
            valentinScore = fastSearch(dl, valentin);
            miriScore = fastSearch(dl, miri);
            volkerScore = fastSearch(dl, volker);
            

            //Console-Output mit allen Teilnehmern und deren Punkten
            Console.WriteLine("Emil: " + emilScore);
            Console.WriteLine("Fabian: " + fabianScore);
            Console.WriteLine("Jasi: " + jasiScore);
            Console.WriteLine("Manu: " + manuScore);
            Console.WriteLine("Michi: " + michiScore);
            //Console.WriteLine("Rene: " + reneScore);
            Console.WriteLine("Valentin: " + valentinScore);
            Console.WriteLine("Miri: " + miriScore);
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
                whoPointed += pointSearch(Int32.Parse(oDiff), jasi, "Jasi ");
                whoPointed += pointSearch(Int32.Parse(oDiff), manu, "Manu ");
                whoPointed += pointSearch(Int32.Parse(oDiff), michi, "Michi ");
                //whoPointed += pointSearch(Int32.Parse(oDiff), rene, "Rene ");
                whoPointed += pointSearch(Int32.Parse(oDiff), valentin, "Valentin ");
                whoPointed += pointSearch(Int32.Parse(oDiff), miri, "Miri ");
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
                        Title = "Deathlist "+year,
                        Body = "RIP "+ d[Int32.Parse(oDiff)].Substring(30).Replace("_", " ") + "\n" + whoPointed
                    };

                    PushbulletSharp.Models.Responses.PushResponse response = client.PushNote(request);
                }

                Pushover pclient = new Pushover("asf1vbpvec3x6h288p46nu8xj2i6xb");
                PushoverClient.PushResponse responsePO = pclient.Push(
                              "Deathlist "+year,
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

        static string[] dlInitA()
        {
            try
            {
                return File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "List.txt");                          //Einlesen des Text-Files mit den Verstorbenen, die bereits vor der Abfrage abgedankt haben
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("Please provide Link-List");
            }
        }
    }
}
