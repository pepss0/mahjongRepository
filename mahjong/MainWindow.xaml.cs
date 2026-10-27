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

        public void creaTessera(float x, float y, int z, String immagine) //creazione di una tessera nella grafica
        {
            MahjongTile tessera = new MahjongTile(x, y, z, immagine);
            
            // 1. Crea e carica la bitmap (come in precedenza)
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(immagine, UriKind.Absolute);
            bitmap.EndInit();

            // 2. Crea un controllo Image e assegna la bitmap come sorgente
            Image imgControl = new Image();
            imgControl.Source = bitmap;
            imgControl.Width = 50;  // Opzionale: ridimensiona per adattarla al bottone
            imgControl.Height = 70;

            Button MyButton = new Button()
            {
                Width = 50,
                Height = 60,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Content = imgControl,
                Tag = tessera
            };

            tessera.BottoneUI = MyButton;

            MyButton.Click += Tessera_Click; //ricordati di questo, è l'eventio fatto al click, senza questo ovviamente non succedera nulla

            //posizione nella grafica delle tessere nonche bottoni
            Canvas.SetLeft(MyButton, x * 40);
            Canvas.SetTop(MyButton, y * 60);
            Panel.SetZIndex(MyButton, z);

            TavoloDiGioco.Children.Add(MyButton); //disegna nella grafica le tessere, nonche i bottoni creati
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
            creaTessera(2.5f, 1.5f, 0, "https://images.vexels.com/media/users/3/268432/isolated/preview/290f0ffb97b3fbaa139839f854eff6ee-mahjong-green-dragon-card.png");
            creaTessera(5f, 3f, 0, "https://images.vexels.com/media/users/3/268432/isolated/preview/290f0ffb97b3fbaa139839f854eff6ee-mahjong-green-dragon-card.png");
            creaTessera(7f, 5f, 0, "https://images.vexels.com/media/users/3/268473/isolated/preview/36cd41604a25199f095ccd260a693a9c-mahjong-six-of-dots-tile.png");
            creaTessera(8f, 6f, 0, "https://images.vexels.com/media/users/3/268473/isolated/preview/36cd41604a25199f095ccd260a693a9c-mahjong-six-of-dots-tile.png");
            
        }
        public class MahjongTile
        {
            private float x;
            private float y;
            private int z;
            private string tipo;
            public bool isSelezionata = false;
            public Button BottoneUI;

            public MahjongTile(float x, float y, int z, string tipo)
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
            public float getX()
            {
                return this.x;
            }
            public float getY()
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
