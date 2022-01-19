using System;
using System.Collections.Generic;
using System.Linq;

namespace OkeyApp
{
    class Program
    {

        //Oyuncunun elindeki perlerde bulunan taş sayısına göre toplam puanını hesaplamak için kullanılır.
        //Oyuncunun elinde okey varsa artı puan yazılır.
        public static int totalPoints = 0;
        public static int maxPoint = 0;
        public static int winner = 0;

        static void Main(string[] args)
        {
            int[] okeyStones;

            //104 adet okey taşını oluşturur ve bu taşları karıştırır
            okeyStones = CreateStones();

            //Gösterge taşını oluşturur
            var gosterge = CreateGosterge(okeyStones);

            //Göstergeye göre okey taşını belirler
            int okey = CreateOkey(gosterge);


            IDictionary<int, List<int>> userTaslar = new Dictionary<int, List<int>>();
            
            //Taşları oyunculara dağıtır. 1 oyuncu 15, diğerleri 14 taş alır.
            DistributeStones( userTaslar, okeyStones);

            //Dağıtım yapıldıktan sonra, Oyuncuların elinde olan taşları gösterir.
            ShowPlayerStones(userTaslar, 1);
            ShowPlayerStones(userTaslar, 2);
            ShowPlayerStones(userTaslar, 3);
            ShowPlayerStones(userTaslar, 4);

            Console.WriteLine("----------------------Puanlar----------------------");

            for(var i= 1; i<=4; i++ )
            {
                Console.WriteLine("Oyuncu " + i + " :");

                //Oyuncunun sahip olduğu taşları aynı numara, farklı renk olacak şekilde per yapar. (Örnek: Siyah1 Siyah2 Siyah3)
                CalculatePerByNumber(userTaslar[i], okey);

                //Oyuncunun sahip olduğu taşları farklı numara, aynı renk olacak şekilde per yapar. (Örnek: Kırmızı13 Sarı13 Mavi13)
                CalculatePerByColor(userTaslar[i], okey);

                //Oyuncunun sahip olduğu toplam puanı gösterir. Kazanan bu puana göre belirlenir.
                Console.WriteLine("\nToplam Puan: " + totalPoints);

                if(totalPoints > 0 && totalPoints > maxPoint)
                {
                    maxPoint = totalPoints;
                    winner = i;
                }
                totalPoints = 0;

                Console.WriteLine("________________________________________________\n");
            }

            if(winner!=0)
            {
                Console.WriteLine("Bitirmeye En Yakın El Oyuncu: " + winner + ". oyuncudadır.");
            }
            else
            {
                Console.WriteLine("Oyuncular yeteri kadar per toplayamadı. Kazanan oyuncu 1. elde belirlenemedi.");
            }
            

        }

        //104 adet okey taşını oluşturur ve bu taşları karıştırır
        public static int[] CreateStones()
        {
            var randomNumber = new Random();
            int[] okeyStones;
            List<int> okeyStonesList = new();

            //okeyStonesList'e 0-52 arasındaki sayılardan her birini 2 defa ekler. 
            for (var i = 0; i <= 52; i++)
            {
                okeyStonesList.Add(i);
                okeyStonesList.Add(i);
            }

            //Oluşturulan okeyStonesList, array'e dönüştürülür.
            okeyStones = okeyStonesList.ToArray();

            //Okey taşlarının karıştırılması için, sıralama array içinde random olarak dizilir.
            okeyStones = okeyStones.OrderBy(x => randomNumber.Next()).ToArray();

            return okeyStones;
        }

        //Gösterge taşı belirlenir.
        public static int CreateGosterge(int[] okeyTaslari)
        {
            var randomNumber = new Random();
            int gostergeNo = randomNumber.Next(0, 104);

            //Göstergenin Joker(52) çıkarsa, yeniden gösterge seçimi yapılır.
            while (okeyTaslari[gostergeNo] == 52)
            {
                okeyTaslari[gostergeNo] = randomNumber.Next(0, 104);
            }

            return okeyTaslari[gostergeNo];
        }

        //Gösterge taşına göre okey belirlenir.
        public static int CreateOkey(int gosterge)
        {
            int okey;

            //Gösterge 13 ise okey 1 olarak belirlenir, değil ise okey, göstergenin bir fazlasıdır.
            if (gosterge == 12 || gosterge == 25 || gosterge == 38 || gosterge == 51)
            {
                okey = gosterge - 12;
            }
            else
            {
                okey = gosterge + 1;
            }

            Console.Write("OKEY TAŞI: ");
            DisplayStones(okey);

            Console.WriteLine("\n");

            return okey;
        }

        //Taşlar oyuncular arasında dağıtılır.
        public static void DistributeStones(IDictionary<int, List<int>> playerStones, int[] okeyTaslari)
        {
            var randomNumber = new Random();
            List<int> users = new() { 1, 2, 3, 4 };

            //Hangi oyuncunun 15 taş ile başlayacağını belirlemek için, random numara belirlenir.
            int userNo = randomNumber.Next(1, 5);
            Console.WriteLine("Bu elde " + userNo + ". oyuncu 15 taş ile başlayacak.\n");

            //İlk oyuncuya 15 taş verilir
            List<int> getOkeyStones = new();
            for (var i = 0; i < 15; i++)
            {
                getOkeyStones.Add(okeyTaslari[i]);
            }

            playerStones.Add(userNo, getOkeyStones);

            users.Remove(userNo);
            var start = 15;
            var end = 29;

            //Diğer oyunculara 14 taş verilir.
            foreach (var i in users)
            {
                getOkeyStones = new();

                for (var k = start; k < end; k++)
                {
                    getOkeyStones.Add(okeyTaslari[k]);
                }

                playerStones.Add(i, getOkeyStones);
                start = end;
                end += 14;

            }

        }

