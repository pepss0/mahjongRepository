using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mahjong
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MahjongTile primaTesseraSelezionata = null;
        public MainWindow()
        {

            InitializeComponent();
            CreaLivelloDiTest();

        }

        public void creaTessera(int x, int y, int z, string tipo) //creazione di una tessera nella grafica
        {
            MahjongTile tessera = new MahjongTile(x, y, z, tipo);

            Button b = new Button()
            {
                Width = 50,
                Height = 70,
                Content = tipo, //contenuto della tessera (dragone, bamboo ecc) 
                Tag = tessera //associa l'oggetto logico al bottone per poterlo recuperare durante il click, permette al bottone cliccato di sapere qualè la sua tessera

            };


            tessera.BottoneUI = b; //permette alla tessera di "sapere" qual è il suo bottone sullo schermo
            b.Click += Tessera_Click; //controllo del click


            //posizione nella grafica delle tessere nonche bottoni
            Canvas.SetLeft(b, x * 50);
            Canvas.SetTop(b, y * 70);
            Panel.SetZIndex(b, z);

            TavoloDiGioco.Children.Add(b); //disegna nella grafica
        }

        public bool isFree(MahjongTile t)
        {
            bool bloccataSinistra = false;
            bool bloccataDestra = false;

            for (int i = 0; i < TavoloDiGioco.Children.Count; i++)
            {
                Button b = (Button)TavoloDiGioco.Children[i];
                MahjongTile altraTessera = (MahjongTile)b.Tag;

                if (t != altraTessera)
                {
                    if (t.getZ() == altraTessera.getZ() && t.getY() == altraTessera.getY())
                    {

                        if (t.getX() + 1 == altraTessera.getX())
                        {
                            bloccataDestra = true;
                        }
                        if (t.getX() - 1 == altraTessera.getX())
                        {
                            bloccataSinistra = true;
                        }
                    }

                    if (t.getZ() + 1 == altraTessera.getZ() && t.getY() == altraTessera.getY() && t.getX() == altraTessera.getX())
                    {
                        return false;
                    }
                }
            }

            if (bloccataSinistra == true && bloccataDestra == true)
            {
                return false;
            }

            return true;
        }
        public void Tessera_Click(object sender, RoutedEventArgs e)
        {
            Button bottoneCliccato = (Button)sender; //prendiamo un bottone cliccato QUALSIASI

            MahjongTile tesseraCliccata = (MahjongTile)bottoneCliccato.Tag; //in questo caso grazie al tag noi ci riferiamo esplicitamente a la tessera cliccata del bottone cliccato con le specifiche del tag

            if (isFree(tesseraCliccata) == false)
            {
                return;
            }


            if (primaTesseraSelezionata == null) //se la prima tessera non è ancora stata cliccata
            {
                primaTesseraSelezionata = tesseraCliccata; //prima tessera sara la prima tessera cliccata
                primaTesseraSelezionata.isSelezionata = true;

            }
            else //ALTRIMENTI

                if (tesseraCliccata == primaTesseraSelezionata) //se la prima tessera è stata selezionata e la seconda (tesseraCliccata) è uguale in tutto e pertutto allora sono la stessa tessera, la deseleziono
                {
                    primaTesseraSelezionata.isSelezionata = false;
                    primaTesseraSelezionata = null;
                    tesseraCliccata.isSelezionata = false;
                    tesseraCliccata = null;
                }
                else //ALTRIMENTI se non sono uguali in tutto e per tutto
                {
                    if (primaTesseraSelezionata.isEqual(tesseraCliccata)) //controllo il tipo, se le due tessere hanno stesso tipo allora le elimino
                    {
                        TavoloDiGioco.Children.Remove(primaTesseraSelezionata.BottoneUI); //cancellata del canvas
                        TavoloDiGioco.Children.Remove(tesseraCliccata.BottoneUI);
                        primaTesseraSelezionata.isSelezionata = false;
                        primaTesseraSelezionata = null;
                        tesseraCliccata.isSelezionata = false;
                        tesseraCliccata = null;
                    }
                    else //se i tipi non sono uguali allora le deselezioni
                    {
                        primaTesseraSelezionata.isSelezionata = false;
                        primaTesseraSelezionata = null;
                        tesseraCliccata.isSelezionata = false;
                        tesseraCliccata = null;
                    }
                }

            if (TavoloDiGioco.Children.Count == 0)
            {

                MessageBox.Show("HAI VINTO!!");
            }
        }


        private void CreaLivelloDiTest()
        {
            creaTessera(2, 1, 0, "Bambu1");
            creaTessera(4, 1, 0, "Bambu1");
            creaTessera(6, 1, 0, "Cerchio3");
            creaTessera(8, 1, 0, "Cerchio3");

            // Riga 2 (Y = 2)
            creaTessera(2, 2, 0, "VentoEst");
            creaTessera(4, 2, 0, "VentoEst");
            creaTessera(6, 2, 0, "DragoRosso");
            creaTessera(8, 2, 0, "DragoRosso");

            // Riga 3 (Y = 3)
            creaTessera(2, 3, 0, "VentoOvest");
            creaTessera(4, 3, 0, "VentoOvest");
            creaTessera(6, 3, 0, "DragoVerde");
            creaTessera(8, 3, 0, "DragoVerde");

            // Riga 4 (Y = 4)
            creaTessera(2, 4, 0, "Fiore1");
            creaTessera(4, 4, 0, "Fiore1");
            creaTessera(6, 4, 0, "Cerchio1");
            creaTessera(8, 4, 0, "Cerchio1");


            // --- LIVELLO 1 (Il piano intermedio - 4 tessere al centro) ---
            // Queste tessere si appoggiano sopra il livello 0 bloccandolo parzialmente
            creaTessera(4, 2, 1, "Stagione1");
            creaTessera(6, 2, 1, "Stagione1");
            creaTessera(4, 3, 1, "Caratteri5");
            creaTessera(6, 3, 1, "Caratteri5");


            // --- LIVELLO 2 (La cima della piramide - 2 tessere) ---
            // Le prime tessere da cui dovrai obbligatoriamente partire
            creaTessera(5, 2, 2, "Caratteri9");
            creaTessera(5, 3, 2, "Caratteri9");
        }
        public class MahjongTile
        {
            private int x;
            private int y;
            private int z;
            private string tipo;
            public bool isSelezionata = false;
            public Button BottoneUI;

            public MahjongTile(int x, int y, int z, string tipo)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.tipo = tipo;
            }

            public string getTipo()
            {
                return this.tipo;
            }
            public int getX()
            {
                return this.x;
            }
            public int getY()
            {
                return this.y;
            }
            public int getZ()
            {
                return this.z;
            }

            public bool isEqual(MahjongTile t)
            {
                if (this.getTipo() != t.getTipo())
                {
                    return false;
                }
                return true;
            }

        }
    }
}
