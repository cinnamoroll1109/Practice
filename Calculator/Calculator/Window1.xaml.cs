using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Calculator
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            LoadDataIntoDataGrid();
        }

        object selectItem;
        string selectID;

        private void LoadDataIntoDataGrid()
        {
            string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=c#";

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM calculator";

                MySqlDataReader sdr = cmd.ExecuteReader();

                DataTable dtRecords = new DataTable();
                dtRecords.Load(sdr);
                dataGrid.DataContext = dtRecords;
                sdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        #region Buttons Setting

        private void Button_previous_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void Button_delete_Click(object sender, RoutedEventArgs e)
        {                        
            string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=c#";

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM calculator WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", selectID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("delete successfully");
                selectItem = null;
                selectID = null;

                cmd.CommandText = "UPDATE calculator SET id = id - 1 WHERE id > @id; ";
                cmd.ExecuteNonQuery();
                //MessageBox.Show("update successfully");

                cmd.CommandText = "SELECT * FROM calculator";
                MySqlDataReader sdr = cmd.ExecuteReader();
                DataTable dtRecords = new DataTable();
                dtRecords.Load(sdr);
                dataGrid.DataContext = dtRecords;
                sdr.Close();
                conn.Close();               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        #endregion Buttons Setting

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                selectItem = dataGrid.SelectedItem;
                selectID = (dataGrid.SelectedCells[0].Column.GetCellContent(selectItem) as TextBlock).Text;
                //MessageBox.Show(ID);
            }
        }
    }
}
