using System.Collections.Specialized;
using System.Reflection.Metadata;

namespace TheatreSeating
{
    public class SeatingUnit
    {
        public string Name { get; set; }
        public bool Reserved { get; set; }

        public SeatingUnit(string name, bool reserved = false)
        {
            Name = name;
            Reserved = reserved;
        }

    }

    public partial class MainPage : ContentPage
    {
        SeatingUnit[,] seatingChart = new SeatingUnit[5, 10];

        public MainPage()
        {
            InitializeComponent();
            GenerateSeatingNames();
            RefreshSeating();
        }

        private async void ButtonReserveSeat(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");

            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            seatingChart[i, j].Reserved = true;
                            await DisplayAlert("Successfully Reserverd", "Your seat was reserverd successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }

        private void GenerateSeatingNames()
        {
            List<string> letters = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            int letterIndex = 0;

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    seatingChart[row, column] = new SeatingUnit(letters[letterIndex] + (column + 1).ToString());
                }

                letterIndex++;
            }
        }

        private void RefreshSeating()
        {
            grdSeatingView.RowDefinitions.Clear();
            grdSeatingView.ColumnDefinitions.Clear();
            grdSeatingView.Children.Clear();

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                var grdRow = new RowDefinition();
                grdRow.Height = 50;

                grdSeatingView.RowDefinitions.Add(grdRow);

                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    var grdColumn = new ColumnDefinition();
                    grdColumn.Width = 50;

                    grdSeatingView.ColumnDefinitions.Add(grdColumn);

                    var text = seatingChart[row, column].Name;

                    var seatLabel = new Label();
                    seatLabel.Text = text;
                    seatLabel.HorizontalOptions = LayoutOptions.Center;
                    seatLabel.VerticalOptions = LayoutOptions.Center;
                    seatLabel.BackgroundColor = Color.Parse("#333388");
                    seatLabel.Padding = 10;

                    if (seatingChart[row, column].Reserved == true)
                    {
                        //Change the color of this seat to represent its reserved.
                        seatLabel.BackgroundColor = Color.Parse("#883333");

                    }

                    Grid.SetRow(seatLabel, row);
                    Grid.SetColumn(seatLabel, column);
                    grdSeatingView.Children.Add(seatLabel);

                }
            }
        }

        //Assign to Team 1 Member
        private async void ButtonReserveRange(object sender, EventArgs e)
        {
            // Taking input for the first and last seats in the range and creating variables
            // for the seat numbers
            string first_seat = await DisplayPromptAsync("Enter Seat Range", "Enter first seat: ");
            string last_seat = await DisplayPromptAsync("Enter Seat Range", "Enter last seat: ");
            int f_seat_num = 0;
            int l_seat_num = 0;

            // Main if statement to try to find the seats and reserve them if they're available
            if (first_seat != null && last_seat != null)
            {
                // This if statement makes sure the seats are on the same row
                if (first_seat[0] != last_seat[0])
                {
                    await DisplayAlert("Error", "Seats must all be on the same row.", "Ok");
                    return;
                }

                // These statements to make sure the seat numbers are valid, meaning they're not the same
                // and they are within the range of the row
                if (first_seat == last_seat)
                {
                    await DisplayAlert("Error", "First and last seat must be different.", "Ok");
                    return;
                }
                else
                {
                    // First seat number shouldn't be greater than 9
                    if (first_seat.Length > 2)
                    {
                        await DisplayAlert("Error", "Invalid seat numbers.", "Ok");
                        return;
                    }
                    else
                    {
                        f_seat_num = (Convert.ToInt32(Char.GetNumericValue(first_seat, 1)));
                    }
                    // Last seat number can be 10, but no greater
                    if (last_seat.Length > 2)
                    {
                        if (last_seat[1] == '1' && last_seat[2] == '0')
                        {
                            l_seat_num = 10;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Invalid seat numbers.", "Ok");
                            return;
                        }
                    }
                    else
                    {
                        l_seat_num = (Convert.ToInt32(Char.GetNumericValue(last_seat, 1)));
                    }
                }

                // For loop that finds the seats in the array seatingChart
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        // These two if statements find the first seat
                        if (seatingChart[i, j].Name[0] == first_seat[0])
                        {
                            if (seatingChart[i, j].Name[1] == first_seat[1])
                            {
                                // bool variable and for loop to check if a seat within the range is already reserved
                                bool seatAlreadyReserved = false;
                                for (int k = (f_seat_num - 1); k <= (l_seat_num - 1); k++)
                                {
                                    if (seatingChart[i, k].Reserved == true)
                                    {
                                        seatAlreadyReserved = true;
                                    }
                                }
                                // If none of the seats in the range are reserved already, then they will be.
                                // Otherwise, an error message is displayed
                                if (seatAlreadyReserved != true)
                                {
                                    for (int k = (f_seat_num - 1); k <= (l_seat_num - 1); k++)
                                    {
                                        seatingChart[i, k].Reserved = true;
                                    }
                                    await DisplayAlert("Successfully Reserved", "Your seats were reserved successfully!", "Ok");
                                    RefreshSeating();
                                    return;
                                }
                                else
                                {
                                    await DisplayAlert("Error", "Seats in that range are already reserved.", "Ok");
                                    return;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }

        //Assign to Team 2 Member
        //Zavian Holmes
        private async void ButtonCancelReservation(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Range", "Enter Seat Number to Cancel Reservation:");

            //Proceeds only if the user gives input
            if (seat != null)
            {
                //Loops through the seating chart to locate the seat specifed by the user
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        // Check if the current seat matches the user's input
                        if (seatingChart[i, j].Name == seat)
                        {
                            //If the seat is reserved, the reservation is cancelled
                            if (seatingChart[i,j].Reserved)
                            {
                                seatingChart[i, j].Reserved = false; // Sets the seat as available
                                RefreshSeating(); //Refreshes the UI to reflect changes made
                                await DisplayAlert("Success", "Reservation canceled successfully!", "Ok");
                                return; // Exit once the reservation is canceled
                            }
                            else
                            {
                                //If the seat is not reserved, an error message is displayed
                                await DisplayAlert("Error", "Seat is not reserved.", "Ok");
                                return;
                            }
                        }
                    }
                }
                //If no matching seat is found, an error message is displayed
                await DisplayAlert("Error", "Seat not found.", "Ok");
            }
        }

        private async void ButtonCancelReservationRange(object sender, EventArgs e)
        {
            //Divine Precious-Esue
        }
        private async void ButtonResetSeatingChart(object sender, EventArgs e)
        {
            //Divine Precious-Esue
            //Confirms whether the user wants to reset the seating chart
            bool input = await DisplayAlert("Confirmation", "Are you sure you would like to reset the seating chart?", "Yes", "No");
            if (input)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        seatingChart[i, j].Reserved = false;
                    }
                }
                await DisplayAlert("Succesful!","The seating chart was reset succesfully", "Ok");
                RefreshSeating();
            }
        }
    }
}
           

