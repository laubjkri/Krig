using System;
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

namespace Krig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly KrigGame game;

        public MainWindow()
        {
            InitializeComponent();            

            game = new KrigGame();
            game.GameChanged += UpdateGui;
            game.PlayersChanged += GeneratePlayers;
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

        private readonly List<PlayerGui> players = new List<PlayerGui>();
        private class PlayerGui
        {
            public Grid grid = new Grid();
            public Label nameLabel = new Label();
            public Label cardLabel = new Label();
            public Label scoreLabel = new Label();
            public Player playerModel;

            public PlayerGui(Player player)
            {

                if(player == null)
                {
                    throw new ArgumentNullException("Player model object is null!");
                }

                playerModel = player;
                // Row height is given by xaml style
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.0, GridUnitType.Star) });


                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200.0) });

                nameLabel.Content = "Player " + playerModel.PlayerId;
                nameLabel.VerticalAlignment = VerticalAlignment.Center;
                nameLabel.HorizontalAlignment = HorizontalAlignment.Center;
                nameLabel.Margin = new Thickness(20, 0, 20, 0);
                Grid.SetRow(nameLabel, 0);
                grid.Children.Add(nameLabel);

                cardLabel.Content = playerModel.CurrentCard?.ToString();
                cardLabel.VerticalAlignment = VerticalAlignment.Center;
                cardLabel.HorizontalAlignment = HorizontalAlignment.Center;
                cardLabel.Margin = new Thickness(20, 0, 20, 0);
                Grid.SetRow(cardLabel, 1);
                grid.Children.Add(cardLabel);

                scoreLabel.VerticalAlignment = VerticalAlignment.Center;
                scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
                scoreLabel.Margin = new Thickness(20, 0, 20, 0);
                Grid.SetRow(scoreLabel, 2);
                grid.Children.Add(scoreLabel);
            }

            public void AddToStack(StackPanel stackPanel)
            {
                stackPanel.Children.Add(grid);                
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
                }
                
                scoreLabel.Content = playerModel.Score;
            }
        }

        private void GeneratePlayers()
        {
            players.Clear();
            foreach (var player in game.Players)
            {
                players.Add(new PlayerGui(player));
            }

            playerStackPanel.Children.Clear();
            foreach (var player in players) 
            {
                player.AddToStack(playerStackPanel);
            }
        }

    }
}
