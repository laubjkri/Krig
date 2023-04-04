using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Krig;

namespace KrigGrafisk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly KrigGame game;
        private readonly List<PlayerGui> players;
        private readonly Dictionary<string, BitmapImage> cardImages;

        public MainWindow()
        {
            InitializeComponent();

            game = new KrigGame();
            game.GameChanged += UpdateGui;
            game.PlayersChanged += GeneratePlayers;

            players = new List<PlayerGui>();
            cardImages = ReadImages();

            GeneratePlayers();
            UpdateGui();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            game.StartNewGame();       
        }

        private void DealButton_Click(object sender, RoutedEventArgs e)
        {
            game.Deal();            
        }

        private void UpdateGui()
        {
            cardsInDeckLabel.Content = game.RemainingCards;
            infoLabel.Text = game.CurrentStateString;            
            dealButton.IsEnabled = game.CurrentState == KrigGame.State.InProgress;

            players.ForEach(p => p.Update());
        }

        private void NumberOfPlayersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;

            string? strValue = cbItem.Content.ToString();
            if (int.TryParse(strValue, out int value))
            {
                if(game != null)
                {
                    game.NumberOfPlayers = value;
                }
            }
        }
        
        private class PlayerGui
        {
            public Label nameLabel;
            public Image cardImage;
            public Label cardLabel;
            public Label scoreLabel;
            public Player playerModel;
            private readonly Dictionary<string, BitmapImage> _cardImages;

            public PlayerGui(Player player, Grid grid, Dictionary<string, BitmapImage> cardImages)
            {

                if(player == null)
                {
                    throw new ArgumentNullException("Player model object is null!");
                }

                ColumnDefinition columnDefinition = new ColumnDefinition() { Width = new GridLength(200) };
                grid.ColumnDefinitions.Add(columnDefinition);
                int columnIndex = grid.ColumnDefinitions.Count - 1;

                playerModel = player;
                // Player name
                nameLabel = new Label();
                nameLabel.Content = "Player " + playerModel.PlayerId;
                nameLabel.VerticalAlignment = VerticalAlignment.Center;
                nameLabel.HorizontalAlignment = HorizontalAlignment.Center;
                nameLabel.Margin = new Thickness(20, 0, 20, 0);
                Grid.SetRow(nameLabel, 0);
                Grid.SetColumn(nameLabel, columnIndex);
                grid.Children.Add(nameLabel);

                // If there is no card image this label will be visible
                cardLabel = new Label();
                cardLabel.Content = "No card";
                cardLabel.VerticalAlignment = VerticalAlignment.Center;
                cardLabel.HorizontalAlignment = HorizontalAlignment.Center;
                cardLabel.Margin = new Thickness(20, 0, 20, 0);
                Grid.SetRow(cardLabel, 1);
                Grid.SetColumn(cardLabel, columnIndex);
                grid.Children.Add(cardLabel);

                // Current card image
                _cardImages = cardImages;
                cardImage = new Image();
                if (playerModel.CurrentCard != null)
                {
                    cardImage.Source = _cardImages[playerModel.CurrentCard.GetCardCode()];
                }

                Grid.SetRow(cardImage, 1);
                Grid.SetColumn(cardImage, columnIndex);
                grid.Children.Add(cardImage);

                // Score
                scoreLabel = new Label();
                scoreLabel.VerticalAlignment = VerticalAlignment.Center;
                scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
                scoreLabel.Margin = new Thickness(20, 0, 20, 0);
                Grid.SetRow(scoreLabel, 2);
                Grid.SetColumn(scoreLabel, columnIndex);
                grid.Children.Add(scoreLabel);
            }

            public void Update()
            {
                if(playerModel.CurrentCard == null)
                {
                    cardLabel.Content = "No card";
                }
                else
                {
                    cardLabel.Content = playerModel.CurrentCard.ToString();
                    cardImage.Source = _cardImages[playerModel.CurrentCard.GetCardCode()];
                }
                
                scoreLabel.Content = playerModel.Score;
            }
        }

        private void GeneratePlayers()
        {
            ResetGrid();
            players.Clear();
            foreach (var player in game.Players)
            {
                players.Add(new PlayerGui(player, contentGrid, cardImages));
            }
        }

        private void ResetGrid()
        {
            // First remove all items not in first column (thats where the row labels are)
            List<UIElement> itemsToBeRemoved = new List<UIElement>();            
            for (int i = 0; i < contentGrid.Children.Count; i++)
            {
                UIElement child = contentGrid.Children[i];
                if(Grid.GetColumn(child) != 0)
                {
                    itemsToBeRemoved.Add(child);
                }                
            }

            // Then remove the column definitions
            foreach (UIElement child in itemsToBeRemoved)
            {
                contentGrid.Children.Remove(child);
            }

            contentGrid.ColumnDefinitions.RemoveRange(1, contentGrid.ColumnDefinitions.Count - 1);
        }

        private Dictionary<string, BitmapImage> ReadImages()
        {
            Dictionary<string, BitmapImage> images = new();
            CardDeck cardDeck = new CardDeck();

            foreach (Card card in cardDeck.Cards)
            {
                string cardCode = card.GetCardCode();
                Uri uri = new Uri(@$"/card_images/{cardCode}.png", UriKind.RelativeOrAbsolute);
                images[cardCode] = new BitmapImage(uri);
            }

            return images;
        }

    }
}