        //Dağıtım yapıldıktan sonra, Oyuncuların elinde olan taşları gösterir.
        public static void ShowPlayerStones(IDictionary<int, List<int>> userTaslar, int order)
        {
            Console.Write(order + ". Oyuncu'nun Taşları: \n");
            foreach (var tas in userTaslar[order])
            {

                DisplayStones(tas);

            }
            Console.Write("\n\n");
        }

        //0-52 arası numaralandırma yapılan taşları, oyundaki gerçek isimleri ile göstermek için kullanılan fonksiyondur. (Örnek: 0 -> Sarı1)
        public static void DisplayStones(int stone)
        {
            
            if (stone < 13)
            {
                Console.Write(" *Sarı" + (stone + 1));
            }
            else
            {
                var calculateStone = (stone % 13) + 1;

                if (stone < 26)
                {

                    Console.Write(" *Mavi" + calculateStone);
                }
                else if (stone < 39)
                {
                    Console.Write(" *Siyah" + calculateStone);
                }
                else if (stone == 52)
                {
                    Console.Write(" *Joker52");
                }
                else
                {
                    Console.Write(" *Kırmızı" + calculateStone);
                }

            }
        }

        //Oyuncunun sahip olduğu taşları farklı numara, aynı renk olacak şekilde per yapar. (Örnek: Kırmızı13 Sarı13 Mavi13)
        public static void CalculatePerByColor(List<int> playerStones, int okey)
        {
            var stonesCount = 0;
            var okeyCount = 0;

            List<int> temp = new (playerStones);
            List<int> stonesInPer = new();

            for (var fs = 0; fs < playerStones.Count; fs++)
            {
                var currentStone = temp[fs];
                stonesInPer.Add(currentStone);

                foreach (var stone in playerStones)
                {
                    var tempStone = stone;

                    if (tempStone == okey)
                    {
                        okeyCount++;
                    }

                    if (tempStone == 52)
                    {
                        tempStone = okey;
                    }

                    if ((tempStone % 13) == (currentStone % 13) && (tempStone != currentStone) && !stonesInPer.Contains(tempStone) && tempStone!=52)
                    {
                        stonesInPer.Add(tempStone);
                        stonesCount++;

                    }

                }

                if (stonesCount >= 2)
                {
                    totalPoints += (stonesCount + 1);

                    Console.Write("\nPER(Aynı Sayı Farklı Renkte): ");
                    for (var s = 0; s <= stonesCount; s++)
                    {

                        DisplayStones(stonesInPer.ToArray()[s]);

                        playerStones.Remove(stonesInPer.ToArray()[s]);
                    }
                    Console.WriteLine("\n");
                }

                stonesCount = 0;
                stonesInPer = new();
                okeyCount = 0;

            }
        }

        //Oyuncunun sahip olduğu taşları aynı numara, farklı renk olacak şekilde per yapar. (Örnek: Siyah1 Siyah2 Siyah3)
        public static void CalculatePerByNumber(List<int> playerStones, int okey)
        {

            var stonesCount = 0;
            List<int> tempPlayerStones = new(playerStones);

            for (var m = 0; m < tempPlayerStones.Count; m++)
            {
                var currentStone = tempPlayerStones[m];


                var tempCurrentStone = currentStone;
                if (currentStone == 12 || currentStone == 25 || currentStone == 38 || currentStone == 51)
                {
                    tempCurrentStone -= 12;
                } else
                {
                    tempCurrentStone = currentStone + 1;
                }

                
                var okeyCount = 0;
                foreach (var stone in playerStones)
                {
                    var tempPlayerStone = stone;
                    if (tempPlayerStone == okey)
                    {
                        okeyCount++;
                    }

                    if (tempPlayerStone == 52)
                    {
                        tempPlayerStone = okey;
                    }


                    if (currentStone != tempPlayerStone && tempPlayerStone == tempCurrentStone)
                    {
                        tempCurrentStone += 1;
                        stonesCount++;
                    }

                }

                if (stonesCount == 1 && okeyCount == 1)
                {
                    totalPoints += (stonesCount + 1);
                    playerStones.Remove(okey);
                    stonesCount++;
                }
                else if (stonesCount == 0 && okeyCount == 2)
                {
                    totalPoints += (stonesCount + 2);
                    var ind = 0;
                    stonesCount = 2;
                    do
                    {

                        if (playerStones[ind] != okey)
                        {
                            playerStones.Remove(ind);
                        }

                    } while (playerStones[ind] == okey);

                    playerStones.Remove(okey);
                    playerStones.Remove(okey);
                }

                if (stonesCount >= 2)
                {
                    totalPoints += (stonesCount + 1);
                    var esse = tempPlayerStones[m];

                    Console.Write("\nPER(Farklı Sayı Aynı Renkte): ");
                    for (var s = 0; s <= stonesCount; s++)
                    {
                        esse = tempPlayerStones[m] + s;
                        DisplayStones(esse);

                        playerStones.Remove(esse);
                    }

                    Console.WriteLine("\n");

                }

                stonesCount = 0;
                okeyCount = 0;
            }

        }


    }
}
