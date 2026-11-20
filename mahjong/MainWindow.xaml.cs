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
    public partial class MainWindow : Window
    {
        private MahjongTile primaTesseraSelezionata = null;

        public MainWindow()
        {
            InitializeComponent();
            RestartButton.Click += RestartButton_Click;

            CreaLivelloDiTest();

        }

        public void creaTessera(float x, float y, int z, String immagine) //creazione di una tessera nella grafica
        {
            MahjongTile tessera = new MahjongTile(x, y, z, immagine);

            // 1. Crea e carica la bitmap (come in precedenza)
            BitmapImage bitmap = new BitmapImage(new Uri(immagine, UriKind.Relative));

            // 2. Crea un controllo Image e assegna la bitmap come sorgente
            Image imgControl = new Image()
            {
                Source = bitmap,
                Width = 70,
                Height = 90,
                Tag = tessera
            };

            tessera.ImageUI = imgControl;

            // imgControl.Click += Tessera_Click; //ricordati di questo, è l'eventio fatto al click, senza questo ovviamente non succedera nulla
            imgControl.MouseLeftButtonDown += ImgControl_MouseLeftButtonDown;

            //posizione nella grafica delle tessere nonche bottoni
            Canvas.SetLeft(imgControl, x * 40);
            Canvas.SetTop(imgControl, y * 60);
            Canvas.SetZIndex(imgControl, z);


            TavoloDiGioco.Children.Add(imgControl); //disegna nella grafica le tessere, nonche i bottoni creati
        }

        private void ImgControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image i = (Image)sender;

            MahjongTile tesseraCliccata = (MahjongTile)i.Tag; //in questo caso grazie al tag noi ci riferiamo esplicitamente a la tessera cliccata del bottone cliccato con le specifiche del tag

            if (isFree(tesseraCliccata) == false)
            {
                return;
            }

            if (primaTesseraSelezionata == null) //se la prima tessera non è ancora stata cliccata
            {
                primaTesseraSelezionata = tesseraCliccata; //prima tessera sara la prima tessera cliccata
                primaTesseraSelezionata.isSelezionata = true;
                primaTesseraSelezionata.ImageUI.Opacity = 0.5;

            }
            else //ALTRIMENTI
            {
                if (tesseraCliccata == primaTesseraSelezionata) //se la prima tessera è stata selezionata e la seconda (tesseraCliccata) è uguale in tutto e pertutto allora sono la stessa tessera, la deseleziono
                {
                    primaTesseraSelezionata.isSelezionata = false;
                    primaTesseraSelezionata = null;
                    tesseraCliccata.isSelezionata = false;
                    tesseraCliccata = null;
                }
                else //ALTRIMENTI se non sono uguali in tutto e per tutto
                {
                    if (primaTesseraSelezionata.isSameType(tesseraCliccata)) //controllo il tipo, se le due tessere hanno stesso tipo allora le elimino
                    {
                        TavoloDiGioco.Children.Remove(primaTesseraSelezionata.ImageUI); //cancellata del canvas
                        TavoloDiGioco.Children.Remove(tesseraCliccata.ImageUI);
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
        }

        public bool isFree(MahjongTile t)
        {
            bool bloccataSinistra = false;
            bool bloccataDestra = false;

            for (int i = 0; i < TavoloDiGioco.Children.Count; i++)
            {
                Image I = (Image)TavoloDiGioco.Children[i];
                MahjongTile altraTessera = (MahjongTile)I.Tag;

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

                    if (t.getZ() + 1 == altraTessera.getZ() && ((t.getY() == altraTessera.getY() || t.getY() == altraTessera.getY() + 0.5f || t.getY() == altraTessera.getY() - 0.5f) && (t.getX() == altraTessera.getX() || t.getX() == altraTessera.getX() + 0.5f || t.getX() == altraTessera.getX() - 0.5f)))
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

        private void CreaLivelloDiTest()
        {

            creaTessera(1f, 1f, 0, "tessere/1.png");
            creaTessera(1f, 2f, 0, "tessere/1.png");
            creaTessera(2f, 1f, 0, "tessere/1.png");
            creaTessera(2f, 2f, 0, "tessere/1.png");
            creaTessera(3f, 2f, 0, "tessere/1.png");
            creaTessera(2f, 3f, 0, "tessere/1.png");

            creaTessera(7.5f, 3.5f, 0, "tessere/5.png");
            creaTessera(3.5f, 6.5f, 0, "tessere/5.png");
            creaTessera(5.5f, 4.5f, 0, "tessere/5.png");
            creaTessera(3.5f, 3.5f, 0, "tessere/5.png");

            creaTessera(5.5f, 6.5f, 0, "tessere/4.png");
            creaTessera(2.5f, 3.5f, 2, "tessere/4.png");
            creaTessera(3.5f, 6.5f, 2, "tessere/4.png");
            creaTessera(1.5f, 3.5f, 0, "tessere/4.png");

            creaTessera(4.5f, 7.5f, 1, "tessere/6.png");
            creaTessera(1.5f, 5.5f, 1, "tessere/6.png");


            creaTessera(3.5f, 5.5f, 0, "tessere/7.png");
            creaTessera(1.5f, 15.5f, 0, "tessere/7.png");
            creaTessera(8.5f, 5.5f, 2, "tessere/7.png");
            creaTessera(1.5f, 15.5f, 1, "tessere/7.png");
            creaTessera(3.5f, 3.5f, 1, "tessere/7.png");
            creaTessera(5.5f, 10.5f, 2, "tessere/7.png");
            creaTessera(3.5f, 2.5f, 0, "tessere/7.png");
            creaTessera(1.5f,4.5f, 0, "tessere/7.png");

            creaTessera(1.5f, 1.5f, 1, "tessere/3.png");
            creaTessera(2.5f, 1.5f, 1, "tessere/3.png");
            creaTessera(1.5f, 2.5f, 1, "tessere/3.png");
            creaTessera(2.5f, 2.5f, 1, "tessere/3.png");
            creaTessera(3.5f, 2.5f, 1, "tessere/3.png");
            creaTessera(2.5f, 3.5f, 1, "tessere/3.png");




            creaTessera(1f+1, 1f+1, 2, "tessere/1.png");
            creaTessera(1f+1, 2f+1, 2, "tessere/1.png");
            creaTessera(2f+1, 1f+1, 2, "tessere/1.png");
            creaTessera(2f+1, 2f+1, 2, "tessere/1.png");




        }
        public class MahjongTile
        {
            private float x;
            private float y;
            private int z;
            private string tipo;
            public bool isSelezionata = false;
            public Image ImageUI;

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


            public bool isSameType(MahjongTile t)
            {
                if (this.getTipo() != t.getTipo())
                {
                    return false;
                }
                return true;
            }


        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            TavoloDiGioco.Children.Clear();
            primaTesseraSelezionata = null;
            CreaLivelloDiTest();
        }
    }
}